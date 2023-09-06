using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BasePanelController : MonoBehaviour 
{
    #region Const
    const string ShowKey = "Show";
    const string HideKey = "Hide";
    #endregion
    #region Fields
    [SerializeField] StatPanel primaryPanel;
    [SerializeField] StatPanel secondaryPanel;
    
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
    public void ShowPrimary (GameObject obj){
        primaryPanel.Display(obj);
        primaryPanel.ShowPanel();
        // MovePanel(primaryPanel, ShowKey);
    }
    public void HidePrimary (){
        primaryPanel.HidePanel();
        // MovePanel(primaryPanel, HideKey);
    }
    public void ShowSecondary (GameObject obj){
        secondaryPanel.Display(obj);
        secondaryPanel.ShowPanel();
        // MovePanel(secondaryPanel, ShowKey);
    }
    public void HideSecondary (){
        secondaryPanel.HidePanel();
        // MovePanel(secondaryPanel, HideKey);
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