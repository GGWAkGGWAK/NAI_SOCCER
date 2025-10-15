using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMove : MonoBehaviour
{
    public GameObject character;

    Vector3 startPosition;          //���� ��ġ
    Vector3 cameraPositon;          //ī�޶� ��ġ
    Vector3 startCharacterPosition; //ĳ���� ���� ��ġ
    Vector3 characterPosition;      //ĳ���� ��ġ

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
        Vector3 viewVector = startCharacterPosition - startPosition;  //�ʱ� ī�޶�� ĳ���� ���� ����
        Vector3 nowViewVector = characterPosition - cameraPositon;    //���� ī�޶�� ĳ���� ���� ����
        float viewDistance = viewVector.magnitude; // ���� �Ÿ�
        float nowViewDistance = nowViewVector.magnitude; // ����Ÿ�

        float speedCorrection = 0.5f;
        float moveSpeed = (nowViewDistance - viewDistance) * speedCorrection; //�̵� �ӵ�

        Vector3 moveVector = nowViewVector - viewVector; //�̵� ����
        moveVector = new Vector3(moveVector.x, moveVector.y, moveVector.z);
        transform.position = transform.position + ((moveVector) * Time.deltaTime); // �̵�

    }
}
