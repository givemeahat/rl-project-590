using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TutorialManager : MonoBehaviour
{
    int tutorialCount = 0;
    Animator anim;
    GameObject player;

    public void Start()
    {
        anim = this.GetComponent<Animator>();
        player = GameObject.FindGameObjectWithTag("Player");
        player.GetComponent<jankController>().enabled = false;

    }

    // Update is called once per frame
    void Update()
    {
        if (tutorialCount == 0)
        {
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                anim.Play("Tutorial_P1_Fade");
            }
        }
    }

    public void EndTutorial()
    {
        tutorialCount++;
        this.gameObject.SetActive(false);
        player.GetComponent<jankController>().enabled = true;

    }

}
