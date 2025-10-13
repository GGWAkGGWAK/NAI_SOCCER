using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReSpawn3 : MonoBehaviour
{
    public GameObject[] charPrefabs;
    public GameObject[] otherCharPrefabs;

    public GameObject player;

    public GameObject ball;


    // Start is called before the first frame update
    void Start()
    {

        player = Instantiate(charPrefabs[(int)DataMgr.instance.currentCharacter_3]);
        //player = Instantiate(charPrefabs[(int)DataMgr.instance.currentCharacter_1]);
        //player = Instantiate(charPrefabs[(int)DataMgr.instance.currentCharacter_2]);
        //player = Instantiate(charPrefabs[(int)DataMgr.instance.currentCharacter_3]);
        //player = Instantiate(charPrefabs[(int)DataMgr.instance.currentCharacter_4]);
        //player = Instantiate(charPrefabs[(int)DataMgr.instance.currentCharacter_5]);
        player.transform.position = transform.position;

        //nowControl = player;
        //nowControl = true;
        player.GetComponent<PlayerController>().teamIndex = 2;
        player.GetComponent<PlayerController>().nowControl = false;
        player.GetComponent<PlayerController>().respawnIndex = 3;
        //ball.GetComponent<BallController>().allcharacter[3] = player;
    }


    // Update is called once per frame
    void Update()
    {
        
    }

}
