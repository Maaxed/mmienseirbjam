﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering.PostProcessing;

public class TurnManager : MonoBehaviour
{
    public static TurnManager Instance { get; private set; }

    public int turnCount;
    public float thiefTurnTime;
    public float securityTurnTime;
    public PostProcessVolume postProcess;
    public PostProcessProfile thiefProfile;
    public PostProcessProfile securityProfile;

    public bool ActivatedLamp { get; set; } = false;

    private int turnIndex;
    public float turnStartTime { get; private set; }

    public bool IsThiefTurn => turnIndex % 2 == 0;

    public float CurrentTirnTime => IsThiefTurn ? thiefTurnTime : securityTurnTime;

    private void Start()
    {
        Instance = this;
        initTurn();
    }

    private void FixedUpdate()
    {
        if (Time.time - turnStartTime >= CurrentTirnTime)
        {
            nextTurn();
        }
    }

    private void nextTurn()
    {
        turnIndex++;
        if (turnIndex >= turnCount)
        {
            //the end
        }
        else
        {
            initTurn();
        }
    }

    private void initTurn()
    {
        if (IsThiefTurn)
        {
            postProcess.profile = thiefProfile;
        }
        else
        {
            postProcess.profile = securityProfile;
            ActivatedLamp = false;
        }
        turnStartTime = Time.time;
    }
}
