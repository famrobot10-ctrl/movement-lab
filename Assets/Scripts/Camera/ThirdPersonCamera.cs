using UnityEngine;
public class ThirdPersonCamera:MonoBehaviour{
 public Transform target;
 [Header("Shoulder Camera")] public float distance=3.15f,height=1.25f,shoulderOffset=.85f;
 public float sensitivityX=180f,sensitivityY=140f,minPitch=-55f,maxPitch=70f,positionSharpness=18f;
 float yaw,pitch;
 void Start(){Vector3 e=transform.eulerAngles;yaw=e.y;pitch=e.x>180?e.x-360:e.x;}
 void LateUpdate(){if(!target)return;
#if ENABLE_INPUT_SYSTEM
  var mouse=UnityEngine.InputSystem.Mouse.current;if(mouse!=null){Vector2 d=mouse.delta.ReadValue();yaw+=d.x*sensitivityX*.01f*Time.unscaledDeltaTime;pitch-=d.y*sensitivityY*.01f*Time.unscaledDeltaTime;}
#else
  yaw+=Input.GetAxis("Mouse X")*sensitivityX*Time.unscaledDeltaTime;pitch-=Input.GetAxis("Mouse Y")*sensitivityY*Time.unscaledDeltaTime;
#endif
  pitch=Mathf.Clamp(pitch,minPitch,maxPitch);Quaternion rot=Quaternion.Euler(pitch,yaw,0);
  Vector3 pivot=target.position+Vector3.up*height;
  Vector3 desired=pivot-rot*Vector3.forward*distance-rot*Vector3.right*shoulderOffset;
  transform.position=Vector3.Lerp(transform.position,desired,1-Mathf.Exp(-positionSharpness*Time.unscaledDeltaTime));transform.rotation=rot;
 }
}