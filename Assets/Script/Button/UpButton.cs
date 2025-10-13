using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class UpButton : MonoBehaviour , IDropHandler
{
    GameObject shotButton;
    GameObject soccerBall;

    public float chargingTime;

    void Awake()
    {
        soccerBall = GameObject.FindWithTag("Ball");
        shotButton = transform.parent.parent.gameObject;
    }

    // Update is called once per frame
    void Update()
    {

    }

    public void OnDrop(PointerEventData eventData)
    {
        if(gameObject.name == "Up")
        {
            soccerBall.GetComponent<BallController>().ShootCall(chargingTime);
        }
        if(gameObject.name == "Left")
        {
            soccerBall.GetComponent<BallController>().LeftShootCall(chargingTime);
        }
        if(gameObject.name == "Right")
        {
            soccerBall.GetComponent<BallController>().RightShootCall(chargingTime);
        }
    }
}
