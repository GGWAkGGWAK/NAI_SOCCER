using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BuffSpon : MonoBehaviour
{
    public GameObject rangeObject;
    public GameObject Sun;
    public GameObject Rain;
    public GameObject Moon;
    public GameObject Wind;
    BoxCollider rangeCollider;
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(RandomRespawn_Coroutine());
        rangeCollider = rangeObject.GetComponent<BoxCollider>();
        Sun.gameObject.SetActive(true);
    }
    Vector3 Return_RandomPosition()
    {
        Vector3 originPosition = rangeObject.transform.position;
        // 콜라이더의 사이즈를 가져오는 bound.size 사용
        float range_X = rangeCollider.bounds.size.x;
        float range_Z = rangeCollider.bounds.size.z;

        range_X = Random.Range((range_X / 2) * -1, range_X / 2);
        range_Z = Random.Range((range_Z / 2) * -1, range_Z / 2);
        Vector3 RandomPostion = new Vector3(range_X, 0f, range_Z);

        Vector3 respawnPosition = originPosition + RandomPostion;
        return respawnPosition;
    }
    // Update is called once per frame
    IEnumerator RandomRespawn_Coroutine()
    {
        while (true)
        {
            yield return new WaitForSeconds(3f);
            // BB
            // 생성 위치 부분에 위에서 만든 함수 Return_RandomPosition() 함수 대입
            GameObject instantSun = Instantiate(Sun, Return_RandomPosition(), Quaternion.identity);
        }
    }
    void Update()
    {
        
    }
    //private void OnCollisionEnter(Collision collision)
    //{
    //    if(collision.gameObject.tag == "Character")
    //    {
    //        Destroy(collision.gameObject);
    //    }
    //}
}
