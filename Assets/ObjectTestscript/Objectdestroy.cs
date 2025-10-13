using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Objectdestroy : MonoBehaviour
{
    public float destroyTime = 30.0f;

    public List<GameObject> crList = new List<GameObject>();

    GoalCheck gc;
    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GoalCheck").GetComponent<GoalCheck>();
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(Destroy());
        }
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(destroyTime);
        if(this.gameObject.tag == "Sun")
        {
            ReturnSpeed();
        }
        gc.objectList.Remove(this.gameObject);
        Destroy(this.gameObject);
    }
        
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Character")
        {
            if (crList.Contains(other.gameObject))
            {
                return;
            }
            else
            {
                crList.Add(other.gameObject);
            }
                
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Character")
        {
            crList.Remove(other.gameObject);
        }
    }
    public void ReturnSpeed()
    {
        for(int i = 0; i<crList.Count; i++)
        {
            crList[i].gameObject.GetComponent<PlayerController>().isSun = false;
            crList[i].gameObject.GetComponent<PlayerController>().speed = 75.0f;
        }
        
    }
}
