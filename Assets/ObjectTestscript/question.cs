using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class question : MonoBehaviour
{
    [SerializeField]
    GameObject[] barricade;
    [SerializeField]
    GameObject goalPost;
    [SerializeField]
    GameObject minigoalPost;

    [SerializeField]
    GameObject miniGoal;

    public int hp = 1;


    [SerializeField]
    GameObject ball;

    [SerializeField]
    GameObject goalSound;

    AudioSource goalaudioSource;
    // Start is called before the first frame update
    void Start()
    {
        goalSound = GameObject.Find("GoalSound");
        ball = GameObject.Find("Soccer Ball");
        goalaudioSource = goalSound.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        if(hp <= 0)
        {
            goalaudioSource.Play();
            goalPost.SetActive(false);
            
            int random = Random.Range(1, 3);
            Debug.Log(random);
            if (random == 1)
                Barricade();
            else if (random == 2)
                MiniGoal();
            //barricade[0].SetActive(true);
            //barricade[1].SetActive(true);
            gameObject.SetActive(false);
            hp = 1;
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Ball")
        {
            hp--;
        }
    }

    void Barricade()
    {
        barricade[0].SetActive(true);
        barricade[1].SetActive(true);
    }
    void MiniGoal()
    {
        minigoalPost.SetActive(false);
        miniGoal.SetActive(true);
    }
}
