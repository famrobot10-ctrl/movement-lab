using UnityEngine;
public class StaminaSystem : MonoBehaviour {
    public MovementSettings settings;
    public float CurrentBars { get; private set; }
    float lastSpend=-999f;
    public void Initialize(MovementSettings s){settings=s; CurrentBars=s.staminaBars;}
    public bool Spend(float bars){
        if(CurrentBars+0.0001f<bars) return false;
        CurrentBars-=bars; lastSpend=Time.time; return true;
    }
    public void Tick(){
        float max=settings.staminaBars;
        CurrentBars=Mathf.Min(CurrentBars,max);
        if(Time.time-lastSpend>=settings.staminaRegenDelay)
            CurrentBars=Mathf.MoveTowards(CurrentBars,max,settings.staminaRegenBarsPerSecond*Time.deltaTime);
    }
}
