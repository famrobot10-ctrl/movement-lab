using System;
using System.Collections.Generic;
[Serializable]
public class MovementProfile {
    public int version=1;
    public string profileName="Default";
    public MovementSettings settings=new MovementSettings();
    public List<string> pinned=new List<string>();
    public bool showMovementChain=true;
}
