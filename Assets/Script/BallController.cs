using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BallController : MonoBehaviour
{
    Vector3 ballPos;      //공의 위치
    Vector3 passVec;      //패스 벡터
    Vector3 characterFoward; //캐릭터가 바라보는 방향

    Vector3 rightShot; // 오른쪽 감아차기 보정값
    Vector3 leftShot; //왼쪽 감아차기 보정값

    [SerializeField]
    public GameObject character; //드리블하는 캐릭터
    [SerializeField]
    GameObject camera;  //카메라
    [SerializeField]
    public GameObject[] allcharacter;
    [SerializeField]
    GameObject chargingBar;

    public bool isCharging;
    int characterTimindex;

    //공의 상태
    public bool nowDribble; // 드리블상태
    public bool nowFree; // 자유상태
    public bool nowGround; // 바닥 체크
    public bool nowinvincibility; // 무적상태

    [SerializeField]
    public int attack_def_index; //공수 인덱스  0=자유상태 , 드리블 상태일때 1=수비 2=공격

    public bool isScored;
    bool _isOnNet;

    int shotDir; // 감아차기 방향

    [SerializeField]
    float passFos; //패스 힘
    [SerializeField]
    float rigidDrag; // 공기저항

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
        // 정의부
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





        if (character!=null) // 드리블하는 캐릭터가 있다면?
        {
            if (nowDribble) //드리블 상태라면?
            {
                dribbleTime += Time.deltaTime;
                BallDribble(); //드리블
                transform.position = ballPos;
            }
            rightShot = (character.transform.forward + character.transform.right).normalized;
            leftShot = (character.transform.forward + ( -1 * character.transform.right)).normalized;
        }
        
        if(attack_def_index == 1)   // 수비 상태일때
        {
            if(Input.GetKeyDown(KeyCode.X))
            {
                if(character.GetComponent<PlayerController>().CheckDistance() != null) // 감지된 캐릭터가 있을때
                {
                    if(nowDribble) //드리블 중이라면
                    {
                        DefencePass(); // 패스
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

        if(attack_def_index == 2)     // 공격 상태일때
        {
            if (Input.GetKeyDown(KeyCode.X)) //x키를 눌렀을때
            {
                if (nowDribble) //드리블 중이라면
                {
                    StartCoroutine(BallPass()); // 패스
                }
            }

            if (Input.GetKeyDown(KeyCode.C)) //c키를 눌렀을때
            {
                if (nowDribble) //드리블 중이라면
                {
                    StartCoroutine(BallLongPass()); // 긴패스
                }
            }

            if (Input.GetKeyDown(KeyCode.Z)) //z키를 눌렀을때
            {
                if (nowDribble) //드리블 중이라면
                {
                    StartCoroutine(BallShortPass()); // 짧은패스
                }
            }

            if (Input.GetKeyDown(KeyCode.E)) //e키를 눌렀을때
            {
                if (nowDribble) //드리블 중이라면
                {
                    StartCoroutine(BallRightPass()); // 오감차
                }
            }

            if (Input.GetKeyDown(KeyCode.Q)) //q키를 눌렀을때
            {
                if (nowDribble) //드리블 중이라면
                {
                    StartCoroutine(BallLeftPass()); // 왼감차
                }
            }

            /*if(Input.GetKey(KeyCode.R))// r키를 누르고 있을 떼
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

        

        if (debugMode) //디버그 모드일떄
        {
            if(Input.GetKeyDown(KeyCode.Space))
            {
                if(attack_def_index == 1) //공수 변경
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
        if(collision.gameObject.tag == "Character") //캐릭터와 충돌시
        {
            allcharacter = GameObject.FindGameObjectsWithTag("Character");
            if (nowFree)  //자유 상태일떄
            {
                if(!collision.gameObject.GetComponent<PlayerController>().nowControl) // 컨트롤 중이지 않은 캐릭터와 충돌
                {
                    if (character != null) // 만약 이전에 드리블 중인 캐릭터가 있었다면
                    {
                        if(collision.gameObject.GetComponent<PlayerController>().teamIndex !=
                                        character.gameObject.GetComponent<PlayerController>().teamIndex)// 두팀이 다르면
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
                            character.GetComponent<PlayerController>().nowControl = false; //이전에 드리블하던 캐릭터를 AI로
                            character.GetComponent<PlayerController>().AiMoveDirInGame();
                            AttackDefenceShift(collision);
                            ChangeCharacter(collision);
                            character.GetComponent<PlayerController>().nowControl = true;    // 공을 받은 캐릭터를 컨트롤상태로 변경
                            nowDribble = true;
                        }
                    }
                    if( character == null) // 만약 이전에 드리블 중인 캐릭터가 없었다면
                    {
                        allcharacter[0].GetComponent<PlayerController>().nowControl = false; //모두 ai로
                        allcharacter[1].GetComponent<PlayerController>().nowControl = false; //모두 ai로
                        allcharacter[2].GetComponent<PlayerController>().nowControl = false; //모두 ai로
                        allcharacter[3].GetComponent<PlayerController>().nowControl = false; //모두 ai로
                        allcharacter[4].GetComponent<PlayerController>().nowControl = false; //모두 ai로
                        allcharacter[5].GetComponent<PlayerController>().nowControl = false; //모두 ai로
                        AttackDefenceShift(collision);
                        ChangeCharacter(collision); //충돌한 캐릭터는 다시 컨트롤로
                        nowDribble = true;
                    }
                }
                if(collision.gameObject.GetComponent<PlayerController>().nowControl) // 컨트롤 중인 캐릭터와 충돌
                {
                    if(character == null)                                            //이전에 드리블중인 캐릭터가 없다면
                    {
                        AttackDefenceShift(collision);
                        ChangeCharacter(collision);
                        nowDribble = true; 
                    }
                    if(character != null)                                            //이전에 드리블중인 캐릭터가 있으면
                    {
                        if(collision.gameObject == character)                        //충돌한 캐릭터가 이전에 드리블 중인 캐릭터라면
                        {
                            nowDribble = true;
                        }
                        else if(collision.gameObject.GetComponent<PlayerController>().teamIndex == characterTimindex) //두 캐릭터의 팀이 같다면
                        {
                            //AttackDefenceShift(collision);
                            ChangeCharacter(collision);
                            nowDribble = true;
                        }
                        else if(collision.gameObject.GetComponent<PlayerController>().teamIndex != characterTimindex) //두 캐릭터의 팀이 다르다면
                        {
                            AttackDefenceShift(collision);
                            ChangeCharacter(collision);
                            nowDribble = true;
                        }
                    }
                }
            }
            if(nowDribble)//드리블 상태일때
            {
                if(!nowinvincibility) //무적상태가 아닐때
                {
                    //여기에 확률을 넣어줘야
                    if (collision.gameObject.GetComponent<PlayerController>().teamIndex !=
                                        character.gameObject.GetComponent<PlayerController>().teamIndex) //충돌한 캐릭터가 드리블중인 캐릭터와 팀이 다르면
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
                                        character.gameObject.GetComponent<PlayerController>().teamIndex) //같은 팀이면
                    {
                        character.GetComponent<PlayerController>().nowControl = false;
                        character.GetComponent<PlayerController>().AiMoveDirInGame();
                        AttackDefenceShift(collision);
                        ChangeCharacter(collision);
                    }
                }
                
            }
        }

        if (collision.gameObject.CompareTag("GoalPost")) //골대에 부딪혔을 경우
        {
            audioSource.Play();
            _ball.material.bounciness *= 1.0f;
            _isOnNet = true;
            StartCoroutine(MoveBallOnNet());
            return;
        }

        if(collision.gameObject.tag == "Ground") // 바닥과 부딪혔을 경우
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
        character = collision.gameObject;                                // 드리블 중인 캐릭터 설정
        latestTeamIndex = characterTimindex;
        characterTimindex = character.GetComponent<PlayerController>().teamIndex;
        //camera.GetComponent<CameraMove>().character = character;
        character.GetComponent<PlayerController>().nowControl = true;

        if (latestTeamIndex != characterTimindex)
            dribbleTime = 0;
    }

    public void AttackDefenceShift(Collision collision)     //패널티 밖일때만
    {
        //공수 변경

        if(!collision.gameObject.GetComponent<PlayerController>().nowInPenalty) //패널티 에리어 밖일때
        {
            if (collision.gameObject.GetComponent<PlayerController>().attack_def_index == 0)
            {
                for (int i = 0; i < 6; i++)
                {
                    if (allcharacter[i].GetComponent<PlayerController>().teamIndex == collision.gameObject.GetComponent<PlayerController>().teamIndex) // 공잡은 친구와 팀이 같으면
                    {
                        allcharacter[i].GetComponent<PlayerController>().attack_def_index = 2; //공격
                    }
                    else if (allcharacter[i].GetComponent<PlayerController>().teamIndex != collision.gameObject.GetComponent<PlayerController>().teamIndex)// 다르면
                    {
                        allcharacter[i].GetComponent<PlayerController>().attack_def_index = 1; //수비
                    }
                }
            }
            if (character != null)
            {
                if (collision.gameObject.GetComponent<PlayerController>().teamIndex != character.GetComponent<PlayerController>().teamIndex) //기존 캐릭터와 다른팀이면
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
        if(!character.GetComponent<PlayerController>().nowInPenalty) //공 소유자가 패널티 에리어 밖일때
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
    /////////////// 공격 스킬 ///////////////

    void BallDribble() //드리블 함수
    {
        ballPos = (character.transform.position + (character.transform.forward * 30)) + new Vector3(0,5,0);
        nowFree = false;
    }

    IEnumerator BallPass() //패스 함수
    {
        PlayerAttack();
        yield return new WaitForSeconds(0.1f);
        passVec = (character.transform.forward * passFos) + new Vector3(0, 20, 0); // 패스 백터 = 캐릭터기준 앞으로 * 힘 + 높이
        ballRigid.AddForce(passVec, ForceMode.Impulse);
        nowDribble = false;
        nowFree = true;
        nowGround = false;
    }
    
    IEnumerator BallLongPass() //롱패스
    {
        PlayerAttack();
        yield return new WaitForSeconds(0.1f);
        passVec = ((character.transform.forward * passFos) + new Vector3(0, 50, 0)) * 1.2f; // 패스 백터 = 캐릭터기준 앞으로 * 힘 + 높이
        ballRigid.AddForce(passVec, ForceMode.Impulse);
        nowDribble = false;
        nowFree = true;
        nowGround = false;
    }

    IEnumerator BallShortPass() //숏패스
    {
        PlayerAttack();
        yield return new WaitForSeconds(0.1f);
        passVec = ((character.transform.forward * passFos) + new Vector3(0, 50, 0)) * 0.8f; // 패스 백터 = 캐릭터기준 앞으로 * 힘 + 높이
        ballRigid.AddForce(passVec, ForceMode.Impulse);
        nowDribble = false;
        nowFree = true;
        nowGround = false;
    }

    IEnumerator ChargingShot() //차징 슛
    {
        PlayerAttack();
        yield return new WaitForSeconds(0.1f);
        passVec = ((character.transform.forward * passFos) + new Vector3(0, 50, 0)); // 패스 백터 = 캐릭터기준 앞으로 * 힘 + 높이
        ballRigid.AddForce(passVec, ForceMode.Impulse);
        nowDribble = false;
        nowFree = true;
        nowGround = false;
    }

    IEnumerator BallRightPass() //오른감차
    {
        PlayerAttack();
        yield return new WaitForSeconds(0.1f);
        passVec = ((rightShot * passFos) + new Vector3(0, 50, 0)) * 1.2f; // 패스 백터 = 캐릭터기준 앞으로 * 힘 + 높이
        ballRigid.AddForce(passVec, ForceMode.Impulse);
        ballRigid.AddTorque(Vector3.up * 100, ForceMode.Impulse);
        shotDir = 0;
        nowDribble = false;
        nowFree = true;
        nowGround = false;
    }

    IEnumerator BallLeftPass() //왼감차
    {
        PlayerAttack();
        yield return new WaitForSeconds(0.1f);
        passVec = ((leftShot * passFos) + new Vector3(0, 50, 0)) * 1.2f; // 패스 백터 = 캐릭터기준 앞으로 * 힘 + 높이
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
        if (character.GetComponent<PlayerController>().CheckDistance() != null) // 감지된 캐릭터가 있을때
        {
            if (nowDribble) //드리블 중이라면
            {
                DefencePass(); // 패스
            }
        }
    }

    void Wind(int dir) //감아차기 보정 바람 dir = 차는 방향 오른쪽 = 0 왼쪽 = 1
    {
        if(!nowGround) // 땅에 없을때
        {
            if (dir == 0) // 오른쪽 감아차기 일경우
            {
                ballRigid.AddForce(Vector3.left * 100, ForceMode.Force); // 왼쪽으로 힘작용
            }
            if (dir == 1)
            {
                ballRigid.AddForce(Vector3.right * 100, ForceMode.Force);// 오른쪽으로 힘작용
            }
        }
    }

    /////////////// 수비 스킬 ///////////////

    void DefencePass() // 가까운 상대에게 패스
    {
        if(character.GetComponent<PlayerController>().hitCharacterList != null) // 감지된 캐릭터가 있다면
        {
            Vector3 position = character.GetComponent<PlayerController>().CheckDistance().transform.position; //패스할 캐릭터 워치
            Vector3 passDirection = position - character.transform.position; // 방향벡터
            Vector3 direction = passDirection.normalized; // 방향벡터의 단위벡터
            passVec = direction * passFos; //패스백터 = 방향 x 힘
            ballRigid.AddForce(passVec, ForceMode.Impulse);
            nowDribble = false;
            nowFree = true;
            nowGround = false;
        }
        
    }

    public void DefenceGuard() //수비 가드
    {
        character.GetComponent<PlayerController>().speed = 50; // 속도감소
        nowinvincibility = true; //무적상태로 변경
        StartCoroutine(GuardTimer());
    }

    void PlayerAttack()
    {
        character.GetComponent<PlayerController>().StartCoroutinAttack();
    }

    public IEnumerator GuardTimer() //가드 타이머
    {
        yield return new WaitForSeconds(3.0f);
        character.GetComponent<PlayerController>().speed = 75; //속도 복구
        nowinvincibility = false; // 무적상태 해제
    }

    private IEnumerator MoveBallOnNet() //네트 충돌 감지
    {
        int loop = 0;
        while (_isOnNet && loop < 8)
        {
            yield return new WaitForSeconds(0.5f);
            loop++;
        }
    }

    

    

}

