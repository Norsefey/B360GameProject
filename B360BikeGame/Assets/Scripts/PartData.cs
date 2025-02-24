using UnityEngine;

[System.Serializable]
public class PartStats
{
    public float durability = 100f;
    public int price = 10;
    public float weight = 1f;
}
public class PartData : MonoBehaviour
{
    [Header("Basic Info")]
    public string partName = "Part Name";
    [TextArea(3,5)]
    public string partDescription = "Description Goes Here";

    [Header("Stats")]
    public PartStats partStats;

    [Header("Visual References")]
    public Sprite partIcon;
    public GameObject partPrefab;

    [Header("Category/Type")]
    public string category = "Misc";
    public string partType = "Type";


}
