using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UnitPanel : MonoBehaviour 
{
    public GameObject panelBG;
    public Image background;
    public Image portrait;
    public Sprite unitBackground;
    public TMP_Text nameLabel;
    public TMP_Text lvLabel;
    public TMP_Text xpLabel;
    public TMP_Text hpLabel;
    public TMP_Text bpLabel;
    public TMP_Text skpLabel;
    public TMP_Text atLabel;
    public TMP_Text dfLabel;
    public TMP_Text spLabel;
    public TMP_Text cpLabel;
    public TMP_Text cdLabel;
    
    public void Display (GameObject obj)
    {
        // Temp until I add a component to determine unit alliances
        // background.sprite = Random.value > 0.5f? enemyBackground : unitBackground;
        
        // portrait.sprite = null; Need a component which provides this data
        nameLabel.text = obj.GetComponent<Unit>().name;
        portrait.sprite = obj.GetComponent<Unit>().portrait;
        Stats stats = obj.GetComponent<Stats>();
        if (stats) {
            lvLabel.text = string.Format( "LV. {0}", stats[StatTypes.LV]);
            xpLabel.text = string.Format( "XP. {0} / {1}", stats.GetCurrentXP(), obj.GetComponent<UnitLevel>().xpData.experiencePerLevel[stats[StatTypes.LV]]);
            hpLabel.text = string.Format( "HEALTH {0} / {1}", stats[StatTypes.HP], stats[StatTypes.MHP] );
            bpLabel.text = string.Format( "BURST {0} / {1}", stats[StatTypes.BP], stats[StatTypes.MBP] );
            skpLabel.text = string.Format( "SKILL PTS {0} / {1}", stats[StatTypes.SKP], stats[StatTypes.MSKP] );

            atLabel.text = string.Format( "ATTACK {0}", stats[StatTypes.AT]);
            dfLabel.text = string.Format( "DEFENSE {0}", stats[StatTypes.DF]);
            spLabel.text = string.Format( "SPEED {0}", stats[StatTypes.SP]);
            cpLabel.text = string.Format( "CRIT% {0}", stats[StatTypes.CP]);
            cdLabel.text = string.Format( "CRITDMG {0}", stats[StatTypes.CD]);
        }
    }

    public void ShowPanel(){
        panelBG.SetActive(true);
    }
    public void HidePanel(){
        panelBG.SetActive(false);
    }
}