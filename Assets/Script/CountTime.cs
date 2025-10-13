using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CountTime : MonoBehaviour
{
    [SerializeField]
    public float setTime;                         //화면에 보여줄 실수형 변수
    public Text countdownTime;                               //UI Text
    [SerializeField]
    objectManager om;
    // Start is called before the first frame update

    void Start()

    {
        countdownTime.text = setTime.ToString();
        om = GameObject.FindWithTag("Manager").GetComponent<objectManager>();
    }



    // Update is called once per frame

    void Update()

    {
        if (setTime > 0)
        {
            setTime -= Time.deltaTime;
        }
        else if (setTime <= 0)
        {
            Time.timeScale = 0.0f;
        }
        if (om.goal1 >= 3 || om.goal2 >= 3)
        {
            Time.timeScale = 0.0f;
        }

        countdownTime.text = Mathf.Round(setTime).ToString();

        
    }

}
