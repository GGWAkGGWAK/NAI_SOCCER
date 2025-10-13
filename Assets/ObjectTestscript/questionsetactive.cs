using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class questionsetactive : MonoBehaviour
{
    public float destroyTime = 30.0f;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (gameObject.activeSelf)
        {
            StartCoroutine(SetActive());
            StopCoroutine(SetActive());
        }
    }
    IEnumerator SetActive()
    {

        yield return new WaitForSeconds(destroyTime);
        this.gameObject.SetActive(false);
    }
}
