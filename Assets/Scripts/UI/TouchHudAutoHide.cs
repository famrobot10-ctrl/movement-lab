using UnityEngine;
#if ENABLE_INPUT_SYSTEM
using UnityEngine.InputSystem;
#endif
public class TouchHudAutoHide:MonoBehaviour{public ControlProfileManager profiles;public CanvasGroup touchHud;void Update(){
#if ENABLE_INPUT_SYSTEM
if(!profiles||!touchHud)return;bool hide=profiles.profile.autoHideTouchWithGamepad&&Gamepad.current!=null;touchHud.alpha=hide?0:profiles.profile.touch.opacity;touchHud.interactable=!hide;touchHud.blocksRaycasts=!hide;
#endif
}}
