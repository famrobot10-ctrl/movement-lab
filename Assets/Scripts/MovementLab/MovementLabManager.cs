using System;using System.Collections.Generic;using System.IO;using UnityEngine;
public class MovementLabManager:MonoBehaviour{
 public PlayerMotor motor;public GameObject labRoot,quickTuneRoot;public bool IsOpen=>labRoot&&labRoot.activeSelf;
 [Serializable]public struct Change{public string path;public float before,after;public Change(string p,float b,float a){path=p;before=b;after=a;}}
 readonly Stack<Change> undo=new Stack<Change>();readonly Stack<Change> redo=new Stack<Change>();public List<string> pinned=new List<string>();public bool showMovementChain=true;
 void Update(){if(Input.GetKeyDown(KeyCode.Escape))ToggleLab();}
 public void ToggleLab(){bool open=!IsOpen;if(labRoot)labRoot.SetActive(open);Time.timeScale=open?0:1;}
 public void Resume(){if(labRoot)labRoot.SetActive(false);Time.timeScale=1;}
 public void Back(){if(IsOpen)Resume();}
 public void RecordFloat(string path,float before,float after){if(Mathf.Approximately(before,after))return;undo.Push(new Change(path,before,after));redo.Clear();}
 public void Undo(){if(undo.Count==0)return;var c=undo.Pop();if(ReflectionSet(c.path,c.before))redo.Push(c);}
 public void Redo(){if(redo.Count==0)return;var c=redo.Pop();if(ReflectionSet(c.path,c.after))undo.Push(c);}
 bool ReflectionSet(string field,float value){var f=typeof(MovementSettings).GetField(field);if(f==null)return false;if(f.FieldType==typeof(float)){f.SetValue(motor.settings,value);return true;}return false;}
 public void TogglePin(string field){if(pinned.Contains(field))pinned.Remove(field);else pinned.Add(field);}
 public void SaveProfile(string name){var p=new MovementProfile{profileName=name,settings=motor.settings,pinned=new List<string>(pinned),showMovementChain=showMovementChain};string dir=Path.Combine(Application.persistentDataPath,"MovementProfiles");Directory.CreateDirectory(dir);File.WriteAllText(Path.Combine(dir,Safe(name)+".json"),JsonUtility.ToJson(p,true));}
 public bool LoadProfileFromPath(string path){try{var p=JsonUtility.FromJson<MovementProfile>(File.ReadAllText(path));if(p==null)return false;motor.settings=p.settings;pinned=p.pinned??new List<string>();showMovementChain=p.showMovementChain;return true;}catch{return false;}}
 static string Safe(string s){foreach(char c in Path.GetInvalidFileNameChars())s=s.Replace(c,'_');return string.IsNullOrWhiteSpace(s)?"Profile":s;}
}
