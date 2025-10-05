using System.Collections.Generic;
using Newtonsoft.Json;
using System.IO;
using UnityEngine;
using Unity.VisualScripting;
using UnityEngine.InputSystem;

public class GameManager : MonoBehaviour
{
    public static GameManager instance;

    public Dictionary<string, bool> isLevelOpen;
    public Dictionary<string, float> highScore;

    private void Awake()
    {
        if (instance == null)
        {
            Debug.Log("create instance");
            isLevelOpen = new Dictionary<string, bool>();
            highScore = new Dictionary<string, float>();
            InstantiateDictionary();
            LoadData();
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void Update()
    {
        if (Helper.CurrentKeyboard.leftCtrlKey.IsPressed() && Helper.CurrentKeyboard.leftAltKey.IsPressed() && Helper.CurrentKeyboard.nKey.IsPressed())
        {
            isLevelOpen.Add("HarborMap", true);
        }
    }

    private void LoadData()
    {
        string fileNameLevelOpen = Application.persistentDataPath + "/PoomDataLevelOpen.json";
        string fileNameHighscore = Application.persistentDataPath + "/PoomDataHighscore.json";
        if (File.Exists(fileNameLevelOpen))
        {
            using (StreamReader sr = new StreamReader(fileNameLevelOpen))
            {
                string json = sr.ReadToEnd();
                if (json != null)
                {
                    Dictionary<string, bool> temp;
                    temp = JsonConvert.DeserializeObject<Dictionary<string, bool>>(json);
                    if(temp != null)
                    {
                        isLevelOpen = temp;
                    }
                }
            }
        }

        if (File.Exists(fileNameHighscore))
        {
            using (StreamReader sr = new StreamReader(fileNameHighscore))
            {
                string json = sr.ReadToEnd();
                if (json != null)
                {
                    Dictionary<string, float> temp;
                    temp = JsonConvert.DeserializeObject<Dictionary<string, float>>(json);
                    if(temp != null)
                    {
                        highScore = temp;
                    }
                }
            }
        }
    }


    private void InstantiateDictionary()
    {
        isLevelOpen.Add("Tutorial", true);
        isLevelOpen.Add("Church", true);
        isLevelOpen.Add("HarborMap", false);
        isLevelOpen.Add("Menu", true);
        isLevelOpen.Add("oil platform", true);
        highScore.Add("HarborMap", 0);
        highScore.Add("Tutorial", 0);
        highScore.Add("oil platform", 0);
    }

    private void OnApplicationQuit()
    {
        string fileNameLevelOpen = Application.persistentDataPath + "/PoomDataLevelOpen.json";
        string fileNameHighscore = Application.persistentDataPath + "/PoomDataHighscore.json";

        using (StreamWriter sw = new StreamWriter(fileNameLevelOpen))
        {
            string json = JsonConvert.SerializeObject(isLevelOpen);
            sw.Write(json);
        }
        using (StreamWriter sw = new StreamWriter(fileNameHighscore))
        {
            string json = JsonConvert.SerializeObject(highScore);
            sw.Write(json);

        }
    }
}
