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
            Debug.Log("���� ��ҵ�!");
            if (other.gameObject.GetComponent<BallController>().character.GetComponent<PlayerController>().teamIndex == 1)
            {
                Debug.Log("�� 1�� ���� ��Ҵ�!");
                areaColor.material.color = new Color(1, 0.5f, 0, 1);
            }

            else if (other.gameObject.GetComponent<BallController>().character.GetComponent<PlayerController>().teamIndex == 2)
            {
                Debug.Log("�� 2�� ���� ��Ҵ�!");
                areaColor.material.color = new Color(0.5f, 0, 1, 1);
            }
            else
            {
                Debug.Log("���� ������ ����!");
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
