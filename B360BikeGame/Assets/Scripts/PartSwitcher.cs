using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class PartSwitcher : MonoBehaviour
{
    [Header("UI Elements")]
    [SerializeField] private GameObject alternativesPanel;
    [SerializeField] private Transform alternativesContainer;
    [SerializeField] private GameObject alternativeItemPrefab;

    [Header("References")]
    [SerializeField] private BikePartSelector objectInteraction;

    private Dictionary<string, List<GameObject>> partsByType = new Dictionary<string, List<GameObject>>();
    private PartData currentSelectedPart;
    private string currentPartType;

    private void Start()
    {
        if(alternativesPanel != null)
            alternativesPanel.SetActive(false);

        FindAllObjectsWithData();
    }

    public void FindAllObjectsWithData()
    {
        // Clear current dictionary
        partsByType.Clear();

        // Find all objects with ObjectData component
        PartData[] allObjects = FindObjectsByType<PartData>(FindObjectsSortMode.None);

        // Group objects by type
        foreach (PartData objData in allObjects)
        {
            string objType = objData.partStats.partType;

            if (!partsByType.ContainsKey(objType))
            {
                partsByType[objType] = new List<GameObject>();
            }

            partsByType[objType].Add(objData.gameObject);
        }
    }

    public void ShowAlternatives(PartData selectedPart)
    {
        if (selectedPart == null)
            return;

        currentSelectedPart = selectedPart;
        currentPartType = selectedPart.partStats.partType;

        // Make sure we have alternatives for this type
        if (!partsByType.ContainsKey(currentPartType) || partsByType[currentPartType].Count <= 1)
        {
            // No alternatives available
            if (alternativesPanel != null)
                alternativesPanel.SetActive(false);
            return;
        }

        // Clear previous alternatives
        foreach (Transform child in alternativesContainer)
        {
            Destroy(child.gameObject);
        }

        // Populate alternatives
        foreach (GameObject alternative in partsByType[currentPartType])
        {
            if (alternative == currentSelectedPart)
                continue; // Skip the currently selected object

            CreateAlternativeItem(alternative);
        }

        // Show the panel
        if (alternativesPanel != null)
            alternativesPanel.SetActive(true);
    }
    private void CreateAlternativeItem(GameObject alternative)
    {
     /*   if (alternativeItemPrefab == null || alternativesContainer == null)
            return;

        PartData altData = alternative.GetComponent<PartData>();
        if (altData == null)
            return;

        // Instantiate item UI
        GameObject itemUI = Instantiate(alternativeItemPrefab, alternativesContainer);

        // Set up the UI elements
        TextMeshProUGUI nameText = itemUI.transform.Find("NameText")?.GetComponent<TextMeshProUGUI>();
        if (nameText != null)
            nameText.text = altData.partName;

        Image iconImage = itemUI.transform.Find("IconImage")?.GetComponent<Image>();
        if (iconImage != null && altData.partIcon != null)
            iconImage.sprite = altData.partIcon;

        TextMeshProUGUI priceText = itemUI.transform.Find("PriceText")?.GetComponent<TextMeshProUGUI>();
        if (priceText != null)
            priceText.text = altData.price.ToString();

        // Add button functionality
        Button button = itemUI.GetComponent<Button>();
        if (button != null)
        {
            button.onClick.AddListener(() => SwitchToObject(alternative));
        }*/
    }

    public void SwitchToObject(GameObject newObject)
    {
        if (currentSelectedPart == null || newObject == null)
            return;

        // Get transform data from current object
        Vector3 position = currentSelectedPart.transform.position;
        Quaternion rotation = currentSelectedPart.transform.rotation;
        Transform parent = currentSelectedPart.transform.parent;

        // Deactivate current object or destroy it
        currentSelectedPart.gameObject.SetActive(false);

        // Create or activate new object
        if (newObject.activeSelf)
        {
            // Create a new instance if original is already active
            GameObject newInstance = Instantiate(newObject, position, rotation, parent);

            // Update the reference
            currentSelectedPart = newInstance.GetComponent<PartData>();

            // Let the ObjectInteraction know about this new object
            if (objectInteraction != null)
            {
                objectInteraction.SetSelectedObject(currentSelectedPart);
            }
        }
        else
        {
            // Move the object to the right place
            newObject.transform.position = position;
            newObject.transform.rotation = rotation;
            newObject.transform.parent = parent;

            // Activate it
            newObject.SetActive(true);

            // Update the reference
            currentSelectedPart = newObject.GetComponent<PartData>();

            // Let the ObjectInteraction know about this new object
            if (objectInteraction != null)
            {
                objectInteraction.SetSelectedObject(currentSelectedPart);
            }
        }

        // Update UI
        if (objectInteraction != null)
        {
            objectInteraction.PopulateObjectInfo(currentSelectedPart.partStats);
        }
    }
    public bool HasAlternatives(string objectType)
    {
        if (string.IsNullOrEmpty(objectType) || !partsByType.ContainsKey(objectType))
            return false;

        return partsByType[objectType].Count > 1;
    }
    public void CloseAlternativesPanel()
    {
        if (alternativesPanel != null)
            alternativesPanel.SetActive(false);
    }
}
