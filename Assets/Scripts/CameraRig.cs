using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour 
{
    public float smoothTime = .5f;
    [Tooltip("How far from the center can the cursor be before the camera starts moving towards it ")]public float cameraLeniency = 2f;
    Vector2 velocity = Vector2.zero;
    public bool following = false;
    public Transform target;
    Transform cameraTransform;

    
    void Awake ()
    {
        cameraTransform = transform;
    }
    
    void Update ()
    {
        //makes sure there's a target transform and that camera is supposed to be following right now
        if(target){
            Vector2 targetPosition = target.position;
            // if(Vector2.Distance(cameraTransform.position, target.position) > cameraLeniency)
                cameraTransform.position = Vector2.SmoothDamp(cameraTransform.position, target.position, ref velocity, smoothTime);
        }
    }
}
