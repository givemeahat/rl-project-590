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

    public GameObject terrainPrefab;
    public List<GameObject> islands;
    public bool levelGenTriggered;

    public GameObject player;
    public jankController playerCont;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            PauseToggle();
        }
        if (!levelGenTriggered)
        {
            if (playerCont.currentStatus == jankController.PlayerStatus.FLYINGTONEXT)
            {
                GenerateNextIsland();
            }
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

    public void GenerateNextIsland()
    {
        GameObject terrain = Instantiate(terrainPrefab) as GameObject;
        terrain.GetComponent<terrainGen>().generateTerrain();
        playerCont.TerrainGen = terrain.GetComponent<terrainGen>();
        terrain.transform.localPosition = new Vector3(player.transform.localPosition.x + 1000f, 0f, 182f);
        levelGenTriggered = true;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
