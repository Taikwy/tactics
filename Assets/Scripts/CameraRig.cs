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

    Vector2 mousePosition;
    Vector2 targetPos;
    float minScreenBoundX, maxScreenBoundX;
    float minScreenBoundY, maxScreenBoundY;
    public float cameraBufferX, cameraBufferY;

    
    void Awake ()
    {
        cameraTransform = transform;
        cameraTransform.position = Vector2.zero;
        float screenBound = 180;
        minScreenBoundX = screenBound;
        maxScreenBoundX = Screen.width-screenBound;
        minScreenBoundY = screenBound;
        maxScreenBoundY = Screen.height-screenBound;
    }
    
    void Update (){
        mousePosition = Input.mousePosition;
        // print("camera position " + cameraTransform.position + "   | MOUSE " + mousePosition);
        // print("MOUSE " + (mousePosition/16f) + "   | SELECT " + target.position);
        BoundsCameraMovement();
        // SelectCameraMovement();

    }

    void BoundsCameraMovement(){
        mousePosition = Input.mousePosition;
        targetPos = cameraTransform.position;
        if(mousePosition.x <= minScreenBoundX){
            float exceedBy = Mathf.Max((mousePosition.x - minScreenBoundX)/25, -5);
            Debug.Log("left " + (mousePosition.x - minScreenBoundX)/25);
            targetPos += new Vector2(exceedBy,0);
        }
        else if(mousePosition.x >= maxScreenBoundX){
            float exceedBy = Mathf.Min((mousePosition.x - maxScreenBoundX)/25, 5);
            Debug.Log("right " + exceedBy);
            targetPos += new Vector2(exceedBy,0);
        }
        if(mousePosition.y <= minScreenBoundY){
            float exceedBy = Mathf.Max((mousePosition.y - minScreenBoundY)/25, -5);
            Debug.Log("down " + exceedBy);
            targetPos += new Vector2(0,exceedBy);
        }
        else if(mousePosition.y >= maxScreenBoundY){
            float exceedBy = Mathf.Min((mousePosition.y - maxScreenBoundY)/25, 5);
            Debug.Log("up " + exceedBy);
            targetPos += new Vector2(0,exceedBy);
        }
        cameraTransform.position = Vector2.SmoothDamp(cameraTransform.position, targetPos, ref velocity, smoothTime);
        // cameraTransform.position = targetPos;

    }
    void SelectCameraMovement(){
        //makes sure there's a target transform and that camera is supposed to be following right now
        if(target){
            Vector2 targetPosition = target.position;
            // if(Vector2.Distance(cameraTransform.position, target.position) > cameraLeniency)
                cameraTransform.position = Vector2.SmoothDamp(cameraTransform.position, target.position, ref velocity, smoothTime);
        }
    }
}
