using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour 
{
    #region Const
    const string ShowKey = "Show";
    const string HideKey = "Hide";
    #endregion
    #region Fields
    [SerializeField] BasePanel basePanel;
    // [SerializeField] AbilityMenu abilityMenu;
    
    #endregion
    #region MonoBehaviour
    void Start (){
        // if (primaryPanel.panel.transform.position == null)
        //     primaryPanel.panel.transform.position = 
        // if (secondaryPanel.panel.transform.position == null)
        //     secondaryPanel.panel.SetPosition(HideKey, false);
    }
    #endregion
    #region Public
    public void ShowBase (GameObject obj){
        basePanel.Display(obj);
        basePanel.ShowPanel();
        // MovePanel(primaryPanel, ShowKey);
    }
    public void HideBase (){
        basePanel.HidePanel();
        // MovePanel(primaryPanel, HideKey);
    }

    #endregion
    #region Private
    // void MovePanel (StatPanel obj, string pos) {
    //     Panel.Position target = obj.panel[pos];
    //     if (obj.panel.CurrentPosition != target)
    //     {
    //     if (t != null && t.easingControl != null)
    //         t.easingControl.Stop();
    //     t = obj.panel.SetPosition(pos, true);
    //     t.easingControl.duration = 0.5f;
    //     t.easingControl.equation = EasingEquations.EaseOutQuad;
    //     }
    // }
    #endregion
}