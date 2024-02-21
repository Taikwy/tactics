using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class TurnIndicator : MonoBehaviour
{
    public Image icon;
    public Image background;
    public Color defaultBGColor;
    public TMP_Text counter;
    [HideInInspector] public Unit unitScript;
    [HideInInspector] public Stats statsScript;

}
