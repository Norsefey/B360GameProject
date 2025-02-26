using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PartData))]
public class AlternativeParts : MonoBehaviour
{
    [Header("Available Alternatives")]
    [SerializeField] private List<PartStats> alternatives = new List<PartStats>();
    
    private PartData partData;

    private void Awake()
    {
        partData = GetComponent<PartData>();
    }

    public int GetAlternativesCount()
    {
        return alternatives.Count;
    }
    public List<PartStats> GetAlternatives()
    {
        return alternatives;
    }
    public PartStats GetAAlternative(int index)
    {
        return alternatives[index];
    }
    // Switch to a specific alternative
    public void SwitchToAlternative(int index)
    {
        if (index < 0 || index >= alternatives.Count)
            return;

        PartStats alternative = alternatives[index];

        // Update the object data
        if (partData != null)
        {
            partData.partStats.partName = alternative.partName;
            partData.partStats.description = alternative.description;
            partData.partStats.durability = alternative.durability;
            partData.partStats.price = alternative.price;
            partData.partStats.weight = alternative.weight;
            partData.partStats.rarity = alternative.rarity;
            partData.partStats.icon = alternative.icon;
            partData.partStats.prefab = alternative.prefab;

            // Reset current durability
            partData.ResetDurability();
        }

        // Change the model if a prefab is provided
        if (alternative.prefab != null)
        {
            ReplaceModel(alternative.prefab);
        }
    }
    private void ReplaceModel(GameObject newModel)
    {
        // Store current transform data
        Vector3 position = transform.position;
        Quaternion rotation = transform.rotation;
        Vector3 scale = transform.localScale;
        Transform parent = transform.parent;

        // Remove current model child if it exists
        Transform currentModel = transform.Find("Model");
        if (currentModel != null)
        {
            Destroy(currentModel.gameObject);
        }

        // Instantiate new model as child
        GameObject modelInstance = Instantiate(newModel, transform);
        modelInstance.name = "Model";

        // Reset transform to match the parent
        modelInstance.transform.localPosition = Vector3.zero;
        modelInstance.transform.localRotation = Quaternion.identity;
        modelInstance.transform.localScale = Vector3.one;
    }
}
