using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum Characters
{
    Ren,Amy,Dr7,CoCo,Lucy,Ouka
}

public class DataMgr : MonoBehaviour
{
    public static DataMgr instance;
    public Characters currentCharacter;
    public Characters currentCharacter_1;
    public Characters currentCharacter_2;
    public Characters currentCharacter_3;
    public Characters currentCharacter_4;
    public Characters currentCharacter_5;

    public List<Characters> selectedCharacters = new List<Characters>();
    private void Awake()
    {
        if (instance == null) instance = this;
        else if (instance != null) return;
        DontDestroyOnLoad(gameObject);
    }


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
