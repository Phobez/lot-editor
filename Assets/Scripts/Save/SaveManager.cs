using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using UnityEngine;

/// <summary>
/// Manages saving and loading data from file.
/// </summary>
public static class SaveManager
{
    /// <summary>
    /// Saves the lot to a file.
    /// </summary>
    /// <param name="fileName">Name of the save file.</param>
    /// <param name="lotData">Data to be saved.</param>
    public static void SaveLot(string fileName, LotData lotData)
    {
        BinaryFormatter formatter = new BinaryFormatter();
        string path = Application.persistentDataPath + "/" + fileName + ".lot";
        FileStream stream = new FileStream(path, FileMode.Create);

        formatter.Serialize(stream, lotData);
        stream.Close();
    }

    /// <summary>
    /// Loads the lot from a file.
    /// </summary>
    /// <param name="fileName">Name of the save file.</param>
    /// <returns>The saved data.</returns>
    public static LotData LoadLot(string fileName)
    {
        string path = Application.persistentDataPath + "/" + fileName + ".lot";

        if (File.Exists(path))
        {
            BinaryFormatter formatter = new BinaryFormatter();
            FileStream stream = new FileStream(path, FileMode.Open);

            LotData data = formatter.Deserialize(stream) as LotData;
            stream.Close();

            return data;
        }
        else
        {
            Debug.LogError("No such lot file found.");
            return null;
        }
    }
}
