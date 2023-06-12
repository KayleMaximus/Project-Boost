using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MenuLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject[] _menu;

    [SerializeField]
    private GameObject _Menutransition = null;

    private string _menuOn;

    public float _transitionTime = 1f;

    #region old transition
    public void GetMenu(string input)
    {
        _menuOn = input;
    }

    public void LoadMenu()
    {
        StartCoroutine(LoadSelectMenu(_menuOn));
    }

    public void DisableMenu(string menuOff)
    {
        StartCoroutine(UnLoadSelectMenu(menuOff));
    }

    IEnumerator UnLoadSelectMenu(string menuOff)
    {
        _Menutransition.SetActive(true);
        yield return new WaitForSeconds(_transitionTime);

        GameObject tempMenuOff = null;
        foreach (GameObject menuName in _menu)
        {
            if (menuName.transform.name.Equals(menuOff)) tempMenuOff = menuName;
        }
        tempMenuOff.SetActive(false);

        Invoke("LoadMenu", 0f);
    }

    IEnumerator LoadSelectMenu(string menuOn)
    {
        GameObject tempMenuOn = null;
        foreach (GameObject menuName in _menu)
        {
            if (menuName.transform.name.Equals(menuOn)) tempMenuOn = menuName;
        }
        tempMenuOn.SetActive(true);

        _Menutransition.GetComponent<Animator>().SetTrigger("Start");
        yield return new WaitForSeconds(_transitionTime);

        Invoke("DisableAnimator", 0.75f);
    }

    private void DisableAnimator()
    {
        _Menutransition.SetActive(false);
    }
    #endregion

    public void OpenSetting()
    {
        GetComponent<Animator>().SetTrigger("OpenSetting");
    }
}
