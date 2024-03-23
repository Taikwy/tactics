using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class CommandPanelController : MonoBehaviour
{

    const string ShowKey = "Show";
    const string HideKey = "Hide";
    const string EntryPoolKey = "AbilityMenuPanel.Entry";
    const int MenuCount = 4;

    [SerializeField] GameObject menuPanel;
    List<AbilityMenuEntry> menuEntries = new List<AbilityMenuEntry>(MenuCount);
    [Header("COMMAND ENTRY STUFF")]
    [SerializeField] GameObject entryPrefab;
    public Sprite moveIcon, actionIcon, focusIcon, passIcon;
    [Header("ABILITY STUFF")]
    public GameObject abilityInfoPanelPrefab;
    public Sprite buffIcon, damageIcon, debuffIcon, healIcon;
    public List<Action> menuFunctions = new List<Action>(MenuCount);
    public int currentSelection { get; private set; }
    GameObject abilityInfoPanel = null;

    void Awake (){
        GameObjectPoolController.AddEntry(EntryPoolKey, entryPrefab, MenuCount, int.MaxValue);
    }

    // Start is called before the first frame update
    void Start ()    {
        menuPanel.SetActive(false);
    }

    AbilityMenuEntry Dequeue (){
        Poolable p = GameObjectPoolController.Dequeue(EntryPoolKey);
        AbilityMenuEntry entry = p.GetComponent<AbilityMenuEntry>();

        // p.GetComponent<ReactiveButton>().Reset();

        entry.Reset();
        entry.transform.SetParent(menuPanel.transform, false);
        entry.transform.localScale = Vector3.one;
        entry.gameObject.SetActive(true);

        return entry;
    }
    
    void Enqueue (AbilityMenuEntry entry){
        Poolable p = entry.GetComponent<Poolable>();
        GameObjectPoolController.Enqueue(p);
    }

    void Clear (){
        for (int i = menuEntries.Count - 1; i >= 0; --i){
            // Debug.Log("looping thru");
            menuEntries[i].gameObject.GetComponent<Button>().onClick.RemoveAllListeners();
            // Destroy(menuEntries[i].gameObject);
            Enqueue(menuEntries[i]);
        }
        menuEntries.Clear();
    }

    //creates ability entries with names from the list of strings passed in
    //returns list of menu entries
    //its called show, but it essentially creates it from scratch
    public List<AbilityMenuEntry> Show (List<string> options, List<UnityEngine.Events.UnityAction> functions){
        menuPanel.SetActive(true);
        Clear ();
        print("showing commands");
        for (int i = 0; i < options.Count; ++i){
            AbilityMenuEntry entry = Dequeue();
            entry.Setup();
            // print(options[i]);
            switch(options[i]){
                default:
                    Debug.LogError("invalid command menu entry name");
                    entry.icon.color = Color.red;
                    break;
                case "MOVE":
                    entry.icon.sprite = moveIcon;
                    break;
                case "ACTION":
                    entry.icon.sprite = actionIcon;
                    break;
                case "FOCUS":
                    entry.icon.sprite = focusIcon;
                    break;
                case "PASS":
                    entry.icon.sprite = passIcon;
                    break;
            }
            entry.button.onClick.AddListener(functions[i]);
            menuEntries.Add(entry);
        }
        return menuEntries;
    }
    //used for ability menu, so there's more stuff such as highltigh functions
    public List<AbilityMenuEntry> Show (List<GameObject> abilities, List<string> names, List<bool> performable, List<UnityEngine.Events.UnityAction> functions){
        menuPanel.SetActive(true);
        Clear ();
        // Debug.Log(abilities.Count);
        for (int i = 0; i < names.Count; ++i){

            AbilityMenuEntry entry = Dequeue();
            entry.abilityEntry = abilities[i];
            entry.gameObject.GetComponent<Button>().onClick.AddListener(functions[i]);
            entry.highlightFunc = delegate { CreateAbilityInfoPanel(entry); };
            entry.unhighlightFunc = delegate { DestroyAbilityInfoPanel(); };

            switch(abilities[i].GetComponent<Ability>().overlayColor){
                default:
                    Debug.LogError("invalid overlay color for ability");
                    entry.icon.color = Color.red;
                    break;
                case Board.OverlayColor.BUFF:
                    entry.icon.sprite = buffIcon;
                    break;
                case Board.OverlayColor.ATTACK:
                    entry.icon.sprite = damageIcon;
                    break;
                case Board.OverlayColor.DEBUFF:
                    entry.icon.sprite = debuffIcon;
                    break;
                case Board.OverlayColor.HEAL:
                    entry.icon.sprite = healIcon;
                    break;
            }

            entry.button.interactable = performable[i];
            menuEntries.Add(entry);

        }
        return menuEntries;
    }
    
    public void Hide (){
        // Debug.Log("hiding panel controller");
        Clear();
        menuPanel.SetActive(false);
    }
    public void SetLocked (int index, bool value){
        if (index < 0 || index >= menuEntries.Count)
            return;
        
        menuEntries[index].button.interactable = !value;
    }
    public void CreateAbilityInfoPanel(AbilityMenuEntry entry){
        GameObject label = entry.gameObject;
        GameObject ability = entry.abilityEntry;
        // Debug.Log("creating ability info panel");
        Destroy(abilityInfoPanel);
        Vector2 pos = label.transform.position;
        // print("height " + entry.GetComponent<RectTransform>().rect.height + " | width " +  entry.GetComponent<RectTransform>().rect.width);
        pos += new Vector2(entry.GetComponent<RectTransform>().rect.width, 0);
        // print(entry.transform.position + " | placing at " + pos);
        abilityInfoPanel = Instantiate(abilityInfoPanelPrefab, pos, Quaternion.identity, label.transform);
        abilityInfoPanel.transform.localPosition = new Vector2(entry.GetComponent<RectTransform>().rect.width, entry.GetComponent<RectTransform>().rect.height);
        abilityInfoPanel.GetComponent<AbilityInfoPanel>().Display(ability);
        abilityInfoPanel.GetComponent<AbilityInfoPanel>().ShowPanel();
    }
    public void CreateAbilityInfoPanel(GameObject label, GameObject ability){
        // Debug.Log("creating ability info panel");
        Destroy(abilityInfoPanel);
        Vector2 pos = label.transform.position;
        pos += new Vector2(200, 16);
        abilityInfoPanel = Instantiate(abilityInfoPanelPrefab, pos, Quaternion.identity, label.transform);
        abilityInfoPanel.GetComponent<AbilityInfoPanel>().Display(ability);
        abilityInfoPanel.GetComponent<AbilityInfoPanel>().ShowPanel();
    }
    public void DestroyAbilityInfoPanel(){
        if(abilityInfoPanel && abilityInfoPanel.GetComponent<AbilityInfoPanel>())
            abilityInfoPanel.GetComponent<AbilityInfoPanel>().HidePanel();
        Destroy(abilityInfoPanel);
    }

}
