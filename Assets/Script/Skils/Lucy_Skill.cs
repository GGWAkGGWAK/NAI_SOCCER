using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Lucy_Skill : MonoBehaviour
{
    [SerializeField]
    bool canUseSkil;
    float coolTime = 10f;

    public GameObject skill;
    public Transform pos;
    PlayerController playerconteroller;

    Animator anim;

    LucyAgent lucyA;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        playerconteroller = GetComponent<PlayerController>();
        lucyA = GetComponent<LucyAgent>();
        canUseSkil = true;
    }

    // Update is called once per frame
    void Update()
    {
        if (this.gameObject.GetComponent<PlayerController>().nowControl)
        {
            if (this.gameObject.GetComponent<PlayerController>().teamIndex == 1)
            {
                if (Input.GetKeyDown(KeyCode.Y))
                {
                    if (playerconteroller.nowControl)
                    {
                        if (canUseSkil)
                        {
                            StartCoroutine(SkillCoolDown());
                        }
                    }
                }
            }
            if (this.gameObject.GetComponent<PlayerController>().teamIndex == 2)
            {
                if (Input.GetKeyDown(KeyCode.Keypad3))
                {
                    if (playerconteroller.nowControl)
                    {
                        if (canUseSkil)
                        {
                            StartCoroutine(SkillCoolDown());
                        }
                    }
                }
            }
        }
        if(lucyA != null)
        {
            lucyA.AgentNow = playerconteroller.nowControl;
        }
        
    }
    void Skill()
    {
        GameObject instance = Instantiate(skill, transform.position, pos.transform.rotation);
        
    }

    IEnumerator SkillCoolDown()
    {
        canUseSkil = false;
        anim.SetTrigger("isSkill");
        Skill();// 소환 함수
        yield return new WaitForSeconds(coolTime);
        canUseSkil = true;
    }
}
