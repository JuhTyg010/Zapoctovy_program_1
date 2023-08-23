using System;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    public static void SaveShipId(int id)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/ship.id";
        FileStream stream = new FileStream(path, FileMode.Create);
        
        formatter.Serialize(stream, id);
        stream.Close();
    }

    public static void SaveName(string name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Last.name";
        FileStream stream = new FileStream(path, FileMode.Create);
        
        formatter.Serialize(stream, name);
        stream.Close();
    }

    public static string LoadName()
    {
        string path = Application.persistentDataPath + "/Last.name";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            return (string) formatter.Deserialize(stream);
        }
        Debug.Log("No Data Found");
        return "Player";
    }

    public static int LoadShipId()
    {
        string path = Application.persistentDataPath + "/ship.id";
        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);
            return (int) formatter.Deserialize(stream);
        }
        Debug.Log("No Data Found");
        return 1;
    }
}
