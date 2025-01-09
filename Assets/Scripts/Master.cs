using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Master : MonoBehaviour
{
    public GameObject settingsPanel;

    public void ToggleSettingsPanel()
    {
        settingsPanel.SetActive(!settingsPanel.activeInHierarchy);
    }

    public void LoadScene(int _index)
    {
        SceneManager.LoadScene(_index);
    }
}
