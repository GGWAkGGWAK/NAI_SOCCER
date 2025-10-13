using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ouka_Skill : MonoBehaviour
{
    [SerializeField]
    bool canUseSkil;
    float coolTime;

    [SerializeField]
    GameObject smokeBoom;

    GameObject ball;
    BallController ballcon;
    PlayerController playerconteroller;

    OukaAgent oukaA;
    void Start()
    {
        ball = GameObject.FindWithTag("Ball");
        ballcon = ball.GetComponent<BallController>();
        playerconteroller = GetComponent<PlayerController>();
        oukaA = GetComponent<OukaAgent>();
        canUseSkil = true;
        coolTime = 7f;
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
        if(oukaA != null)
        {
            oukaA.AgentNow = playerconteroller.nowControl;
        }
        
    }

    void Jump()
    {
        Vector3 bcDistacne = ball.transform.position - ballcon.character.transform.position;
        bcDistacne = bcDistacne * -3;
        bcDistacne = new Vector3(bcDistacne.x, transform.position.y, bcDistacne.z);
        Instantiate(smokeBoom, transform.position, Quaternion.identity);
        transform.position = ballcon.character.transform.position + bcDistacne;
        Instantiate(smokeBoom, transform.position, Quaternion.identity);
    }
    IEnumerator SkillCoolDown()
    {
        canUseSkil = false;
        Jump();
        yield return new WaitForSeconds(coolTime);
        canUseSkil = true;
    }
}
