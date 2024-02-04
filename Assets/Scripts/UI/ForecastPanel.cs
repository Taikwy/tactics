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
  [SerializeField] TMP_Text actorName;
  [SerializeField] TMP_Text targetName;
  public Image actorPortrait, targetPortrait;
  [SerializeField] TMP_Text actionName;
  [SerializeField] TMP_Text abilityEffect;         //used to be damage, indicates ability's effect in the forecast
  [SerializeField] TMP_Text abilityHitrate;

//   Tweener transition;
  void Start ()
  {
    forecastPanel.SetActive(false);
  }
  public void SetStats (Unit actor, GameObject target, GameObject ability, float chance, int amount)
  {
      actorPortrait.sprite = actor.portrait;
      targetPortrait.sprite = target.GetComponent<Unit>().portrait;
      // arrow.fillAmount = chance / 100f;

      actorName.text = actor.name;
      if(target.GetComponent<Unit>() != null)
        targetName.text = target.name;
      
      actionName.text = ability.name;

      string effect= "effect";
      
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