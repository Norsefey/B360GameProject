using UnityEngine;

[System.Serializable]
public class PartStats
{
    /// <summary>
    /// Stores all information about a part
    /// </summary>
    public string partName;
    public Sprite icon; // Used for UI menu
    public GameObject prefab; // 3D model
    [Range(0, 100)]
    public float durability = 100f; // for future wear
    public int price = 10; // for future shop and purchasing
    [TextArea(3, 5)]
    public string description = "Part description";

    // Additional stats can be added here
    [Range(0, 10)]
    public float weight = 1f; // for future bike riding
    [Range(0, 100)]
    public int rarity = 0; // for potential random loot system

    public string partType = "Type"; // used to organize parts
}
