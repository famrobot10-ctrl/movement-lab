#if UNITY_EDITOR
using UnityEditor;using UnityEditor.SceneManagement;using UnityEngine;using UnityEngine.SceneManagement;
public static class PrototypeSceneBuilder{
 static GameObject Box(string n,Vector3 p,Vector3 s,Transform parent){var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=n;g.transform.SetParent(parent);g.transform.position=p;g.transform.localScale=s;return g;}
 [MenuItem("Movement Prototype/Build Complete Test Arena")]
 public static void Build(){var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);var env=new GameObject("MOVEMENT TEST ARENA").transform;
 var floor=Box("Main Floor",new Vector3(0,-.61f,10),new Vector3(32,1,90),env);var sh=Shader.Find("Universal Render Pipeline/Lit");if(!sh)sh=Shader.Find("Standard");var grass=new Material(sh);grass.color=new Color(.18f,.52f,.16f);floor.GetComponent<Renderer>().sharedMaterial=grass;
 var run=new GameObject("01 Acceleration Runway").transform;run.SetParent(env);for(int i=0;i<10;i++)Box("Speed Marker "+(i+1),new Vector3(-12,.03f,-28+i*5),new Vector3(.12f,.05f,4),run);
 var walls=new GameObject("04 Wall Bounce Corridor").transform;walls.SetParent(env);Box("Wall L",new Vector3(-4,2.5f,30),new Vector3(.5f,5,22),walls);Box("Wall R",new Vector3(4,2.5f,30),new Vector3(.5f,5,22),walls);
 var p=new GameObject("Player");p.transform.position=new Vector3(0,1.055f,-30);var visual=GameObject.CreatePrimitive(PrimitiveType.Capsule);visual.name="Player Visual";visual.transform.SetParent(p.transform,false);Object.DestroyImmediate(visual.GetComponent<CapsuleCollider>());
 var cc=p.AddComponent<CharacterController>();cc.slopeLimit=45;cc.stepOffset=.30f;cc.skinWidth=.08f;cc.minMoveDistance=.001f;cc.radius=.68f;cc.height=2.33f;
 p.AddComponent<PlayerInputState>();var stamina=p.AddComponent<StaminaSystem>();var motor=p.AddComponent<PlayerMotor>();var hud=new GameObject("HUD Systems").AddComponent<RuntimeStaminaHUD>();hud.stamina=stamina;
 var cg=new GameObject("Main Camera");cg.tag="MainCamera";var cam=cg.AddComponent<Camera>();cam.fieldOfView=75;cg.AddComponent<AudioListener>();var follow=cg.AddComponent<ThirdPersonCamera>();follow.target=p.transform;motor.cameraTransform=cg.transform;
 var lg=new GameObject("Directional Light");var l=lg.AddComponent<Light>();l.type=LightType.Directional;l.intensity=1.25f;lg.transform.rotation=Quaternion.Euler(45,-35,0);
 EditorSceneManager.SaveScene(scene,"Assets/MovementPlayground.unity");Selection.activeGameObject=p;}
}
#endif
