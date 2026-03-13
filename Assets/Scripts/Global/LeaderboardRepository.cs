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
    private const int MaxEntries = 10;

    /// <summary>Adds a new entry, sorts by score descending and saves.</summary>
    public static void AddEntry(string playerName, int score, string key = SaveKey)
    {
        LeaderboardData data = Load(key);
        data.entries.Add(new LeaderboardEntry(playerName, score));
        data.entries.Sort((a, b) => b.score.CompareTo(a.score));

        if (data.entries.Count > MaxEntries)
            data.entries.RemoveRange(MaxEntries, data.entries.Count - MaxEntries);

        Save(data, key);
        Debug.Log($"[Leaderboard] Sauvegardé : {playerName} — {score} pts. Total : {data.entries.Count} entrée(s).");
    }

    /// <summary>Returns all entries sorted by score descending.</summary>
    public static List<LeaderboardEntry> GetEntries(string key = SaveKey)
    {
        LeaderboardData data = Load(key);
        Debug.Log($"[Leaderboard] Chargé : {data.entries.Count} entrée(s).");
        return data.entries;
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