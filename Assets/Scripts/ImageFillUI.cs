using UnityEngine;
using UnityEngine.Events;
using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;

public class ImageFillUI : MonoBehaviour
{
    [SerializeField] private int ability;
    private float coolDown;
    private float coolDownSince;
    private Image image;
    [SerializeField] private jankController jc;
    private void Start() {
        image = GetComponent<Image>();
    }
    private void Update(){
        if(ability == 0){//hop
            coolDown = jc.hopCoolDown;
            coolDownSince = jc.timeSinceHop;
        } 
        if(ability == 1){//jet
            coolDown = jc.jetCoolDown;
            coolDownSince = jc.timeSinceJet;
        }
        image.fillAmount = coolDownSince/coolDown;
    }
}