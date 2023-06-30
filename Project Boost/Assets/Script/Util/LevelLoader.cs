using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Networking;
using TMPro;
using Newtonsoft.Json;
using UnityEngine.UI;

public class LevelLoader : MonoBehaviour
{
    [SerializeField]
    private GameObject _itemPrefab;
    [SerializeField]
    private GameObject _content;

    private void Start()
    {
        StartCoroutine(GetLevel());
    }

    IEnumerator GetLevel()
    {
        UnityWebRequest getLevelRequest = UnityWebRequest.Get("https://kaylemaximus.github.io/testApi/saveData.json");
        yield return getLevelRequest.SendWebRequest();
        if (getLevelRequest.error == null)
        {
            List<LevelData> myObjects = JsonConvert.DeserializeObject<List<LevelData>>(getLevelRequest.downloadHandler.text);
            foreach (LevelData item in myObjects)
            {
                var inventoryItem = Instantiate(_itemPrefab, new Vector3(), Quaternion.identity);
                inventoryItem.transform.SetParent(_content.transform);
                inventoryItem.GetComponent<Transform>().localScale = new Vector3(1f,1f,1f);
                inventoryItem.GetComponentInChildren<TMP_Text>().text = item.level.ToString();
                inventoryItem.GetComponent<LevelData>().AssignLevelValue(item.sceneName, item.active, item.time);
                inventoryItem.GetComponent<LevelData>().TriggerAssign();
            }
        }
        else
        {
            Debug.Log(getLevelRequest.error);
        }
    }

}
