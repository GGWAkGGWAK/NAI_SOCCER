using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Coco_Skill : MonoBehaviour
{
    [SerializeField]
    bool canUseSkil;
    float coolTime;

    PlayerController playerconteroller;
    Animator anim;

    [SerializeField]
    GameObject targetArea;

    CoCoAgent cocoA;
    void Start()
    {
        playerconteroller = GetComponent<PlayerController>();
        anim = GetComponent<Animator>();
        cocoA = GetComponent<CoCoAgent>();
        canUseSkil = true;
        coolTime = 5f;
    }

    void Update()
    {
        if (playerconteroller.teamIndex == 1)
        {
            if (Input.GetKeyDown(KeyCode.Y))
            {
                if (playerconteroller.nowControl)
                {
                    if (canUseSkil)
                    {
                        Debug.Log("스킬 나가야함");
                        StartCoroutine(SkillCoolDown());
                    }
                }
            }
        }
        else if (playerconteroller.teamIndex == 2)
        {
            if (Input.GetKeyDown(KeyCode.Keypad3))
            {
                if (playerconteroller.nowControl)
                {
                    if (canUseSkil)
                    {
                        Debug.Log("스킬 나가야함");
                        StartCoroutine(SkillCoolDown());
                    }
                }
            }
        }
        if(cocoA != null)
        {
            cocoA.AgentNow = playerconteroller.nowControl;
        }
        
    }


    void PlaySkillEffect()
    {
        Vector3 summonVec = transform.position + (transform.forward * 20);
        Vector3 rotation = new Vector3(transform.rotation.y + 180, 0, transform.rotation.y - 90);
        Instantiate(targetArea, summonVec,Quaternion.Euler(rotation));
    }

    void ActiveSkill()
    {
        //Debug.Log("z");
    }

    void Skill1End()
    {

    }
    IEnumerator SkillCoolDown()
    {
        canUseSkil = false;
        playerconteroller.speed = 0;
        anim.SetTrigger("isSkill");
        yield return new WaitForSeconds(1);
        playerconteroller.speed = 75;
        yield return new WaitForSeconds(coolTime - 1);
        canUseSkil = true;
    }
}
