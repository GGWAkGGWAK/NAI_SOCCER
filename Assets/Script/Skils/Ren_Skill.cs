using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ren_Skill : MonoBehaviour
{
    public GameObject shilds;
    [SerializeField]
    bool canUseSkil;
    float coolTime = 10f;

    PlayerController playerconteroller;
    Animator anim;

    RenAgent renA;
    // Start is called before the first frame update
    void Start()
    {
        anim = GetComponent<Animator>();
        playerconteroller = GetComponent<PlayerController>();
        canUseSkil = true;
        renA = GetComponent<RenAgent>();
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
        if(renA != null)
        {
            renA.AgentNow = playerconteroller.nowControl;
        }
        

    }

    void Skill()
    {
        GameObject instance = Instantiate(shilds, this.gameObject.transform.position, Quaternion.identity);
        //instance.GetComponent<Ren_Shild>().target = transform;
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
