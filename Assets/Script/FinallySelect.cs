using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FinallySelect : MonoBehaviour
{
    //private IEnumerator coroutine;
    public Image emptyImage_3; // 빈 이미지
    public Image emptyImage_4;
    public Image emptyImage_5;
    public LayerMask characterLayer; // 캐릭터 이미지가 있는 레이어

    private Rigidbody2D rigid;
    GraphicRaycaster raycaster;
    PointerEventData pointerEventData;
    EventSystem eventSystem;
    [SerializeField]
    public Sprite Box_1;
    //public Sprite Box_2;
    public Characters characters;
    public Characters characters_1;
    public Characters characters_2;

    public Sprite characters_Ren;
    public Sprite characters_Amy;
    public Sprite characters_Dr7;
    public Sprite characters_CoCo;
    public Sprite characters_Lucy;
    public Sprite characters_Ouka;

    Animator anim;
    SpriteRenderer sr_1;
    public SelectChar[] chars;
    public GameObject spotlight;
    RaycastHit hit;
    public int maxSelectedCharacters = 100;

    public List<FinallySelect> selectedCharacters1P = new List<FinallySelect>();
    public List<FinallySelect> selectedCharacters2P = new List<FinallySelect>();
    public Sprite[] sprites;
    public Image[] characterSlots_1P;

    // Flag to determine whether it's currently 1P's turn or 2P's turn
    private bool isPlayer1Turn_1 = true;
    public LayerMask selectableLayer; // 선택 가능한 게임 오브젝트의 레이어
    private bool characterEnter;

    AudioSource audioSource;
    AudioSource childaudioSource;
    [SerializeField]
    GameObject childAudio;
    void Start()
    {
        rigid = GetComponent<Rigidbody2D>();
        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
        anim = GetComponent<Animator>();
        sr_1 = GetComponent<SpriteRenderer>();

        childAudio = gameObject.transform.GetChild(0).gameObject;
        audioSource = this.gameObject.GetComponent<AudioSource>();
        childaudioSource = childAudio.GetComponent<AudioSource>();
        /*characters = GetComponent<SelectChar>().characters;
        characters_1 = GetComponent<SelectChar>().characters_1;
        characters_2 = GetComponent<SelectChar>().characters_2;*/
    }
    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(rigid.position, Vector2.down, 100, LayerMask.GetMask("Character"));

        List<FinallySelect> currentPlayerSelection_1 = new List<FinallySelect>();
        //List<FinallySelect1> otherPlayerSelection_2 = isPlayer1Turn_2 ? selectedCharacters2P : selectedCharacters1P;



        if (Input.GetKeyDown(KeyCode.A))
        {
            
            if(transform.position.x <= -801)
            {
                transform.position = new Vector3(-801, transform.position.y, transform.position.z);
                transform.Translate(0, 0, 0);
            }
            else
            {
                transform.Translate(-3, 0, 0);
                audioSource.Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.D))
        {
            
            if(transform.position.x >= 820)
            {
                transform.position = new Vector3(820, transform.position.y, transform.position.z);
                transform.Translate(0, 0, 0);
            }
            else
            {
                transform.Translate(3, 0, 0);
                audioSource.Play();
            }
        }

        if (hit.collider != null)
        {
            if (Input.GetKeyDown(KeyCode.Space))
            {
                isPlayer1Turn_1 = !isPlayer1Turn_1;
                currentPlayerSelection_1.Add(this);
                Debug.Log(hit.collider.name);
                childaudioSource.Play();
                SelectChar zz = hit.collider.GetComponent<SelectChar>();
                if (hit.collider.name == "Ren")
                {
                    characters = zz.characters;
                    characters_1 = zz.characters;
                    characters_2 = zz.characters;
                }
                else if (hit.collider.name == "Amy")
                {
                    characters = zz.characters_1;
                    characters_1 = zz.characters_1;
                    characters_2 = zz.characters_1;
                }
                else if (hit.collider.name == "Dr.7")
                {
                    characters = zz.characters_2;
                    characters_1 = zz.characters_2;
                    characters_2 = zz.characters_2;
                }
                else if (hit.collider.name == "CoCo")
                {
                    characters = zz.characters_3;
                    characters_1 = zz.characters_3;
                    characters_2 = zz.characters_3;
                }
                else if (hit.collider.name == "Lucy")
                {
                    characters = zz.characters_4;
                    characters_1 = zz.characters_4;
                    characters_2 = zz.characters_4;
                }
                else if (hit.collider.name == "Ouka")
                {
                    characters = zz.characters_5;
                    characters_1 = zz.characters_5;
                    characters_2 = zz.characters_5;
                }

                int index = currentPlayerSelection_1.Count - 1;
                SetImage(hit.collider.GetComponent<SpriteRenderer>().sprite);


            }
        }
    }
    void SetImage(Sprite characterSprite)
    {
        // 빈 이미지에 새로운 스프라이트 할당
        if (emptyImage_3.sprite == Box_1)
        {
            emptyImage_3.sprite = characterSprite;



            DataMgr.instance.currentCharacter = characters;
            //characters = SelectChar.characters_2;


        }
        else if (emptyImage_4.sprite == Box_1)
        {
            emptyImage_4.sprite = characterSprite;
            DataMgr.instance.currentCharacter_1 = characters_1;
        }
        else if (emptyImage_5.sprite == Box_1)
        {
            emptyImage_5.sprite = characterSprite;
            DataMgr.instance.currentCharacter_2 = characters_2;
        }

    }
}