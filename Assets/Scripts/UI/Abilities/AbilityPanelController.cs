using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class AbilityPanelController : MonoBehaviour
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

        for (int i = 0; i < options.Count; ++i){
            AbilityMenuEntry entry = Dequeue();
            // entry.Title = options[i];
            // entry.gameObject.GetComponent<RectTransform>().wid
            entry.button.onClick.AddListener(functions[i]);
            // entry.gameObject.GetComponent<Button>().onClick.AddListener(delegate{ButtonClicked(entry.Title);});
            // entry.button.interactable = performable[i];
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
            // entry.Title = names[i];
            entry.abilityEntry = abilities[i];
            entry.gameObject.GetComponent<Button>().onClick.AddListener(functions[i]);
            // Debug.Log("index " + i);
            // entry.highlightFunc = delegate { CreateAbilityInfoPanel(entry.gameObject, entry.entry); };
            entry.highlightFunc = delegate { CreateAbilityInfoPanel(entry); };
            // Debug.Log("null? " +  abilities[i] + " gameobject " + entry.gameObject + " highlight func " + entry.highlightFunc);
            // entry.highlightFunc = delegate { CreateAbilityInfoPanel(entry.gameObject, abilities[i]); };
            entry.unhighlightFunc = delegate { DestroyAbilityInfoPanel(); };

            // entry.highlightFunc = highglightFuncs[i];
            // entry.unhighlightFunc = unhighlightFuncs[i];
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
        // print("hello?");
        print("height " + entry.GetComponent<RectTransform>().rect.height + " | width " +  entry.GetComponent<RectTransform>().rect.width);
        // pos += new Vector2(200, 16);
        pos += new Vector2(entry.GetComponent<RectTransform>().rect.width, 0);
        print(entry.transform.position + " | placing at " + pos);
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
