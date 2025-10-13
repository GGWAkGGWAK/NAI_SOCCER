using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SkillCutScene : MonoBehaviour
{

    public GameObject Skill;
    public Button btnSkill;
    // Start is called before the first frame update
    void Start()
    {
        if(Skill != null)
        {
            Skill.SetActive(false);
        }


        btnSkill.onClick.AddListener(ShowSkillCutScene);
    }

    void ShowSkillCutScene()
    {
        Skill.SetActive(true);
        Invoke("HideSkillCutScene", 2);
    }

    void HideSkillCutScene()
    {
        Skill.SetActive(false);
    }
    // Update is called once per frame
    void Update()
    {
        
    }
}
