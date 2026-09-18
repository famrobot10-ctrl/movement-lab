using UnityEngine;
[RequireComponent(typeof(CharacterController),typeof(PlayerInputState),typeof(StaminaSystem))]
public class PlayerMotor:MonoBehaviour{
 public MovementSettings settings=new MovementSettings();public Transform cameraTransform;public Animator animator;public LayerMask worldMask=~0;
 CharacterController cc;PlayerInputState input;StaminaSystem stamina;Vector3 velocity,momentumHeading;float forwardRamp,dodgeTimer=-1,lastAirCrouch=-99,lastWallBounce=-99,crouchPressedAt=-1,nextSpeedLog;bool wasGrounded;bool doubleJumpUsed,crouched,sliding,crouchHeld;
 public string CurrentTechnique{get;private set;}="Idle";public Vector3 Velocity=>velocity;
 public Vector2 MoveInput=>input!=null?input.Move:Vector2.zero;
 public float StickMagnitude=>input!=null?Mathf.Clamp01(input.Move.magnitude):0f;
 public float HorizontalSpeed=>new Vector2(velocity.x,velocity.z).magnitude;
 void Awake(){cc=GetComponent<CharacterController>();input=GetComponent<PlayerInputState>();stamina=GetComponent<StaminaSystem>();stamina.Initialize(settings);SetStandingGeometry();}
 void Update(){if(Time.timeScale==0){input.ConsumeFrameButtons();return;}stamina.Tick();bool grounded=cc.isGrounded;
 if(grounded&&!wasGrounded){Vector3 landingH=Vector3.ProjectOnPlane(velocity,Vector3.up);if(landingH.magnitude>settings.sprintSpeed){landingH=landingH.normalized*settings.sprintSpeed;velocity=new Vector3(landingH.x,velocity.y,landingH.z);}}
 if(grounded){doubleJumpUsed=false;if(velocity.y<0)velocity.y=-2;}Vector3 wish=Wish();HandleCrouchInput(grounded);HandleSlide(grounded);HandleDodge(wish);HandleJump(grounded,wish);HandleDownDash(grounded);if(dodgeTimer<0)Locomotion(grounded,wish);if(!grounded)velocity.y=Mathf.Max(velocity.y-settings.gravity*Time.deltaTime,-settings.maxFallSpeed);cc.Move(velocity*Time.deltaTime);wasGrounded=cc.isGrounded;LogSpeed();input.ConsumeFrameButtons();}
 Vector3 Wish(){Vector3 f=cameraTransform?Vector3.ProjectOnPlane(cameraTransform.forward,Vector3.up).normalized:transform.forward,r=cameraTransform?Vector3.ProjectOnPlane(cameraTransform.right,Vector3.up).normalized:transform.right;return Vector3.ClampMagnitude(f*input.Move.y+r*input.Move.x,1);}
 float Smooth(float t){t=Mathf.Clamp01(t);return t*t*(3-2*t);}
 void Locomotion(bool grounded,Vector3 wish){Vector3 h=Vector3.ProjectOnPlane(velocity,Vector3.up);
  if(grounded){if(sliding){h=Vector3.MoveTowards(h,Vector3.zero,settings.slideFriction*Time.deltaTime);CurrentTechnique="Slide";}else{
   float stick=Mathf.Clamp01(input.Move.magnitude);
   bool hasInput=stick>.05f;
   bool sprintIntent=!crouched&&stick>=(1f/3f);
   if(sprintIntent)forwardRamp=Mathf.MoveTowards(forwardRamp,1f,Time.deltaTime/Mathf.Max(.01f,settings.walkToSprintSeconds));
   else forwardRamp=Mathf.MoveTowards(forwardRamp,0f,Time.deltaTime/Mathf.Max(.01f,settings.rampResetSeconds));

   float desiredSpeed=0f;
   if(crouched&&hasInput)desiredSpeed=settings.walkSpeed*.60f;
   else if(hasInput&&stick<=(1f/3f))desiredSpeed=settings.walkSpeed*(stick/(1f/3f));
   else if(hasInput)desiredSpeed=Mathf.Lerp(settings.walkSpeed,settings.sprintSpeed,Smooth(forwardRamp));
   // One radial speed budget: forward, backward, lateral and diagonal movement all use the same gait speed.
   // Stick direction only chooses heading, never a separate lateral speed cap.

   if(!hasInput){if(h.magnitude<settings.walkSpeed+.05f)momentumHeading=Vector3.zero;float blend=Mathf.Clamp01(h.magnitude/Mathf.Max(.01f,settings.brakingTransitionSpeed));float brake=Mathf.Lerp(settings.lowSpeedBraking,settings.highSpeedBraking,blend);h=Vector3.MoveTowards(h,Vector3.zero,brake*Time.deltaTime);}
   else{
    Vector3 desiredDir=wish.normalized;
    float currentSpeed=h.magnitude;
    if(momentumHeading.sqrMagnitude<.01f)momentumHeading=desiredDir;
    float turnAngle=Vector3.Angle(momentumHeading,desiredDir);
    // Momentum cone narrows continuously with speed: 360 degrees at rest, 45 degrees
    // at configured sprintSpeed. No hard-coded 9 m/s dependency.
    float speedRatio=Mathf.Clamp01(currentSpeed/Mathf.Max(.01f,settings.sprintSpeed));
    float momentumRange=Mathf.Lerp(360f,45f,speedRatio);
    float turnRetention=turnAngle<=momentumRange?1f:Mathf.Clamp01(1f-(turnAngle-momentumRange)/Mathf.Max(.01f,180f-momentumRange));
    float retainedSpeed=currentSpeed*turnRetention;
    float newSpeed=retainedSpeed;
    if(desiredSpeed>retainedSpeed)newSpeed=Mathf.MoveTowards(retainedSpeed,desiredSpeed,settings.groundAcceleration*Time.deltaTime);
    else if(stick<=(1f/3f))newSpeed=Mathf.MoveTowards(retainedSpeed,desiredSpeed,settings.lowSpeedBraking*Time.deltaTime);
    // Once the turn penalty has been paid, the requested direction becomes the new
    // momentum heading so the player can immediately build speed around/after a corner.
    if(turnAngle>momentumRange)momentumHeading=desiredDir;
    else if(currentSpeed<=settings.walkSpeed+.05f)momentumHeading=desiredDir;
    h=desiredDir*newSpeed;
   }
   if(crouched)CurrentTechnique="Crouch";else if(sprintIntent&&forwardRamp>=.98f)CurrentTechnique="Sprint";else if(stick>(1f/3f))CurrentTechnique="Jog";else CurrentTechnique="Walk";}}
  else{float fr=input.Move.x!=0?0:settings.airFriction;h=Vector3.MoveTowards(h,Vector3.zero,fr*Time.deltaTime);
   // Air control changes direction without creating free horizontal speed from an ordinary jump.
   if(wish.sqrMagnitude>.001f){float airSpeed=Mathf.Min(h.magnitude,settings.sprintSpeed);Vector3 target=wish.normalized*airSpeed;h=Vector3.MoveTowards(h,target,settings.airAcceleration*settings.airControl*Time.deltaTime);}
   h=Vector3.ClampMagnitude(h,settings.maxAirSpeed);}
  velocity=new Vector3(h.x,velocity.y,h.z);}
 void LogSpeed(){if(Time.time<nextSpeedLog)return;nextSpeedLog=Time.time+.25f;Vector3 h=Vector3.ProjectOnPlane(velocity,Vector3.up);Debug.Log(string.Format("[Movement] {0} | Speed {1:0.00} m/s | X {2:0.00} | Z {3:0.00} | Stick {4:0.00} ({5:0.00},{6:0.00}) | Ramp {7:0.00}",CurrentTechnique,h.magnitude,h.x,h.z,StickMagnitude,MoveInput.x,MoveInput.y,forwardRamp));}
 void HandleCrouchInput(bool grounded){if(input.CrouchPressed&&grounded){crouchPressedAt=Time.time;crouchHeld=true;}if(crouchHeld&&!input.CrouchHeld){float held=Time.time-crouchPressedAt;if(held<settings.crouchHoldThreshold&&!sliding)ToggleCrouch();if(sliding)sliding=false;crouchHeld=false;}}
 void HandleSlide(bool grounded){if(!grounded||!input.CrouchHeld){if(sliding&&!input.CrouchHeld){sliding=false;if(crouched)SetCrouchGeometry();else SetStandingGeometry();}return;}if(Time.time-crouchPressedAt<settings.crouchHoldThreshold)return;float speed=Vector3.ProjectOnPlane(velocity,Vector3.up).magnitude;if(speed>=settings.slideEntrySpeed){if(!sliding){sliding=true;crouched=false;SetCrouchGeometry();}CurrentTechnique="Slide";}}
 void ToggleCrouch(){if(crouched){if(CanStand()){crouched=false;SetStandingGeometry();}}else{crouched=true;SetCrouchGeometry();}}
 void SetStandingGeometry(){cc.height=1.7018f;cc.radius=.4967f;cc.stepOffset=.30f;cc.skinWidth=.08f;cc.minMoveDistance=.001f;cc.slopeLimit=45;cc.center=Vector3.zero;SetVisual(false);}
 void SetCrouchGeometry(){cc.height=.8509f;cc.radius=.4016f;cc.center=new Vector3(0,-.42545f,0);SetVisual(true);}
 void SetVisual(bool on){Transform v=transform.Find("Player Visual");if(v){v.localScale=on?new Vector3(.818f,.422f,.818f):new Vector3(.7304f,.7304f,.7304f);v.localPosition=on?new Vector3(0,-.358f,0):Vector3.zero;}}
 bool CanStand(){Vector3 origin=transform.position+Vector3.up*(cc.height-.05f);return !Physics.SphereCast(origin,cc.radius*.9f,Vector3.up,out _,.9f,worldMask,QueryTriggerInteraction.Ignore);}
 void HandleJump(bool grounded,Vector3 wish){if(!input.JumpPressed)return;if(!grounded&&TryWallBounce())return;if(grounded){velocity.y=settings.jumpVelocity;CurrentTechnique="Jump";}else if(!doubleJumpUsed&&stamina.Spend(1)){doubleJumpUsed=true;velocity.y=settings.doubleJumpVelocity;CurrentTechnique="Double-Jump";}}
 bool TryWallBounce(){if(Time.time-lastWallBounce<settings.wallBounceLockout)return false;Vector3 origin=transform.position+Vector3.up*.9f;Vector3[] ds={transform.forward,-transform.forward,transform.right,-transform.right};foreach(Vector3 d in ds)if(Physics.SphereCast(origin,.32f,d,out RaycastHit hit,settings.wallDetectionDistance,worldMask,QueryTriggerInteraction.Ignore)&&Vector3.Dot(hit.normal,Vector3.up)<.35f&&stamina.Spend(.5f)){Vector3 tangent=Vector3.ProjectOnPlane(Vector3.ProjectOnPlane(velocity,Vector3.up),hit.normal);Vector3 carry=(tangent.sqrMagnitude>.01f?tangent.normalized:Vector3.ProjectOnPlane(transform.forward,hit.normal).normalized)*settings.wallBounceForwardVelocity;Vector3 outv=hit.normal*settings.wallBounceOutwardVelocity+carry;velocity=new Vector3(outv.x,settings.wallBounceVerticalVelocity,outv.z);lastWallBounce=Time.time;CurrentTechnique="Wall-Bounce";return true;}return false;}
 void HandleDodge(Vector3 wish){if(input.DodgePressed&&stamina.Spend(1)){dodgeTimer=0;Vector3 d=wish.sqrMagnitude>.01f?wish:transform.forward;velocity=new Vector3(d.x*settings.dodgeSpeed,velocity.y,d.z*settings.dodgeSpeed);CurrentTechnique="Dodge";}if(dodgeTimer>=0){dodgeTimer+=Time.deltaTime;if(dodgeTimer>settings.dodgeDuration)dodgeTimer=-1;}}
 void HandleDownDash(bool grounded){if(grounded||!input.CrouchPressed)return;if(Time.time-lastAirCrouch<=settings.downDashDoubleTapWindow&&stamina.Spend(.5f)){Vector3 h=Vector3.ProjectOnPlane(velocity,Vector3.up)*(1-settings.downDashForwardVelocityReduction);velocity=new Vector3(h.x,-settings.downDashVelocity,h.z);CurrentTechnique="Down-Dash";lastAirCrouch=-99;}else lastAirCrouch=Time.time;}
}