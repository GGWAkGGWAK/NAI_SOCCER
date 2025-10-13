using System.Collections;
using System.Collections.Generic;
using UnityEngine;
public class objectManager : MonoBehaviour
{
    [SerializeField]
    GameObject[] spawnerList;
    
    private int count = 0;
    private bool isCoroutineRunning = false;

    public int goal1;
    public int goal2;

    AudioSource audioSource;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        

        if (GetComponent<CountTime>().setTime <= 297)
        {
            if (!isCoroutineRunning &&
           !spawnerList[0].activeSelf &&
           !spawnerList[1].activeSelf &&
           !spawnerList[2].activeSelf &&
           !spawnerList[3].activeSelf)
            {
                StartCoroutine(Spawn());

                //RandSpawn();
            }
            
                

        }
        

    }

    void RandSpawn()
    {
        
        List<int> randList = new List<int>() { 0, 1, 2, 3 };
        for (int i = 0; i < 3; i++)
        {
            int rand = Random.Range(0, randList.Count);
            spawnerList[randList[rand]].SetActive(true);
            print(randList[rand]);
            randList.RemoveAt(rand);
            
        }
    }


    IEnumerator Spawn()
    {
        isCoroutineRunning = true;
        yield return new WaitForSeconds(2f);
        audioSource.Play();
        RandSpawn();
        isCoroutineRunning = false;
    }

}
