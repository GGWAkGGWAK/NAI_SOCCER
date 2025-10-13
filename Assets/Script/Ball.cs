using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{

    public float Force;
    public Vector3 Tr;
    private Rigidbody BallRigidbody;
    private Collider _ball;
    private bool _isOnNet;
    // Start is called before the first frame update
    void Start()
    {
        BallRigidbody = GetComponent<Rigidbody>();
        _ball = GetComponent<Collider>();
    }
    // Update is called once per frame
    void Update()
    {
        
    }
    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Character")
        {
            //BallRigidbody.AddForce(Vector3.forward * Force, ForceMode.Impulse);
        }
        if (collision.gameObject.CompareTag("GoalPost"))
        {
            _ball.material.bounciness *= 1.0f;
            _isOnNet = true;
            StartCoroutine(MoveBallOnNet());
            return;
        }

        _ball.material.bounciness = 0.8f;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("GoalPost"))
        {
            _isOnNet = false;
        }
    }
    private IEnumerator MoveBallOnNet()
    {
        int loop = 0;
        while(_isOnNet && loop < 8)
        {
            yield return new WaitForSeconds(0.5f);
            loop++;
        }
    }

}
