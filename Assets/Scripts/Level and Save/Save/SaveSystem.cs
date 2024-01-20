using UnityEngine;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

public static class SaveSystem
{
    private static string saveDataFilePath = "/MythstonesSaveData.txt";
    public static void SaveDataOfPlayer(SaveSingleton saveSingleton)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + saveDataFilePath;
        FileStream stream = new FileStream(path, FileMode.Create);

        PlayerData data = new PlayerData(saveSingleton);
        formatter.Serialize(stream, data);
        stream.Close();
    }

    public static PlayerData LoadDataOfPlayer()
    {
        string path = Application.persistentDataPath + saveDataFilePath;

        if(File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            PlayerData data = formatter.Deserialize(stream) as PlayerData;
            stream.Close();

            return data;
        } else
        {
            Debug.LogError("Save file not found in " + path);

            //Create Save Data File if it doesn't exist
            SaveSingleton.Instance.InitializeSaveData();        //Initialize some variables, for example levelsUnlocked[0] = true, has to be done so first level can be accessed
            SaveDataOfPlayer(SaveSingleton.Instance);
            Debug.LogWarning("Creating save file by saveing data (PlayerData) using Save Singleton Instance as a parameter");
            return null;
        }
    }
   
}
