using UnityEngine;
using UnityEngine.UI;
public class RuntimeStaminaHUD:MonoBehaviour{
 public StaminaSystem stamina;RectTransform[] fills;Image[] backgrounds;
 void Start(){if(stamina==null)stamina=FindAnyObjectByType<StaminaSystem>();Build();}
 void Build(){if(stamina==null)return;var cgo=new GameObject("Stamina HUD");var c=cgo.AddComponent<Canvas>();c.renderMode=RenderMode.ScreenSpaceOverlay;var sc=cgo.AddComponent<CanvasScaler>();sc.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;sc.referenceResolution=new Vector2(1920,1080);cgo.AddComponent<GraphicRaycaster>();
  var pgo=new GameObject("Stamina Bars");pgo.transform.SetParent(cgo.transform,false);var p=pgo.AddComponent<RectTransform>();p.anchorMin=p.anchorMax=new Vector2(.5f,.08f);p.pivot=new Vector2(.5f,.5f);p.sizeDelta=new Vector2(330,34);var lay=pgo.AddComponent<HorizontalLayoutGroup>();lay.spacing=10;lay.childForceExpandWidth=true;lay.childForceExpandHeight=true;
  int n=Mathf.Max(1,stamina.settings.staminaBars);fills=new RectTransform[n];backgrounds=new Image[n];
  for(int i=0;i<n;i++){var slot=new GameObject("Stamina "+(i+1));slot.transform.SetParent(pgo.transform,false);var bg=slot.AddComponent<Image>();bg.color=new Color(.02f,.02f,.02f,1);backgrounds[i]=bg;
   var fg=new GameObject("Fill");fg.transform.SetParent(slot.transform,false);var rt=fg.AddComponent<RectTransform>();rt.anchorMin=Vector2.zero;rt.anchorMax=Vector2.one;rt.pivot=new Vector2(0,.5f);rt.offsetMin=rt.offsetMax=Vector2.zero;var im=fg.AddComponent<Image>();im.color=new Color(.15f,.8f,1f,1);fills[i]=rt;}
 }
 void Update(){if(stamina==null||fills==null)return;float current=Mathf.Clamp(stamina.CurrentBars,0,fills.Length);for(int i=0;i<fills.Length;i++){float a=Mathf.Clamp01(current-i);fills[i].anchorMax=new Vector2(a,1);fills[i].offsetMin=fills[i].offsetMax=Vector2.zero;}}
}