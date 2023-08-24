using System;
using System.Collections.Generic;
using UnityEngine;
//Special library for leaderboard
using Dan.Main;


public static class LeaderBoard
{
    //this is public key to connect to the leaderboard
    private static string _leaderboardKey = "ecbd72bad89772548533cba5157bf397876cf0bac69d63d7d281667e1f287451";

    //this method returns a list of strings with the top scores
    //max is the number of scores to return, we check if the number of scores is less than max
    //if so we return the number of scores, otherwise we return max
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
    
    //this method uploads a new entry to the leaderboard and then calls GetLeaderboard() to update the list
    public static void SetLearderboardEntry(string name, int score)
    {
        LeaderboardCreator.UploadNewEntry(_leaderboardKey, name, score, ((msg) =>
        {
            GetLeaderboard(5);
        }));
    }
}
