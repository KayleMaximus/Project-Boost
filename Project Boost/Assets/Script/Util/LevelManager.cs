using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Newtonsoft.Json;
public class LevelManager : MonoBehaviour
{
    public Button[] _levelButtons;

    private  ISaveClient _client;

    private void Start()
    {
        _client = new CloudSaveClient();
        for (int i = 0; i < _levelButtons.Length; i++)
        {
            int levelReach = PlayerPrefs.GetInt("lvReach", 1);
            if (i + 1 > levelReach)
            {
                _levelButtons[i].interactable = false;
            }
        }
        if (PlayerPrefs.GetInt("lvReach", 1) > 1)
        {
            CloudLoad();
        }
    }

    private async void CloudLoad()
    {
        PlayerData objectData = await _client.Load<PlayerData>("PlayerLevelData");
        Debug.Log("Cloud Load Object: " + objectData.playerId + " : " + objectData.levelReach);
    }
}
