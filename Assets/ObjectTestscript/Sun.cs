using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sun : MonoBehaviour
{
    public float speed = 25.0f;
    Rigidbody rigid;
    public Transform target;
    new Vector3 dir;
    
    void Start()
    {
        rigid = GetComponent<Rigidbody>();

    }

    void Update()
    {
        target = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallController>().character.GetComponent<Transform>();

        dir = target.position - transform.position;

        transform.position = Vector3.MoveTowards(new Vector3(transform.position.x, 45.0f, transform.position.z),
                                            new Vector3(target.position.x, 45.0f, target.position.z), speed * Time.deltaTime);

        this.transform.LookAt(dir);
    }

}