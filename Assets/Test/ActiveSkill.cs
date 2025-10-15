using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSkill : MonoBehaviour
{
    //�׽�Ʈ�� ��ũ��Ʈ 
    //��ɺ� �и� ���� ���� ���� (��ų ��ư�Լ��� �÷��̾� ��ų �۵� �Լ��� �Բ� �ξ���) 
    public enum ACTIVE { NONE, IDLE, MOVE ,ATTACK,SKILL}
    // Start is called before the first frame update

    public ACTIVE currentState; //���� ����
    [SerializeField]
    Animator anim;

    private int attackId;
    private int skillId;
    private int idleId;

    private Button skillBtn;
    private Button attackBtn;



    private void Start()
    {
        if(anim==null)
            anim = GetComponent<Animator>();

        currentState = ACTIVE.IDLE;
        InitPrameters();

        skillBtn = TestGameManager.Instance.skillBtn.GetComponent<Button>();
        attackBtn = TestGameManager.Instance.attackBtn.GetComponent<Button>();

        //��ư�� ��� �ֱ� 
        skillBtn.onClick.AddListener(() => Btn_Active_Skill());
        attackBtn.onClick.AddListener(() => Btn_Active_Attack());
    }
    private void InitPrameters()
    {
        attackId = Animator.StringToHash("isAttack");
        skillId = Animator.StringToHash("isSkill");
        idleId = Animator.StringToHash("isIdle");
    }

    public void Btn_Active_Attack()
    {
        ChangeState(ACTIVE.ATTACK);
    }

    public void Btn_Active_Skill()
    {
        ChangeState(ACTIVE.SKILL);
    }

    public void Btn_Active_Idle()
    {
        ChangeState(ACTIVE.IDLE);
    }
    /// <summary>
    /// �ӽ� �׽�Ʈ�� State 
    /// </summary>
    /// <param name="state"></param>
    public void ChangeState(ACTIVE state)
    {
        if (state.Equals(currentState))
            return;

        if (!currentState.Equals(ACTIVE.IDLE))
        {
            Debug.Log("���� �ٸ� ����" + currentState + "�� �۵� ���Դϴ�.");
            return;
        }

        currentState = state;

        switch (currentState)
        {
            case ACTIVE.ATTACK:
                anim.SetTrigger(attackId);
                
                break;
            case ACTIVE.SKILL:
                anim.SetTrigger(skillId);
             
                break;
            case ACTIVE.IDLE:
             
                break;
        }
        Debug.Log("����" + currentState + "�� �۵� ���Դϴ�.");
    }

    

    public void Idle()
    {
        currentState = ACTIVE.IDLE;
    }

}
