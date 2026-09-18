using System;using System.IO;using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
public class ControlProfileManager:MonoBehaviour{
#if ENABLE_INPUT_SYSTEM
 public PlayerInput playerInput;
#endif
 public ControlProfile profile=new ControlProfile();public event Action OnControlsChanged;
 void Awake(){EnsureDefaults();Apply();}
 public void EnsureDefaults(){if(profile.bindings.Count>0)return;Add("Move","<Keyboard>/w","<Gamepad>/leftStick");Add("Jump","<Keyboard>/space","<Gamepad>/buttonSouth");Add("Crouch","<Keyboard>/c","<Gamepad>/buttonEast");Add("Dodge","<Keyboard>/leftShift","<Gamepad>/leftStickPress");Add("Menu","<Keyboard>/escape","<Gamepad>/start");Add("QuickTune","<Keyboard>/tab","<Gamepad>/select");Add("Undo","<Keyboard>/z","<Gamepad>/dpad/left");Add("Redo","<Keyboard>/y","<Gamepad>/dpad/right");}
 void Add(string a,string k,string g)=>profile.bindings.Add(new ActionBinding{action=a,keyboardPath=k,gamepadPath=g});
 public void Apply(){
#if ENABLE_INPUT_SYSTEM
 if(playerInput&&playerInput.actions!=null){playerInput.actions.RemoveAllBindingOverrides();foreach(var b in profile.bindings){var a=playerInput.actions.FindAction(b.action,false);if(a==null)continue;for(int i=0;i<a.bindings.Count;i++){string p=a.bindings[i].effectivePath;if(p.Contains("Keyboard")&&!string.IsNullOrEmpty(b.keyboardPath))a.ApplyBindingOverride(i,b.keyboardPath);else if(p.Contains("Gamepad")&&!string.IsNullOrEmpty(b.gamepadPath))a.ApplyBindingOverride(i,b.gamepadPath);}}}
#endif
 OnControlsChanged?.Invoke();}
 public void Save(string name){profile.profileName=name;string d=Path.Combine(Application.persistentDataPath,"ControlProfiles");Directory.CreateDirectory(d);File.WriteAllText(Path.Combine(d,name+".json"),JsonUtility.ToJson(profile,true));}
 public void ResetToDefault(){profile=new ControlProfile();EnsureDefaults();Apply();}
}
