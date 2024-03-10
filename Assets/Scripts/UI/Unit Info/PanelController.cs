using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PanelController : MonoBehaviour 
{
    const string ShowKey = "Show";
    const string HideKey = "Hide";
    // [SerializeField] BasePanel basePanel;
    [SerializeField] UnitPanel primaryPanel, secondaryPanel;              //selected and targeted unit
    [SerializeField] StatusPanel primaryStatusPanel;              //selected and targeted unit
    [SerializeField] AbilityInfoPanel abilityInfoPanel;              //selected and targeted unit
    [SerializeField] DetailsPanel detailsPanel;              //selected and targeted unit
    [SerializeField] AbilityDisplayPanel abilityDisplayPanel;              //selected and targeted unit
    // [SerializeField] AbilityMenu abilityMenu;
    [HideInInspector] public bool showingPrimary, showingPrimaryStatus, showingSecondary, showingAbilityInfo, showingDetails, showingAbilityDisplay = false;
    
    void Start (){
        HidePrimary();
        HideStatus();
        HideSecondary(); 
        HideAbilityInfo();
        HideDetails();
        HideAbilityDisplay();
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

    public void ShowAbilityDisplay (GameObject unit){
        showingAbilityDisplay = true;
        abilityDisplayPanel.Display(unit);
        abilityDisplayPanel.ShowPanel();
    }    public void HideAbilityDisplay (){
        showingAbilityDisplay = false;
        abilityDisplayPanel.HidePanel();
    }
}