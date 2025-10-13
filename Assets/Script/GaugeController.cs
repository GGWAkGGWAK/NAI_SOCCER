using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GaugeController : MonoBehaviour
{

    public Slider sliderA;
    float gauge;
    float gauge_full;
    // Start is called before the first frame update
    private void Awake()
    {

    }

    void Start()
    {
        gauge = 0.33f;
        gauge_full = 1f;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public void OnCilickButtonA()
    {
        sliderA.value =   gauge / gauge_full;
    }
}


