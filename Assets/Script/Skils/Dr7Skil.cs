using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Dr7Skil : MonoBehaviour
{
    [SerializeField]
    bool canUseSkil;
    float coolTime;

    IEnumerator skillcooldown;

    [SerializeField]
    GameObject turret;

    PlayerController playerconteroller;

    Dr7Agent dr7A;
    void Start()
    {
        canUseSkil = true;
        coolTime = 10f;

        skillcooldown = SkillCoolDown();

        playerconteroller = GetComponent<PlayerController>();
        dr7A = GetComponent<Dr7Agent>();
    }

    void Update()
    {
        if(playerconteroller.teamIndex == 1)
        {
            if(Input.GetKeyDown(KeyCode.Y))
            {
                if(playerconteroller.nowControl)
                {
                    if (canUseSkil)
                    {
                        StartCoroutine(SkillCoolDown());
                    }
                }
            }
        }
        else if(playerconteroller.teamIndex == 2)
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
        if(dr7A != null)
        {
            dr7A.AgentNow = playerconteroller.nowControl;
        }
        

    }
    public void TurretSummon()
    {
        Vector3 summonVec = transform.position + (transform.forward * 30);
        Instantiate(turret, summonVec, Quaternion.Euler(transform.rotation.x,transform.rotation.y, transform.rotation.z));
    }
    IEnumerator SkillCoolDown()
    {
        canUseSkil = false;
        TurretSummon();// 소환 함수
        yield return new WaitForSeconds(coolTime);
        canUseSkil = true;
    }
}
