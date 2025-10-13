using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Wind : MonoBehaviour
{
    public float speed;
    public float power;
    Rigidbody rigid;
    Transform target;
    GameObject ball;
    BallController ballController;
    Rigidbody ballRigid;

    void Start()
    {
        ball = GameObject.Find("Soccer Ball");
        ballController = ball.GetComponent<BallController>();
        ballRigid = ball.GetComponent<Rigidbody>();

        speed = 200f;
        power = 50f;
        rigid = GetComponent<Rigidbody>();
        target = GameObject.FindGameObjectWithTag("Ball").GetComponent<Transform>();
        StartCoroutine(ShotBall());
    }

    void Update()
    {
        Chasing();
        this.transform.LookAt(target);
    }

    void Shot()
    {
        Vector3 balldir = ball.transform.position - transform.position;
        ballRigid.AddForce(balldir.normalized * power, ForceMode.Impulse);
        ballController.nowDribble = false;
        ballController.nowFree = true;
        ballController.nowGround = false;
    }
    IEnumerator ChaseBall()
    {
        for(int i=0; i<11; i++)
        {
            Debug.Log("따라간다");
            Chasing();
            yield return new WaitForSeconds(3f);
        }
        
    }

    IEnumerator ShotBall()
    {
        for (int i = 0; i < 11; i++)
        {
            yield return new WaitForSeconds(3f);
            Debug.Log("쏜다");
            Shot();
            if (i == 10)
                Destroy(this.gameObject);
        }
    }
    void Chasing()
    {
        transform.position = Vector3.MoveTowards(transform.position, target.position, speed * Time.deltaTime);
    }
}
