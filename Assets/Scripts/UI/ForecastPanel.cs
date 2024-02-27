using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ForecastPanel : MonoBehaviour 
{

  const string ShowKey = "Show";
  const string HideKey = "Hide";
  [SerializeField] GameObject forecastPanel;
//   [SerializeField] Image arrow;
  [SerializeField] TMP_Text actionName;
  [SerializeField] TMP_Text abilityEffect;         //used to be damage, indicates ability's effect in the forecast
  [SerializeField] TMP_Text abilityHitrate;
  [SerializeField] TMP_Text subAbilityEffect, subAbilityHitrate;

//   Tweener transition;
  void Start ()
  {
    forecastPanel.SetActive(false);
  }

  //old and unused
  // public void SetStats (Unit actor, GameObject target, GameObject ability, float chance, int amount, float subChance, int subAmount)
  // {      
  //     actionName.text = ability.name;

  //     string effect = ability.GetComponent<Ability>().primaryEffect.GetComponent<BaseAbilityEffect>().abilityEffectName;
  //     string subEffect = ability.GetComponent<Ability>().primaryEffect.GetComponent<BaseAbilityEffect>().abilityEffectName;
      
  //     abilityEffect.text = string.Format("{0} : {1} pts", effect, amount);
  //     abilityHitrate.text = string.Format("HIT : {0} %", (int)chance);

  //     subAbilityEffect.text = string.Format("{0} : {1} pts", subEffect, amount);
  //     subAbilityHitrate.text = string.Format("HIT : {0} %", (int)subChance);
  // }
  public void SetStats (Unit actor, GameObject target, GameObject ability, float chance, int amount)
  {      
      actionName.text = ability.name;

      string effect = ability.GetComponent<Ability>().primaryEffect.GetComponent<BaseAbilityEffect>().abilityEffectName;
      
      abilityEffect.text = string.Format("{0} : {1} pts", effect, amount);
      abilityHitrate.text = string.Format("HIT : {0} %", (int)(chance));

      // Debug.Log(chance);
      // Debug.Log(hitrate.text);
  }
  public void Show ()
  {
    forecastPanel.SetActive(true);
    SetPanelPos(ShowKey);
  }
  public void Hide ()
  {
    forecastPanel.SetActive(false);
  }
  void SetPanelPos (string pos)
  {
  }
}