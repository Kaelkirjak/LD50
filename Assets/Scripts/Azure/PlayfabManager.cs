using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using PlayFab;
using PlayFab.ClientModels;

public class PlayfabManager : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        Login();
    }

    void Login()
    {
        if (!PlayerPrefs.HasKey("id"))
        {
            PlayerPrefs.SetInt("id", Random.Range(int.MinValue, int.MaxValue));
        }
        var request = new LoginWithCustomIDRequest
        {
            //CustomId = "Diccuric_Sigeon",
            CustomId = SystemInfo.deviceUniqueIdentifier,
            //CustomId = PlayerPrefs.GetInt("id").ToString(),
            CreateAccount = true
        };
        PlayFabClientAPI.LoginWithCustomID(request, OnSuccess, OnError);
    }

    void OnSuccess(LoginResult result)
    {
        Debug.Log("Successful login/account create!");
        LeaderboardManager.instance.LoadLeaderboard();
    }

    void OnError(PlayFabError error)
    {
        Debug.Log("Error while logging in/creating account!");
        Debug.Log(error.GenerateErrorReport());
    }

    public void SendLeaderboard(int score)
    {
        var request = new UpdatePlayerStatisticsRequest
        {
            Statistics = new List<StatisticUpdate>
            {
                new StatisticUpdate
                {
                    StatisticName = "TetrisScore",
                    Value = score
                }
            }
        };
        PlayFabClientAPI.UpdatePlayerStatistics(request, OnLeaderboardUpdate, OnError);
    }

    void OnLeaderboardUpdate(UpdatePlayerStatisticsResult result)
    {
        Debug.Log("Successful leaderboard sent");
    }

    public void GetLeaderboard()
    {
        var request = new GetLeaderboardRequest
        {
            StatisticName = "TetrisScore",
            StartPosition = 0,
            MaxResultsCount = 5
        };
        PlayFabClientAPI.GetLeaderboard(request, OnLeaderboardGet, OnError);
    }

    void OnLeaderboardGet(GetLeaderboardResult result)
    {
        foreach (var item in result.Leaderboard)
        {
            Debug.Log(item.Position + " " + item.PlayFabId + " " + item.StatValue);
            LeaderboardManager.instance.AddToLeaderboard(item.PlayFabId, item.Position, item.StatValue);
        }
    }
    public void SaveName(string name)
    {
        var request = new UpdateUserDataRequest
        {
            Data = new Dictionary<string, string>
            {
                { "Name", name }
            }, 
            Permission = UserDataPermission.Public
        };
        PlayFabClientAPI.UpdateUserData(request, OnDataSent, OnError);
    }

    public void GetName(string playfabID)
    {
        Debug.Log("Getting name");
        var request = new GetUserDataRequest
        {
            Keys = new List<string>() { "Name" },
            PlayFabId = playfabID
        };
        PlayFabClientAPI.GetUserData(request, (result) => OnDataRecieved(result, playfabID), OnError);
    }

    void OnDataRecieved(GetUserDataResult result, string idf)
    {
        LeaderboardManager.instance.SetUserName(idf, result.Data["Name"].Value);
    }

    void OnDataSent(UpdateUserDataResult result)
    {
        Debug.Log("Successful send of user data");
    }

    public static PlayfabManager instance;

    private void Awake()
    {
        if (instance == null)
        {
            instance = this;
        }
        else
        {
            Destroy(gameObject);
        }
    }
   

}
