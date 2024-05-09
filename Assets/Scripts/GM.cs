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

    public int islandCount = 0;

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
        islandCount++;
        GameObject.FindGameObjectWithTag("Hunter").GetComponent<Hunter>().speed = GameObject.FindGameObjectWithTag("Hunter").GetComponent<Hunter>().speed + 20;
        GameObject terrain = Instantiate(terrainPrefab) as GameObject;
        terrainGen _gen = terrain.GetComponent<terrainGen>();
        //_gen.pointCount = 250;
        _gen.scale = 3000;
        _gen.minHeightDifference = 1;
        _gen.heightRange = 1.5f + islandCount;
        _gen.generateTerrain();
        playerCont.TerrainGen = _gen;
        terrain.transform.localPosition = new Vector3(player.transform.position.x + 350f, 0f, 182f);
        levelGenTriggered = true;
    }

    public void ResetGame()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex);
    }
}
