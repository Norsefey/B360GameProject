using UnityEngine;

public class PartData : MonoBehaviour
{
    /// <summary>
    /// Used on the part holder, sets initial part information, explains what the part is and does, 
    /// </summary>

    [Header("Stats")]
    public PartStats partStats;
    public PartHighlighter partHighlighter; // used to highlight selected parts 
    // Current state tracking
    private float currentDurability; // for future wear and breakdown system

    void Awake()
    {
        // Initialize current durability when object is created
        currentDurability = partStats.durability;

        partHighlighter = GetComponentInChildren<PartHighlighter>();
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
