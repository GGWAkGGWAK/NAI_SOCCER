using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ren_Shild : MonoBehaviour
{
    
    public GameObject[] allTarget;
    public Transform target;
    public float rotateSpeed;
    Vector3 offset;

    bool check;

    public GameObject[] shilds;

    // Start is called before the first frame update
    void Start()
    {
        
        check = false;
        allTarget = GameObject.FindGameObjectsWithTag("Character");

        target = DistanceCheck();
        offset = transform.position - target.position;
        
    }

    // Update is called once per frame
    void Update()
    {

        if (target != null)
        {
            transform.position = target.position + offset;
            transform.RotateAround(target.position, Vector3.up, rotateSpeed * Time.deltaTime);
            offset = transform.position - target.position;
        }
        


    }
    
    Transform DistanceCheck()
    {
        Vector3 distance1;
        Vector3 distance2;
        for(int i =0; i<6; i++)
        {
            if(i == 0)
            {
                target = allTarget[i].transform;
            }
            else
            {
                distance1 = target.position - this.transform.position;
                distance2 = allTarget[i].transform.position - this.transform.position;
                
                if(distance1.magnitude > distance2.magnitude)
                {
                    target = allTarget[i].transform;
                }

            }
        }
        return target;
    }


}
