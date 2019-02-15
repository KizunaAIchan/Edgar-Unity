﻿namespace Assets.ProceduralLevelGenerator.Scripts.GeneratorPipeline.DungeonGenerators.GraphBasedGenerator
{
	using System;
	using System.Collections.Generic;
	using System.Diagnostics;
	using System.Linq;
	using System.Threading.Tasks;
	using Data.Graphs;
	using InputSetup;
	using MapGeneration.Core.MapDescriptions;
	using MapGeneration.Interfaces.Core.LayoutGenerator;
	using MapGeneration.Interfaces.Core.MapLayouts;
	using Payloads;
	using Payloads.Interfaces;
	using Pipeline;
	using RoomTemplates;
	using RoomTemplates.Doors;
	using RoomTemplates.Transformations;
	using UnityEngine;
	using UnityEngine.Tilemaps;
	using Utils;
	using Debug = UnityEngine.Debug;
	using Object = UnityEngine.Object;
	using OrthogonalLine = GeneralAlgorithms.DataStructures.Common.OrthogonalLine;

	public class GraphBasedGeneratorTask<TPayload> : ConfigurablePipelineTask<TPayload, GraphBasedGeneratorConfig>
		where TPayload : class, IGeneratorPayload, IGraphBasedGeneratorPayload, IRandomGeneratorPayload
	{
		private List<RoomInfo<Room>> generatedRooms;

		public override void Process()
		{
			var stopwatch = new Stopwatch();
			stopwatch.Start();

			if (Config.ShowDebugInfo)
			{
				Debug.Log("--- Generator started ---"); 
			}

			// Setup map description
			var mapDescription = Payload.MapDescription;

			// Generate layout
			var layout = GenerateLayout(mapDescription);

			// Setup room templates
			SetupRoomTemplates(layout);

			// Apply tempaltes
			if (Config.ApplyTemplate)
			{
				ApplyTemplates();
			}

			// Center grid
			if (Config.CenterGrid)
			{
				Payload.Tilemaps[0].CompressBounds();
				Payload.Tilemaps[0].transform.parent.position = -Payload.Tilemaps[0].cellBounds.center;
			}
			
			if (Config.ShowDebugInfo)
			{
				Debug.Log($"--- Completed. {stopwatch.ElapsedMilliseconds / 1000f:F} s ---");
			}
		}

		protected IMapLayout<Room> GenerateLayout(MapDescription<Room> mapDescription)
		{
			if (Config.Timeout <= 0)
			{
				throw new ArgumentException("Timeout must be a positive number.");
			}

			// Setup layout generator
			IBenchmarkableLayoutGenerator<MapDescription<Room>, IMapLayout<Room>> generator;
			if (mapDescription.IsWithCorridors)
			{
				var gen = UnityLayoutGeneratorFactory.GetChainBasedGeneratorWithCorridors(mapDescription.CorridorsOffsets, corridorNodesCreator: new CorridorsNodeCreator(mapDescription));
				gen.InjectRandomGenerator(Payload.Random);
				generator = gen;
			}
			else
			{
				var gen = UnityLayoutGeneratorFactory.GetDefaultChainBasedGenerator<Room>();
				gen.InjectRandomGenerator(Payload.Random);
				generator = gen;
			}

			// Run generator
			IMapLayout<Room> layout = null;
			var task = Task.Run(() => layout = generator.GetLayouts(mapDescription, 1)[0]);
			var taskCompleted = task.Wait(Config.Timeout);

			if (!taskCompleted)
			{
				throw new DungeonGeneratorException("Timeout was reached when generating the layout");
			}

			if (Config.ShowDebugInfo)
			{
				Debug.Log($"Layout generated in {generator.TimeFirst / 1000f:F} seconds");
				Debug.Log($"{generator.IterationsCount} iterations needed, {(generator.IterationsCount / (generator.TimeFirst / 1000d)):0} iterations per second");
			}

			return layout;
		}

		protected void SetupRoomTemplates(IMapLayout<Room> layout)
		{
			var roomTransformations = new RoomTransformations();

			// Prepare an object to hold instantiated room templates
			var parentGameObject = new GameObject("Room template instances");
			parentGameObject.transform.parent = Payload.GameObject.transform;

			// Initialize rooms
			Payload.LayoutData = new Dictionary<Room, RoomInfo<Room>>();
			foreach (var layoutRoom in layout.Rooms)
			{
				var roomTemplate = Payload.RoomDescriptionsToRoomTemplates[layoutRoom.RoomDescription];

				// Instatiate room template
				var room = Object.Instantiate(roomTemplate);
				room.SetActive(false);
				room.transform.SetParent(parentGameObject.transform);

				// Transform room template if needed
				var transformation = layoutRoom.Transformation;
				roomTransformations.Transform(room, transformation);

				// Compute correct room position
				// We cannot directly use layoutRoom.Position because the dungeon moves
				// all room shapes in a way that they are in the first plane quadrant
				// and touch the xy axes. So we have to subtract the original lowest
				// x and y coordinates.
				var smallestX = layoutRoom.RoomDescription.Shape.GetPoints().Min(x => x.X);
				var smallestY = layoutRoom.RoomDescription.Shape.GetPoints().Min(x => x.Y);
				var correctPosition = layoutRoom.Position.ToUnityIntVector3() - new Vector3Int(smallestX, smallestY, 0);
				room.transform.position = correctPosition;

				var roomInfo = new RoomInfo<Room>(roomTemplate, room, correctPosition, TransformDoorInfo(layoutRoom.Doors),
					layoutRoom.IsCorridor, layoutRoom);

				Payload.LayoutData.Add(layoutRoom.Node, roomInfo);
			}
		}

		protected List<DoorInfo<Room>> TransformDoorInfo(IEnumerable<IDoorInfo<Room>> oldDoorInfos)
		{
			return oldDoorInfos.Select(TransformDoorInfo).ToList();
		}

		protected DoorInfo<Room> TransformDoorInfo(IDoorInfo<Room> oldDoorInfo)
		{
			var doorLine = oldDoorInfo.DoorLine;

			switch (doorLine.GetDirection())
			{
				case OrthogonalLine.Direction.Right:
					return new DoorInfo<Room>(new Utils.OrthogonalLine(doorLine.From.ToUnityIntVector3(), doorLine.To.ToUnityIntVector3()), Vector2Int.up, oldDoorInfo.Node);

				case OrthogonalLine.Direction.Left:
					return new DoorInfo<Room>(new Utils.OrthogonalLine(doorLine.To.ToUnityIntVector3(), doorLine.From.ToUnityIntVector3()), Vector2Int.down, oldDoorInfo.Node);

				case OrthogonalLine.Direction.Top:
					return new DoorInfo<Room>(new Utils.OrthogonalLine(doorLine.From.ToUnityIntVector3(), doorLine.To.ToUnityIntVector3()), Vector2Int.left, oldDoorInfo.Node);

				case OrthogonalLine.Direction.Bottom:
					return new DoorInfo<Room>(new Utils.OrthogonalLine(doorLine.To.ToUnityIntVector3(), doorLine.From.ToUnityIntVector3()), Vector2Int.right, oldDoorInfo.Node);

				default:
					throw new ArgumentOutOfRangeException();
			}
		}

		protected void ApplyTemplates()
		{
			foreach (var pair in Payload.LayoutData)
			{
				var roomInfo = pair.Value;

				ApplyTemplate(roomInfo);
			}
		}

		protected void ApplyTemplate(RoomInfo<Room> roomInfo)
		{
			DeleteNonNullTiles(roomInfo);

			var tilemaps = roomInfo.Room.GetComponentsInChildren<Tilemap>();

			for (int i = 0; i < tilemaps.Length; i++)
			{
				var sourceTilemap = tilemaps[i];
				var destinationTilemap = Payload.Tilemaps[i];

				foreach (var position in sourceTilemap.cellBounds.allPositionsWithin)
				{
					var tile = sourceTilemap.GetTile(position);

					if (tile != null)
					{
						destinationTilemap.SetTile(position + roomInfo.Position, tile);
					}
				}
			}
		}

		/// <summary>
		/// Finds all non null tiles in a given room and then takes these positions and deletes
		/// all such tiles on all tilemaps of the dungeon. The reason for this is that we want to
		/// replace all existing tiles with new tiles from the room. 
		/// </summary>
		/// <param name="roomInfo"></param>
		protected void DeleteNonNullTiles(RoomInfo<Room> roomInfo)
		{
			var tilemaps = roomInfo.Room.GetComponentsInChildren<Tilemap>();
			var tilesToRemove = new HashSet<Vector3Int>();

			// Find non-null tiles acrros all tilemaps of the room
			foreach (var sourceTilemap in tilemaps)
			{
				foreach (var position in sourceTilemap.cellBounds.allPositionsWithin)
				{
					var tile = sourceTilemap.GetTile(position);

					if (tile != null)
					{
						tilesToRemove.Add(position);
					}
				}
			}

			// Delete all found tiles acrros all tilemaps of the dungeon
			for (int i = 0; i < tilemaps.Length; i++)
			{
				var destinationTilemap = Payload.Tilemaps[i];

				foreach (var position in tilesToRemove)
				{
					destinationTilemap.SetTile(position + roomInfo.Position, null);
				}
			}

		}
	}
}