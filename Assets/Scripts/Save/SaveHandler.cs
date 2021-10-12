using System.Collections.Generic;
using System.IO;
using TMPro;
using UnityEngine;

/// <summary>
/// Handles translating data between the scene and the SaveManager.
/// </summary>
public class SaveHandler : MonoBehaviour
{
    // inspector fields
    [SerializeField]
    private GameObject wallPrefab = null;
    [SerializeField]
    private GameObject doorWallPrefab = null;
    [SerializeField]
    private List<GameObject> objPrefabs = new List<GameObject>();
    [SerializeField]
    private TMP_InputField inputField = null;
    [SerializeField]
    private GameObject existsPanel = null;

    /// <summary>
    /// Saves the lot if there are no errors and there isn't an existing save file.
    /// </summary>
    public void SaveGameWithCheck()
    {
        if (!inputField) Debug.LogError("No save file name input detected.");

        if (File.Exists(Application.persistentDataPath + "/" + inputField.text + ".lot"))
        {
            if (existsPanel) existsPanel.SetActive(true);
        }
        else SaveLot(inputField.text);
    }

    /// <summary>
    /// Saves the lot. Helper method for buttons.
    /// </summary>
    public void SaveGame() { SaveLot(inputField.text); }

    /// <summary>
    /// Saves the lot.
    /// </summary>
    /// <param name="fileName">Name of the save file.</param>
    public void SaveLot(string fileName)
    {
        GameObject[] allObjs = GetAllSceneObjects();

        List<LotObjectData> lotObjList = new List<LotObjectData>();

        foreach (GameObject obj in allObjs)
        {
            if (obj.activeInHierarchy && obj.transform.parent == null && obj.GetComponent<BuildObject>().Saveable)
            {
                int objId = -1;

                objId = obj.GetComponent<BuildObject>().GetObjID();

                Color tempColour;

                tempColour = obj.GetComponent<BuildObject>().GetColour();

                lotObjList.Add(new LotObjectData(objId, obj.transform, tempColour));

                Debug.Log(objId + " at " + obj.transform.localPosition);
            }
        }

        LotObjectData[] lotObjs = lotObjList.ToArray();

        LotData lotData = new LotData(lotObjs);

        SaveManager.SaveLot(fileName, lotData);
    }

    /// <summary>
    /// Loads the lot. Helper method for buttons.
    /// </summary>
    public void LoadGame() { LoadLot(inputField.text); }

    /// <summary>
    /// Loads the lot.
    /// </summary>
    /// <param name="fileName">Name of the save file.</param>
    public void LoadLot(string fileName)
    {
        List<GameObject> allObjs = new List<GameObject>(GetAllSceneObjects());

        foreach (GameObject obj in allObjs)
        {
            Destroy(obj);
        }

        LotData lotData = SaveManager.LoadLot(fileName);

        foreach (LotObjectData lotObj in lotData.lotObjects)
        {
            GameObject prefab = null;

            switch (lotObj.objId)
            {
                case 1:
                    prefab = wallPrefab;
                    break;
                case 2:
                    prefab = doorWallPrefab;
                    break;
                default:
                    prefab = objPrefabs[lotObj.objId - 3];
                    break;
            }

            GameObject newObj = Instantiate(prefab, lotObj.GetPos(), lotObj.GetRot());
            newObj.transform.localScale = lotObj.GetScale();

            newObj.GetComponent<BuildObject>().SetColour(lotObj.GetColour());

            Debug.Log(lotObj.objId + " at " + lotObj.GetPos());
        }
    }

    /// <summary>
    /// Returns all objects in the scene.
    /// </summary>
    /// <returns>Array of GameObjects.</returns>
    private GameObject[] GetAllSceneObjects()
    {
        GameObject[] walls = GameObject.FindGameObjectsWithTag("Wall");
        GameObject[] doorWalls = GameObject.FindGameObjectsWithTag("Door Wall");
        GameObject[] objects = GameObject.FindGameObjectsWithTag("Object");

        GameObject[] allObjs = new GameObject[walls.Length + doorWalls.Length + objects.Length];

        walls.CopyTo(allObjs, 0);
        doorWalls.CopyTo(allObjs, walls.Length);
        objects.CopyTo(allObjs, walls.Length + doorWalls.Length);

        return allObjs;
    }
}
