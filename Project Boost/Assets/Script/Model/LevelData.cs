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

    [SerializeField]
    private Sprite[] _startImage;
    [SerializeField]
    private Image _liveImage;

    private void Start()
    {
        _liveImage = GetComponent<Transform>().GetChild(1).GetComponent<Image>();
    }

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
        EvaluateLevel();
    }

    private void LoadLevel()
    {
        GameObject.Find("LevelLoader").GetComponent<SceneLoader>().LoadSelectedLevel(sceneName);
    }

    private void EvaluateLevel()
    {
        if (time == 0)
        {
            GetComponent<Transform>().GetChild(1).GetComponent<Image>().sprite = _startImage[0];
        }
        else
        if (time <= 30)
        {
            GetComponent<Transform>().GetChild(1).GetComponent<Image>().sprite = _startImage[3];
        }
        else if (time <= 60f && time > 30)
        {
            GetComponent<Transform>().GetChild(1).GetComponent<Image>().sprite = _startImage[2];
        }
        else
        {
            GetComponent<Transform>().GetChild(1).GetComponent<Image>().sprite = _startImage[1];
        }
    }

}
