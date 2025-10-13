using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
public class JoyStickController : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    [SerializeField]
    private RectTransform lever;
    private RectTransform rectTransform;
    // 추가
    [SerializeField, Range(10f, 150f)]
    private float leverRange;

    private Vector2 inputVector;   
    private bool isInput;
    public PlayerController playerController;
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        ControlJoystickLever(eventData);  
        isInput = true; 

    } 
    public void OnDrag(PointerEventData eventData)
    {

        ControlJoystickLever(eventData); 
        isInput = false;       
    }
    public void ControlJoystickLever(PointerEventData eventData)
    {
        var inputDir = eventData.position - rectTransform.anchoredPosition;
        var clampedDir = inputDir.magnitude < leverRange ? inputDir
            : inputDir.normalized * leverRange;
        lever.anchoredPosition = clampedDir;
        inputVector = clampedDir / leverRange;
    }
    public void OnEndDrag(PointerEventData eventData)
    {
        lever.anchoredPosition = Vector2.zero;
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }
    private void InputControlVector()
    {
        //if (playerController.nowControl)
        //{
        //    Update();
        //}
        //Debug.Log(inputDirection.x + " / " + inputDirection.y);
        // 캐릭터에게 입력벡터를 전달
    }
    // Update is called once per frame
    void Update()
    {
        if (isInput)
        {
            playerController.nowControl = true;
        }
    }

}
