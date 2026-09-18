using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
public class ThirdPersonCamera:MonoBehaviour{
 public Transform target; public Vector3 pivotOffset=new Vector3(0,1.25f,0); public float distance=3.15f,shoulderOffset=.85f,sensitivity=150f,minPitch=-25f,maxPitch=70f,followSharpness=16f; float yaw,pitch=18f;
 void Start(){if(target)yaw=target.eulerAngles.y;Cursor.lockState=CursorLockMode.Locked;Cursor.visible=false;}
 void LateUpdate(){if(!target)return;float mx=0,my=0;
#if ENABLE_INPUT_SYSTEM
 if(Mouse.current!=null){Vector2 d=Mouse.current.delta.ReadValue();mx=d.x*.02f;my=d.y*.02f;}if(Gamepad.current!=null){Vector2 s=Gamepad.current.rightStick.ReadValue();mx+=s.x;my+=s.y;}
#endif
 yaw+=mx*sensitivity*Time.unscaledDeltaTime;pitch=Mathf.Clamp(pitch+my*sensitivity*Time.unscaledDeltaTime,minPitch,maxPitch);Quaternion r=Quaternion.Euler(pitch,yaw,0);Vector3 pivot=target.position+pivotOffset;Vector3 desired=pivot-r*Vector3.forward*distance-r*Vector3.right*shoulderOffset;float t=1-Mathf.Exp(-followSharpness*Time.unscaledDeltaTime);transform.position=Vector3.Lerp(transform.position,desired,t);transform.rotation=r;}
}
