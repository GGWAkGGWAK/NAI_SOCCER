using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dr7Turret : MonoBehaviour
{
    float turretPower;

    GameObject ball;
    BallController ballController;
    AudioSource audioSource;
    Rigidbody ballRigid;
    Animator anim;

    void Start()
    {
        ball = GameObject.Find("Soccer Ball");
        ballController = ball.GetComponent<BallController>();
        ballRigid = ball.GetComponent<Rigidbody>();
        anim = GetComponent<Animator>();
        audioSource = this.gameObject.GetComponent<AudioSource>();

        turretPower = 50f;

        StartCoroutine(ShotTheBall());
    }

    void Update()
    {
        
    }
    
    void Shot()
    {
        Vector3 balldir = ball.transform.position - transform.position;
        ballRigid.AddForce(balldir.normalized * turretPower, ForceMode.Impulse);
        ballController.nowDribble = false;
        ballController.nowFree = true;
        ballController.nowGround = false;
        transform.LookAt(new Vector3(ball.transform.position.x, transform.position.y, ball.transform.position.z));
        Debug.Log("터렛 공격");
    }

    void Attack() { }
    IEnumerator ShotTheBall()
    {
        yield return new WaitForSeconds(0.5f);
        for(int i = 0; i < 3; i ++)
        {
            anim.SetTrigger("isAttack");
            yield return new WaitForSeconds(0.8f);
            this.audioSource.Play();
            Shot();//발사함수
            yield return new WaitForSeconds(2.2f);
            if(i == 2)
            {
                Destroy(this.gameObject);
            }
        }
    }
}
