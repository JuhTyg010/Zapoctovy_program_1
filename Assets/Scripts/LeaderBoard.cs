using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Dan.Main;


public static class LeaderBoard
{
    private static List<string> names;
    private static List<int> _scores;

    private static string _leaderboardKey = "f3d1d6855dbffd955f9af704c0b4ce4373d7e48de3b07277d293ea8fc2c68d65";

    public static void GetLeaderboard()
    {
        LeaderboardCreator.GetLeaderboard(_leaderboardKey,((msg) =>
        {
            for (int i = 0; i < names.Count; i++)
            {
                names[i] = msg[i].Username;
                _scores[i] = msg[i].Score;
            }
        }));
    }
    
    public static void SetLearderboard(string name, int score)
    {
        LeaderboardCreator.UploadNewEntry(_leaderboardKey, name, score, ((msg) =>
        {
            GetLeaderboard();
        }));
    }
}
