using System;
using System.Collections.Generic;
using UnityEngine;
using Dan.Main;


public static class LeaderBoard
{
    private static string _leaderboardKey = "f3d1d6855dbffd955f9af704c0b4ce4373d7e48de3b07277d293ea8fc2c68d65";

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
