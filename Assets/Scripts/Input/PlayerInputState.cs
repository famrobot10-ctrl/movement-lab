using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
public class PlayerInputState : MonoBehaviour {
 public Vector2 Move{get;private set;} public Vector2 Look{get;private set;}
 public bool JumpPressed{get;private set;} public bool CrouchPressed{get;private set;} public bool CrouchHeld{get;private set;} public bool DodgePressed{get;private set;} public bool MenuPressed{get;private set;}
 public void SetMove(Vector2 v)=>Move=v; public void SetLook(Vector2 v)=>Look=v; public void PressJump()=>JumpPressed=true; public void PressCrouch()=>CrouchPressed=true; public void PressDodge()=>DodgePressed=true; public void PressMenu()=>MenuPressed=true;
 void Update(){CrouchHeld=false;
#if ENABLE_INPUT_SYSTEM
  var gp=Gamepad.current;if(gp!=null){Move=gp.leftStick.ReadValue();Look=gp.rightStick.ReadValue();JumpPressed|=gp.buttonSouth.wasPressedThisFrame;CrouchPressed|=gp.buttonEast.wasPressedThisFrame;CrouchHeld|=gp.buttonEast.isPressed;DodgePressed|=gp.leftStickButton.wasPressedThisFrame;MenuPressed|=gp.startButton.wasPressedThisFrame;}
  if(Keyboard.current!=null){Vector2 k=Vector2.zero;if(Keyboard.current.wKey.isPressed)k.y++;if(Keyboard.current.sKey.isPressed)k.y--;if(Keyboard.current.dKey.isPressed)k.x++;if(Keyboard.current.aKey.isPressed)k.x--;if(k.sqrMagnitude>0)Move=Vector2.ClampMagnitude(k,1);JumpPressed|=Keyboard.current.spaceKey.wasPressedThisFrame;CrouchPressed|=Keyboard.current.cKey.wasPressedThisFrame;CrouchHeld|=Keyboard.current.cKey.isPressed;DodgePressed|=Keyboard.current.leftShiftKey.wasPressedThisFrame;MenuPressed|=Keyboard.current.escapeKey.wasPressedThisFrame;}
#endif
 }
 public void ConsumeFrameButtons(){JumpPressed=CrouchPressed=DodgePressed=MenuPressed=false;}
}
