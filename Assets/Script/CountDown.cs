using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CountDown : MonoBehaviour
{

    private int Timer = 0;
    public GameObject Num_A;
    public GameObject Num_B;
    public GameObject Num_C;
    // Start is called before the first frame update
    void Start()
    {
        Timer = 0;

        Num_A.SetActive(false);
        Num_B.SetActive(false);
        Num_C.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if(Timer == 0)
        {
            Time.timeScale = 0.0f;


        }

        if(Timer <= 150)
        {
            Timer++;


            if(Timer > 60)
            {
                Num_C.SetActive(true);
            }

            if (Timer > 90)
            {
                Num_C.SetActive(false);
                Num_B.SetActive(true);
            }

            if (Timer > 120)
            {
                Num_B.SetActive(false);
                Num_A.SetActive(true);
            }

            if (Timer >= 150)
            {
                Num_A.SetActive(false);
                StartCoroutine(this.LoadingEnd());
                Time.timeScale = 1.0f;
            }
        }
    }

    IEnumerator LoadingEnd()
    {
        yield return new WaitForSeconds(1.0f);
    }
}
