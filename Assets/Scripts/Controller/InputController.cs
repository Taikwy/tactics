using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputController : MonoBehaviour
{

    Repeater _hor = new Repeater("Horizontal");
    Repeater _ver = new Repeater("Vertical");

    public static event EventHandler<InfoEventArgs<Point>> moveEvent;
    public static event EventHandler<InfoEventArgs<int>> fireEvent;

    string[] _buttons = new string[] {"Fire1", "Fire2", "Fire3"};

    GameUIController guiController;


    // Start is called before the first frame update
    void Start()
    {
        guiController = GetComponent<BattleController>().guiController;
    }

    // Update is called once per frame
    void Update()
    {
        // if(guiController.showingGameOver || guiController.showingPause)
        if(guiController.isPaused)
            return;
        // int x = _hor.Update();
        // int y = _ver.Update();
        // if (x != 0 || y != 0){
        //     if (moveEvent != null)
        //         moveEvent(this, new InfoEventArgs<Point>(new Point(x, y)));
        // }

        // Debug.Log(Input.GetAxisRaw("Horizontal"));
        // Debug.Log(Input.GetAxisRaw("Vertical"));

        //loops thru the 3 fire buttons to see if any of them were pressed (released)
        for (int i = 0; i < 3; ++i){
            if (Input.GetButtonUp(_buttons[i])){
                if (fireEvent != null)
                    fireEvent(this, new InfoEventArgs<int>(i));
            }
        }
    }

    class Repeater{
        const float threshold = 0.5f;
        const float rate = 0.2f;
        float _next;
        bool _hold;
        string _axis;

        public Repeater (string axisName){
            _axis = axisName;
        }

        public int Update () {
            int retValue = 0;
            int value = Mathf.RoundToInt( Input.GetAxisRaw(_axis) );
            if (value != 0){
                if (Time.time > _next){
                    retValue = value;
                    // _next = Time.time + (_hold ? rate : threshold);
                    // _hold = true;
                    _next = Time.time + rate;
                }
            }
            else{
                _hold = false;
                _next = 0;
            }
            return retValue;
        }
    }
}
