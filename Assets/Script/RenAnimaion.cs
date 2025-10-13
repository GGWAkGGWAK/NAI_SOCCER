using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenAnimaion : MonoBehaviour
{
    Animator ani;
    // Start is called before the first frame update
    void Awake()
    {
        ani = GetComponentInChildren<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //일단 잠시 대기 애니메이션은 잠시 대기
        //ani.SetBool("isRun", )
    }
}
