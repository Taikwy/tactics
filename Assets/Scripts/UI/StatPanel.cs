using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class StatPanel : MonoBehaviour 
{
    // public Panel panel;
    public GameObject panel;
    public Sprite allyBackground;
    public Sprite enemyBackground;
    public Image background;
    public Image avatar;
    public TMP_Text nameLabel;
    public TMP_Text lvLabel;
    public TMP_Text xpLabel;
    public TMP_Text hpLabel;
    public TMP_Text atLabel;
    public TMP_Text dfLabel;
    public TMP_Text skLabel;
    public TMP_Text luLabel;
    public TMP_Text acLAbel;
    public TMP_Text spLabel;
    public TMP_Text mvLabel;
    public void Display (GameObject obj)
    {
        // Temp until I add a component to determine unit alliances
        background.sprite = UnityEngine.Random.value > 0.5f? enemyBackground : allyBackground;
        // avatar.sprite = null; Need a component which provides this data
        nameLabel.text = obj.GetComponent<Unit>().name;
        Stats stats = obj.GetComponent<Stats>();
        if (stats) {
            lvLabel.text = string.Format( "LV. {0}", stats[StatTypes.LV]);
            // xpLabel.text = string.Format( "XP. {0} / {1}", stats[StatTypes.XP], obj.GetComponent<Unit>().xpData.experiencePerLevel[stats[StatTypes.LV]]);
            xpLabel.text = string.Format( "XP. {0} / {1}", stats.GetCurrentXP(), obj.GetComponent<UnitLevel>().xpData.experiencePerLevel[stats[StatTypes.LV]]);
            hpLabel.text = string.Format( "HP. {0} / {1}", stats[StatTypes.HP], stats[StatTypes.MHP] );
            atLabel.text = string.Format( "AT. {0}", stats[StatTypes.AT]);
            dfLabel.text = string.Format( "DF. {0}", stats[StatTypes.DF]);
            skLabel.text = string.Format( "SK. {0}", stats[StatTypes.SK]);
            luLabel.text = string.Format( "LU. {0}", stats[StatTypes.LU]);
            acLAbel.text = string.Format( "AC. {0}", stats[StatTypes.AC]);
            spLabel.text = string.Format( "SP. {0}", stats[StatTypes.SP]);
            mvLabel.text = string.Format( "MV. {0}", stats[StatTypes.MV]);
        }
    }

    public void ShowPanel(){
        panel.SetActive(true);
    }
    public void HidePanel(){
        panel.SetActive(false);
    }
}