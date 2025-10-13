using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Amy_Skill : MonoBehaviour
{
    [SerializeField]
    bool canUseSkil;
    float coolTime = 10f;

    public GameObject skill;
    public Transform pos;
    PlayerController playerconteroller;
    Animator ani;

    AmyAgent amyA;
    // Start is called before the first frame update
    void Start()
    {
        playerconteroller = GetComponent<PlayerController>();
        ani = GetComponent<Animator>();
        canUseSkil = true;
        amyA = GetComponent<AmyAgent>();
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
        if (amyA != null)
        {
            amyA.AgentNow = playerconteroller.nowControl;
        }
        
    }
    void Skill()
    {
        GameObject instance = Instantiate(skill, pos.transform.position, pos.transform.rotation);

    }

    IEnumerator SkillCoolDown()
    {
        canUseSkil = false;
        ani.SetTrigger("isSkill");
        Skill();// 소환 함수
        yield return new WaitForSeconds(coolTime);
        canUseSkil = true;
    }
}
