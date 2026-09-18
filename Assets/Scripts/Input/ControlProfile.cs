using System;using System.Collections.Generic;using UnityEngine;
[Serializable] public class ActionBinding{public string action,keyboardPath,gamepadPath;}
[Serializable] public class TouchControlPlacement{public string id,action;public Vector2 anchor;public float size=100f;}
[Serializable] public class TouchControlLayout{public float opacity=.55f,leftStickSize=190f,rightLookSize=240f,moveDeadzone=.12f,lookDeadzone=.10f;public List<TouchControlPlacement> controls=new List<TouchControlPlacement>();}
[Serializable] public class ControlProfile{public int version=1;public string profileName="Default Controls";public float lookSensitivity=1f;public bool invertY=true;public bool autoHideTouchWithGamepad=true;public List<ActionBinding> bindings=new List<ActionBinding>();public TouchControlLayout touch=new TouchControlLayout();}
