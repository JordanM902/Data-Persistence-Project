using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using TMPro;

#if UNITY_EDITOR
using UnityEditor;
#endif

[DefaultExecutionOrder(1000)]
public class GameManager : MonoBehaviour
{
    private MainManager mainManager;

    public string playerName;
    public string highScorePlayerName;
    public int highScore;
    public TMP_InputField inputField; // Change to InputField
    public TextMeshProUGUI welcomeText;
    public TextMeshProUGUI highScoreText; // Change to TextMeshProUGUI

    void Awake()
    {
        LoadData();
        if (!string.IsNullOrEmpty(playerName))
        {
            welcomeText.text = "Welcome " + playerName + "!";
            highScoreText.text = "High Score: " + highScorePlayerName + " - " + highScore;
        }
    }

    void Start()
    {
        mainManager = FindObjectOfType<MainManager>();
        inputField.onEndEdit.AddListener(OnInputEndEdit);
    }

    void OnInputEndEdit(string input)
    {
        if (Input.GetKeyDown(KeyCode.Return) || Input.GetKeyDown(KeyCode.KeypadEnter))
        {
            SetPlayerName();
        }
    }

    public void SetPlayerName()
    {
        playerName = inputField.text;
        welcomeText.text = "Welcome " + playerName + "!";
        SaveGame();
    }
    public void ResetHighScore()
    {
        highScore = 0;
        highScorePlayerName = "None";
        playerName = null;
        highScoreText.text = "High Score: " + highScorePlayerName + " - " + highScore;
        SaveGame();
    }

    public void StartGame()
    {
        SceneManager.LoadScene(1);
    }

    public void ReturnToMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void Exit()
    {
        SaveGame();
#if UNITY_EDITOR
        EditorApplication.ExitPlaymode();
#else
        Application.Quit();
#endif
    }

    [System.Serializable]
    class SaveData
    {
        public string playerName;
        public int highScore;
        public string highScorePlayerName;
    }

    public void SaveGame()
    {
        SaveData data = new SaveData();
        data.playerName = playerName;
        data.highScore = highScore;
        data.highScorePlayerName = highScorePlayerName;
        //data.mainManager.highScorePlayerName = mainManager.highScorePlayerName;
        string json = JsonUtility.ToJson(data);
        System.IO.File.WriteAllText(Application.persistentDataPath + "/savefile.json", json);
    }

    public void LoadData()
    {
        string path = Application.persistentDataPath + "/savefile.json";
        if (System.IO.File.Exists(path))
        {
            string json = System.IO.File.ReadAllText(path);
            SaveData data = JsonUtility.FromJson<SaveData>(json);
            playerName = data.playerName;
            highScore = data.highScore;
            highScorePlayerName = data.highScorePlayerName;
        }
    }
}
