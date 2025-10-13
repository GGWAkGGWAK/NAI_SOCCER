using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
public class LucyAgent : Agent
{
    public float Ren_KickPower;
    float Ren_BallTouch;
    const float Ren_Power = 2000f;
    private Transform tr;
    private Rigidbody rb;
    public Transform targetTr;
    public Transform Moving_1;
    public Transform Moving_2;
    public Transform Moving_3;
    public Transform Moving_4;
    public Transform Moving_5;
    public Transform Moving_6;
    public Transform Moving_7;
    public Transform Moving_8;
    private Vector3 initialPosition;
    public Renderer floorRd;
    //public Vector3 playerPosition;
    float Ren_ForwardSpeed;

    //바닥의 색생을 변경하기 위한 머티리얼
    private Material originMt;
    public Material badMt;
    public Material goodMt;
    public bool AgentNow;
    PlayerController playercontroller;
    public override void Initialize()
    {
        tr = GetComponent<Transform>();
        rb = GetComponent<Rigidbody>();
        playercontroller = GetComponent<PlayerController>();
        originMt = floorRd.material;
        MaxStep = 200000;
        AgentNow = GetComponent<PlayerController>().nowControl;
        //this.transform.position = playerPosition;
    }
    public void MoveAgent(ActionSegment<int> act)
    {
        var dirToGo = Vector3.zero;
        var rotateDir = Vector3.zero;

        Ren_KickPower = 0f;

        var forwardAxis = act[0];
        var rightAxis = act[1];
        var rotateAxis = act[2];

        switch (forwardAxis)
        {
            case 1:
                dirToGo = transform.forward * Ren_ForwardSpeed;
                Ren_KickPower = 1f;
                break;
            case 2:
                dirToGo = transform.forward * -Ren_ForwardSpeed;
                break;
        }

        switch (rightAxis)
        {
            case 1:
                dirToGo = transform.right * Ren_ForwardSpeed;
                break;
            case 2:
                dirToGo = transform.right * -Ren_ForwardSpeed;
                break;
        }

        switch (rotateAxis)
        {
            case 1:
                rotateDir = transform.up * -1f;
                break;
            case 2:
                rotateDir = transform.up * 1f;
                break;
        }

        transform.Rotate(rotateDir, Time.deltaTime * 100f);
        //rb.AddForce(dirToGo * m_SoccerSettings.agentRunSpeed,
        //    ForceMode.VelocityChange);
    }

    //에피소드(학습단위)가 시작할때마다 호출
    public override void OnEpisodeBegin()
    {
        targetTr = GameObject.FindWithTag("Ball").transform;
        Moving_1 = GameObject.Find("MovingPoint_Box").transform.GetChild(0);
        Moving_2 = GameObject.Find("MovingPoint_Box").transform.GetChild(1);
        Moving_3 = GameObject.Find("MovingPoint_Box").transform.GetChild(2);
        Moving_4 = GameObject.Find("MovingPoint_Box").transform.GetChild(3);
        Moving_5 = GameObject.Find("MovingPoint_Box").transform.GetChild(4);
        Moving_6 = GameObject.Find("MovingPoint_Box").transform.GetChild(5);
        Moving_7 = GameObject.Find("MovingPoint_Box").transform.GetChild(6);
        Moving_8 = GameObject.Find("MovingPoint_Box").transform.GetChild(7);
        floorRd = GameObject.Find("Gost_Tile_Map_012").GetComponent<Renderer>();
        if (AgentNow == false)
        {
            rb.velocity = Vector3.zero;
            rb.angularVelocity = Vector3.zero;

            //에이젼트의 위치를 불규칙하게 변경
            //tr.localPosition = playerPosition;
            //new Vector3(Random.Range(-150.0f, 150.0f)
            // , 12.0f
            // , Random.Range(150.0f, 250.0f));
            //targetTr.localPosition = new Vector3(Random.Range(-190.0f, -100.0f)
            //                                    , 11.0f
            //                                    , Random.Range(150.0f, 250.0f));
            StartCoroutine(RevertMaterial());
        }


    }
    IEnumerator RevertMaterial()
    {
        yield return new WaitForSeconds(0.2f);
        floorRd.material = originMt;
    }
    //환경 정보를 관측 및 수집해 정책 결정을 위해 브레인에 전달하는 메소드
    public override void CollectObservations(VectorSensor sensor)
    {
        sensor.AddObservation(targetTr.transform.position); //3
        sensor.AddObservation(Moving_1.transform.position); //3
        sensor.AddObservation(Moving_2.transform.position);
        sensor.AddObservation(Moving_3.transform.position);
        sensor.AddObservation(Moving_4.transform.position);
        sensor.AddObservation(Moving_5.transform.position);
        sensor.AddObservation(Moving_6.transform.position);
        sensor.AddObservation(Moving_7.transform.position);
        sensor.AddObservation(Moving_8.transform.position);  //3 (x,y,z)
        sensor.AddObservation(tr.transform.position);        //3 (x,y,z)
        sensor.AddObservation(rb.velocity.x);           //1 (x)
        sensor.AddObservation(rb.velocity.z); // 1
    }

    //브레인(정책)으로 부터 전달 받은 행동을 실행하는 메소드
    public float forceMulte = 1000;
    public override void OnActionReceived(ActionBuffers actions)
    {
        //int Discreate = actions.DiscreteActions[0];
        //float h = Mathf.Clamp(actions.ContinuousActions[0], -1.0f, 1.0f);
        //float v = Mathf.Clamp(actions.ContinuousActions[1], -1.0f, 1.0f);
        //Vector3 dir = (Vector3.forward * v) + (Vector3.right * h);
        // rb.AddForce(dir.normalized * 50.0f);

        //지속적으로 이동을 이끌어내기 위한 마이너스 보상
        //SetReward(-0.001f);
        if (!AgentNow)
        {
            float moveX = Random.Range(-100.0f, 100.0f);
            float moveZ = Random.Range(-100.0f, 100.0f);
            transform.LookAt(new Vector3(targetTr.transform.position.x, transform.position.y, targetTr.transform.position.z));

            // 이동

            Vector3 movement = new Vector3(moveX, 0f, moveZ);
            rb.AddForce(movement * forceMulte);
        }

        // 움직임이 지나치게 커지는 것을 제한
        //if (Mathf.Abs(rb.velocity.x) > 1000.0f)
        //    rb.velocity = new Vector3(Mathf.Sign(rb.velocity.x) * 1000.0f, rb.velocity.y, rb.velocity.z);

        //if (Mathf.Abs(rb.velocity.z) > 1000.0f)
        //    rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, Mathf.Sign(rb.velocity.z) * 1000.0f);
        SetReward(-0.1f);
    }

    //개발자(사용자)가 직접 명령을 내릴때 호출하는 메소드(주로 테스트용도 또는 모방학습에 사용)
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actionsOutt = actionsOut.ContinuousActions;
        actionsOutt[0] = Input.GetAxis("Horizontal_LR"); ; //좌,우 화살표 키 //-1.0 ~ 0.0 ~ 1.0
        actionsOutt[1] = Input.GetAxis("Vertical_UD");   //상,하 화살표 키 //연속적인 값
        Debug.Log($"[0]={actionsOutt[0]} [1]={actionsOutt[1]}");
    }
    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("Die"))
        {
            
            Debug.Log("죽어라 제발 좀");
            //잘못된 행동일 때 마이너스 보상을 준다.
            SetReward(-1.0f);
            //학습을 종료시키는 메소드
            //EndEpisode();
        }
        else if (coll.collider.CompareTag("Ball"))
        {
            
            Debug.Log("하이루");
            //올바른 행동일 때 플러스 보상을 준다.
            AddReward(+5.0f);
            playercontroller.nowControl = true;
            //AgentNow = true;
            //학습을 종료시키는 메소드
            //EndEpisode();
        }
        else if (coll.collider.CompareTag("Character"))
        {
            
            Debug.Log("꺼져");
            //올바른 행동일 때 플러스 보상을 준다.
            AddReward(-10.0f);
            //학습을 종료시키는 메소드
            //EndEpisode();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Here"))
        {
            
            Debug.Log("잘왔어");
            AddReward(+1.0f);
            //EndEpisode();

        }
    }
}
