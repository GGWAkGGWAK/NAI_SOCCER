using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Spawn : MonoBehaviour
{
    public GameObject[] prefabs;

    private BoxCollider area;

    public int count = 10;


    float timer;
    int waitTime;

    GoalCheck gc;
    // Start is called before the first frame update
    void Start()
    {
        gc = GameObject.FindGameObjectWithTag("GoalCheck").GetComponent<GoalCheck>();
        timer = 0.0f;
        waitTime = 3;
        area = GetComponent<BoxCollider>();
        if (prefabs[0].gameObject.tag == "Sun")
        {
            Spawner();
            area.enabled = false;
        }
        else if (prefabs[0].gameObject.tag == "Wind")
        {
            Spawner();
            area.enabled = false;
        }




    }
    void Update()
    {
        timer += Time.deltaTime;
        if(prefabs[0].gameObject.tag == "Lightning")
        {
            forSpawn();
        }
        
    }

    private Vector3 GetRandomPosition()
    {
        Vector3 basePos = transform.position;
        Vector3 size = area.size;

        float posX = basePos.x + Random.Range(-size.x/2f, size.x/2f);
        //float posY = basePos.y + Random.Range(-size.y / 2f, size.y / 2f);
        float posY = 3.0f;
        float posZ = basePos.z + Random.Range(-size.z/2f, size.z/2f);

        Vector3 spawnPos = new Vector3(posX, posY, posZ);

        return spawnPos;
    }

    private void Spawner()
    {
        int selection = Random.Range(0, prefabs.Length);

        GameObject selectedPrefab = prefabs[selection];

        Vector3 spawnPos = GetRandomPosition();

        GameObject instance = Instantiate(selectedPrefab, spawnPos, Quaternion.identity);
        if(instance.gameObject.tag == "Sun" || instance.gameObject.tag == "Wind")
        {
            gc.objectList.Add(instance);
        }

        Debug.Log("오브젝트 생성!");
    }

    private void forSpawn()
    {
        for (int i = 0; i < count; i++)
        {
            if (timer > waitTime)
            {
                Spawner();
                timer = 0;
            }
            
        }
        area.enabled = false;
    }
}
