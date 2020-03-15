(window.webpackJsonp=window.webpackJsonp||[]).push([[88],{178:function(e,t,n){"use strict";n.r(t),n.d(t,"frontMatter",(function(){return c})),n.d(t,"metadata",(function(){return l})),n.d(t,"rightToc",(function(){return p})),n.d(t,"default",(function(){return u}));var r=n(1),i=n(6),a=(n(0),n(191)),o=n(194),c={title:"Generator pipeline"},l={id:"version-1.0.3/generator-pipeline/introduction",title:"Generator pipeline",description:'import { Image, Gallery, GalleryImage } from "@theme/Gallery";\r',source:"@site/versioned_docs\\version-1.0.3\\generator-pipeline\\introduction.md",permalink:"/ProceduralLevelGenerator-Unity/docs/1.0.3/generator-pipeline/introduction",editUrl:"https://github.com/OndrejNepozitek/ProceduralLevelGenerator-Unity/tree/docusaurus/versioned_docs/version-1.0.3/generator-pipeline/introduction.md",version:"1.0.3",sidebar:"version-1.0.3/docs",previous:{title:"Platformers",permalink:"/ProceduralLevelGenerator-Unity/docs/1.0.3/examples/platformers"},next:{title:"Pipeline payload",permalink:"/ProceduralLevelGenerator-Unity/docs/1.0.3/generator-pipeline/pipeline-payload"}},p=[],s={rightToc:p};function u(e){var t=e.components,n=Object(i.a)(e,["components"]);return Object(a.b)("wrapper",Object(r.a)({},s,n,{components:t,mdxType:"MDXLayout"}),Object(a.b)("p",null,"From the beginning, I wanted the plugin to be as extensible as possible, but also usable for people who are not programmers. The main goals were:"),Object(a.b)("ul",null,Object(a.b)("li",{parentName:"ul"},"For users to be able to configure the generator in Editor."),Object(a.b)("li",{parentName:"ul"},"For users to be able to easily replace individual parts of the algorithm without having to change the plugin's code."),Object(a.b)("li",{parentName:"ul"},"For me to be able to provide basic functionality that can be combined with functionality added by users.")),Object(a.b)("p",null,"After some research, I decided to implement the plugin as a pipeline consisting of tasks that operate on a payload that goes through all the tasks. Below you can see an example of a pipeline - both an abstract example and a real-life example."),Object(a.b)(o.c,{src:"img/original/pipeline_diagram.png",caption:"Pipeline example",mdxType:"Image"}),Object(a.b)("p",null,"Internally, the generator is simply an array of pipeline tasks together with a script that can initialize the payload. And because all these scripts inherit from ScriptableObject, we can easily work with them in the inspector window."),Object(a.b)(o.c,{src:"img/original/pipeline_inspector.png",caption:"Example pipeline as seen in the inspector window.",mdxType:"Image"}),Object(a.b)("blockquote",null,Object(a.b)("p",{parentName:"blockquote"},Object(a.b)("strong",{parentName:"p"},"Note:")," You are not forced to divide your own code into multiple tasks. If you feel like there is no benefit in doing that, you can simply create one task and put all your logic there.")))}u.isMDXComponent=!0},191:function(e,t,n){"use strict";n.d(t,"a",(function(){return u})),n.d(t,"b",(function(){return f}));var r=n(0),i=n.n(r);function a(e,t,n){return t in e?Object.defineProperty(e,t,{value:n,enumerable:!0,configurable:!0,writable:!0}):e[t]=n,e}function o(e,t){var n=Object.keys(e);if(Object.getOwnPropertySymbols){var r=Object.getOwnPropertySymbols(e);t&&(r=r.filter((function(t){return Object.getOwnPropertyDescriptor(e,t).enumerable}))),n.push.apply(n,r)}return n}function c(e){for(var t=1;t<arguments.length;t++){var n=null!=arguments[t]?arguments[t]:{};t%2?o(Object(n),!0).forEach((function(t){a(e,t,n[t])})):Object.getOwnPropertyDescriptors?Object.defineProperties(e,Object.getOwnPropertyDescriptors(n)):o(Object(n)).forEach((function(t){Object.defineProperty(e,t,Object.getOwnPropertyDescriptor(n,t))}))}return e}function l(e,t){if(null==e)return{};var n,r,i=function(e,t){if(null==e)return{};var n,r,i={},a=Object.keys(e);for(r=0;r<a.length;r++)n=a[r],t.indexOf(n)>=0||(i[n]=e[n]);return i}(e,t);if(Object.getOwnPropertySymbols){var a=Object.getOwnPropertySymbols(e);for(r=0;r<a.length;r++)n=a[r],t.indexOf(n)>=0||Object.prototype.propertyIsEnumerable.call(e,n)&&(i[n]=e[n])}return i}var p=i.a.createContext({}),s=function(e){var t=i.a.useContext(p),n=t;return e&&(n="function"==typeof e?e(t):c({},t,{},e)),n},u=function(e){var t=s(e.components);return i.a.createElement(p.Provider,{value:t},e.children)},d={inlineCode:"code",wrapper:function(e){var t=e.children;return i.a.createElement(i.a.Fragment,{},t)}},b=Object(r.forwardRef)((function(e,t){var n=e.components,r=e.mdxType,a=e.originalType,o=e.parentName,p=l(e,["components","mdxType","originalType","parentName"]),u=s(n),b=r,f=u["".concat(o,".").concat(b)]||u[b]||d[b]||a;return n?i.a.createElement(f,c({ref:t},p,{components:n})):i.a.createElement(f,c({ref:t},p))}));function f(e,t){var n=arguments,r=t&&t.mdxType;if("string"==typeof e||r){var a=n.length,o=new Array(a);o[0]=b;var c={};for(var l in t)hasOwnProperty.call(t,l)&&(c[l]=t[l]);c.originalType=e,c.mdxType="string"==typeof e?e:r,o[1]=c;for(var p=2;p<a;p++)o[p]=n[p];return i.a.createElement.apply(null,o)}return i.a.createElement.apply(null,n)}b.displayName="MDXCreateElement"},192:function(e,t,n){"use strict";var r=n(0),i=n(32);t.a=function(){return Object(r.useContext)(i.a)}},193:function(e,t,n){"use strict";n.d(t,"a",(function(){return i}));var r=n(192);function i(e){const{siteConfig:t}=Object(r.a)(),{baseUrl:n="/"}=t||{};if(!e)return e;return/^(https?:|\/\/)/.test(e)?e:e.startsWith("/")?n+e.slice(1):n+e}},194:function(e,t,n){"use strict";n.d(t,"a",(function(){return p})),n.d(t,"b",(function(){return s})),n.d(t,"c",(function(){return d}));var r=n(1),i=n(6),a=n(0),o=n.n(a),c=n(193),l=function(e){return o.a.createElement("div",{style:{display:"inline-block",margin:2,overflow:"hidden",position:"relative",width:"calc("+100/e.cols+"% - 4px)",verticalAlign:"top"}},e.children)},p=function(e){return o.a.createElement("div",{style:{fontSize:"0px",margin:"20px 0"}},o.a.Children.map(e.children,(function(t){return o.a.cloneElement(t,{cols:e.cols||4,fixedHeight:e.fixedHeight})})))},s=function(e){return o.a.createElement(l,{cols:e.cols},o.a.createElement("a",{href:Object(c.a)(e.src),target:"_blank"},o.a.createElement("img",{src:Object(c.a)(e.src),alt:"result",style:{height:!0===e.fixedHeight?800/e.cols+"px":"auto",objectFit:!0===e.fixedHeight?"cover":"initial"}})),void 0!==e.caption&&o.a.createElement(u,null,e.caption))},u=function(e){return o.a.createElement("div",{style:{fontSize:16,fontStyle:"italic",textAlign:"center",margin:"10px 0px 15px"}},e.children)},d=function(e){e.src,e.caption;var t=Object(i.a)(e,["src","caption"]);return o.a.createElement("div",{style:{textAlign:"center"}},o.a.createElement("img",Object(r.a)({src:Object(c.a)(e.src)},t)),void 0!==e.caption&&o.a.createElement(u,null,e.caption))}}}]);