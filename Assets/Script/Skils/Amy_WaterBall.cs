using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amy_WaterBall : MonoBehaviour
{
    public GameObject[] allTarget;
    public Transform target;
    public GameObject target_obj;
    public float power = 500.0f;
    

    // Start is called before the first frame update
    void Start()
    {
        allTarget = GameObject.FindGameObjectsWithTag("Character");
        target = DistanceCheck();
        
    }

    // Update is called once per frame
    void Update()
    {
        transform.Translate(Vector3.forward * power * Time.deltaTime);
        StartCoroutine(Destroy(5.0f));
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


    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject != target.gameObject)
        {
            if(other.tag == "Character")
            {
                if (other.gameObject.GetComponent<PlayerController>().teamIndex != target.gameObject.GetComponent<PlayerController>().teamIndex)
                {
                    other.GetComponent<PlayerController>().StartCoroutine(other.GetComponent<PlayerController>().Lightning());
                    transform.position = other.transform.position;
                    power = 0;
                    this.gameObject.GetComponent<Collider>().enabled = false;
                    StartCoroutine(Destroy(2.0f));
                }
                    
                

                

            }
        }
        if(other.tag == "Ren_Shild")
        {
            power *= -1;
        }

        if(power < 0)
        {
            if(other.tag == "Character")
            {
                if (other.gameObject.GetComponent<PlayerController>().teamIndex == target.gameObject.GetComponent<PlayerController>().teamIndex)
                {
                    
                    if (other.gameObject == target.gameObject)
                    {
                        StartCoroutine(Destroy(2.0f));
                        transform.position = target.transform.position;
                        target.gameObject.GetComponent<PlayerController>().StartCoroutine(target.gameObject.GetComponent<PlayerController>().Lightning());
                        power = 0;
                        this.gameObject.GetComponent<Collider>().enabled = false;

                    }
                    else
                    {
                        StartCoroutine(Destroy(2.0f));
                        transform.position = other.transform.position;
                        other.GetComponent<PlayerController>().StartCoroutine(other.GetComponent<PlayerController>().Lightning());
                        power = 0;
                        this.gameObject.GetComponent<Collider>().enabled = false;
                    }

                }
                
            }
        }
    }
    

    IEnumerator Destroy(float time)
    {
        yield return new WaitForSeconds(time);
        Destroy(this.gameObject);
    }

}
