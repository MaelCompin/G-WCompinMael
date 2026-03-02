using System;

[Serializable]
public class LeaderboardEntry
{
    public string playerName;
    public int score;

    public LeaderboardEntry(string name, int score)
    {
        playerName = name;
        this.score = score;
    }
}
