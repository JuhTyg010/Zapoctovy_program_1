using System;
using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    
    // SaveShipId is called from outside as static method
    // It saves the ship id to the ship.id file in the persistent data path.
    // By using the BinaryFormatter, we can serialize the data to a file,
    // which is a binary file, so it is not readable.
    public static void SaveShipId(int id)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/ship.id";
        FileStream stream = new FileStream(path, FileMode.Create);
        
        formatter.Serialize(stream, id);
        stream.Close();
    }

    //SaveName works the same way as SaveShipId, but it saves the name to the Last.name file.
    public static void SaveName(string name)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/Last.name";
        FileStream stream = new FileStream(path, FileMode.Create);
        
        formatter.Serialize(stream, name);
        stream.Close();
    }

    //LoadName works the same way as LoadShipId, but it loads the name from the Last.name file.
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

    //LoadShipId is also called from outside as static method
    //It loads the ship id from the ship.id file in the persistent data path.
    //By using the BinaryFormatter, we can deserialize the data from a file,
    //which is a binary file, so it is not readable, and return exact same variables.
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
