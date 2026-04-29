using System;
using System.Collections.Generic;
using UnityEngine;

[Serializable]
public class LeaderboardData
{
    public List<LeaderboardEntry> entries = new List<LeaderboardEntry>();
}

public static class LeaderboardRepository
{
    private const string SaveKey = "LeaderboardData";
    private const string Lvl2Key = "LeaderboardData_Lvl2";
    private const string Lvl3Key = "LeaderboardData_Lvl3";
    private const int MaxEntries = 10;

    private const string OtavioInitKey = "OtavioInit_v4";
    private const string OtavioName = "OTAVIO";
    private const int OtavioScoreLvl1 = 18660;
    private const int OtavioScoreLvl2 = 475;
    private const int OtavioScoreLvl3 = 258;

    /// <summary>Runs automatically at game start, before any scene loads.</summary>
    [RuntimeInitializeOnLoadMethod(RuntimeInitializeLoadType.BeforeSceneLoad)]
    private static void InitializeOtavioScores()
    {
        if (PlayerPrefs.GetInt(OtavioInitKey, 0) == 1) return;

        ClearEntries(SaveKey);
        ClearEntries(Lvl2Key);
        ClearEntries(Lvl3Key);

        AddEntry(OtavioName, OtavioScoreLvl1, SaveKey);
        AddEntry(OtavioName, OtavioScoreLvl2, Lvl2Key);
        AddEntry(OtavioName, OtavioScoreLvl3, Lvl3Key);

        PlayerPrefs.SetInt(OtavioInitKey, 1);
        PlayerPrefs.Save();

        Debug.Log("[Leaderboard] OTAVIO scores initialized on all 3 leaderboards.");
    }

    /// <summary>Adds a new entry, sorts by score descending and saves.</summary>
    public static void AddEntry(string playerName, int score, string key = SaveKey)
    {
        LeaderboardData data = Load(key);
        data.entries.Add(new LeaderboardEntry(playerName, score));
        data.entries.Sort((a, b) => b.score.CompareTo(a.score));

        if (data.entries.Count > MaxEntries)
            data.entries.RemoveRange(MaxEntries, data.entries.Count - MaxEntries);

        Save(data, key);
    }

    /// <summary>Returns all entries sorted by score descending.</summary>
    public static List<LeaderboardEntry> GetEntries(string key = SaveKey)
    {
        LeaderboardData data = Load(key);
        return data.entries;
    }

    /// <summary>Clears all entries for the given leaderboard key.</summary>
    public static void ClearEntries(string key = SaveKey)
    {
        Save(new LeaderboardData(), key);
    }

    /// <summary>Returns true if any entry (excluding excludeName) has a score above the threshold.</summary>
    public static bool HasEntryAboveScore(int threshold, string key = SaveKey, string excludeName = null)
    {
        List<LeaderboardEntry> entries = GetEntries(key);
        foreach (LeaderboardEntry entry in entries)
        {
            if (excludeName != null && entry.playerName == excludeName)
                continue;
            if (entry.score > threshold)
                return true;
        }
        return false;
    }

    /// <summary>Removes all entries with the given player name from the leaderboard.</summary>
    public static void RemoveEntriesByName(string playerName, string key = SaveKey)
    {
        LeaderboardData data = Load(key);
        data.entries.RemoveAll(e => e.playerName == playerName);
        Save(data, key);
    }

    private static void Save(LeaderboardData data, string key)
    {
        string json = JsonUtility.ToJson(data);
        PlayerPrefs.SetString(key, json);
        PlayerPrefs.Save();
    }

    private static LeaderboardData Load(string key)
    {
        if (!PlayerPrefs.HasKey(key)) return new LeaderboardData();

        try
        {
            string json = PlayerPrefs.GetString(key);
            return JsonUtility.FromJson<LeaderboardData>(json) ?? new LeaderboardData();
        }
        catch
        {
            return new LeaderboardData();
        }
    }
}
