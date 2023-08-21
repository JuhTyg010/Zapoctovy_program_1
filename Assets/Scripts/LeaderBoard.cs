using System;
using System.Collections.Generic;
using UnityEngine;
using Dan.Main;


public static class LeaderBoard
{
    private static string _leaderboardKey = "ecbd72bad89772548533cba5157bf397876cf0bac69d63d7d281667e1f287451";

    public static List<string> GetLeaderboard(int max)
    {
        List<string> high = new List<string>();
        LeaderboardCreator.GetLeaderboard(_leaderboardKey,((msg) =>
        {
            int count = (msg.Length < max) ? msg.Length : max;
            for (int i = 0; i < count; i++)
            {
                high.Add(msg[i].Username + ": " + msg[i].Score);
            }
        }));
        return high;
    }
    
    
    public static void SetLearderboardEntry(string name, int score)
    {
        LeaderboardCreator.UploadNewEntry(_leaderboardKey, name, score, ((msg) =>
        {
            GetLeaderboard(5);
        }));
    }
}
