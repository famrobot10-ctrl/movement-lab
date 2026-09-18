#if UNITY_EDITOR
using UnityEditor;using UnityEditor.SceneManagement;using UnityEngine;using UnityEngine.SceneManagement;
public static class PrototypeSceneBuilder{
 static GameObject Box(string n,Vector3 p,Vector3 s,Transform parent){var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=n;g.transform.SetParent(parent);g.transform.position=p;g.transform.localScale=s;return g;}
 [MenuItem("Movement Prototype/Build Complete Test Arena")]
 public static void Build(){var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);var env=new GameObject("MOVEMENT TEST ARENA").transform;
 var floor=Box("Main Floor",new Vector3(0,-.61f,220),new Vector3(32,1,600),env);var sh=Shader.Find("Universal Render Pipeline/Lit");if(!sh)sh=Shader.Find("Standard");var grass=new Material(sh);grass.color=new Color(.18f,.52f,.16f);floor.GetComponent<Renderer>().sharedMaterial=grass;
 var run=new GameObject("01 Acceleration Runway").transform;run.SetParent(env);
 for(int meters=0;meters<=500;meters+=100){float z=-30+meters;Box("Distance Line "+meters+"m",new Vector3(0,.03f,z),new Vector3(30,.05f,.18f),run);var sign=Box(meters+"m",new Vector3(-13,1.25f,z),new Vector3(.18f,2.5f,2.5f),run);var label=new GameObject("Label "+meters+"m");label.transform.SetParent(sign.transform,false);var tm=label.AddComponent<TextMesh>();tm.text=meters+"m";tm.fontSize=64;tm.characterSize=.08f;tm.anchor=TextAnchor.MiddleCenter;tm.alignment=TextAlignment.Center;label.transform.localPosition=new Vector3(-.6f,0,0);label.transform.localRotation=Quaternion.Euler(0,-90,0);}
 var walls=new GameObject("04 Wall Bounce Corridor").transform;walls.SetParent(env);Box("Wall L",new Vector3(-4,2.5f,30),new Vector3(.5f,5,22),walls);Box("Wall R",new Vector3(4,2.5f,30),new Vector3(.5f,5,22),walls);
 var p=new GameObject("Player");p.transform.position=new Vector3(0,.7409f,-30);var visual=GameObject.CreatePrimitive(PrimitiveType.Capsule);visual.name="Player Visual";visual.transform.SetParent(p.transform,false);Object.DestroyImmediate(visual.GetComponent<CapsuleCollider>());visual.transform.localScale=new Vector3(.7304f,.7304f,.7304f);
 var cc=p.AddComponent<CharacterController>();cc.slopeLimit=45;cc.stepOffset=.30f;cc.skinWidth=.08f;cc.minMoveDistance=.001f;cc.radius=.4967f;cc.height=1.7018f;
 p.AddComponent<PlayerInputState>();var stamina=p.AddComponent<StaminaSystem>();var motor=p.AddComponent<PlayerMotor>();var hud=new GameObject("HUD Systems").AddComponent<RuntimeStaminaHUD>();hud.stamina=stamina;
 var cg=new GameObject("Main Camera");cg.tag="MainCamera";var cam=cg.AddComponent<Camera>();cam.fieldOfView=68;cg.AddComponent<AudioListener>();var follow=cg.AddComponent<ThirdPersonCamera>();follow.target=p.transform;follow.distance=3.15f;follow.height=1.25f;follow.shoulderOffset=.85f;motor.cameraTransform=cg.transform;new GameObject("Crosshair System").AddComponent<RuntimeCrosshair>();
 var lg=new GameObject("Directional Light");var l=lg.AddComponent<Light>();l.type=LightType.Directional;l.intensity=1.25f;lg.transform.rotation=Quaternion.Euler(45,-35,0);
 EditorSceneManager.SaveScene(scene,"Assets/MovementPlayground.unity");Selection.activeGameObject=p;}
}
#endif
