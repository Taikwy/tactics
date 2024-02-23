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
    public List<AbilityMenuEntry> menuEntries = new List<AbilityMenuEntry>(MenuCount);
    [SerializeField] GameObject entryPrefab;
    public GameObject abilityInfoPanelPrefab;
    public List<Action> menuFunctions = new List<Action>(MenuCount);
    public int currentSelection { get; private set; }
    GameObject abilityInfoPanel;

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
            entry.Title = options[i];
            // entry.gameObject.GetComponent<RectTransform>().wid
            entry.button.onClick.AddListener(functions[i]);
            // entry.gameObject.GetComponent<Button>().onClick.AddListener(delegate{ButtonClicked(entry.Title);});
            // entry.button.interactable = performable[i];
            menuEntries.Add(entry);
        }
        return menuEntries;
    }
    //used for ability menu, so there's more stuff such as highltigh functions
    public List<AbilityMenuEntry> Show (List<string> options, List<bool> performable, List<UnityEngine.Events.UnityAction> functions, List<UnityEngine.Events.UnityAction> highglightFuncs, List<UnityEngine.Events.UnityAction> unhighlightFuncs){
        menuPanel.SetActive(true);
        Clear ();


        // GameObject ability = catalog.GetCategory(i);
        // //checks whether the ability should be locked depending on whether the unit has enough skill points
        // performable.Add(ability.GetComponent<Ability>().CanPerform());
        // //new, checks the abilities' cost to add to the name
        // AbilitySkillCost skillCost = ability.GetComponent<AbilitySkillCost>();
        // AbilityBurstCost burstCost = ability.GetComponent<AbilityBurstCost>();
        // if (skillCost)
        //     menuOptions.Add(string.Format("{0}: {1} skpts", ability.name, skillCost.cost));
        // else if (burstCost)
        //     menuOptions.Add(string.Format("{0}: {1} bpts", ability.name, burstCost.cost));
        // else
        //     menuOptions.Add( ability.name );




        for (int i = 0; i < options.Count; ++i){
            AbilityMenuEntry entry = Dequeue();
            entry.Title = options[i];
            entry.gameObject.GetComponent<Button>().onClick.AddListener(functions[i]);
            entry.highlightFunc = highglightFuncs[i];
            entry.unhighlightFunc = unhighlightFuncs[i];
            entry.button.interactable = performable[i];
            menuEntries.Add(entry);

            entry.highlightFunc = delegate { CreateAbilityInfoPanel(entry.gameObject, null); };
            entry.unhighlightFunc = delegate { DestroyAbilityInfoPanel(); };

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
    void CreateAbilityInfoPanel(GameObject label, GameObject ability){
        Debug.Log("creating ability info panel");
        Destroy(abilityInfoPanel);
        Vector2 pos = label.transform.position;
        pos += new Vector2(60, 0);
        abilityInfoPanel = Instantiate(abilityInfoPanelPrefab, pos, Quaternion.identity, label.transform);
        // abilityInfoPanel.GetComponent<StatusInfoPanel>().Setup(ability);
        // abilityInfoPanel.GetComponent<StatusInfoPanel>().ShowPanel();
    }
    void DestroyAbilityInfoPanel(){
        if(abilityInfoPanel && abilityInfoPanel.GetComponent<StatusInfoPanel>())
            abilityInfoPanel.GetComponent<StatusInfoPanel>().HidePanel();
        Destroy(abilityInfoPanel);
    }

}
