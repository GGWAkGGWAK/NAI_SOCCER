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
        //�ϴ� ��� ��� �ִϸ��̼��� ��� ���
        //ani.SetBool("isRun", )
    }
}
