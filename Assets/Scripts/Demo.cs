using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Demo : MonoBehaviour
{
    void OnMoveEvent (object sender, InfoEventArgs<Point> e){
        Debug.Log("Move " + e.info.ToString());
    }
    void OnFireEvent (object sender, InfoEventArgs<int> e){
        Debug.Log("Fire " + e.info);
    }

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    void OnEnable () {
        InputController.moveEvent += OnMoveEvent;
        InputController.fireEvent += OnFireEvent;
    }
    void OnDisable (){
        InputController.moveEvent -= OnMoveEvent;
        InputController.fireEvent -= OnFireEvent;
    }
}
