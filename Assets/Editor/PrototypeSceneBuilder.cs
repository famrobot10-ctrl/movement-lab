#if UNITY_EDITOR
using UnityEditor;using UnityEditor.SceneManagement;using UnityEngine;using UnityEngine.SceneManagement;
public static class PrototypeSceneBuilder{
 static Material Solid(string name,Color c){
  var sh=Shader.Find("Sprites/Default");
  if(!sh)sh=Shader.Find("UI/Default");
  var m=new Material(sh);m.name=name;m.color=c;return m;
 }
 static GameObject Box(string n,Vector3 p,Vector3 s,Transform parent){var g=GameObject.CreatePrimitive(PrimitiveType.Cube);g.name=n;g.transform.SetParent(parent);g.transform.position=p;g.transform.localScale=s;return g;}
 static void Outline(GameObject g,Material edge,float thickness=.035f){
  Vector3 s=g.transform.localScale;Vector3 p=g.transform.position;
  Vector3[] c={new Vector3(-1,-1,-1),new Vector3(1,-1,-1),new Vector3(-1,-1,1),new Vector3(1,-1,1),new Vector3(-1,1,-1),new Vector3(1,1,-1),new Vector3(-1,1,1),new Vector3(1,1,1)};
  int[,] e={{0,1},{2,3},{4,5},{6,7},{0,2},{1,3},{4,6},{5,7},{0,4},{1,5},{2,6},{3,7}};
  for(int k=0;k<12;k++){Vector3 a=p+Vector3.Scale(c[e[k,0]]*.5f,s),z=p+Vector3.Scale(c[e[k,1]]*.5f,s);Vector3 d=z-a;var q=GameObject.CreatePrimitive(PrimitiveType.Cube);q.name=g.name+" Edge";q.transform.SetParent(g.transform.parent);q.transform.position=(a+z)*.5f;q.transform.rotation=Quaternion.FromToRotation(Vector3.right,d.normalized);q.transform.localScale=new Vector3(d.magnitude,thickness,thickness);Object.DestroyImmediate(q.GetComponent<BoxCollider>());q.GetComponent<Renderer>().sharedMaterial=edge;}
 }
 [MenuItem("Movement Prototype/Build Complete Test Arena")]
 public static void Build(){var scene=EditorSceneManager.NewScene(NewSceneSetup.EmptyScene,NewSceneMode.Single);var env=new GameObject("MOVEMENT TEST ARENA").transform;
 var floor=Box("Main Floor",new Vector3(0,-.61f,220),new Vector3(32,1,600),env);
 var grass=Solid("Arena Green",new Color(.18f,.52f,.16f));floor.GetComponent<Renderer>().sharedMaterial=grass;
 var run=new GameObject("01 Acceleration Runway").transform;run.SetParent(env);
 for(int meters=0;meters<=500;meters+=100){float z=-30+meters;Box("Distance Line "+meters+"m",new Vector3(0,.03f,z),new Vector3(30,.05f,.18f),run);var sign=Box(meters+"m",new Vector3(-13,1.25f,z),new Vector3(.18f,2.5f,2.5f),run);var label=new GameObject("Label "+meters+"m");label.transform.SetParent(sign.transform,false);var tm=label.AddComponent<TextMesh>();tm.text=meters+"m";tm.fontSize=64;tm.characterSize=.08f;tm.anchor=TextAnchor.MiddleCenter;tm.alignment=TextAlignment.Center;label.transform.localPosition=new Vector3(-.6f,0,0);label.transform.localRotation=Quaternion.Euler(0,-90,0);}
 var mantle=new GameObject("03 Mantle + Mantle-Slide Lab").transform;mantle.SetParent(env);
 var gray=Solid("Mantle Gray",new Color(.42f,.42f,.42f));var edge=Solid("Mantle Edge",new Color(.12f,.12f,.12f));
 var ma=Box("Mantle Approach Platform",new Vector3(8,.09f,5),new Vector3(10,.4f,12),mantle);ma.GetComponent<Renderer>().sharedMaterial=gray;Outline(ma,edge);
 var md=Box("Shoulder-Height Mantle Deck",new Vector3(8,.60f,14),new Vector3(14,1.42f,8),mantle);md.GetComponent<Renderer>().sharedMaterial=gray;Outline(md,edge);
 var ml=Box("Mantle-Slide Left Runout",new Vector3(1,.60f,14),new Vector3(8,1.42f,5),mantle);ml.GetComponent<Renderer>().sharedMaterial=gray;Outline(ml,edge);
 var mr=Box("Mantle-Slide Right Runout",new Vector3(15,.60f,14),new Vector3(8,1.42f,5),mantle);mr.GetComponent<Renderer>().sharedMaterial=gray;Outline(mr,edge);
 var mb=Box("Mantle Slide Backstop",new Vector3(8,1.55f,19),new Vector3(18,.35f,.35f),mantle);mb.GetComponent<Renderer>().sharedMaterial=gray;Outline(mb,edge);
 var walls=new GameObject("04 Wall Bounce Corridor").transform;walls.SetParent(env);Box("Wall L",new Vector3(-4,2.5f,30),new Vector3(.5f,5,22),walls);Box("Wall R",new Vector3(4,2.5f,30),new Vector3(.5f,5,22),walls);
 var p=new GameObject("Player");p.transform.position=new Vector3(0,.7409f,-30);var visual=GameObject.CreatePrimitive(PrimitiveType.Capsule);visual.name="Player Visual";visual.transform.SetParent(p.transform,false);Object.DestroyImmediate(visual.GetComponent<CapsuleCollider>());visual.transform.localScale=new Vector3(.7304f,.7304f,.7304f);
 var cc=p.AddComponent<CharacterController>();cc.slopeLimit=45;cc.stepOffset=.30f;cc.skinWidth=.08f;cc.minMoveDistance=.001f;cc.radius=.4967f;cc.height=1.7018f;
 p.AddComponent<PlayerInputState>();var stamina=p.AddComponent<StaminaSystem>();var motor=p.AddComponent<PlayerMotor>();var hud=new GameObject("HUD Systems").AddComponent<RuntimeStaminaHUD>();hud.stamina=stamina;
 var cg=new GameObject("Main Camera");cg.tag="MainCamera";var cam=cg.AddComponent<Camera>();cam.fieldOfView=68;cg.AddComponent<AudioListener>();var follow=cg.AddComponent<ThirdPersonCamera>();follow.target=p.transform;follow.distance=2.7f;follow.pivotOffset=new Vector3(0,.90f,0);follow.shoulderOffset=-1.05f;motor.cameraTransform=cg.transform;new GameObject("Crosshair System").AddComponent<RuntimeCrosshair>();
 var lg=new GameObject("Directional Light");var l=lg.AddComponent<Light>();l.type=LightType.Directional;l.intensity=1.25f;lg.transform.rotation=Quaternion.Euler(45,-35,0);
 EditorSceneManager.SaveScene(scene,"Assets/MovementPlayground.unity");Selection.activeGameObject=p;}
}
#endif
