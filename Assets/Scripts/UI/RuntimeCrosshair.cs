using UnityEngine;using UnityEngine.UI;
public class RuntimeCrosshair:MonoBehaviour{
 void Start(){var cgo=new GameObject("Crosshair HUD");var c=cgo.AddComponent<Canvas>();c.renderMode=RenderMode.ScreenSpaceOverlay;var sc=cgo.AddComponent<CanvasScaler>();sc.uiScaleMode=CanvasScaler.ScaleMode.ScaleWithScreenSize;sc.referenceResolution=new Vector2(1920,1080);cgo.AddComponent<GraphicRaycaster>();
  Make(cgo.transform,new Vector2(0,10),new Vector2(3,12));Make(cgo.transform,new Vector2(0,-10),new Vector2(3,12));Make(cgo.transform,new Vector2(-10,0),new Vector2(12,3));Make(cgo.transform,new Vector2(10,0),new Vector2(12,3));Make(cgo.transform,Vector2.zero,new Vector2(3,3));
 }
 void Make(Transform p,Vector2 pos,Vector2 size){var g=new GameObject("Reticle");g.transform.SetParent(p,false);var r=g.AddComponent<RectTransform>();r.anchorMin=r.anchorMax=new Vector2(.5f,.5f);r.anchoredPosition=pos;r.sizeDelta=size;var i=g.AddComponent<Image>();i.color=new Color(1,1,1,.9f);i.raycastTarget=false;}
}