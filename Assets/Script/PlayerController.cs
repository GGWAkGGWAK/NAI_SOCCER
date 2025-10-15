using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Minimalist.Bar.Quantity;

public class PlayerController : MonoBehaviour
{
    public List<Collider> hitCharacterList = new List<Collider>(); //������ ĳ���� ����Ʈ

    [SerializeField]
    GameObject mainCamera;

    private Rigidbody characterRigidbody;
    private Animator anim;
    public SpriteRenderer teamColor;

    [SerializeField] GameObject ball;

    [SerializeField] LayerMask targetMask;

    public int character_ID; //ĳ���� ID
    public int teamIndex;  //�� �ε���
    public int attack_def_index; //���� �ε���  0=�������� , �帮�� �����϶� 1=���� 2=����

    float hAxis;
    float vAxis;
    float viewAngle;      //�þ� ����
    float viewRadius;     //�þ� ��Ÿ�

    public Vector3 moverVec;
    public Vector3 moveVecai;
    public Vector3 moverDir; //����
    Vector3 lookDir; //�ٶ󺸴� ����

    Vector3 chcharacterPos; //��ġ
    float chcharacterPosX; //��ġ x��

    public float speed;
    float keyDownTime;

    public bool nowControl; //���� ����������
    public bool nowAttack; //���� ����������
    public bool nowInPenalty; //�г�Ƽ ������ ������
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
        hitCharacterList.Clear(); // ����Ʈ �ʱ�ȭ

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




        if (nowControl) // �������� ���
        {
            if (teamIndex == 1) //1�� �϶�
            {
                ///////////// �̵� /////////////

                //Ű����
                hAxis = Input.GetAxis("Horizontal_AD");
                vAxis = Input.GetAxis("Vertical_SW");
                // -1 ~ 1

                float fallSpeed = characterRigidbody.velocity.y; // �������� �ӵ� ����

                moverVec = new Vector3(hAxis, 0, vAxis).normalized;

                transform.position += moverVec * speed * Time.deltaTime;
                EnterWall();

                moverDir = transform.position + moverVec;

                transform.LookAt(moverDir);

                if (hAxis != 0 || vAxis != 0) { anim.SetBool("isRun", true); }// �̵� ���ϸ��̼�
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
                    if (ball.GetComponent<BallController>().nowDribble) //�帮�� ���϶�
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
                    if (attack_def_index == 1) // ��������϶�
                    {
                        if (ball.GetComponent<BallController>().character != this.gameObject) // ���� ������ ĳ���Ͱ� �ƴҶ�
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
                        Debug.Log("��������!");
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

                                Debug.Log("��ų");
                            }


                        }

                    }

                }
            }
            if (teamIndex == 2)
            {
                ///////////// �̵� /////////////

                // Ű����
                hAxis = Input.GetAxis("Horizontal_LR");
                vAxis = Input.GetAxis("Vertical_UD");
                // -1 ~ 1

                float fallSpeed = characterRigidbody.velocity.y; // �������� �ӵ� ����

                moverVec = new Vector3(hAxis, 0, vAxis).normalized;

                transform.position += moverVec * speed * Time.deltaTime;
                EnterWall();

                moverDir = transform.position + moverVec;

                transform.LookAt(moverDir);

                if (hAxis != 0 || vAxis != 0) { anim.SetBool("isRun", true); }// �̵� ���ϸ��̼�
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
                    if (attack_def_index == 1) // ��������϶�
                    {
                        if (!ball.GetComponent<BallController>().nowDribble) // �帮�� ������ ������
                        {

                            DefenceBlock();
                        }
                    }
                }


                if (attack_def_index == 2)
                {
                    if (ball.GetComponent<BallController>().nowDribble) //�帮�� ���϶�
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
                    if (attack_def_index == 1) // ��������϶�
                    {
                        if (ball.GetComponent<BallController>().character != this.gameObject) // ���� ������ ĳ���Ͱ� �ƴҶ�
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
                        Debug.Log("��������!");
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

                                Debug.Log("��ų");
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

        MapEscapeReturn(); //�� ������ ���� �̵�
        OpponentCheck(); //ĳ���� üũ ( �þ߰��� �� ����)
    }

    Vector3 AngleToDir(float angle) //������ ���Ͱ����� ��ȯ
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
        //�¿� �̵� �ڵ�
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
        float lookingAngle = transform.eulerAngles.y; // �ٶ󺸴� ���� 
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

    public Collider CheckDistance() // �þ߰��� ���� ĳ������ ���� ����� ĳ����
    {
        int length = hitCharacterList.Count;  //����Ʈ�� ũ��
        Vector3 distance_v;                 // ���⺤��
        float distance = 0; //�Ÿ�
        Collider minDistance = hitCharacterList[0]; //���� ����� ���

        if (length == 0) //�迭�� ���������
        {
            return null; //��ȯ���� ����
        }
        else
        {
            for (int i = 0; i < length - 1; i++) //�Ÿ���
            {
                if (i == 0)
                {
                    if (hitCharacterList[i].gameObject.GetComponent<PlayerController>().teamIndex == teamIndex) // ���� ���̸�
                    {
                        distance_v = transform.position - hitCharacterList[i].transform.position;
                        distance = distance_v.magnitude;
                    }
                }
                else
                {
                    if (hitCharacterList[i].gameObject.GetComponent<PlayerController>().teamIndex == teamIndex) // ���� ���̸�
                    {
                        distance_v = transform.position - hitCharacterList[i].transform.position;
                        if (distance >= distance_v.magnitude)
                        {
                            minDistance = hitCharacterList[i];
                        }
                    }
                }
            }
            if (minDistance == null) //����� ĳ���Ͱ� ������(���� �����Ǹ�)
            {
                return null;
            }
        }
        Debug.DrawLine(transform.position, minDistance.transform.position, Color.red);
        return minDistance; //���� ����� ��� ��ȯ
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
                    //�г�Ƽ
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