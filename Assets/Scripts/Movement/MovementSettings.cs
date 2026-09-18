using System;
using UnityEngine;
[Serializable]
public class MovementSettings {
 [Header("Ground")]
 public float walkSpeed=3.2f,jogSpeed=5.5f,sprintSpeed=9f,walkToSprintSeconds=2.2f,rampResetSeconds=.35f,lateralSpeedLimit=5.5f;
 [Range(0,1)] public float walkStickThreshold=.72f;
 [Range(0,1)] public float sprintStickThreshold=.90f;
 public float groundAcceleration=46f,directionChangeAcceleration=68f;
 public float walkSteeringRate=720f,jogSteeringRate=540f,sprintSteeringRate=360f;
 public float lowSpeedBraking=85f,highSpeedBraking=16f,brakingTransitionSpeed=6.5f;
 [Range(.25f,4f)] public float accelerationCurvePower=.65f;
 [Header("Crouch")]
 public float crouchSpeed=1.92f,crouchHoldThreshold=.20f; [Range(0,1)] public float crouchFootstepVolume=.35f;
 [Header("Slide")]
 public float slideEntrySpeed=7f,slideSpeed=8f,slideFriction=5f,slopeSlideSpeed=9f,slopeAcceleration=10f,minimumSlopeSlideAngle=10f;
 [Header("Jump")]
 public float jumpVelocity=7.2f,doubleJumpVelocity=6.8f,coyoteTime=.12f,jumpBuffer=.12f;
 [Header("Air")]
 public float airAcceleration=12f,airControl=.75f,airFriction=1.4f,airStrafeFrictionDelay=.18f,airFrictionReturnRate=6f,maxAirSpeed=12f;
 [Header("Dodge / Dash Jump")]
 public float dodgeSpeed=11f,dodgeDuration=.28f,dashJumpWindowStart=.10f,dashJumpWindowEnd=.28f,dashJumpForwardBoost=2.2f,dashJumpVerticalBoost=1.2f;
 [Range(0,1.5f)] public float dashJumpMomentumRetention=1f;
 [Header("Down-Dash")]
 public float downDashVelocity=15f; [Range(0,1)] public float downDashForwardVelocityReduction=.25f; public float downDashDoubleTapWindow=.25f;
 [Header("Wall Bounce")]
 public float wallBounceOutwardVelocity=8f,wallBounceForwardVelocity=5f,wallBounceVerticalVelocity=6f,wallDetectionDistance=.75f,wallBounceLockout=.18f;
 [Header("Mantle / Boost")]
 public float mantleReach=1.1f,mantleHeight=1.5f,mantleSpeed=5f,edgeBoost=1.12f,cornerBoost=1.12f;
 [Header("Stamina")]
 [Min(1)] public int staminaBars=3; public float staminaRegenDelay=1.2f,staminaRegenBarsPerSecond=.75f;
 [Header("World")]
 public float gravity=22f,maxFallSpeed=28f;
}
