using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour 
{
    public float speed = 5f;
    public bool following = false;
    public Transform target;
    Transform _transform;
    
    void Awake ()
    {
        _transform = transform;
    }
    
    void Update ()
    {
        //makes sure there's a target transform and that camera is supposed to be following right now
        if (target)
            _transform.position = Vector3.Lerp(_transform.position, target.position, speed * Time.deltaTime);
            // _transform.position = Vector3.MoveTowards(_transform.position, target.position, 5* speed * Time.deltaTime);
    }
}
