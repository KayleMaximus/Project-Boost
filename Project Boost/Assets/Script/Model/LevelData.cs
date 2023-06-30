using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelData : MonoBehaviour
{
    public string sceneName;
    public int level;
    public float time;
    public bool active;

    public void AssignLevelValue(string name, bool isActive, float finishTime)
    {
        sceneName = name;
        active = isActive;
        time = finishTime;
    }

    public void TriggerAssign()
    {
        GetComponent<Button>().onClick.AddListener(LoadLevel);
        GetComponent<Button>().interactable = active;
    }

    private void LoadLevel()
    {
        GameObject.Find("LevelLoader").GetComponent<SceneLoader>().LoadSelectedLevel(sceneName);
    }

}
