using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Move : MonoBehaviour
{
    public float speed = 2f;
    public float rotSpeed = 3f;
    [SerializeField]
    private Rigidbody ridbody;

    float h;
    float v;

    Vector3 moveDir;

    void Start()
    {
        ridbody = GetComponent<Rigidbody>();
    }

    void Update()
    {
        float h = Input.GetAxisRaw("Horizontal");
        float v = Input.GetAxisRaw("Vertical");

        Vector3 _moveHoizontal = transform.right * h;
        Vector3 _moveVertial = transform.forward * v;

        moveDir = new Vector3(h, 0, v);

        if (!(h == 0 && v == 0))
        {
            transform.position += moveDir * speed * Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, Quaternion.LookRotation(moveDir), Time.deltaTime * rotSpeed);
        }
    }

}
