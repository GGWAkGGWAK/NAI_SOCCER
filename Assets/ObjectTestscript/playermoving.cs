using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class playermoving : MonoBehaviour
{
    // Start is called before the first frame update
    
    public float speed = 7.0f;
    public float time;
    Rigidbody rb;
    public bool isMove;
    public bool isSun;
    public bool isWind;

    private float curTime;
    public float coolTime = 0.5f;
    public Transform atkPos;
    public Vector3 atkSize;

    void Start()
    {
        isMove = true;
        isWind = false;
        isSun = false;
        rb = GetComponent<Rigidbody>();
    }

    // Update is called once per frame
    void Update()
    {
        if (isMove)
        {
            float inputX = Input.GetAxis("Horizontal");
            float inputZ = Input.GetAxis("Vertical");

            Vector3 pPosition = transform.position;
            pPosition.x += inputX * Time.deltaTime * speed;
            pPosition.z += inputZ * Time.deltaTime * speed;

            transform.position = pPosition;
        }
        
        
        

        if (isSun) //isSun이 참일때
        {
            speed = 1.0f;
        }
        else  //isSun이 거짓일때
        {
            speed = 7.0f;
        }

        if (isWind)
        {
            rb.velocity = new Vector3(0, 0, -3);
        }
        else
        {
            rb.velocity = new Vector3(0, 0, 0);
        }

        

    }
    private void OnTriggerEnter(Collider other)//와 충돌 할 때 & 충돌하는 동안
    {
        if(other.gameObject.tag == "Sun")
        {
            isSun = true;
        }
        else if(other.gameObject.tag == "Wind")
        {
            isWind = true;
        }
    }
    private void OnTriggerExit(Collider other)//와 충돌이 끝날때
    {
        if (other.gameObject.tag == "Sun")
        {
            isSun = false;
        }
        else if(other.gameObject.tag == "Wind")
        {
            isWind = false;
        }
    }
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.red;
        Gizmos.DrawWireCube(atkPos.position, atkSize);
    }

}
