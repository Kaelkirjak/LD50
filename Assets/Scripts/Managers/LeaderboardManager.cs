using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class LeaderboardManager : MonoBehaviour
{
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    #region UI Variables

    [Header("Leaderboard UI components")]
    [Tooltip("Text showing the latest score")][SerializeField]                      TMP_Text latestScoreText;
    [Tooltip("Text box for name insertion")][SerializeField]                        TMP_InputField nameField;
    [Tooltip("Submit score button")][SerializeField]                                Button submitButton;
    [Tooltip("Leaderboard texts")][SerializeField]                                  TMP_Text[] leaderboardTexts;

    #endregion UI Variables

    #region Leaderboard Variables

    [Header("Leaderboard Variables")]
    [Tooltip("Latest score achieved by the player")][SerializeField]                int latestScore = 0;
    [Tooltip("Current name given by the player")][SerializeField]                   string currentName;
    [Tooltip("Leaderboard Dictionary")][SerializeField]                             Dictionary<string, (string name, int place, int score)> leaderboardDict = new Dictionary<string, (string name, int place, int score)>();

    #endregion Leaderboard Variables

    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    #region Leaderboard Functions

    void SubmitScore()
    {
        submitButton.interactable = false;
        Debug.Log("Submitting score");
        if (currentName == null || currentName == "")
        {
            currentName = "ANON";
        }
        PlayfabManager.instance.SaveName(currentName);
        PlayfabManager.instance.SendLeaderboard(latestScore);
        Invoke("LoadLeaderboard", 3);
    }

    public void LoadLeaderboard()
    {
        leaderboardDict = new Dictionary<string, (string name, int place, int score)>();
        PlayfabManager.instance.GetLeaderboard();
    }

    public void AddToLeaderboard(string id, int position, int score)
    {
        leaderboardDict.Add(id, ("Anon", position, score));
        PlayfabManager.instance.GetName(id);
    }

    public void SetUserName(string id, string name)
    {
        leaderboardDict[id] = (name, leaderboardDict[id].place, leaderboardDict[id].score);
        SetLeaderboardText(leaderboardDict[id].place, leaderboardDict[id].score, leaderboardDict[id].name);
    }

    void SetLeaderboardText(int place, int score, string name)
    {
        leaderboardTexts[place].text = $"{place+1}. {name}: {score}";
    }

    #endregion Leaderboard Functions

    #region Managment Functions

    public void SetScore(int latestScore)
    {
        this.latestScore = latestScore;
        latestScoreText.text = $"Latest Score: {this.latestScore}";
        submitButton.interactable = true;
    }

    void NameChanged(string newName)
    {
        currentName = newName;
    }

    void UIEventSuscriptions()
    {
        nameField.onValueChanged.AddListener(NameChanged);
        submitButton.onClick.AddListener(SubmitScore);
    }

    void UIEventUnsubscriptions()
    {
        nameField.onValueChanged.RemoveListener(NameChanged);
        submitButton.onClick.RemoveListener(SubmitScore);
    }

    #endregion Managment Functions

    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------

    #region General Functions

    private void Start()
    {
        UIEventSuscriptions();
    }
    private void OnDestroy()
    {
        UIEventUnsubscriptions();
    }
    private void Awake()
    {
        InstanceAwake();
    }

    #endregion General Functions

    #region Instance Managment

    public static LeaderboardManager instance;
    void InstanceAwake()
    {
        if(instance != null)
        {
            Destroy(gameObject);
        }
        else
        {
            instance = this;
            DontDestroyOnLoad(gameObject);
        }
    }

    #endregion Instance Managment

    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
    /// ---------------------------------------------------------------------------------------------------------------------------------------------
}
