using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseScreen : MonoBehaviour
{
    public GM GM;

    public void TriggerClose()
    {
        this.GetComponent<Animator>().Play("Pause_Close");
    }

    public void ClosePause()
    {
        GM.PauseToggle();
    }
}
