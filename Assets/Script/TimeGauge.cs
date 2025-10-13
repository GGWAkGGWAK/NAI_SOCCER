using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class TimeGauge : MonoBehaviour
{
    public Slider slTimer;
    float fSliderBarTime;
    void Start()
    {
       // slTimer = GetComponent<Slider>();
    }

    void Update()
    {
        //if (slTimer.value > 0.0f)
        //{
        //    // 시간이 변경한 만큼 slider Value 변경을 합니다.
        //    slTimer.value += Time.deltaTime;
        //}
        //else
        //{
        //    Debug.Log("Time is Zero.");
        //}

        if (slTimer.value < 1f)
        {
            slTimer.value = Mathf.MoveTowards(slTimer.value, 1f, Time.deltaTime*0.11f);
        }
    }
}
