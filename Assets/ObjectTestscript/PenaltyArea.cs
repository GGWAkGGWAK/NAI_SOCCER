using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PenaltyArea : MonoBehaviour
{
    Renderer areaColor;
    // Start is called before the first frame update
    void Start()
    {
        areaColor = gameObject.GetComponent<Renderer>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    
    private void OnTriggerEnter(Collider other)
    {
        if(other.gameObject.tag == "Ball")
        {
            Debug.Log("공과 닿았따!");
            if (other.gameObject.GetComponent<BallController>().character.GetComponent<PlayerController>().teamIndex == 1)
            {
                Debug.Log("팀 1의 공과 닿았다!");
                areaColor.material.color = new Color(1, 0.5f, 0, 1);
            }

            else if (other.gameObject.GetComponent<BallController>().character.GetComponent<PlayerController>().teamIndex == 2)
            {
                Debug.Log("팀 2의 공과 닿았다!");
                areaColor.material.color = new Color(0.5f, 0, 1, 1);
            }
            else
            {
                Debug.Log("공의 주인이 없다!");
                areaColor.material.color = new Color(0, 0, 0, 1);
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Ball")
            areaColor.material.color = Color.white;
    }

}
