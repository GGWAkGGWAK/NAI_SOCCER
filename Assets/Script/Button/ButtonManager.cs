using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems; //꼭 추가
using UnityEngine.UI;

public class ButtonManager : MonoBehaviour, IPointerClickHandler, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    GameObject soccerBall;
    GameObject controlCharacter;

    GameObject left;
    GameObject up;
    GameObject right;

    float chargingTime;

    Vector3 mouseStart;
    Vector3 mouseEnd;

    bool nowClick;

    void Awake()
    {
        soccerBall = GameObject.FindWithTag("Ball");
        left = transform.GetChild(0).transform.GetChild(0).gameObject;
        up = transform.GetChild(0).transform.GetChild(1).gameObject;
        right = transform.GetChild(0).transform.GetChild(2).gameObject;
    }
    void Start()
    {
        chargingTime = 0f;
        nowClick = false;
    }

    void Update()
    {
        if(nowClick)
        {
            chargingTime += Time.deltaTime;
        }
    }
    public void OnPointerClick(PointerEventData eventData) //클릭시
    {
        
    }
    
    public void OnBeginDrag(PointerEventData eventData) //드로우 시작
    {
        nowClick = true;
        soccerBall.GetComponent<BallController>().isCharging = true;
        transform.GetChild(0).gameObject.SetActive(true);
        transform.GetChild(1).gameObject.SetActive(false);
        transform.GetChild(2).gameObject.SetActive(false);
        transform.GetChild(3).gameObject.SetActive(false);
    }

    public void OnDrag(PointerEventData eventData)
    {
        
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        nowClick = false;
        if(chargingTime >= 2) { chargingTime = 2f;}
        up.GetComponent<UpButton>().chargingTime = chargingTime;
        soccerBall.GetComponent<BallController>().isCharging = false;
        chargingTime = 0f;
        transform.GetChild(0).gameObject.SetActive(false);
        transform.GetChild(1).gameObject.SetActive(true);
        transform.GetChild(2).gameObject.SetActive(true);
        transform.GetChild(3).gameObject.SetActive(true);
    }

}
