using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
public class GoalCheck : MonoBehaviour
{
    [SerializeField]
    private Text team1Goal;
    [SerializeField]
    private Text team2Goal;

    [SerializeField]
    private Text team1Win;
    [SerializeField]
    private Text team2Win;
    [SerializeField]
    private Button robbyBtn;

    [SerializeField]
    private GameObject goalPost;

    public GameObject[] charPrefabs;

    public Transform[] firstTrans_1;

    public List<GameObject> objectList = new List<GameObject>();

    public GameObject[] baricades;
    public GameObject miniGoal;
    public GameObject miniGoalPost;
    [SerializeField]
    BallController ball;
    [SerializeField]
    GameObject whistleSound;

    AudioSource audioSource;
    AudioSource whistleSource;

    [SerializeField]
    objectManager om;
    void Start()
    {
        whistleSound = GameObject.Find("WhistleSound");
        ball = GameObject.FindGameObjectWithTag("Ball").GetComponent<BallController>();
        om = GameObject.FindWithTag("Manager").GetComponent<objectManager>();
        
        
        audioSource = this.gameObject.GetComponent<AudioSource>();
        whistleSource = whistleSound.GetComponent<AudioSource>();
    }
    void Update()
    {
        charPrefabs = ball.allcharacter;
        if (om.goal1 >= 3)
        {
            audioSource.Play();
            team1Win.gameObject.SetActive(true);
            robbyBtn.gameObject.SetActive(true);
            for(int i=0; i<6; i++)
            {
                charPrefabs[i].gameObject.GetComponent<PlayerController>().nowControl = false;
                
            }
        }
        else if (om.goal2 >= 3)
        {
            audioSource.Play();
            team2Win.gameObject.SetActive(true);
            robbyBtn.gameObject.SetActive(true);
            for (int i = 0; i < 6; i++)
            {
                charPrefabs[i].gameObject.GetComponent<PlayerController>().nowControl = false;
            }
        }

        if(!miniGoal.activeSelf && !miniGoalPost.activeSelf)
        {
            miniGoalPost.SetActive(true);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.CompareTag("Ball"))
        {
            GameObject ball = other.gameObject;

            if (!ball.GetComponent<BallController>().isScored)
            {
                ball.GetComponent<BallController>().isScored = true;
                if(ball.GetComponent<BallController>().character.GetComponent<PlayerController>().teamIndex == 1)
                {
                    om.goal1++;
                    team1Goal.text = om.goal1.ToString();
                    whistleSource.Play();
                    StartCoroutine(MoveBall(ball));
                    StartCoroutine(MoveCharacters());
                    StartCoroutine(SpawnPost());
                    StartCoroutine(DestroyObjects());

                }
                if (ball.GetComponent<BallController>().character.GetComponent<PlayerController>().teamIndex == 2)
                {
                    om.goal2++;
                    team2Goal.text = om.goal2.ToString();
                    whistleSource.Play();
                    StartCoroutine(MoveBall(ball));
                    StartCoroutine(MoveCharacters());
                    StartCoroutine(SpawnPost());
                    StartCoroutine(DestroyObjects());
                }
            }
        }
    
    }
    
    
    private IEnumerator MoveBall(GameObject ball)
    {
        yield return new WaitForSeconds(3);
        ball.GetComponent<BallController>().nowDribble = false;
        ball.GetComponent<BallController>().nowFree = true;
        ball.GetComponent<BallController>().nowGround = false;
        ball.GetComponent<BallController>().ballRigid.velocity = new Vector3(0, 0, 0);
        //ball.transform.position = new Vector3(28f, 3f, -9f);
        ball.transform.position = new Vector3(-11f, 3f, 200f);
        ball.GetComponent<BallController>().isScored = false;

        ball.GetComponent<Collider>().material.bounciness = 0.8f;
    }

    private IEnumerator SpawnPost()
    {
        yield return new WaitForSeconds(3);
        goalPost.SetActive(true);
    }
    
    private IEnumerator MoveCharacters()
    {
        yield return new WaitForSeconds(3);
        for (int i = 0; i < 6; i++)
        {
            charPrefabs[i].transform.position = firstTrans_1[i].position;
            charPrefabs[i].gameObject.GetComponent<PlayerController>().speed = 75.0f;
            //if (charPrefabs[i].gameObject.GetComponent<PlayerController>().nowControl)
            //{

            //}

            charPrefabs[i].gameObject.GetComponent<PlayerController>().attack_def_index = 0;
        }
    }
    
    private IEnumerator DestroyObjects()
    {
        yield return new WaitForSeconds(3);
        DestroyObject();
        Destroybaricade();
        
        
        
    }

    void DestroyObject()
    {
        for(int i=0; i<objectList.Count; i++)
        {
            if(objectList[i].tag == "Sun")
            {
                objectList[i].GetComponent<Objectdestroy>().ReturnSpeed();
            }
            Destroy(objectList[i]);
        }
        objectList.Clear();
    }

    void Destroybaricade()
    {
        baricades[0].SetActive(false);
        baricades[1].SetActive(false);
        miniGoalPost.SetActive(true);
        miniGoal.SetActive(false);
    }

    public void LoadRobbyScene()
    {
        SceneManager.LoadScene("Main Menu (Desktop)");
    }
}
