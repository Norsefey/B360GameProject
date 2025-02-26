using Unity.VisualScripting.Antlr3.Runtime.Misc;
using UnityEditor;
using UnityEngine;

[System.Serializable]
public class PartStats
{
    public string partName;
    public Sprite icon;
    public GameObject prefab;
    [Range(0, 100)]
    public float durability = 100f;
    public int price = 10;
    [TextArea(3, 5)]
    public string description = "Alternative description";

    // Additional stats can be added here
    [Range(0, 10)]
    public float weight = 1f;
    [Range(0, 100)]
    public int rarity = 0;

    public string partType = "Type";
}
public class PartData : MonoBehaviour
{
    [Header("Stats")]
    public PartStats partStats;

    // Current state tracking
    private float currentDurability;

    void Awake()
    {
        // Initialize current durability when object is created
        currentDurability = partStats.durability;
    }

    // Method to damage the object
    public void TakeDamage(float damageAmount)
    {
        currentDurability = Mathf.Max(0, currentDurability - damageAmount);

        if (currentDurability <= 0)
        {
            HandleBreaking();
        }
    }

    // Method to repair the object
    public void Repair(float repairAmount)
    {
        currentDurability = Mathf.Min(partStats.durability, currentDurability + repairAmount);
    }
    // Reset durability to max
    public void ResetDurability()
    {
        currentDurability = partStats.durability;
    }

    // Method to handle what happens when durability reaches zero
    private void HandleBreaking()
    {
        // Implement breaking logic here
        // For example: particle effects, sound, disabling functionality
        Debug.Log(partStats.partName + " has broken!");
    }

    // Calculate the actual value based on current durability
    public int GetCurrentValue()
    {
        return Mathf.RoundToInt(price * (currentDurability / partStats.durability));
    }

    // Property for easy access to price
    public int price
    {
        get { return partStats.price; }
        set { partStats.price = value; }
    }
}
