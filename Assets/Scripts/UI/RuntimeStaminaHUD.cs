using UnityEngine;
using UnityEngine.UI;
public class RuntimeStaminaHUD : MonoBehaviour {
 public StaminaSystem stamina; private Image[] fills; private Image[] backgrounds;
 void Start(){if(stamina==null)stamina=FindAnyObjectByType<StaminaSystem>();Build();}
 void Build(){
  var canvasGO=new GameObject("Stamina HUD");var canvas=canvasGO.AddComponent<Canvas>();canvas.renderMode=RenderMode.ScreenSpaceOverlay;
  var scaler=canvasGO.AddComponent<CanvasScaler>();scaler.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;scaler.referenceResolution=new Vector2(1920,1080);canvasGO.AddComponent<GraphicRaycaster>();
  var panelGO=new GameObject("Stamina Bars");panelGO.transform.SetParent(canvasGO.transform,false);var panel=panelGO.AddComponent<RectTransform>();panel.anchorMin=panel.anchorMax=new Vector2(.5f,.08f);panel.pivot=new Vector2(.5f,.5f);panel.sizeDelta=new Vector2(330,34);
  var layout=panelGO.AddComponent<HorizontalLayoutGroup>();layout.spacing=10;layout.childForceExpandWidth=true;layout.childForceExpandHeight=true;
  int count=stamina!=null?stamina.settings.staminaBars:3;fills=new Image[count];backgrounds=new Image[count];
  for(int i=0;i<count;i++){var slot=new GameObject("Stamina "+(i+1));slot.transform.SetParent(panelGO.transform,false);var bg=slot.AddComponent<Image>();bg.color=new Color(1,1,1,.18f);backgrounds[i]=bg;var fillGO=new GameObject("Fill");fillGO.transform.SetParent(slot.transform,false);var rt=fillGO.AddComponent<RectTransform>();rt.anchorMin=Vector2.zero;rt.anchorMax=Vector2.one;rt.offsetMin=rt.offsetMax=Vector2.zero;var image=fillGO.AddComponent<Image>();image.color=new Color(1,1,1,.9f);image.type=Image.Type.Filled;image.fillMethod=Image.FillMethod.Horizontal;image.fillOrigin=0;image.fillAmount=1;fills[i]=image;}
 }
 void Update(){if(stamina==null||fills==null)return;for(int i=0;i<fills.Length;i++){float fill=Mathf.Clamp01(stamina.CurrentBars-i);fills[i].fillAmount=fill;fills[i].enabled=fill>.001f;}}
}
