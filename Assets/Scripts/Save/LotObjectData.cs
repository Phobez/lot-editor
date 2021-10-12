using UnityEngine;

/// <summary>
/// Data on each object in the scene for saving.
/// </summary>
[System.Serializable]
public class LotObjectData
{
    public int objId;
    public float[] pos;
    public float[] rot;
    public float[] scale;
    public float[] rgb;

    /// <summary>
    /// Constructor using raw data.
    /// </summary>
    /// <param name="objId">ID of the object.</param>
    /// <param name="pos">Vector3 position of the object as an float array.</param>
    /// <param name="rot">Quaternion rotation of the object as an float array.</param>
    /// <param name="scale">Vector3 scale of the object as an float array.</param>
    /// <param name="rgb">Vector3 RGB colour values of the object as an float array.</param>
    public LotObjectData(int objId, float[] pos, float[] rot, float[] scale, float[] rgb)
    {
        pos = new float[3];
        rot = new float[4];
        scale = new float[3];
        rgb = new float[3];

        this.objId = objId;
        this.pos = pos;
        this.rot = rot;
        this.scale = scale;
        this.rgb = rgb;
    }

    /// <summary>
    /// Constructor using Transform and Color data.
    /// </summary>
    /// <param name="objId">ID of the object.</param>
    /// <param name="objTransform">The object's Transform component.</param>
    /// <param name="objColour">The object's colour.</param>
    public LotObjectData(int objId, Transform objTransform, Color objColour)
    {
        pos = new float[3];
        rot = new float[4];
        scale = new float[3];
        rgb = new float[3];

        this.objId = objId;
        pos[0] = objTransform.localPosition.x;
        pos[1] = objTransform.localPosition.y;
        pos[2] = objTransform.localPosition.z;
        rot[0] = objTransform.localRotation.x;
        rot[1] = objTransform.localRotation.y;
        rot[2] = objTransform.localRotation.z;
        rot[3] = objTransform.localRotation.w;
        scale[0] = objTransform.localScale.x;
        scale[1] = objTransform.localScale.y;
        scale[2] = objTransform.localScale.z;
        rgb[0] = objColour.r;
        rgb[1] = objColour.g;
        rgb[2] = objColour.b;
    }

    /// <summary>
    /// Returns the position of the object.
    /// </summary>
    /// <returns>Position of the object as a Vector3.</returns>
    public Vector3 GetPos() { return new Vector3(pos[0], pos[1], pos[2]); }

    /// <summary>
    /// Returns the rotation of the object.
    /// </summary>
    /// <returns>Rotation of the object as a Quaternion.</returns>
    public Quaternion GetRot() { return new Quaternion(rot[0], rot[1], rot[2], rot[3]); }

    /// <summary>
    /// Returns the scale of the object.
    /// </summary>
    /// <returns>Scale of the object as a Vector3.</returns>
    public Vector3 GetScale() { return new Vector3(scale[0], scale[1], scale[2]); }

    /// <summary>
    /// Returns the colour of the object.
    /// </summary>
    /// <returns>Colour of the object as a Color struct.</returns>
    public Color GetColour() { return new Color(rgb[0], rgb[1], rgb[2]); }
}
