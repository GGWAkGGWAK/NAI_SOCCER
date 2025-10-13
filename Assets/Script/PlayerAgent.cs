using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using Unity.MLAgents.Actuators;
using Unity.MLAgents.Policies;

public class PlayerAgent : Agent
{
    public enum Team
    {
        BLUE,RED
    }
    private Team team = Team.BLUE;

    private BehaviorParameters bps;

    public override void Initialize()
    {
        bps = GetComponent<BehaviorParameters>();
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
