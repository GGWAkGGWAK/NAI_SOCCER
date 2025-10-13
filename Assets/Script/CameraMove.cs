using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject character;

    Vector3 startPosition;          //시작 위치
    Vector3 cameraPositon;          //카메라 위치
    Vector3 startCharacterPosition; //캐릭터 시작 위치
    Vector3 characterPosition;      //캐릭터 위치

    public float cameraSpeed;

    void Start()
    {
        startPosition = transform.position;
        startCharacterPosition = character.transform.position;
    }

    void Update()
    {
        cameraPositon = transform.position;
        characterPosition = character.transform.position;

      
        CameraMoving();
    }

    public void CameraMoving()
    {
        Vector3 viewVector = startCharacterPosition - startPosition;  //초기 카메라와 캐릭터 사이 벡터
        Vector3 nowViewVector = characterPosition - cameraPositon;    //현재 카메라와 캐릭터 사이 벡터
        float viewDistance = viewVector.magnitude; // 기준 거리
        float nowViewDistance = nowViewVector.magnitude; // 현재거리

        float speedCorrection = 0.5f;
        float moveSpeed = (nowViewDistance - viewDistance) * speedCorrection; //이동 속도

        Vector3 moveVector = nowViewVector - viewVector; //이동 백터
        moveVector = new Vector3(moveVector.x, moveVector.y, moveVector.z);
        transform.position = transform.position + ((moveVector) * Time.deltaTime); // 이동

    }
}
