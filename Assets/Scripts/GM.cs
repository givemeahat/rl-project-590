using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class GM : MonoBehaviour
{
    public GameObject gameOverScreen;
    public GameObject pauseScreen;
    public bool isPaused = false;
    public TMP_Text menuScoreText;
    public GameObject bonusText;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseToggle();
        }
    }
    public void ActivateBonusScoreText()
    {
        bonusText.SetActive(true);
    }
    public void EndGame()
    {
        Time.timeScale = 0;
        isPaused = true;
        gameOverScreen.SetActive(true);
    }

    public void PauseToggle()
    {
        isPaused = !isPaused;
        if (isPaused)
        {
            Time.timeScale = 0;
            pauseScreen.SetActive(true);
        }
        else
        {
            Time.timeScale = 1;
            pauseScreen.SetActive(false);
        }
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
