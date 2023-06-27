using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;
using Newtonsoft.Json;

public class LevelManager : MonoBehaviour
{
    public Button[] _levelButtons;

    private void Start()
    {
        StartCoroutine(GetLevel());
    }

    IEnumerator GetLevel()
    {
        UnityWebRequest getLevelRequest = UnityWebRequest.Get("https://kaylemaximus.github.io/testApi/saveData.json");
        yield return getLevelRequest.SendWebRequest();
        if(getLevelRequest.error == null)
        {
            Debug.Log(getLevelRequest.downloadHandler.text);
            LevelData levelData = JsonConvert.DeserializeObject<LevelData>(getLevelRequest.downloadHandler.text);
            Debug.Log("Level: " + levelData.levelReach);
            PlayerPrefs.SetInt("lvReach", levelData.levelReach);
            for (int i = 0; i < _levelButtons.Length; i++)
            {
                //int levelReach = PlayerPrefs.GetInt("lvReach", 1);
                int levelReach = levelData.levelReach;
                if (i + 1 > levelReach)
                {
                    _levelButtons[i].interactable = false;
                }
            }
        }
        else
        {
            Debug.Log(getLevelRequest.error);
        }
    }
}
