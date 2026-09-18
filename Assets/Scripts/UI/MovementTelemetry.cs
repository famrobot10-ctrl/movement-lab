using UnityEngine;
using UnityEngine.UI;
public class MovementTelemetry:MonoBehaviour{
 public PlayerMotor motor;public StaminaSystem stamina;public Text text;public bool visible=true;
 void LateUpdate(){
  if(!text)return;text.gameObject.SetActive(visible);if(!visible||!motor)return;
  Vector3 v=motor.Velocity;Vector2 input=motor.MoveInput;
  text.text=string.Format(
   "Gait: {0}\nHorizontal Speed: {1:0.00} m/s\nX Velocity: {2:0.00}\nZ Velocity: {3:0.00}\nVertical Velocity: {4:0.00}\nStick Magnitude: {5:0.00}\nStick X: {6:0.00}\nStick Y: {7:0.00}\nStamina: {8:0.00}",
   motor.CurrentTechnique,motor.HorizontalSpeed,v.x,v.z,v.y,motor.StickMagnitude,input.x,input.y,stamina?stamina.CurrentBars:0f);
 }
}
