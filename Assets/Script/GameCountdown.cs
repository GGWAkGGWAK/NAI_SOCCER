using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameCountdown : MonoBehaviour
{
    private int Timer = 0;
    public GameObject Num_A;
    public GameObject Num_B;
    public GameObject Num_C;
    public GameObject Num_Go;
    // Start is called before the first frame update
    void Start()
    {
        Timer = 0;
        
        Num_A.SetActive(false);
        Num_B.SetActive(false);
        Num_C.SetActive(false);
        Num_Go.SetActive(false);

    }

    // Update is called once per frame
    void Update()
    {

        //게임 시작시 정지
        if (Timer == 0)
        {
            Time.timeScale = 0.0f;
        }


        

        if (Timer <= 90)
        {
            Timer++;

            if (Timer < 30)
            {
                Num_C.SetActive(true);
            }

           
            if (Timer > 30)
            {
                Num_C.SetActive(false);
                Num_B.SetActive(true);
            }

           
            if (Timer > 60)
            {
                Num_B.SetActive(false);
                Num_A.SetActive(true);
            }

            
            if (Timer >= 90)
            {
                Num_A.SetActive(false);
                Num_Go.SetActive(true);
                StartCoroutine(this.LoadingEnd());
                Time.timeScale = 1.0f; 
            }
        }

    }



    IEnumerator LoadingEnd()
    {


        yield return new WaitForSeconds(1.0f);
        Num_Go.SetActive(false);
    }
}
