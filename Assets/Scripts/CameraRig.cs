using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraRig : MonoBehaviour 
{
    BattleController bc;
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
    public bool selectMovement = true;
    public bool unitMovement = false;

    public float screenBoundWidth;
    public float screenBoundHeight;
    public float screenBoundWidthPercent;
    public float screenBoundHeightPercent;
    
    void Awake ()
    {
        bc = GetComponentInParent<BattleController>();
        if(bc == null)
            Debug.LogError("unable to find battle controller");
        cameraTransform = transform;
        cameraTransform.position = Vector2.zero;
        float screenBound = 180;
        // float screenBoundWidth = 120;
        // float screenBoundHeight = 80;
        // minScreenBoundX = screenBound;
        // maxScreenBoundX = Screen.width-screenBound;
        // minScreenBoundY = screenBound;
        // maxScreenBoundY = Screen.height-screenBound;
    }
    
    void Update (){
        // minScreenBoundX = (Screen.width/2)-(screenBoundWidth/2);
        // maxScreenBoundX = (Screen.width/2)+(screenBoundWidth/2);
        // minScreenBoundY = (Screen.height/2)-(screenBoundHeight/2);
        // maxScreenBoundY = (Screen.height/2)+(screenBoundHeight/2);
        minScreenBoundX = (Screen.width/2)-((Screen.width*screenBoundWidthPercent/100f)/2);
        maxScreenBoundX = (Screen.width/2)+((Screen.width*screenBoundWidthPercent/100f)/2);
        minScreenBoundY = (Screen.height/2)-((Screen.height*screenBoundHeightPercent/100f)/2);
        maxScreenBoundY = (Screen.height/2)+((Screen.height*screenBoundHeightPercent/100f)/2);
        mousePosition = Input.mousePosition;
        // print("camera position " + cameraTransform.position + "   | MOUSE " + mousePosition);
        // print("MOUSE " + (mousePosition/16f) + "   | SELECT " + target.position);
        if(selectMovement)
            SelectCameraMovement();
        else if(unitMovement)
            UnitCameraMovement();
        else
            BoundsCameraMovement();
        // Debug.Log("camerapos " +  cameraTransform.position);
        ClampCameraPosition();
        cameraTransform.position = Vector2.SmoothDamp(cameraTransform.position, targetPos, ref velocity, smoothTime);

    }
    void SelectCameraMovement(){
        //makes sure there's a target transform and that camera is supposed to be following right now
        if(target){
            targetPos = target.position;
            // if(Vector2.Distance(cameraTransform.position, target.position) > cameraLeniency)
            // targetPos = Vector2.SmoothDamp(cameraTransform.position, target.position, ref velocity, smoothTime);
            // print("selet movement");
        }
    }
    void UnitCameraMovement(){
        targetPos = bc.turn.actingUnit.transform.position;
    }

    void BoundsCameraMovement(){
        mousePosition = Input.mousePosition;
        targetPos = cameraTransform.position;
        if(mousePosition.x <= minScreenBoundX){
            float exceedBy = CalculateBoundsCameraMovement(Screen.width, mousePosition.x, minScreenBoundX);
            // Mathf.Max(-Mathf.Sqrt(Mathf.Abs((mousePosition.x - minScreenBoundX)/25)), -5);
            targetPos += new Vector2(Mathf.Max(-exceedBy, -5),0);
        }
        else if(mousePosition.x >= maxScreenBoundX){
            // float exceedBy = Mathf.Min((mousePosition.x - maxScreenBoundX)/25, 5);
            float exceedBy = CalculateBoundsCameraMovement(Screen.width,mousePosition.x, maxScreenBoundX);
            // targetPos += new Vector2(exceedBy,0);
            targetPos += new Vector2(Mathf.Min(exceedBy, 5),0);
        }
        if(mousePosition.y <= minScreenBoundY){
            float exceedBy = CalculateBoundsCameraMovement(Screen.height,mousePosition.y, minScreenBoundY);
            targetPos += new Vector2(0,Mathf.Max(-exceedBy,-5));
        }
        else if(mousePosition.y >= maxScreenBoundY){
            // float exceedBy = Mathf.Min((mousePosition.y - maxScreenBoundY)/25, 5);
            float exceedBy = CalculateBoundsCameraMovement(Screen.height,mousePosition.y, maxScreenBoundY);
            targetPos += new Vector2(0,Mathf.Min(exceedBy, 5));
        }
        // cameraTransform.position = targetPos;
    }
    float CalculateBoundsCameraMovement(float screen, float mousePos, float screenBound){
        float exceedBy = Mathf.Sqrt(Mathf.Abs((mousePos - screenBound)/screen))*14;
        // float exceedBy = (mousePos - screenBound)/25;
        // Debug.Log("calculating " + exceedBy);
        return exceedBy;
    }

    void ClampCameraPosition(){
        targetPos.x = Mathf.Max(targetPos.x, bc.board.min.x);
        targetPos.x = Mathf.Min(targetPos.x, bc.board.max.x);
        targetPos.y = Mathf.Max(targetPos.y, bc.board.min.y);
        targetPos.y = Mathf.Min(targetPos.y, bc.board.max.y);
    }
    
}
