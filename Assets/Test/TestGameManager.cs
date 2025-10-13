using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class TestGameManager : MonoBehaviour
{
    public static TestGameManager Instance { get; private set; }


    public Button skillBtn;
    public Button attackBtn;

    private void Awake()
    {
        Instance = this;
    }
    private void OnDestroy()
    {
        Instance = null;
    }

    //[Header("PlayerObj 넣어주기")]
    //[SerializeField]
    //GameObject currentPlayer;




}
