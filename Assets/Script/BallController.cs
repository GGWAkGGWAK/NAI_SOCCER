using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    Vector3 ballPos;      //���� ��ġ
    Vector3 passVec;      //�н� ����
    Vector3 characterFoward; //ĳ���Ͱ� �ٶ󺸴� ����

    Vector3 rightShot; // ������ �������� ������
    Vector3 leftShot; //���� �������� ������

    [SerializeField]
    public GameObject character; //�帮���ϴ� ĳ����
    [SerializeField]
    GameObject camera;  //ī�޶�
    [SerializeField]
    public GameObject[] allcharacter;
    [SerializeField]
    GameObject chargingBar;

    public bool isCharging;
    int characterTimindex;

    //���� ����
    public bool nowDribble; // �帮�����
    public bool nowFree; // ��������
    public bool nowGround; // �ٴ� üũ
    public bool nowinvincibility; // ��������

    [SerializeField]
    public int attack_def_index; //���� �ε���  0=�������� , �帮�� �����϶� 1=���� 2=����

    public bool isScored;
    bool _isOnNet;

    int shotDir; // �������� ����

    [SerializeField]
    float passFos; //�н� ��
    [SerializeField]
    float rigidDrag; // ��������

    private TrailRenderer trail;
    public Rigidbody ballRigid;
    Collider _ball;

    float keyDownTime_R;
    float chargingTime;

    public float dribbleTime;
    public int latestTeamIndex;

    [SerializeField]
    bool debugMode;

    AudioSource audioSource;
    void Start()
    {
        // ���Ǻ�
        ballRigid = GetComponent<Rigidbody>();
        _ball = GetComponent<Collider>();
        trail = GetComponent<TrailRenderer>();

        allcharacter = GameObject.FindGameObjectsWithTag("Character");
        passFos = 200f;
        rigidDrag = 0.5f;
        shotDir = 2;

        nowFree = true;
        nowDribble = false;

        ballPos = transform.position;
        ballRigid.angularDrag = rigidDrag;

        latestTeamIndex = 0;

        //allcharacter = new GameObject[6];

        audioSource = this.gameObject.GetComponent<AudioSource>();
    }

    void Update()
    {
        

        /*if (isCharging)
        {
            chargingBar.gameObject.SetActive(true);
            if(chargingTime < 2)
            {
                chargingTime += Time.deltaTime;
            }
            else if(chargingTime >= 2)
            {
                chargingTime = 2;
            }
        }
        else if(!isCharging) { chargingTime = 0; chargingBar.gameObject.SetActive(false); }
        chargingBar.transform.position = new Vector3(transform.position.x ,transform.position.y -6, transform.position.z);
        chargingBar.transform.parent = character.transform;
        chargingBar.transform.localScale = new Vector3(chargingTime * 0.5f, 0.01f,  0.2f);*/





        if (character!=null) // �帮���ϴ� ĳ���Ͱ� �ִٸ�?
        {
            if (nowDribble) //�帮�� ���¶��?
            {
                dribbleTime += Time.deltaTime;
                BallDribble(); //�帮��
                transform.position = ballPos;
            }
            rightShot = (character.transform.forward + character.transform.right).normalized;
            leftShot = (character.transform.forward + ( -1 * character.transform.right)).normalized;
        }
        
        if(attack_def_index == 1)   // ���� �����϶�
        {
            if(Input.GetKeyDown(KeyCode.X))
            {
                if(character.GetComponent<PlayerController>().CheckDistance() != null) // ������ ĳ���Ͱ� ������
                {
                    if(nowDribble) //�帮�� ���̶��
                    {
                        DefencePass(); // �н�
                    }
                }
            }

            if(Input.GetKeyDown(KeyCode.Z))
            {
                if(nowDribble)
                {
                    DefenceGuard();
                }
            }
        }

        if(attack_def_index == 2)     // ���� �����϶�
        {
            if (Input.GetKeyDown(KeyCode.X)) //xŰ�� ��������
            {
                if (nowDribble) //�帮�� ���̶��
                {
                    StartCoroutine(BallPass()); // �н�
                }
            }

            if (Input.GetKeyDown(KeyCode.C)) //cŰ�� ��������
            {
                if (nowDribble) //�帮�� ���̶��
                {
                    StartCoroutine(BallLongPass()); // ���н�
                }
            }

            if (Input.GetKeyDown(KeyCode.Z)) //zŰ�� ��������
            {
                if (nowDribble) //�帮�� ���̶��
                {
                    StartCoroutine(BallShortPass()); // ª���н�
                }
            }

            if (Input.GetKeyDown(KeyCode.E)) //eŰ�� ��������
            {
                if (nowDribble) //�帮�� ���̶��
                {
                    StartCoroutine(BallRightPass()); // ������
                }
            }

            if (Input.GetKeyDown(KeyCode.Q)) //qŰ�� ��������
            {
                if (nowDribble) //�帮�� ���̶��
                {
                    StartCoroutine(BallLeftPass()); // �ް���
                }
            }

            /*if(Input.GetKey(KeyCode.R))// rŰ�� ������ ���� ��
            {
                keyDownTime_R += Time.deltaTime;
            }
            if(Input.GetKeyUp(KeyCode.R))
            {
                Debug.Log(keyDownTime_R);
                if(keyDownTime_R >= 2) { keyDownTime_R = 2; }
                passFos += passFos * (keyDownTime_R - 1) * 0.2f;
                StartCoroutine(ChargingShot());
                keyDownTime_R = 0;
            }*/
        }
        
        Wind(shotDir);

        

        if (debugMode) //����� ����ϋ�
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(attack_def_index == 1) //���� ����
                {
                    attack_def_index = 2;
                }
                else if (attack_def_index == 2)
                {
                    attack_def_index = 1;
                }
            }
        }
    }

    private void OnCollisionEnter(Collision collision)
    {
        if(collision.gameObject.tag == "Character") //ĳ���Ϳ� �浹��
        {
            allcharacter = GameObject.FindGameObjectsWithTag("Character");
            if (nowFree)  //���� �����ϋ�
            {
                if(!collision.gameObject.GetComponent<PlayerController>().nowControl) // ��Ʈ�� ������ ���� ĳ���Ϳ� �浹
                {
                    if (character != null) // ���� ������ �帮�� ���� ĳ���Ͱ� �־��ٸ�
                    {
                        if(collision.gameObject.GetComponent<PlayerController>().teamIndex !=
                                        character.gameObject.GetComponent<PlayerController>().teamIndex)// ������ �ٸ���
                        {
                            for (int i = 0; i < 6; i++)
                            {
                                if (allcharacter[i].GetComponent<PlayerController>().nowControl)
                                {
                                    if (collision.gameObject.GetComponent<PlayerController>().teamIndex == allcharacter[i].GetComponent<PlayerController>().teamIndex)
                                    {
                                        allcharacter[i].GetComponent<PlayerController>().nowControl = false;
                                        allcharacter[i].GetComponent<PlayerController>().AiMoveDirInGame();
                                    }
                                }
                            }
                            AttackDefenceShift(collision);
                            ChangeCharacter(collision);
                        }
                        else if(collision.gameObject.GetComponent<PlayerController>().teamIndex ==
                                        character.gameObject.GetComponent<PlayerController>().teamIndex)
                        {
                            character.GetComponent<PlayerController>().nowControl = false; //������ �帮���ϴ� ĳ���͸� AI��
                            character.GetComponent<PlayerController>().AiMoveDirInGame();
                            AttackDefenceShift(collision);
                            ChangeCharacter(collision);
                            character.GetComponent<PlayerController>().nowControl = true;    // ���� ���� ĳ���͸� ��Ʈ�ѻ��·� ����
                            nowDribble = true;
                        }
                    }
                    if( character == null) // ���� ������ �帮�� ���� ĳ���Ͱ� �����ٸ�
                    {
                        allcharacter[0].GetComponent<PlayerController>().nowControl = false; //��� ai��
                        allcharacter[1].GetComponent<PlayerController>().nowControl = false; //��� ai��
                        allcharacter[2].GetComponent<PlayerController>().nowControl = false; //��� ai��
                        allcharacter[3].GetComponent<PlayerController>().nowControl = false; //��� ai��
                        allcharacter[4].GetComponent<PlayerController>().nowControl = false; //��� ai��
                        allcharacter[5].GetComponent<PlayerController>().nowControl = false; //��� ai��
                        AttackDefenceShift(collision);
                        ChangeCharacter(collision); //�浹�� ĳ���ʹ� �ٽ� ��Ʈ�ѷ�
                        nowDribble = true;
                    }
                }
                if(collision.gameObject.GetComponent<PlayerController>().nowControl) // ��Ʈ�� ���� ĳ���Ϳ� �浹
                {
                    if(character == null)                                            //������ �帮������ ĳ���Ͱ� ���ٸ�
                    {
                        AttackDefenceShift(collision);
                        ChangeCharacter(collision);
                        nowDribble = true; 
                    }
                    if(character != null)                                            //������ �帮������ ĳ���Ͱ� ������
                    {
                        if(collision.gameObject == character)                        //�浹�� ĳ���Ͱ� ������ �帮�� ���� ĳ���Ͷ��
                        {
                            nowDribble = true;
                        }
                        else if(collision.gameObject.GetComponent<PlayerController>().teamIndex == characterTimindex) //�� ĳ������ ���� ���ٸ�
                        {
                            //AttackDefenceShift(collision);
                            ChangeCharacter(collision);
                            nowDribble = true;
                        }
                        else if(collision.gameObject.GetComponent<PlayerController>().teamIndex != characterTimindex) //�� ĳ������ ���� �ٸ��ٸ�
                        {
                            AttackDefenceShift(collision);
                            ChangeCharacter(collision);
                            nowDribble = true;
                        }
                    }
                }
            }
            if(nowDribble)//�帮�� �����϶�
            {
                if(!nowinvincibility) //�������°� �ƴҶ�
                {
                    //���⿡ Ȯ���� �־����
                    if (collision.gameObject.GetComponent<PlayerController>().teamIndex !=
                                        character.gameObject.GetComponent<PlayerController>().teamIndex) //�浹�� ĳ���Ͱ� �帮������ ĳ���Ϳ� ���� �ٸ���
                    {
                        for(int i = 0; i < 6; i++)
                        {
                            if(allcharacter[i].GetComponent<PlayerController>().nowControl)
                            {
                                if(collision.gameObject.GetComponent<PlayerController>().teamIndex == allcharacter[i].GetComponent<PlayerController>().teamIndex)
                                {
                                    allcharacter[i].GetComponent<PlayerController>().nowControl = false;
                                    allcharacter[i].GetComponent<PlayerController>().AiMoveDirInGame();
                                }
                            }
                        }
                        character.GetComponent<PlayerController>().nowControl = true;
                        AttackDefenceShift(collision);
                        ChangeCharacter(collision);
                    }
                    else if(collision.gameObject.GetComponent<PlayerController>().teamIndex ==
                                        character.gameObject.GetComponent<PlayerController>().teamIndex) //���� ���̸�
                    {
                        character.GetComponent<PlayerController>().nowControl = false;
                        character.GetComponent<PlayerController>().AiMoveDirInGame();
                        AttackDefenceShift(collision);
                        ChangeCharacter(collision);
                    }
                }
                
            }
        }

        if (collision.gameObject.CompareTag("GoalPost")) //��뿡 �ε����� ���
        {
            audioSource.Play();
            _ball.material.bounciness *= 1.0f;
            _isOnNet = true;
            StartCoroutine(MoveBallOnNet());
            return;
        }

        if(collision.gameObject.tag == "Ground") // �ٴڰ� �ε����� ���
        {
            nowGround = true;
            shotDir = 2;
        }

        if(collision.gameObject.tag == "AiMoveWall")
        {
            audioSource.Play();
        }
        
        if(collision.gameObject.tag == "Obj")
        {
            audioSource.Play();
        }
        _ball.material.bounciness = 0.8f;
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("GoalPost"))
        {
            _isOnNet = false;
        }

        if (collision.gameObject.tag == "Ground")
        {
            nowGround = false;
        }
    }

   
    void ChangeCharacter(Collision collision)
    {
        character = collision.gameObject;                                // �帮�� ���� ĳ���� ����
        latestTeamIndex = characterTimindex;
        characterTimindex = character.GetComponent<PlayerController>().teamIndex;
        //camera.GetComponent<CameraMove>().character = character;
        character.GetComponent<PlayerController>().nowControl = true;

        if (latestTeamIndex != characterTimindex)
            dribbleTime = 0;
    }

    public void AttackDefenceShift(Collision collision)     //�г�Ƽ ���϶���
    {
        //���� ����

        if(!collision.gameObject.GetComponent<PlayerController>().nowInPenalty) //�г�Ƽ ������ ���϶�
        {
            if (collision.gameObject.GetComponent<PlayerController>().attack_def_index == 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (allcharacter[i].GetComponent<PlayerController>().teamIndex == collision.gameObject.GetComponent<PlayerController>().teamIndex) // ������ ģ���� ���� ������
                    {
                        allcharacter[i].GetComponent<PlayerController>().attack_def_index = 2; //����
                    }
                    else if (allcharacter[i].GetComponent<PlayerController>().teamIndex != collision.gameObject.GetComponent<PlayerController>().teamIndex)// �ٸ���
                    {
                        allcharacter[i].GetComponent<PlayerController>().attack_def_index = 1; //����
                    }
                }
            }
            if (character != null)
            {
                if (collision.gameObject.GetComponent<PlayerController>().teamIndex != character.GetComponent<PlayerController>().teamIndex) //���� ĳ���Ϳ� �ٸ����̸�
                {
                    for (int i = 0; i < 6; i++)
                    {
                        if (allcharacter[i].GetComponent<PlayerController>().attack_def_index == 1)
                        {
                            allcharacter[i].GetComponent<PlayerController>().attack_def_index = 2;
                        }
                        else if (allcharacter[i].GetComponent<PlayerController>().attack_def_index != 1)
                        {
                            allcharacter[i].GetComponent<PlayerController>().attack_def_index = 1;
                        }
                    }
                }
            }
        }
    }

    public void AttackDefenceShift_InPen()
    {
        if(!character.GetComponent<PlayerController>().nowInPenalty) //�� �����ڰ� �г�Ƽ ������ ���϶�
        {
            for (int i = 0; i < 6; i++)
            {
                if (allcharacter[i].GetComponent<PlayerController>().attack_def_index == 1)
                {
                    allcharacter[i].GetComponent<PlayerController>().attack_def_index = 2;
                }
                else if (allcharacter[i].GetComponent<PlayerController>().attack_def_index != 1)
                {
                    allcharacter[i].GetComponent<PlayerController>().attack_def_index = 1;
                }
            }
        } 
    }
    /////////////// ���� ��ų ///////////////

    void BallDribble() //�帮�� �Լ�
    {
        ballPos = (character.transform.position + (character.transform.forward * 30)) + new Vector3(0,5,0);
        nowFree = false;
    }

    IEnumerator BallPass() //�н� �Լ�
    {
        PlayerAttack();
        yield return new WaitForSeconds(0.1f);
        passVec = (character.transform.forward * passFos) + new Vector3(0, 20, 0); // �н� ���� = ĳ���ͱ��� ������ * �� + ����
        ballRigid.AddForce(passVec, ForceMode.Impulse);
        nowDribble = false;
        nowFree = true;
        nowGround = false;
    }
    
    IEnumerator BallLongPass() //���н�
    {
        PlayerAttack();
        yield return new WaitForSeconds(0.1f);
        passVec = ((character.transform.forward * passFos) + new Vector3(0, 50, 0)) * 1.2f; // �н� ���� = ĳ���ͱ��� ������ * �� + ����
        ballRigid.AddForce(passVec, ForceMode.Impulse);
        nowDribble = false;
        nowFree = true;
        nowGround = false;
    }

    IEnumerator BallShortPass() //���н�
    {
        PlayerAttack();
        yield return new WaitForSeconds(0.1f);
        passVec = ((character.transform.forward * passFos) + new Vector3(0, 50, 0)) * 0.8f; // �н� ���� = ĳ���ͱ��� ������ * �� + ����
        ballRigid.AddForce(passVec, ForceMode.Impulse);
        nowDribble = false;
        nowFree = true;
        nowGround = false;
    }

    IEnumerator ChargingShot() //��¡ ��
    {
        PlayerAttack();
        yield return new WaitForSeconds(0.1f);
        passVec = ((character.transform.forward * passFos) + new Vector3(0, 50, 0)); // �н� ���� = ĳ���ͱ��� ������ * �� + ����
        ballRigid.AddForce(passVec, ForceMode.Impulse);
        nowDribble = false;
        nowFree = true;
        nowGround = false;
    }

    IEnumerator BallRightPass() //��������
    {
        PlayerAttack();
        yield return new WaitForSeconds(0.1f);
        passVec = ((rightShot * passFos) + new Vector3(0, 50, 0)) * 1.2f; // �н� ���� = ĳ���ͱ��� ������ * �� + ����
        ballRigid.AddForce(passVec, ForceMode.Impulse);
        ballRigid.AddTorque(Vector3.up * 100, ForceMode.Impulse);
        shotDir = 0;
        nowDribble = false;
        nowFree = true;
        nowGround = false;
    }

    IEnumerator BallLeftPass() //�ް���
    {
        PlayerAttack();
        yield return new WaitForSeconds(0.1f);
        passVec = ((leftShot * passFos) + new Vector3(0, 50, 0)) * 1.2f; // �н� ���� = ĳ���ͱ��� ������ * �� + ����
        ballRigid.AddForce(passVec, ForceMode.Impulse);
        shotDir = 1;
        nowDribble = false;
        nowFree = true;
        nowGround = false;
    }

    public void ShootCall(float chargingTime)
    {
        passFos += passFos * (chargingTime - 1) * 0.2f;
        StartCoroutine(ChargingShot());
    }

    public void LeftShootCall(float chargingTime)
    {
        passFos += passFos * (chargingTime - 1) * 0.2f;
        StartCoroutine(BallLeftPass());
    }

    public void RightShootCall(float chargingTime)
    {
        passFos += passFos * (chargingTime - 1) * 0.2f;
        StartCoroutine(BallRightPass());
    }

    public void ChargingShootCall(float chargingTime)
    {
        passFos += passFos * (chargingTime - 1) * 0.2f;
        StartCoroutine(ChargingShot());
    }

    public void PassCall()
    {
        if (character.GetComponent<PlayerController>().CheckDistance() != null) // ������ ĳ���Ͱ� ������
        {
            if (nowDribble) //�帮�� ���̶��
            {
                DefencePass(); // �н�
            }
        }
    }

    void Wind(int dir) //�������� ���� �ٶ� dir = ���� ���� ������ = 0 ���� = 1
    {
        if(!nowGround) // ���� ������
        {
            if (dir == 0) // ������ �������� �ϰ��
            {
                ballRigid.AddForce(Vector3.left * 100, ForceMode.Force); // �������� ���ۿ�
            }
            if (dir == 1)
            {
                ballRigid.AddForce(Vector3.right * 100, ForceMode.Force);// ���������� ���ۿ�
            }
        }
    }

    /////////////// ���� ��ų ///////////////

    void DefencePass() // ����� ��뿡�� �н�
    {
        if(character.GetComponent<PlayerController>().hitCharacterList != null) // ������ ĳ���Ͱ� �ִٸ�
        {
            Vector3 position = character.GetComponent<PlayerController>().CheckDistance().transform.position; //�н��� ĳ���� ��ġ
            Vector3 passDirection = position - character.transform.position; // ���⺤��
            Vector3 direction = passDirection.normalized; // ���⺤���� ��������
            passVec = direction * passFos; //�н����� = ���� x ��
            ballRigid.AddForce(passVec, ForceMode.Impulse);
            nowDribble = false;
            nowFree = true;
            nowGround = false;
        }
        
    }

    public void DefenceGuard() //���� ����
    {
        character.GetComponent<PlayerController>().speed = 50; // �ӵ�����
        nowinvincibility = true; //�������·� ����
        StartCoroutine(GuardTimer());
    }

    void PlayerAttack()
    {
        character.GetComponent<PlayerController>().StartCoroutinAttack();
    }

    public IEnumerator GuardTimer() //���� Ÿ�̸�
    {
        yield return new WaitForSeconds(3.0f);
        character.GetComponent<PlayerController>().speed = 75; //�ӵ� ����
        nowinvincibility = false; // �������� ����
    }

    private IEnumerator MoveBallOnNet() //��Ʈ �浹 ����
    {
        int loop = 0;
        while (_isOnNet && loop < 8)
        {
            yield return new WaitForSeconds(0.5f);
            loop++;
        }
    }

    

    

}

