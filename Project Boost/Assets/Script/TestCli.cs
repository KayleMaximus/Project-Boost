using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CliWrap;
using CliWrap.Buffered;
using System.IO;
using System.Threading.Tasks;
using Unity.Services.Authentication;
using Unity.Services.Core;
public class TestCli : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {

    }


    // Update this method with the player progress you want to save
    private string GetPlayerProgressData()
    {
        // Example: Saving player level progress
        int playerLevel = PlayerPrefs.GetInt("lvReach", 1);
        PlayerData player = new PlayerData { levelReach = 1, playerId = PlayerData.defaultPlayerId };
        return JsonUtility.ToJson(player);
    }

    // Call this method whenever the player makes progress
    public async void SavePlayerProgress()
    {
        string saveData = GetPlayerProgressData();
        //string filePath = Application.dataPath + "/saveData.json";
        string filePath = @"D:\Skill++\Unity Project\Projects\testApi\saveData.json";
        string folderPath = @"D:\Skill++\Unity Project\Projects\testApi";
        // Write the JSON data to a file
        File.WriteAllText(filePath, saveData);

        // Push the file to GitHub using Git command-line tool with token authentication
        string remoteUrl = "https://github.com/KayleMaximus/testApi.git"; // Replace with your remote repository URL
        string branch = "main"; // Replace with the branch name you want to push to
        string personalAccessToken = "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"; // Replace with your personal access token



        var gitHubResults = await Cli.Wrap(@"D:\Skill++\Unity Project\Projects\pushFile.bat").ExecuteBufferedAsync();
        Debug.Log(gitHubResults.StandardOutput);
        Debug.Log(gitHubResults.StandardError);
    }

    private async void testCli()
    {
        var gitHubResults = await Cli.Wrap("git").WithArguments(new[] { "--version" }).ExecuteBufferedAsync();
        Debug.Log(gitHubResults.StandardOutput);
        Debug.Log(gitHubResults.StandardError);
    }
}
