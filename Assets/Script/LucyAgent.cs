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

    //�ٴ��� ������ �����ϱ� ���� ��Ƽ����
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

    //���Ǽҵ�(�н�����)�� �����Ҷ����� ȣ��
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

            //������Ʈ�� ��ġ�� �ұ�Ģ�ϰ� ����
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
    //ȯ�� ������ ���� �� ������ ��å ������ ���� �극�ο� �����ϴ� �޼ҵ�
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

    //�극��(��å)���� ���� ���� ���� �ൿ�� �����ϴ� �޼ҵ�
    public float forceMulte = 1000;
    public override void OnActionReceived(ActionBuffers actions)
    {
        //int Discreate = actions.DiscreteActions[0];
        //float h = Mathf.Clamp(actions.ContinuousActions[0], -1.0f, 1.0f);
        //float v = Mathf.Clamp(actions.ContinuousActions[1], -1.0f, 1.0f);
        //Vector3 dir = (Vector3.forward * v) + (Vector3.right * h);
        // rb.AddForce(dir.normalized * 50.0f);

        //���������� �̵��� �̲���� ���� ���̳ʽ� ����
        //SetReward(-0.001f);
        if (!AgentNow)
        {
            float moveX = Random.Range(-100.0f, 100.0f);
            float moveZ = Random.Range(-100.0f, 100.0f);
            transform.LookAt(new Vector3(targetTr.transform.position.x, transform.position.y, targetTr.transform.position.z));

            // �̵�

            Vector3 movement = new Vector3(moveX, 0f, moveZ);
            rb.AddForce(movement * forceMulte);
        }

        // �������� ����ġ�� Ŀ���� ���� ����
        //if (Mathf.Abs(rb.velocity.x) > 1000.0f)
        //    rb.velocity = new Vector3(Mathf.Sign(rb.velocity.x) * 1000.0f, rb.velocity.y, rb.velocity.z);

        //if (Mathf.Abs(rb.velocity.z) > 1000.0f)
        //    rb.velocity = new Vector3(rb.velocity.x, rb.velocity.y, Mathf.Sign(rb.velocity.z) * 1000.0f);
        SetReward(-0.1f);
    }

    //������(�����)�� ���� ����� ������ ȣ���ϴ� �޼ҵ�(�ַ� �׽�Ʈ�뵵 �Ǵ� ����н��� ���)
    public override void Heuristic(in ActionBuffers actionsOut)
    {
        var actionsOutt = actionsOut.ContinuousActions;
        actionsOutt[0] = Input.GetAxis("Horizontal_LR"); ; //��,�� ȭ��ǥ Ű //-1.0 ~ 0.0 ~ 1.0
        actionsOutt[1] = Input.GetAxis("Vertical_UD");   //��,�� ȭ��ǥ Ű //�������� ��
        Debug.Log($"[0]={actionsOutt[0]} [1]={actionsOutt[1]}");
    }
    void OnCollisionEnter(Collision coll)
    {
        if (coll.collider.CompareTag("Die"))
        {
            
            Debug.Log("�׾�� ���� ��");
            //�߸��� �ൿ�� �� ���̳ʽ� ������ �ش�.
            SetReward(-1.0f);
            //�н��� �����Ű�� �޼ҵ�
            //EndEpisode();
        }
        else if (coll.collider.CompareTag("Ball"))
        {
            
            Debug.Log("���̷�");
            //�ùٸ� �ൿ�� �� �÷��� ������ �ش�.
            AddReward(+5.0f);
            playercontroller.nowControl = true;
            //AgentNow = true;
            //�н��� �����Ű�� �޼ҵ�
            //EndEpisode();
        }
        else if (coll.collider.CompareTag("Character"))
        {
            
            Debug.Log("����");
            //�ùٸ� �ൿ�� �� �÷��� ������ �ش�.
            AddReward(-10.0f);
            //�н��� �����Ű�� �޼ҵ�
            //EndEpisode();
        }
    }
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Here"))
        {
            
            Debug.Log("�߿Ծ�");
            AddReward(+1.0f);
            //EndEpisode();

        }
    }
}
