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
    [SerializeField] UnitPanel primaryPanel;                //selected unit
    [SerializeField] UnitPanel secondaryPanel;              //targeted unit
    // [SerializeField] AbilityMenu abilityMenu;
    
    #endregion
    #region MonoBehaviour
    void Start (){
        HidePrimary();
        HideSecondary();
    }
    #endregion
    #region Public
    public void ShowPrimary (GameObject unit){
        // basePanel.Display(obj);
        // basePanel.ShowPanel();
        primaryPanel.Display(unit);
        primaryPanel.ShowPanel();
    }
    public void HidePrimary (){
        // basePanel.HidePanel();
        primaryPanel.HidePanel();
    }
    
    public void ShowSecondary (GameObject unit){
        secondaryPanel.Display(unit);
        secondaryPanel.ShowPanel();
    }
    public void HideSecondary (){
        secondaryPanel.HidePanel();
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