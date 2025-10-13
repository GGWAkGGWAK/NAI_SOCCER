using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
public class Goal : MonoBehaviour
{
    public Text scoreText;
    int score = 0;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        scoreText.text = "Score : " + score.ToString();
    }
    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.tag == "Player")
        {
            score++;
        }
    }
}
