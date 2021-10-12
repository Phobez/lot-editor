/// <summary>
/// Base save file class.
/// </summary>
[System.Serializable]
public class LotData
{
    public LotObjectData[] lotObjects;

    /// <summary>
    /// Default constructor.
    /// </summary>
    /// <param name="lotObjects">List of lot object data.</param>
    public LotData(LotObjectData[] lotObjects)
    {
        this.lotObjects = lotObjects;
    }
}
