using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour 
{
    const string ShowKey = "Show";
    const string HideKey = "Hide";
    // [SerializeField] BasePanel basePanel;
    [SerializeField] UnitPanel primaryPanel, secondaryPanel;              //selected and targeted unit
    [SerializeField] StatusPanel primaryStatusPanel;    
    [SerializeField] AbilityInfoPanel abilityInfoPanel; 
    [SerializeField] DetailsPanel detailsPanel;            
    [SerializeField] AbilityDisplayPanel abilityDisplayPanel;         
    [SerializeField] MouseControlsPanel mouseControlsPanel;         
    // [SerializeField] AbilityMenu abilityMenu;
    [HideInInspector] public bool showingPrimary, showingPrimaryStatus, showingSecondary, showingAbilityInfo, showingDetails, showingAbilityDisplay, showingMouseControls = false;
    
    void Start (){
        HidePrimary();
        HideStatus();
        HideSecondary(); 
        HideAbilityInfo();
        HideDetails();
        HideAbilityDisplay();
        HideMouseControls();
    }

    public void ShowPrimary (GameObject unit){
        HideStatus();
        showingPrimary = true;
        primaryPanel.Display(unit);
        primaryPanel.ShowPanel();
    }    public void HidePrimary (){
        showingPrimary = false;
        primaryPanel.HidePanel();
    }

    public void ShowStatus (GameObject unit){
        HidePrimary();
        showingPrimaryStatus = true;
        primaryStatusPanel.Display(unit);
        primaryStatusPanel.ShowStatus();
    }    public void HideStatus (){
        showingPrimaryStatus = false;
        primaryStatusPanel.HideStatus();
    }
    
    public void ShowSecondary (GameObject unit){
        showingSecondary = true;
        secondaryPanel.Display(unit);
        secondaryPanel.ShowPanel();
    }    public void HideSecondary (){
        showingSecondary = false;
        secondaryPanel.HidePanel();
    }

    public void ShowAbilityInfo (GameObject ability){
        showingAbilityInfo = true;
        abilityInfoPanel.Display(ability);
        abilityInfoPanel.ShowPanel();
    }    public void HideAbilityInfo (){
        showingAbilityInfo = false;
        abilityInfoPanel.HidePanel();
    }
    
    public void ShowDetails (GameObject ability){
        showingDetails = true;
        detailsPanel.Display(ability);
        detailsPanel.ShowPanel();
    }    public void HideDetails (){
        showingDetails = false;
        detailsPanel.HidePanel();
    }

    public void ShowAbilityDisplay (GameObject unit, bool focus = false){
        showingAbilityDisplay = true;
        abilityDisplayPanel.Display(unit, focus);
        abilityDisplayPanel.ShowPanel();
    }    public void HideAbilityDisplay (){
        showingAbilityDisplay = false;
        abilityDisplayPanel.HidePanel();
    }

    
    public void ShowMouseControls (string LMB = "SELECT", string RMB = "BACK"){
        showingMouseControls = true;
        mouseControlsPanel.Display(LMB, RMB);
        mouseControlsPanel.ShowPanel();
    }    public void HideMouseControls (){
        showingMouseControls = false;
        mouseControlsPanel.HidePanel();
    }
}