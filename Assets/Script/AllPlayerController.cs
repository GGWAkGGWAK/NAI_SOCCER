using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AllPlayerController : MonoBehaviour
{
    float hAxis;
    float vAxis;
    Vector3 moverVec;
    public float speed;
    private Rigidbody characterRigidbody;

    void Start()
    {
        characterRigidbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        hAxis = Input.GetAxisRaw("Horizontal");
        vAxis = Input.GetAxisRaw("Vertical");
        // -1 ~ 1

        float fallSpeed = characterRigidbody.velocity.y; // 떨어지는 속도 저장

        moverVec = new Vector3(hAxis, 0, vAxis).normalized;
        transform.position += moverVec * speed * Time.deltaTime;

        transform.LookAt(transform.position + moverVec);
    }
}
