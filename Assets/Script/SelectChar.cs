using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SelectChar : MonoBehaviour
{
    [SerializeField]
    public Characters characters;
    public Characters characters_1;
    public Characters characters_2;
    public Characters characters_3;
    public Characters characters_4;
    public Characters characters_5;
    Animator anim;
    SpriteRenderer sr;
    public SelectChar[] chars;
    public GameObject spotlight;

    public int maxSelectedCharacters = 100;
    public List<SelectChar> selectedCharacters1P = new List<SelectChar>();
    public List<SelectChar> selectedCharacters2P = new List<SelectChar>();

    // UI Image components to display selected characters
    public Image[] characterSlots;

    // Flag to determine whether it's currently 1P's turn or 2P's turn
    private bool isPlayer1Turn = true;
    public LayerMask selectableLayer; // 선택 가능한 게임 오브젝트의 레이어

    void Start()
    {
        anim = GetComponent<Animator>();
        sr = GetComponent<SpriteRenderer>();
        if (DataMgr.instance.currentCharacter == characters)
        {
            OnSelect();
        }
        else if (DataMgr.instance.currentCharacter_1 == characters_1)
        {
        }
        else if (DataMgr.instance.currentCharacter_2 == characters_2)
        {

        }
        else if (DataMgr.instance.currentCharacter_3 == characters_3)
        {

        }
        else if (DataMgr.instance.currentCharacter_4 == characters_4)
        {

        }
        else if (DataMgr.instance.currentCharacter_5 == characters_5)
            OnDeSelect();
        spotlight = GameObject.Find("Spotlight");
    }

    private void OnMouseUpAsButton()
    {
        HandleCharacterSelection();
        //HandleCharacterSelectionn();
        // 소스트리 테스트하기.
        // 다시 테스트하기.
    }

    private void HandleCharacterSelection()
    {
        List<SelectChar> currentPlayerSelection = isPlayer1Turn ? selectedCharacters1P : selectedCharacters2P;
        List<SelectChar> otherPlayerSelection = isPlayer1Turn ? selectedCharacters2P : selectedCharacters1P;

        if (currentPlayerSelection.Count < maxSelectedCharacters)
        {

            DataMgr.instance.currentCharacter = characters;
            DataMgr.instance.currentCharacter_1 = characters_1;
            DataMgr.instance.currentCharacter_2 = characters_2;
            DataMgr.instance.currentCharacter_3 = characters_3;
            DataMgr.instance.currentCharacter_4 = characters_4;
            DataMgr.instance.currentCharacter_5 = characters_5;
            OnSelect();
            currentPlayerSelection.Add(this);
            isPlayer1Turn = !isPlayer1Turn;
            // Update UI to display selected characters
            UpdateCharacterUI(currentPlayerSelection);

            // Deselect other characters
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] != this) chars[i].OnDeSelect();
                // Switch turns

            }
        }
        else if (otherPlayerSelection.Count < maxSelectedCharacters)
        {
            DataMgr.instance.currentCharacter = characters;
            DataMgr.instance.currentCharacter_1 = characters_1;
            DataMgr.instance.currentCharacter_2 = characters_2;
            DataMgr.instance.currentCharacter_3 = characters_3;
            DataMgr.instance.currentCharacter_4 = characters_4;
            DataMgr.instance.currentCharacter_5 = characters_5;
            OnSelect();
            otherPlayerSelection.Add(this);
            isPlayer1Turn = !isPlayer1Turn;
            // Update UI to display selected characters
            UpdateCharacterUI(otherPlayerSelection);

            // Deselect other characters
            for (int i = 0; i < chars.Length; i++)
            {
                if (chars[i] != this) chars[i].OnDeSelect();

                // Switch turns

            }


        }

    }

    void OnDeSelect()
    {
        //anim.SetBool("ATTACK", false);
        //transform.GetChild(0).gameObject.SetActive(false);
    }

    void Update()
    {
        // Additional logic if needed


    }

    void OnSelect()
    {
        //anim.SetBool("ATTACK", true);
        // transform.GetChild(0).gameObject.SetActive(true);
    }

    // Update UI to display selected characters
    void UpdateCharacterUI(List<SelectChar> selectedCharacters)
    {
        for (int i = 3; i < selectedCharacters.Count; i++)
        {
            if (i < characterSlots.Length)
            {
                // Set the sprite of the UI Image to the selected character's sprite
                characterSlots[i].sprite = selectedCharacters[i].sr.sprite;
                characterSlots[i].gameObject.SetActive(true);
            }
        }
    }
}
