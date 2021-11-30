using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.MLAgents;
using Unity.MLAgents.Sensors;
using System;
using Unity.MLAgents.Actuators;
// TODO: regenerate terrain each time the episode restarts;
public class jankGent : Agent
{
    private Rigidbody2D m_Rigidbody2D;
    public jankController controller;
    public Vector2 velocity;
    public float episodeLength;
    public float buffer; //arbitrary buffer added to min_xforce when deciding reward; buffer needed because min_xforce is often exceeded due to natural gravity
    public float timerCountDown;
    private bool timerOn;
    public float reward;

    public bool diving;

    public override void Initialize()
    {
        m_Rigidbody2D = GetComponent<Rigidbody2D>();
    }

    public override void OnEpisodeBegin()
    {
        StartCoroutine(controller.Reset());
        // controller.Reset();
        timerOn = true;
        timerCountDown = episodeLength;

    }

    public override void Heuristic(in ActionBuffers actionsOut)
    {
        ActionSegment<int> discreteActions = actionsOut.DiscreteActions;
        // discreteActions[0] = 1;
        /* bug is possibly happening because in heuristics it's assuming to have three actions somehow...
        0 is do nothing; but in heuristics after diving somehow 

        wait this makes kinda of sense ... why three branches work for heuristics but doesnt for the bot

        cuz we can check for getbuttonup so gravity resets, but this cheat doesnt exist for bot
        for bot it's just
        2. RESTORES original gravity 
        1. dive added gravity; once added gravity in one click, unless select do nothing immediately, gravit doesnt change 
        0. do nothing (retains gravity)

        */
        if(Input.GetButtonDown("Dive")){
            discreteActions[0] = 1;
        }
        if(Input.GetButtonUp("Dive")){
            discreteActions[0] = 2;
        }

        Debug.Log("passing" + discreteActions[0]);

    }

    public override void OnActionReceived(ActionBuffers actions)
    {
        int dive = actions.DiscreteActions[0];
        Debug.Log("received" + dive);
        if (dive == 1){
            controller.dive();
            diving = true;

        }
        if (dive == 2){
            controller.diveFalse();
            diving = false;
        }
    }


    // Update is called once per frame
    void Update()
    {
        velocity = m_Rigidbody2D.velocity;
        float speed = velocity.magnitude;
        // if (velocity.x < controller.min_xforce + buffer){
        //     AddReward(-speed/10);
        // }
        if (velocity.x == 0){
            AddReward(-.05f);
        }
        if (velocity.x < 0){
            AddReward(-speed/10);
        }
        else if (velocity.x > controller.min_xforce + buffer){
            AddReward(speed/10);
        }

        if(timerOn){
            if(timerCountDown > 0){
                timerCountDown -= Time.deltaTime;
            }
            else{
                EndEpisode();
            }
        }
        reward = GetCumulativeReward();

    }

}
