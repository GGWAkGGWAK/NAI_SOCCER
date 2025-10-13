using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ojbSet : MonoBehaviour
{
    public float destroyTime = 30.0f;

    public GameObject minigoalPost;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        StartCoroutine(Destroy());
    }

    IEnumerator Destroy()
    {
        yield return new WaitForSeconds(destroyTime);
        if (!minigoalPost.activeSelf)
        {
            minigoalPost.SetActive(true);
        }
        gameObject.SetActive(false);

    }
}
