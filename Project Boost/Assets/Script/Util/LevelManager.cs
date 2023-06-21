using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LevelManager : MonoBehaviour
{
    public Button[] _levelButtons;

    private void Start()
    {
        for (int i = 0; i < _levelButtons.Length; i++)
        {
            int levelReach = PlayerPrefs.GetInt("lvReach", 1);
            if( i + 1 > levelReach)
            {
                _levelButtons[i].interactable = false;
            }
        }
    }
}
