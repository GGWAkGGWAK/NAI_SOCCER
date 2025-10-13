using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Minimalist.Bar.Quantity;

public class PlayerController : MonoBehaviour
{
    public List<Collider> hitCharacterList = new List<Collider>(); //감지된 캐릭터 리스트

    [SerializeField]
    GameObject mainCamera;

    private Rigidbody characterRigidbody;
    private Animator anim;
    public SpriteRenderer teamColor;

    [SerializeField] GameObject ball;

    [SerializeField] LayerMask targetMask;

    public int character_ID; //캐릭터 ID
    public int teamIndex;  //팀 인덱스
    public int attack_def_index; //공수 인덱스  0=자유상태 , 드리블 상태일때 1=수비 2=공격

    float hAxis;
    float vAxis;
    float viewAngle;      //시야 각도
    float viewRadius;     //시야 사거리

    public Vector3 moverVec;
    public Vector3 moveVecai;
    public Vector3 moverDir; //방향
    Vector3 lookDir; //바라보는 방향

    Vector3 chcharacterPos; //위치
    float chcharacterPosX; //위치 x값

    public float speed;
    float keyDownTime;

    public bool nowControl; //현재 조작중인지
    public bool nowAttack; //현재 공격중인지
    public bool nowInPenalty; //패널티 에리어 안인지
    bool isRight;

    bool dashCoolDown;

    public bool isSun;

    public bool aiStop;

    public bool canUseSkill;
    public float coolTime = 10f;

    public int respawnIndex;

    AudioSource audioSource;

    public float hpGauge;
    public float manaGauge;
    public float energyGauge;
    public float progressGauge;

    public QuantityBhv hp;
    public QuantityBhv mana;
    public QuantityBhv energy;
    public QuantityBhv progress;

    void Awake()
    {
        characterRigidbody = GetComponent<Rigidbody>();
        anim = GetComponentInChildren<Animator>();
        teamColor = GetComponent<SpriteRenderer>();
        ball = GameObject.FindWithTag("Ball");
        audioSource = this.gameObject.GetComponent<AudioSource>();
        hp = transform.GetChild(0).GetChild(0).gameObject.GetComponent<QuantityBhv>();
        mana = transform.GetChild(0).GetChild(1).gameObject.GetComponent<QuantityBhv>();
        energy = transform.GetChild(0).GetChild(2).gameObject.GetComponent<QuantityBhv>();
        progress = transform.GetChild(0).GetChild(3).gameObject.GetComponent<QuantityBhv>();
    }
    void Start()
    {
        character_ID = CharacterID();

        speed = 75;
        viewAngle = 60f;
        viewRadius = 10000f;
        attack_def_index = 0;

        hpGauge = hp.MaximumAmount;
        manaGauge = mana.MaximumAmount;
        energyGauge = energy.MaximumAmount;
        progressGauge = progress.MaximumAmount;
        
        isSun = false;

        dashCoolDown = false;
        canUseSkill = true;

        AiMoveDirStart();
        hitCharacterList.Clear(); // 리스트 초기화

        //InAllChar();
        if (teamIndex == 1)
        {
            transform.GetChild(3).GetChild(0).gameObject.GetComponent<Image>().color = new Color(1, 0.5f, 0, 0.9f);
        }
        else if (teamIndex == 2)
        {
            transform.GetChild(3).GetChild(0).gameObject.GetComponent<Image>().color = new Color(0.5f, 0, 1, 0.9f);
        }

    }

    void Update()
    {
        chcharacterPos = transform.position;
        chcharacterPosX = chcharacterPos.x;



        if (manaGauge >= mana.MaximumAmount)
        {
            manaGauge = mana.MaximumAmount;
        }
        else if (manaGauge <= mana.MinimumAmount)
        {
            manaGauge = mana.MinimumAmount;
        }
        else
        {
            manaGauge += (25 * Time.deltaTime);
        }
        if (energyGauge >= energy.MaximumAmount)
        {

            energyGauge = energy.MaximumAmount;
        }
        else if (energyGauge <= energy.MinimumAmount)
        {
            energyGauge = energy.MinimumAmount;

        }
        else
        {
            energyGauge += (10 * Time.deltaTime);
        }




        energy.FillAmount = energyGauge / energy.MaximumAmount;
        mana.FillAmount = manaGauge / mana.MaximumAmount;
        progress.FillAmount = progressGauge / progress.MaximumAmount;




        if (nowControl) // 조작중일 경우
        {
            if (teamIndex == 1) //1팀 일때
            {
                ///////////// 이동 /////////////

                //키보드
                hAxis = Input.GetAxis("Horizontal_AD");
                vAxis = Input.GetAxis("Vertical_SW");
                // -1 ~ 1

                float fallSpeed = characterRigidbody.velocity.y; // 떨어지는 속도 저장

                moverVec = new Vector3(hAxis, 0, vAxis).normalized;

                transform.position += moverVec * speed * Time.deltaTime;
                EnterWall();

                moverDir = transform.position + moverVec;

                transform.LookAt(moverDir);

                if (hAxis != 0 || vAxis != 0) { anim.SetBool("isRun", true); }// 이동 에니메이션
                else
                {
                    anim.SetBool("isRun", false);
                    characterRigidbody.velocity = Vector3.zero;
                }

                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(2).gameObject.SetActive(true);

                ///////////////////////////////////////



                if (attack_def_index == 2)
                {
                    if (ball.GetComponent<BallController>().nowDribble) //드리블 중일때
                    {
                        if (Input.GetKey(KeyCode.R))
                        {
                            keyDownTime += Time.deltaTime;
                        }
                        if (Input.GetKeyUp(KeyCode.R))
                        {
                            if (keyDownTime >= 2) { keyDownTime = 2; }
                            energyGauge -= energy.MaximumAmount / 4;

                            audioSource.Play();
                            ball.GetComponent<BallController>().ChargingShootCall(keyDownTime);
                            keyDownTime = 0;
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.R))
                {
                    if (attack_def_index == 1) // 수비상태일때
                    {
                        if (ball.GetComponent<BallController>().character != this.gameObject) // 공을 소유한 캐릭터가 아닐때
                        {
                            DefenceBlock();
                            energyGauge -= energy.MaximumAmount / 4;

                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.T))
                {
                    if (ball.GetComponent<BallController>().character == this.gameObject)
                    {
                        if (attack_def_index == 1)
                        {
                            energyGauge -= energy.MaximumAmount / 4;
                            ball.GetComponent<BallController>().PassCall();
                            audioSource.Play();
                        }
                        if (attack_def_index == 2)
                        {
                            energyGauge -= energy.MaximumAmount / 4;
                            ball.GetComponent<BallController>().PassCall();
                            audioSource.Play();
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.Y))
                {
                    if (manaGauge < mana.MaximumAmount / 3)
                    {
                        Debug.Log("마나부족!");
                    }
                    else
                    {
                        if (canUseSkill)
                        {
                            manaGauge -= mana.MaximumAmount / 3;
                            StartCoroutine(CoolDownSkill());
                            if (attack_def_index == 1)
                            {
                                if (ball.GetComponent<BallController>().character == this.gameObject)
                                {

                                    ball.GetComponent<BallController>().DefenceGuard();
                                }
                            }
                            if (attack_def_index == 2)
                            {

                                Debug.Log("스킬");
                            }


                        }

                    }

                }
            }
            if (teamIndex == 2)
            {
                ///////////// 이동 /////////////

                // 키보드
                hAxis = Input.GetAxis("Horizontal_LR");
                vAxis = Input.GetAxis("Vertical_UD");
                // -1 ~ 1

                float fallSpeed = characterRigidbody.velocity.y; // 떨어지는 속도 저장

                moverVec = new Vector3(hAxis, 0, vAxis).normalized;

                transform.position += moverVec * speed * Time.deltaTime;
                EnterWall();

                moverDir = transform.position + moverVec;

                transform.LookAt(moverDir);

                if (hAxis != 0 || vAxis != 0) { anim.SetBool("isRun", true); }// 이동 에니메이션
                else
                {
                    anim.SetBool("isRun", false);
                    characterRigidbody.velocity = Vector3.zero;
                }

                transform.GetChild(0).gameObject.SetActive(true);
                transform.GetChild(1).gameObject.SetActive(true);
                transform.GetChild(2).gameObject.SetActive(true);
                ///////////////////////////////////////

                if (Input.GetKeyDown(KeyCode.C))
                {
                    if (attack_def_index == 1) // 수비상태일때
                    {
                        if (!ball.GetComponent<BallController>().nowDribble) // 드리블 중이지 않을때
                        {

                            DefenceBlock();
                        }
                    }
                }


                if (attack_def_index == 2)
                {
                    if (ball.GetComponent<BallController>().nowDribble) //드리블 중일때
                    {
                        if (Input.GetKey(KeyCode.Keypad1))
                        {
                            keyDownTime += Time.deltaTime;
                        }
                        if (Input.GetKeyUp(KeyCode.Keypad1))
                        {
                            if (keyDownTime >= 2) { keyDownTime = 2; }
                            audioSource.Play();
                            energyGauge -= energy.MaximumAmount / 4;
                            ball.GetComponent<BallController>().ChargingShootCall(keyDownTime);
                            keyDownTime = 0;
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.Keypad1))
                {
                    if (attack_def_index == 1) // 수비상태일때
                    {
                        if (ball.GetComponent<BallController>().character != this.gameObject) // 공을 소유한 캐릭터가 아닐때
                        {
                            energyGauge -= energy.MaximumAmount / 4;
                            DefenceBlock();
                        }
                    }
                }
                if (Input.GetKeyDown(KeyCode.Keypad2))
                {
                    if (ball.GetComponent<BallController>().character == this.gameObject)
                    {
                        if (attack_def_index == 1)
                        {
                            energyGauge -= energy.MaximumAmount / 4;
                            ball.GetComponent<BallController>().PassCall();
                            audioSource.Play();
                        }
                        if (attack_def_index == 2)
                        {
                            energyGauge -= energy.MaximumAmount / 4;
                            ball.GetComponent<BallController>().PassCall();
                            audioSource.Play();
                        }
                    }
                }

                if (Input.GetKeyDown(KeyCode.Keypad3))
                {
                    if (manaGauge < mana.MaximumAmount / 3)
                    {
                        Debug.Log("마나부족!");
                    }
                    else
                    {
                        if (canUseSkill)
                        {
                            manaGauge -= mana.MaximumAmount / 3;
                            StartCoroutine(CoolDownSkill());
                            if (attack_def_index == 1)
                            {
                                if (ball.GetComponent<BallController>().character == this.gameObject)
                                {
                                    ball.GetComponent<BallController>().DefenceGuard();
                                }
                            }
                            if (attack_def_index == 2)
                            {

                                Debug.Log("스킬");
                            }

                        }
                    }
                }
            }
        }
        else if (!nowControl)
        {
            transform.GetChild(0).gameObject.SetActive(false);
            transform.GetChild(1).gameObject.SetActive(false);
            transform.GetChild(2).gameObject.SetActive(false);
            if (!aiStop)
            {
                AiMoveDirInGame();
                moverDir = transform.position + moveVecai;
                transform.position += (moveVecai * speed * 0.5f * Time.deltaTime);

                if(isRight)
                {
                    transform.LookAt(transform.position + Vector3.right);
                }
                else if(!isRight)
                {
                    transform.LookAt(transform.position + Vector3.left);
                }
                
                anim.SetBool("isRun", true);
            }
            else if(aiStop)
            {
                anim.SetBool("isRun", true);
            }
        }

        if (isSun)
        {
            speed = 25.0f;
            progressGauge -= (10 * Time.deltaTime);

        }
        else
        {
            if (progressGauge >= progress.MaximumAmount)
            {
                progressGauge = progress.MaximumAmount;
            }
            else if (progressGauge <= progress.MinimumAmount)
            {
                progressGauge = progress.MinimumAmount;
            }
            else
            {
                progressGauge += (10 * Time.deltaTime);
            }

        }

        MapEscapeReturn(); //맵 밖으로 가면 이동
        OpponentCheck(); //캐릭터 체크 ( 시야각네 팀 구분)
    }

    Vector3 AngleToDir(float angle) //각도를 벡터값으로 반환
    {
        float radian = angle * Mathf.Deg2Rad;
        return new Vector3(Mathf.Sin(radian), 0f, Mathf.Cos(radian));
    }

    int CharacterID()
    {
        if (this.gameObject.name == "ACE_01_Ren_001")
        {
            return 6; // Ren ID = 6
        }
        else if (this.gameObject.name == "ACE_02_Amy_001")
        {
            return 13; // Amy ID = 13
        }
        else return 0;
    }

    void AiMoveDirStart()
    {
        //좌우 이동 코드
        if (teamIndex == 1)
        {
            isRight = true;
            moveVecai = new Vector3(1, 0, 0);
        }
        else if (teamIndex == 2)
        {
            isRight = false;
            moveVecai = new Vector3(-1, 0, 0);
        }
    }

    public void AiMoveDirInGame()
    {
        if (transform.position.x < -350)
        {
            if(!isRight)
            {
                isRight = true;
                moveVecai = moveVecai * -1;
            }
        }
        if (transform.position.x > 350)
        {
            if(isRight)
            {
                isRight = false;
                moveVecai = moveVecai * -1;
            }
        }
    }

    public void EnterWall()
    {
        //-+ 310
        if(transform.position.x > 415)
        {
            transform.position = new Vector3(415, transform.position.y, transform.position.z);
        }
        if (transform.position.x < -415)
        {
            transform.position = new Vector3(-415, transform.position.y, transform.position.z);
        }
        if (transform.position.z > 475)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 475);
        }
        if (transform.position.z < 0)
        {
            transform.position = new Vector3(transform.position.x, transform.position.y, 0);
        }
    }
    void OpponentCheck()
    {
        float lookingAngle = transform.eulerAngles.y; // 바라보는 방향 
        lookDir = AngleToDir(lookingAngle);
        Collider[] Targets = Physics.OverlapSphere(transform.position, viewRadius, targetMask);

        if (Targets.Length == 0) return;
        foreach (Collider EnemyColli in Targets)
        {
            Vector3 targetPos = EnemyColli.transform.position;
            Vector3 targetDir = (targetPos - transform.position).normalized;
            float targetAngle = Mathf.Acos(Vector3.Dot(lookDir, targetDir)) * Mathf.Rad2Deg;
            if (targetAngle <= viewAngle * 0.5f)
            {
                if (EnemyColli.gameObject.tag == "Character")
                {
                    int check = 0;
                    if (hitCharacterList.Count == 0)
                    {
                        hitCharacterList.Add(EnemyColli);
                    }
                    else if (hitCharacterList.Count > 0)
                    {
                        for (int i = 0; i < hitCharacterList.Count; i++)
                        {
                            if (hitCharacterList[i] == EnemyColli)
                            {
                                check = 1;
                            }
                        }
                        if (check == 1)
                        {
                            check = 0;
                        }
                        else if (check == 0)
                        {
                            hitCharacterList.Add(EnemyColli);
                        }
                    }
                    //Debug.DrawLine(transform.position, targetPos, Color.red);
                }

            }
        }
    }

    public Collider CheckDistance() // 시야각에 들어온 캐릭터중 가장 가까운 캐릭터
    {
        int length = hitCharacterList.Count;  //리스트의 크기
        Vector3 distance_v;                 // 방향벡터
        float distance = 0; //거리
        Collider minDistance = hitCharacterList[0]; //가장 가까운 요소

        if (length == 0) //배열이 비어있으면
        {
            return null; //반환하지 않음
        }
        else
        {
            for (int i = 0; i < length - 1; i++) //거리비교
            {
                if (i == 0)
                {
                    if (hitCharacterList[i].gameObject.GetComponent<PlayerController>().teamIndex == teamIndex) // 같은 팀이면
                    {
                        distance_v = transform.position - hitCharacterList[i].transform.position;
                        distance = distance_v.magnitude;
                    }
                }
                else
                {
                    if (hitCharacterList[i].gameObject.GetComponent<PlayerController>().teamIndex == teamIndex) // 같은 팀이면
                    {
                        distance_v = transform.position - hitCharacterList[i].transform.position;
                        if (distance >= distance_v.magnitude)
                        {
                            minDistance = hitCharacterList[i];
                        }
                    }
                }
            }
            if (minDistance == null) //가까운 캐릭터가 없으면(적만 감지되면)
            {
                return null;
            }
        }
        Debug.DrawLine(transform.position, minDistance.transform.position, Color.red);
        return minDistance; //가장 가까운 요소 반환
    }

    void DefenceBlock()
    {
        if (dashCoolDown == false)
        {
            StartCoroutine(DefenceCoolDown());
            transform.position = transform.position + (moverVec * 25);
        }
    }

    IEnumerator DefenceCoolDown()
    {
        dashCoolDown = false;
        yield return new WaitForSeconds(1f);
        dashCoolDown = true;
    }

    public void StartCoroutinAttack()
    {
        if (!nowAttack)
        {
            StartCoroutine(Attack());
        }
    }

    public void TeamColor()
    {
        if (teamIndex == 1)
        {
            teamColor.color = new Color(1, 0.5f, 0, 0.9f);
        }
        if (teamIndex == 2)
        {
            teamColor.color = new Color(0.5f, 0, 1, 0.9f);
        }
    }

    void MapEscapeReturn()
    {
        Vector3 realtiempos = transform.position;

        if (realtiempos.x < -400 && realtiempos.z > 500)
        {
            transform.position = new Vector3(0, transform.position.y, 0);
        }
        else if (realtiempos.x < -400 && realtiempos.z < -270)
        {
            transform.position = new Vector3(0, transform.position.y, 0);
        }
        else if (realtiempos.x > 500 && realtiempos.z > 500)
        {
            transform.position = new Vector3(0, transform.position.y, 0);
        }
        else if (realtiempos.x > 500 && realtiempos.z < -270)
        {
            transform.position = new Vector3(0, transform.position.y, 0);
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Sun")
            isSun = true;
        else if (other.gameObject.tag == "Lightning")
            StartCoroutine(Lightning());
        else if (other.gameObject.tag == "Penalty")
        {
            nowInPenalty = true;
        }

        if (!nowControl && !aiStop)
        {
            if (other.gameObject.tag == "AiMoveWall")
            {
                moverVec = moverVec * -1;
            }
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (other.gameObject.tag == "Sun")
        {
            isSun = false;
            speed = 75.0f;
        }
        else if (other.gameObject.tag == "Penalty")
        {
            nowInPenalty = false;

            if (this.gameObject == ball.GetComponent<BallController>().character)
            {
                if (attack_def_index == 1)
                {
                    //패널티
                    ball.GetComponent<BallController>().AttackDefenceShift_InPen();
                }
            }
        }

    }

    void InAllChar()
    {
        if (ball.GetComponent<BallController>().allcharacter[respawnIndex] == null)
            ball.GetComponent<BallController>().allcharacter[respawnIndex] = this.gameObject;
    }

    public IEnumerator Attack()
    {
        speed = 0f;
        nowAttack = true;
        anim.SetBool("isAttack", true);
        yield return new WaitForSeconds(1.2f);
        speed = 75f;
        nowAttack = false;
        anim.SetBool("isAttack", false);
        StopCoroutine(Attack());
    }

    public IEnumerator Lightning()
    {
        speed = 0f;
        yield return new WaitForSeconds(2f);
        speed = 75.0f;
    }
    public IEnumerator CoolDownSkill()
    {
        canUseSkill = false;
        yield return new WaitForSeconds(coolTime);
        canUseSkill = true;
    }
    public void AttackSoundPlay()
    {
        Debug.Log("AttackSoundPlay");
    }

    

}