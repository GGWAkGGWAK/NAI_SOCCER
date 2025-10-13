using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ActiveSkill : MonoBehaviour
{
    //테스트용 스크립트 
    //기능별 분리 하지 않은 상태 (스킬 버튼함수와 플레이어 스킬 작동 함수를 함께 두었음) 
    public enum ACTIVE { NONE, IDLE, MOVE ,ATTACK,SKILL}
    // Start is called before the first frame update

    public ACTIVE currentState; //현재 상태
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

        //버튼에 기능 넣기 
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
    /// 임시 테스트용 State 
    /// </summary>
    /// <param name="state"></param>
    public void ChangeState(ACTIVE state)
    {
        if (state.Equals(currentState))
            return;

        if (!currentState.Equals(ACTIVE.IDLE))
        {
            Debug.Log("현재 다른 동작" + currentState + "이 작동 중입니다.");
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
        Debug.Log("현재" + currentState + "을 작동 중입니다.");
    }

    

    public void Idle()
    {
        currentState = ACTIVE.IDLE;
    }

}
