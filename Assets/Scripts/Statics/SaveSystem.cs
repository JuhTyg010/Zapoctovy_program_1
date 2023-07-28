using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using System.Collections.Generic;

public static class SaveSystem
{
    public static void SaveNewHighScore(string newHighScore)
    {
        int newHighScoreInt = int.Parse(newHighScore.Split(": ")[1]);
        List<string> highScore = LoadHighScore();
        List<int> highScoreInt = new List<int>();
        if (highScore == null )
        {
            highScore = new List<string>();
            highScore.Add(newHighScore);
            SaveHighScore(highScore);
            return;
        }
        if(highScore.Count < 5)
        {
            highScore.Add(newHighScore);
            SaveHighScore(highScore);
            return;
        }
        foreach (string score in highScore)
        {
            highScoreInt.Add(int.Parse(score.Split(": ")[1]));
        }
        
        #region something like sort and insert
            
            for(int i = 0; i < highScore.Count; i++)
            {
                for(int j = i + 1; j < highScore.Count; j++)
                {
                    if (highScoreInt[i] < highScoreInt[j])
                    {
                        string temp = highScore[i];
                        highScore[i] = highScore[j];
                        highScore[j] = temp;
                        
                        int tempInt = highScoreInt[i];
                        highScoreInt[i] = highScoreInt[j];
                        highScoreInt[j] = tempInt;
                    }
                }
            }
            for (int i = 0; i < highScore.Count; i++)
            {
                Debug.Log(highScore[i]);
                if(newHighScoreInt > highScoreInt[i])
                {
                    highScore.Insert(i, newHighScore);
                    if (highScore.Count > 5)
                    {
                        highScore.RemoveAt(highScore.Count - 1);
                    }
                    break;
                }
            }
            
        #endregion
        
        SaveHighScore(highScore);
        
    }

    public static List<string> LoadHighScore()
    {
        //haram haram haram
        string path = Application.persistentDataPath + "/highscore.data";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            
            
            List<string> data = formatter.Deserialize(stream) as List<string>;
            stream.Close();
            return data;
        }
        Debug.Log("We are fucked");
        return null;
    }
    
    static void SaveHighScore(List<string> highScore)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/highScore.data";
        FileStream stream = new FileStream(path, FileMode.Create);
        formatter.Serialize(stream, highScore);
        stream.Close();
    } 
}
