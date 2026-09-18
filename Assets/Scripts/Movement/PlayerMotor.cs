using UnityEngine;
[RequireComponent(typeof(CharacterController),typeof(PlayerInputState),typeof(StaminaSystem))]
public class PlayerMotor:MonoBehaviour{
 public MovementSettings settings=new MovementSettings();public Transform cameraTransform;public Animator animator;public LayerMask worldMask=~0;
 CharacterController cc;PlayerInputState input;StaminaSystem stamina;Vector3 velocity;float forwardRamp,dodgeTimer=-1,lastAirCrouch=-99,lastWallBounce=-99,crouchPressedAt=-1;bool doubleJumpUsed,crouched,sliding,crouchHeld;
 public string CurrentTechnique{get;private set;}="Idle";public Vector3 Velocity=>velocity;
 void Awake(){cc=GetComponent<CharacterController>();input=GetComponent<PlayerInputState>();stamina=GetComponent<StaminaSystem>();stamina.Initialize(settings);SetStandingGeometry();}
 void Update(){if(Time.timeScale==0){input.ConsumeFrameButtons();return;}stamina.Tick();bool grounded=cc.isGrounded;if(grounded){doubleJumpUsed=false;if(velocity.y<0)velocity.y=-2;}Vector3 wish=Wish();HandleCrouchInput(grounded);HandleSlide(grounded);HandleDodge(wish);HandleJump(grounded,wish);HandleDownDash(grounded);if(dodgeTimer<0)Locomotion(grounded,wish);if(!grounded)velocity.y=Mathf.Max(velocity.y-settings.gravity*Time.deltaTime,-settings.maxFallSpeed);cc.Move(velocity*Time.deltaTime);input.ConsumeFrameButtons();}
 Vector3 Wish(){Vector3 f=cameraTransform?Vector3.ProjectOnPlane(cameraTransform.forward,Vector3.up).normalized:transform.forward,r=cameraTransform?Vector3.ProjectOnPlane(cameraTransform.right,Vector3.up).normalized:transform.right;return Vector3.ClampMagnitude(f*input.Move.y+r*input.Move.x,1);}
 float Smooth(float t){t=Mathf.Clamp01(t);return t*t*(3-2*t);}
 void Locomotion(bool grounded,Vector3 wish){Vector3 h=Vector3.ProjectOnPlane(velocity,Vector3.up);
  if(grounded){if(sliding){h=Vector3.MoveTowards(h,Vector3.zero,settings.slideFriction*Time.deltaTime);CurrentTechnique="Slide";}else{
   float inputAmount=Mathf.Clamp01(input.Move.magnitude);bool moving=!crouched&&inputAmount>.05f;
   if(moving)forwardRamp=Mathf.MoveTowards(forwardRamp,1f,Time.deltaTime/Mathf.Max(.01f,settings.walkToSprintSeconds));else forwardRamp=Mathf.MoveTowards(forwardRamp,0f,Time.deltaTime/Mathf.Max(.01f,settings.rampResetSeconds));
   float phase=Smooth(forwardRamp);float movementSpeed=crouched?settings.crouchSpeed:Mathf.Lerp(settings.walkSpeed,settings.sprintSpeed,phase);
   // Steering changes direction without resetting accumulated movement speed.
   Vector3 target=wish.sqrMagnitude>.001f?wish.normalized*(movementSpeed*inputAmount):Vector3.zero;
   if(wish.sqrMagnitude<.01f){float blend=Mathf.Clamp01(h.magnitude/Mathf.Max(.01f,settings.brakingTransitionSpeed));float brake=Mathf.Lerp(settings.lowSpeedBraking,settings.highSpeedBraking,blend);h=Vector3.MoveTowards(h,Vector3.zero,brake*Time.deltaTime);}
   else{
    float currentSpeed=h.magnitude;
    float targetSpeed=target.magnitude;
    Vector3 desiredDir=targetSpeed>.001f?target/targetSpeed:(currentSpeed>.001f?h.normalized:Vector3.zero);
    Vector3 currentDir=currentSpeed>.001f?h/currentSpeed:desiredDir;
    float alignment=Vector3.Dot(currentDir,desiredDir);
    if(alignment>=0f&&currentSpeed>.05f){
      float steerT=Mathf.Clamp01(settings.steeringAcceleration*Time.deltaTime/Mathf.Max(currentSpeed,.01f));
      Vector3 steeredDir=Vector3.Slerp(currentDir,desiredDir,steerT).normalized;
      float speedAccel=targetSpeed>currentSpeed?settings.groundAcceleration:settings.lowSpeedBraking;
      float newSpeed=Mathf.MoveTowards(currentSpeed,targetSpeed,speedAccel*Time.deltaTime);
      h=steeredDir*newSpeed;
    }else{
      h=Vector3.MoveTowards(h,target,settings.directionChangeAcceleration*Time.deltaTime);
    }
   }
   if(crouched)CurrentTechnique="Crouch";else if(forwardRamp>=settings.sprintPhaseStart)CurrentTechnique="Sprint";else if(forwardRamp>=settings.jogPhaseStart)CurrentTechnique="Jog";else CurrentTechnique="Walk";}}
  else{float fr=input.Move.x!=0?0:settings.airFriction;h=Vector3.MoveTowards(h,Vector3.zero,fr*Time.deltaTime);h+=wish*settings.airAcceleration*settings.airControl*Time.deltaTime;h=Vector3.ClampMagnitude(h,settings.maxAirSpeed);}
  velocity=new Vector3(h.x,velocity.y,h.z);}
 void HandleCrouchInput(bool grounded){if(input.CrouchPressed&&grounded){crouchPressedAt=Time.time;crouchHeld=true;}if(crouchHeld&&!input.CrouchHeld){float held=Time.time-crouchPressedAt;if(held<settings.crouchHoldThreshold&&!sliding)ToggleCrouch();if(sliding)sliding=false;crouchHeld=false;}}
 void HandleSlide(bool grounded){if(!grounded||!input.CrouchHeld){if(sliding&&!input.CrouchHeld)sliding=false;return;}if(Time.time-crouchPressedAt<settings.crouchHoldThreshold)return;float speed=Vector3.ProjectOnPlane(velocity,Vector3.up).magnitude;if(speed>=settings.slideEntrySpeed){sliding=true;crouched=false;CurrentTechnique="Slide";}}
 void ToggleCrouch(){if(crouched){if(CanStand()){crouched=false;SetStandingGeometry();}}else{crouched=true;SetCrouchGeometry();}}
 void SetStandingGeometry(){cc.height=2.33f;cc.radius=.68f;cc.stepOffset=.30f;cc.skinWidth=.08f;cc.minMoveDistance=.001f;cc.slopeLimit=45;cc.center=Vector3.zero;SetVisual(false);}
 void SetCrouchGeometry(){cc.height=1.165f;cc.radius=.55f;cc.center=new Vector3(0,-.5825f,0);SetVisual(true);}
 void SetVisual(bool on){Transform v=transform.Find("Player Visual");if(v){v.localScale=on?new Vector3(1.12f,.58f,1.12f):Vector3.one;v.localPosition=on?new Vector3(0,-.49f,0):Vector3.zero;}}
 bool CanStand(){Vector3 origin=transform.position+Vector3.up*(cc.height-.05f);return !Physics.SphereCast(origin,cc.radius*.9f,Vector3.up,out _,.9f,worldMask,QueryTriggerInteraction.Ignore);}
 void HandleJump(bool grounded,Vector3 wish){if(!input.JumpPressed)return;if(!grounded&&TryWallBounce())return;if(grounded){velocity.y=settings.jumpVelocity;CurrentTechnique="Jump";}else if(!doubleJumpUsed&&stamina.Spend(1)){doubleJumpUsed=true;velocity.y=settings.doubleJumpVelocity;CurrentTechnique="Double-Jump";}}
 bool TryWallBounce(){if(Time.time-lastWallBounce<settings.wallBounceLockout)return false;Vector3 origin=transform.position+Vector3.up*.9f;Vector3[] ds={transform.forward,-transform.forward,transform.right,-transform.right};foreach(Vector3 d in ds)if(Physics.SphereCast(origin,.32f,d,out RaycastHit hit,settings.wallDetectionDistance,worldMask,QueryTriggerInteraction.Ignore)&&Vector3.Dot(hit.normal,Vector3.up)<.35f&&stamina.Spend(.5f)){Vector3 tangent=Vector3.ProjectOnPlane(Vector3.ProjectOnPlane(velocity,Vector3.up),hit.normal);Vector3 carry=(tangent.sqrMagnitude>.01f?tangent.normalized:Vector3.ProjectOnPlane(transform.forward,hit.normal).normalized)*settings.wallBounceForwardVelocity;Vector3 outv=hit.normal*settings.wallBounceOutwardVelocity+carry;velocity=new Vector3(outv.x,settings.wallBounceVerticalVelocity,outv.z);lastWallBounce=Time.time;CurrentTechnique="Wall-Bounce";return true;}return false;}
 void HandleDodge(Vector3 wish){if(input.DodgePressed&&stamina.Spend(1)){dodgeTimer=0;Vector3 d=wish.sqrMagnitude>.01f?wish:transform.forward;velocity=new Vector3(d.x*settings.dodgeSpeed,velocity.y,d.z*settings.dodgeSpeed);CurrentTechnique="Dodge";}if(dodgeTimer>=0){dodgeTimer+=Time.deltaTime;if(dodgeTimer>settings.dodgeDuration)dodgeTimer=-1;}}
 void HandleDownDash(bool grounded){if(grounded||!input.CrouchPressed)return;if(Time.time-lastAirCrouch<=settings.downDashDoubleTapWindow&&stamina.Spend(.5f)){Vector3 h=Vector3.ProjectOnPlane(velocity,Vector3.up)*(1-settings.downDashForwardVelocityReduction);velocity=new Vector3(h.x,-settings.downDashVelocity,h.z);CurrentTechnique="Down-Dash";lastAirCrouch=-99;}else lastAirCrouch=Time.time;}
}