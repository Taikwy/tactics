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

    [SerializeField] GameObject entryPrefab;
    [SerializeField] TMP_Text titleLabel;
    [SerializeField] GameObject menuPanel;
    public List<AbilityMenuEntry> menuEntries = new List<AbilityMenuEntry>(MenuCount);
    public List<Action> menuFunctions = new List<Action>(MenuCount);
    public int currentSelection { get; private set; }

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


        p.GetComponent<ReactiveButton>().Reset();

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
    public List<AbilityMenuEntry> Show (List<string> options, List<Action> functions){
        menuPanel.SetActive(true);
        Clear ();

        menuFunctions = functions;
        for (int i = 0; i < options.Count; ++i){
            AbilityMenuEntry entry = Dequeue();
            entry.Title = options[i];
            entry.gameObject.GetComponent<Button>().onClick.AddListener(new UnityEngine.Events.UnityAction(functions[i]));
            // Debug.Log(functions[i]);
            // entry.gameObject.GetComponent<Button>().onClick.AddListener(delegate{ButtonClicked(entry.Title);});
            menuEntries.Add(entry);
        }
        // SetSelection(0);
        return menuEntries;
    }

    public List<AbilityMenuEntry> Show (List<string> options, List<UnityEngine.Events.UnityAction> actions){
        menuPanel.SetActive(true);
        Clear ();

        // menuFunctions = actions;
        for (int i = 0; i < options.Count; ++i){
            AbilityMenuEntry entry = Dequeue();
            entry.Title = options[i];
            entry.gameObject.GetComponent<Button>().onClick.AddListener(actions[i]);
            menuEntries.Add(entry);
        }
        return menuEntries;
    }
    
    public void Hide (){
        Debug.Log("hiding panel controller");
        Clear();
        menuPanel.SetActive(false);
    }
    public List<AbilityMenuEntry> Show (List<string> options){
        menuPanel.SetActive(true);
        Clear ();
        for (int i = 0; i < options.Count; ++i){
            AbilityMenuEntry entry = Dequeue();
            entry.Title = options[i];
            // entry.gameObject.GetComponent<Button>().onClick.AddListener(new UnityEngine.Events.UnityAction(functions[i]));
            entry.gameObject.GetComponent<Button>().onClick.AddListener(delegate{ButtonClicked(entry.Title);});
            menuEntries.Add(entry);
        }
        SetSelection(0);
        return menuEntries;

    }


    private void ButtonClicked(string name)
    {
        // Debug.Log("Button: " + name + " was clicked!");   
    }

    bool SetSelection (int value){
        if (menuEntries[value].IsLocked)
            return false;
        
        // Deselect the previously selected entry
        if (currentSelection >= 0 && currentSelection < menuEntries.Count)
            menuEntries[currentSelection].IsSelected = false;
        
        currentSelection = value;
        
        // Select the new entry
        if (currentSelection >= 0 && currentSelection < menuEntries.Count)
            menuEntries[currentSelection].IsSelected = true;
        
        return true;
    }

    public void Next () {
        // Debug.Log("nexting w " + menuEntries.Count + " options");
        for (int i = currentSelection + 1; i < currentSelection + menuEntries.Count; ++i){
            int index = i % menuEntries.Count;
            if (SetSelection(index))
                break;
        }
    }
    public void Previous (){
        for (int i = currentSelection - 1 + menuEntries.Count; i > currentSelection; --i) {
            int index = i % menuEntries.Count;
            if (SetSelection(index))
                break;
        }
    }
    

    public void SetLocked (int index, bool value){
        if (index < 0 || index >= menuEntries.Count)
            return;
        menuEntries[index].IsLocked = value;
        if (value && currentSelection == index)
            Next();
    }

}
