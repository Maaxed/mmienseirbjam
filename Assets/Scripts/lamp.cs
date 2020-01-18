using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.Experimental.Rendering.LWRP;
using UnityEngine.SceneManagement;

public class lamp : MonoBehaviour {
    public GameObject player;
    public float radius;
    public float totalDuration;
    public float blinkDuration;

    private bool isOn = false;
    private float activeTimer;
    private new Light2D light;
    

    private bool hasElectricity = true;

    private bool isBlinking = false;

    public bool HasElectricity
    {
        get => hasElectricity;
        set
        {
            hasElectricity = value;
            light.intensity = value && isOn ? 1 : 0;
        }
    }

    private void Start() {
        // Get component
        light = this.GetComponent<Light2D>();
        CircleCollider2D collider = this.GetComponent<CircleCollider2D>();

        // Init radius for each component and state
        collider.radius = radius;
        light.pointLightOuterRadius = radius;
        light.intensity = 0;

    }

    private void Update()
    {
        if (isOn && hasElectricity)
        {
            activeTimer += Time.deltaTime;
            if (activeTimer >= totalDuration)
            {
                light.intensity = 0;
                isBlinking = false;
                isOn = false;
            }
            else if (activeTimer >= totalDuration - blinkDuration)
            {
                isBlinking = true;
            }
            else if (isBlinking && activeTimer >= blinkDuration)
            {
                light.intensity = 1;
                isBlinking = false;
            }

            if (isBlinking)
            {
                float blinkValue = (activeTimer % 1);
                if (blinkValue < 0) blinkValue += 1;
                light.intensity = (blinkValue < 0.25f || blinkValue > 0.75f) ? 0 : 1;
            }
        }
    }

    void OnMouseDown() {
        if(!TurnManager.Instance.IsThiefTurn && !isOn && HasElectricity && !TurnManager.Instance.ActivatedLamp)
        {
            TurnManager.Instance.ActivatedLamp = true;
            isOn = true;
            activeTimer = Time.time - TurnManager.Instance.turnStartTime - TurnManager.Instance.securityTurnTime;
            isBlinking = true;
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (isOn && hasElectricity && !isBlinking)
        {
            Destroy(player);
        }
    }
    private void OnTriggerStay2D(Collider2D collision)
    {
        if (isOn && hasElectricity && !isBlinking)
        {
            SceneManager.LoadSceneAsync("EndScreen");
        }
    }
}