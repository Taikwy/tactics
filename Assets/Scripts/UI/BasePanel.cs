using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class BasePanel : MonoBehaviour 
{
    // public Panel panel;
    public GameObject panelBG;
    public Sprite allyBackground;
    public Sprite enemyBackground;
    public Image background;
    public Image portrait;
    public TMP_Text nameLabel;
    public TMP_Text lvLabel;
    public TMP_Text xpLabel;
    public TMP_Text hpLabel;
    public TMP_Text enLabel;
    
    public void Display (GameObject unit)
    {
        // Temp until I add a component to determine unit alliances
        // background.sprite = Random.value > 0.5f? enemyBackground : allyBackground;
        
        // portrait.sprite = null; Need a component which provides this data
        nameLabel.text = unit.GetComponent<Unit>().name;
        portrait.sprite = unit.GetComponent<Unit>().portrait;
        portrait.color = unit.GetComponent<Unit>().portraitColor;
        Stats stats = unit.GetComponent<Stats>();
        if (stats) {
            lvLabel.text = string.Format( "LV. {0}", stats[StatTypes.LV]);
            // xpLabel.text = string.Format( "XP. {0}", stats[StatTypes.XP]);
            xpLabel.text = string.Format( "XP. {0} / {1}", stats.GetCurrentXP(), unit.GetComponent<UnitLevel>().xpData.experiencePerLevel[stats[StatTypes.LV]]);
            hpLabel.text = string.Format( "HEALTH {0} / {1}", stats[StatTypes.HP], stats[StatTypes.MHP] );
            enLabel.text = string.Format( "ENERGY {0} / {1}", stats[StatTypes.EN], stats[StatTypes.MEN] );
        }
    }

    public void ShowPanel(){
        panelBG.SetActive(true);
    }
    public void HidePanel(){
        panelBG.SetActive(false);
    }
}