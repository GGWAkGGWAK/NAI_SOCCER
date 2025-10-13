using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class spawnerObject : MonoBehaviour
{
    public int hp = 1;

    [SerializeField]
    GameObject spawner;
    [SerializeField]
    GameObject goal;

    [SerializeField]
    GameObject ball;
    [SerializeField]
    GameObject goalSound;

    AudioSource goalaudioSource;

    GoalCheck gc;
    // Start is called before the first frame update
    void Start()
    {
        ball = GameObject.Find("Soccer Ball");
        goalSound = GameObject.Find("GoalSound");
        goalaudioSource = goalSound.GetComponent<AudioSource>();
        gc = GameObject.FindGameObjectWithTag("GoalCheck").GetComponent<GoalCheck>();
        
    }

    // Update is called once per frame
    void Update()
    {
        if (hp <= 0)
        {
            goalaudioSource.Play();
            goal.SetActive(false);
            StartCoroutine(spawn());
            gameObject.SetActive(false);
            hp = 1;
        }
            

    }

    

    IEnumerator spawn()
    {
        GameObject instance = Instantiate(spawner,new Vector3(-11, 3, 200), Quaternion.identity);
        gc.objectList.Add(instance);
        yield return null;
        
    }


    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.tag == "Ball")
        {
            hp--;
        }
            
    }
}
