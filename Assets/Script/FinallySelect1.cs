using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class FinallySelect1 : MonoBehaviour
{
    //private IEnumerator coroutine;
    public Image emptyImage; // 빈 이미지
    public Image emptyImage_1;
    public Image emptyImage_2;
    public LayerMask characterLayer; // 캐릭터 이미지가 있는 레이어

    private Rigidbody2D rigid;
    GraphicRaycaster raycaster;
    PointerEventData pointerEventData;
    EventSystem eventSystem;
    public Sprite Box_2;

    public Characters characters_3;
    public Characters characters_4;
    public Characters characters_5;
    Animator anim;
    SpriteRenderer sr_2;
    public SelectChar[] chars;
    public GameObject spotlight;
    RaycastHit hit;
    public int maxSelectedCharacters = 100;

    public List<FinallySelect1> selectedCharacters1P = new List<FinallySelect1>();
    public List<FinallySelect1> selectedCharacters2P = new List<FinallySelect1>();
    public Sprite[] sprites;
    public Image[] characterSlots_2P;

    // Flag to determine whether it's currently 1P's turn or 2P's turn
    private bool isPlayer1Turn_2 = true;
    public LayerMask selectableLayer; // 선택 가능한 게임 오브젝트의 레이어
    private bool characterEnter;

    AudioSource audioSource;
    AudioSource childaudioSource;
    [SerializeField]
    GameObject childAudio;
    void Start()
    {
        //coroutine = SetImage_1(2.0f); //반환값 IEnumerator를 저장해둔다.
       // StartCoroutine(coroutine);
        //Invoke("SetImage_1", 100f);
        //Invoke("SetImage_2", 100f);
        rigid = GetComponent<Rigidbody2D>();
        raycaster = GetComponent<GraphicRaycaster>();
        eventSystem = GetComponent<EventSystem>();
        anim = GetComponent<Animator>();
        sr_2 = GetComponent<SpriteRenderer>();

        childAudio = gameObject.transform.GetChild(0).gameObject;
        audioSource = this.gameObject.GetComponent<AudioSource>();
        childaudioSource = childAudio.GetComponent<AudioSource>();

        if (DataMgr.instance.currentCharacter == characters_3) OnSelect_2();
        else OnDeSelect_2();
    }

    private void OnMouseUpAsButton()
    {
        Debug.Log("ㅋㅋ");
    }

    void OnSelect_2()
    {
        // 선택된 캐릭터에 대한 추가적인 로직을 작성하세요.
    }

    void OnDeSelect_2()
    {
        // 선택이 해제된 경우에 대한 로직을 작성하세요.
    }

    void Update()
    {
        RaycastHit2D hit = Physics2D.Raycast(rigid.position, Vector2.down, 100, LayerMask.GetMask("Character"));

        List<FinallySelect1> currentPlayerSelection_2 = new List<FinallySelect1>();
        //List<FinallySelect1> otherPlayerSelection_2 = isPlayer1Turn_2 ? selectedCharacters2P : selectedCharacters1P;

 

        if (Input.GetKeyDown(KeyCode.LeftArrow))
        {
            
            if (transform.position.x <= -801)
            {
                transform.position = new Vector3(-800, transform.position.y, transform.position.z);
                transform.Translate(0, 0, 0);
            }
            else
            {
                transform.Translate(-3, 0, 0);
                audioSource.Play();
            }
        }
        if (Input.GetKeyDown(KeyCode.RightArrow))
        {
            
            if (transform.position.x >= 820)
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
            if (Input.GetKeyDown(KeyCode.Return))
            {
                
                OnSelect_2();
                isPlayer1Turn_2 = !isPlayer1Turn_2;
                Debug.Log("안녕");
                currentPlayerSelection_2.Add(this);
                UpdateCharacterUI_2(currentPlayerSelection_2);
                Debug.Log(hit.collider.name);
                childaudioSource.Play();
                SelectChar zzz = hit.collider.GetComponent<SelectChar>();
                if (hit.collider.name == "Ren")
                {
                    characters_3 = zzz.characters;
                    characters_4 = zzz.characters;
                    characters_5 = zzz.characters;
                }
                else if (hit.collider.name == "Amy")
                {
                    characters_3 = zzz.characters_1;
                    characters_4 = zzz.characters_1;
                    characters_5 = zzz.characters_1;
                }
                else if (hit.collider.name == "Dr.7")
                {
                    characters_3 = zzz.characters_2;
                    characters_4 = zzz.characters_2;
                    characters_5 = zzz.characters_2;
                }
                else if (hit.collider.name == "CoCo")
                {
                    characters_3 = zzz.characters_3;
                    characters_4 = zzz.characters_3;
                    characters_5 = zzz.characters_3;
                }
                else if (hit.collider.name == "Lucy")
                {
                    characters_3 = zzz.characters_4;
                    characters_4 = zzz.characters_4;
                    characters_5 = zzz.characters_4;
                }
                else if (hit.collider.name == "Ouka")
                {
                    characters_3 = zzz.characters_5;
                    characters_4 = zzz.characters_5;
                    characters_5 = zzz.characters_5;
                }

                // 엔터 키를 누를 때마다 선택된 캐릭터의 이미지를 변경
                int index = currentPlayerSelection_2.Count - 1;
                SetImage(hit.collider.GetComponent<SpriteRenderer>().sprite);
                //SetImage_1(hit.collider.GetComponent<SpriteRenderer>().sprite);

                //SetImage_2(hit.collider.GetComponent<SpriteRenderer>().sprite);
                //Invoke("SetImage_1", 100000000000f);

                // 추가된 부분: 현재 선택된 캐릭터의 인덱스가 배열의 길이를 초과하지 않는지 확인
                if (index < sprites.Length && index < characterSlots_2P.Length)
                {
                    // 이미지 UI에 새로운 스프라이트 할당
                    characterSlots_2P[index].sprite = sprites[index];
                    characterSlots_2P[index].gameObject.SetActive(true);
                }

            }
        }
    }
    void SetImage(Sprite characterSprite)
    {
        // 빈 이미지에 새로운 스프라이트 할당
        //emptyImage.sprite = characterSprite;
        if (emptyImage.sprite == Box_2)
        {
            emptyImage.sprite = characterSprite;
            DataMgr.instance.currentCharacter_3 = characters_3;
        }
        else if (emptyImage_1.sprite == Box_2)
        {
            emptyImage_1.sprite = characterSprite;
            DataMgr.instance.currentCharacter_4 = characters_4;
        }
        else if (emptyImage_2.sprite == Box_2)
        {
            emptyImage_2.sprite = characterSprite;
            DataMgr.instance.currentCharacter_5 = characters_5;
        }
        //emptyImage_1.sprite = characterSprite;
        //emptyImage_2.sprite = characterSprite;
    }
    void SetImage_1(Sprite characterSprite)
    {
        // 빈 이미지에 새로운 스프라이트 할당
        emptyImage_1.sprite = characterSprite;
        //emptyImage_1.sprite = characterSprite;
        //emptyImage_2.sprite = characterSprite;
    }
    void SetImage_2(Sprite characterSprite)
    {
        // 빈 이미지에 새로운 스프라이트 할당
        emptyImage_2.sprite = characterSprite;
        //emptyImage_1.sprite = characterSprite;
        //emptyImage_2.sprite = characterSprite;
    }
    void UpdateCharacterUI_2(List<FinallySelect1> selectedCharacters)
    {
        for (int i = 3; i < selectedCharacters.Count; i++)
        {
            if (i < characterSlots_2P.Length)
            {
                // Set the sprite of the UI Image to the selected character's sprite
                characterSlots_2P[i].sprite = selectedCharacters[i].sr_2.sprite;
                characterSlots_2P[i].gameObject.SetActive(true);
            }
        }
    }
}
