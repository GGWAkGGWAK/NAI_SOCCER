using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lightning : MonoBehaviour
{
    [SerializeField]
    GameObject player;
    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Character");
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    IEnumerator LightningObject()
    {
        Debug.Log("스턴!");
        player.GetComponent<PlayerController>().nowControl = false;
        yield return new WaitForSeconds(2f);
        Debug.Log("스턴해제!");
        player.GetComponent<PlayerController>().nowControl = true;
        StopCoroutine(LightningObject());
    }
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Character")
        {
            StartCoroutine(LightningObject());
            StopCoroutine(LightningObject());
        }
    }
}
