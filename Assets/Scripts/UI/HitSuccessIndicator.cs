using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class HitSuccessIndicator : MonoBehaviour 
{
  const string ShowKey = "Show";
  const string HideKey = "Hide";
  [SerializeField] Canvas canvas;
//   [SerializeField] Panel panel;
  [SerializeField] GameObject panel;
  [SerializeField] Image arrow;
  [SerializeField] TMP_Text label;
//   Tweener transition;
  void Start ()
  {
    // panel.SetPosition(HideKey, false);
    canvas.gameObject.SetActive(false);
  }
  public void SetStats (float chance, int amount)
  {
    arrow.fillAmount = (chance / 100f);
    label.text = string.Format("{0}% {1}pt(s)", chance, amount);
  }
  public void Show ()
  {
    canvas.gameObject.SetActive(true);
    SetPanelPos(ShowKey);
  }
  public void Hide ()
  {
    canvas.gameObject.SetActive(false);
    // SetPanelPos(HideKey);
    // transition.easingControl.completedEvent += delegate(object sender, System.EventArgs e) {
    //   canvas.gameObject.SetActive(false);
    // };
  }
  void SetPanelPos (string pos)
  {
    // if (transition != null && transition.easingControl.IsPlaying)
    //   transition.easingControl.Stop();
    // transition = panel.SetPosition(pos, true);
    // transition.easingControl.duration = 0.5f;
    // transition.easingControl.equation = EasingEquations.EaseInOutQuad;
  }
}