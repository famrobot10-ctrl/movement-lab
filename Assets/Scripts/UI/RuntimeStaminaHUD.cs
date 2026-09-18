using UnityEngine;
using UnityEngine.UI;
public class RuntimeStaminaHUD : MonoBehaviour {
 public StaminaSystem stamina;
 private Image[] fullLayers;
 private Image[] halfLayers;
 void Start(){if(stamina==null)stamina=FindAnyObjectByType<StaminaSystem>();Build();}
 void Build(){
  var canvasGO=new GameObject("Stamina HUD");var canvas=canvasGO.AddComponent<Canvas>();canvas.renderMode=RenderMode.ScreenSpaceOverlay;
  var scaler=canvasGO.AddComponent<CanvasScaler>();scaler.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;scaler.referenceResolution=new Vector2(1920,1080);canvasGO.AddComponent<GraphicRaycaster>();
  var panelGO=new GameObject("Stamina Bars");panelGO.transform.SetParent(canvasGO.transform,false);var panel=panelGO.AddComponent<RectTransform>();panel.anchorMin=panel.anchorMax=new Vector2(.5f,.08f);panel.pivot=new Vector2(.5f,.5f);panel.sizeDelta=new Vector2(330,34);
  var layout=panelGO.AddComponent<HorizontalLayoutGroup>();layout.spacing=10;layout.childForceExpandWidth=true;layout.childForceExpandHeight=true;
  int count=stamina!=null?stamina.settings.staminaBars:3;fullLayers=new Image[count];halfLayers=new Image[count];
  for(int i=0;i<count;i++){
   var slot=new GameObject("Stamina "+(i+1));slot.transform.SetParent(panelGO.transform,false);var bg=slot.AddComponent<Image>();bg.color=new Color(.04f,.04f,.04f,.82f);
   var halfGO=new GameObject("Half");halfGO.transform.SetParent(slot.transform,false);var halfRT=halfGO.AddComponent<RectTransform>();halfRT.anchorMin=Vector2.zero;halfRT.anchorMax=new Vector2(.5f,1);halfRT.offsetMin=halfRT.offsetMax=Vector2.zero;var half=halfGO.AddComponent<Image>();half.color=new Color(.15f,.8f,1f,.9f);halfLayers[i]=half;
   var fullGO=new GameObject("Full");fullGO.transform.SetParent(slot.transform,false);var fullRT=fullGO.AddComponent<RectTransform>();fullRT.anchorMin=Vector2.zero;fullRT.anchorMax=Vector2.one;fullRT.offsetMin=fullRT.offsetMax=Vector2.zero;var full=fullGO.AddComponent<Image>();full.color=new Color(.15f,.8f,1f,1f);full.type=Image.Type.Filled;full.fillMethod=Image.FillMethod.Horizontal;full.fillOrigin=0;fullLayers[i]=full;
  }
 }
 void Update(){
  if(stamina==null||fullLayers==null)return;
  float current=Mathf.Clamp(stamina.CurrentBars,0f,fullLayers.Length);
  for(int i=0;i<fullLayers.Length;i++){
   float amount=Mathf.Clamp01(current-i);
   fullLayers[i].fillAmount=amount;
   fullLayers[i].enabled=amount>.001f;
   // Background is always visible. A partial bar therefore has an obvious dark missing section.
   halfLayers[i].enabled=false;
  }
 }
}