using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucy_Skill_effect : MonoBehaviour
{
    public GameObject[] allTarget;
    public Transform target;
    public GameObject target_obj;

    GameObject ball;
    BallController ballController;
    Rigidbody ballRigid;
    Lucy_Skill ls;
    
    public float kbPower = 500f;
    
    Vector3 kbPos;
    // Start is called before the first frame update
    void Start()
    {
        allTarget = GameObject.FindGameObjectsWithTag("Character");
        target = DistanceCheck();
        

        ball = GameObject.Find("Soccer Ball");
        ballController = ball.GetComponent<BallController>();
        ballRigid = ball.GetComponent<Rigidbody>();

        ls = GetComponent<Lucy_Skill>();
        
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Destroy());
    }

    private void OnTriggerEnter(Collider other)
    {
        
        if (other.gameObject != target.gameObject)
        {
            if (other.gameObject.tag == "Character")
            {
                if (other.gameObject.GetComponent<PlayerController>().teamIndex != target.gameObject.GetComponent<PlayerController>().teamIndex)
                {
                    Vector3 dir = other.transform.position - transform.position;
                    other.transform.Translate(dir.normalized * 100f);

                }
            }
            else if (other.gameObject.tag == "Ball")
            {
                Vector3 balldir = other.transform.position - transform.position;
                ballRigid.velocity = -ballRigid.velocity;
                ballRigid.AddForce(balldir.normalized * kbPower, ForceMode.VelocityChange);
                ballController.nowDribble = false;
                ballController.nowFree = true;
                ballController.nowGround = false;
            }

        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(0.3f);
        Destroy(this.gameObject);
    }

    Transform DistanceCheck()
    {
        Vector3 distance1;
        Vector3 distance2;
        for (int i = 0; i < 6; i++)
        {
            if (i == 0)
            {
                target = allTarget[i].transform;
            }
            else
            {
                distance1 = target.position - this.transform.position;
                distance2 = allTarget[i].transform.position - this.transform.position;

                if (distance1.magnitude > distance2.magnitude)
                {
                    target = allTarget[i].transform;
                }

            }
        }
        return target;
    }
}
