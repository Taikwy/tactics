using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using System;

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

  public void SetStats (Unit actor, GameObject target, GameObject ability, float chance, int amount)
  {      
      actionName.text = ability.name;

      string effect = ability.GetComponent<Ability>().primaryEffect.GetComponent<BaseAbilityEffect>().abilityEffectName;
      
      // abilityEffect.text = string.Format("{0} : {1} pts", effect, Mathf.Abs(amount));
      // abilityHitrate.text = string.Format("HITRATE : {0} %", (int)(chance));
      Type type = ability.GetComponent<Ability>().primaryEffect.GetComponent<BaseAbilityEffect>().GetType();
      if(type == typeof(DamageAbilityEffect)){
        abilityEffect.text = string.Format("HEALTH : -{0} pts", Mathf.Abs(amount));
      abilityHitrate.text = string.Format("HITRATE : {0} %", (int)(chance));
      }
      else if(type == typeof(HealAbilityEffect)){
        abilityEffect.text = string.Format("HEALTH : +{0} pts",  Mathf.Abs(amount));
      abilityHitrate.text = string.Format("HITRATE : {0} %", (int)(chance));
      }
      else if(type == typeof(InflictAbilityEffect)){
        abilityEffect.text = string.Format("INFLICT : {0} ", effect);
      abilityHitrate.text = string.Format("HITRATE : {0} %", (int)(chance));
      }
      else if(type == typeof(PurifyAbilityEffect)){
        abilityEffect.text = string.Format("PURIFYING");
      abilityHitrate.text = string.Format("HITRATE : {0} %", (int)(chance));
      }
      

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