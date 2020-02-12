﻿using Assets.ProceduralLevelGenerator.Examples.Common;
using Assets.ProceduralLevelGenerator.Examples.Example1.Scripts;
using UnityEngine;

namespace Assets.ProceduralLevelGenerator.Examples.Example2.Scripts
{
    public class Example2Exit : InteractableBase
    {
        public override void BeginInteract()
        {
            ShowText("Press E to exit the level");
        }

        public override void Interact()
        {
            if (Input.GetKey(KeyCode.E))
            {
                Example2GameManager.Instance.LoadNextLevel();
            }
        }

        public override void EndInteract()
        {
            HideText();
        }
    }
}