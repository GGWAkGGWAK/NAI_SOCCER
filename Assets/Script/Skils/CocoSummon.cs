using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CocoSummon : MonoBehaviour
{
    void Start()
    {
        StartCoroutine(Distroy());
    }

    void Update()
    {
        
    }

    IEnumerator Distroy()
    {
        yield return new WaitForSeconds(1f);
        GameObject.Destroy(this.gameObject);
    }

    private void OnTriggerStay(Collider other)
    {
        if(other.tag == "Wind"  || other.tag == "Turret")
        {
            Destroy(other.gameObject);
        }
        else if(other.tag == "Sun")
        {
            other.gameObject.GetComponent<Objectdestroy>().ReturnSpeed();
            Destroy(other.gameObject);
        }
        else if(other.tag == "Obj")
        {
            other.gameObject.SetActive(false);
        }
        else if(other.tag == "Ren_Shild")
        {
            Destroy(other.gameObject);
        }
    }
}
