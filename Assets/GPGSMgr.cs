using System;
using System.Linq;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using GooglePlayGames.BasicApi.SavedGame;
using UnityEngine;

public static class GPGSMgr
{
    private static string savedGameFilename = "save.bin";
    private static string saveData = "세이브 로드 확인";

    public static void ShowSelectUI()
    {
        uint maxNumToDisplay = 5;
        bool allowCreateNew = false;
        bool allowDelete = true;

        ISavedGameClient savedGameClient = PlayGamesPlatform.Instance.SavedGame;
        savedGameClient.ShowSelectSavedGameUI("Select saved game",
            maxNumToDisplay,
            allowCreateNew,
            allowDelete,
            OnSavedGameSelected);
    }

    public static void OnSavedGameSelected(SelectUIStatus status, ISavedGameMetadata game)
    {
        if (status == SelectUIStatus.SavedGameSelected)
        {
            // handle selected game save
            OpenSavedGame(game.Filename, OnSavedGameOpenedForLoad);
        }
        else
        {
            // handle cancel or error
        }
    }

    public static void SaveGame()
    {
        OpenSavedGame(savedGameFilename, OnSavedGameOpenedForSave);
    }

    static void OnSavedGameOpenedForSave(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            byte[] data = System.Text.Encoding.UTF8.GetBytes(saveData);
            SavedGameMetadataUpdate update = new SavedGameMetadataUpdate.Builder()
                .WithUpdatedDescription("Saved at " + DateTime.Now.ToString())
                .Build();

            PlayGamesPlatform.Instance.SavedGame.CommitUpdate(game, update, data, OnSavedGameCommit);
        }
        else
        {
            //log.text = "Failed to open saved game.";
        }
    }

    static void OnSavedGameCommit(SavedGameRequestStatus commitStatus, ISavedGameMetadata updatedGame)
    {
        if (commitStatus == SavedGameRequestStatus.Success)
        {
            //log.text = "Game saved successfully.";
        }
        else
        {
            //log.text = "Failed to save game.";
        }
    }

    public static void LoadGame()
    {
        OpenSavedGame(savedGameFilename, OnSavedGameOpenedForLoad);
    }

    static void OnSavedGameOpenedForLoad(SavedGameRequestStatus status, ISavedGameMetadata game)
    {
        if (status == SavedGameRequestStatus.Success)
        {
            PlayGamesPlatform.Instance.SavedGame.ReadBinaryData(game, OnSavedGameDataRead);
        }
        else
        {
            //log.text = "Failed to open saved game.";
        }
    }

    static void OnSavedGameDataRead(SavedGameRequestStatus readStatus, byte[] data)
    {
        if (readStatus == SavedGameRequestStatus.Success)
        {
            string loadedData = System.Text.Encoding.UTF8.GetString(data);
            //log.text = "Game loaded successfully: " + loadedData;
        }
        else
        {
            //log.text = "Failed to load game.";
        }
    }

    private static void OpenSavedGame(string filename, Action<SavedGameRequestStatus, ISavedGameMetadata> callback)
    {
        PlayGamesPlatform.Instance.SavedGame.OpenWithAutomaticConflictResolution(
            filename,
            DataSource.ReadCacheOrNetwork,
            ConflictResolutionStrategy.UseLongestPlaytime,
            callback);
    }

    public static void ShowLeaderBoard()
    {
        Social.ShowLeaderboardUI();
    }

    public static void ReportScore(int score)
    {
        Social.ReportScore(score, MyGPGSIds.leaderboard, (bool success) => {
            // Handle success or failure
        });
    }

    public static void ReportAchievement(string achievementId)
    {
        Social.ReportProgress(achievementId, 100.0f, (bool success) =>
        {
            if(success)
            {
                if(MyGPGSIds.scoreAchievements.Contains(achievementId))
                {
                    ++Achievements.currentIndex;
                }
            }
        });
    }

    public static void ReportTestAchievement(string achievementId)
    {
        Social.ReportProgress(achievementId, 100.0f, (bool success) =>
        {
        });
    }
}
