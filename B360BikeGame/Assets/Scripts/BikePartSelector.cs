using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BikePartSelector : MonoBehaviour
{
    [Header("Highlighting Settings")]
    [SerializeField] private Material highlightMaterial;

    [Header("UI References")]
    [SerializeField] private GameObject objectInfoPanel;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private TextMeshProUGUI descriptionText;
    [SerializeField] private TextMeshProUGUI priceText;
    [SerializeField] private Slider durabilitySlider;
    [SerializeField] private Image objectIconImage;
    [SerializeField] private TextMeshProUGUI typeName;
    [SerializeField] private TextMeshProUGUI typeDescription;
    [SerializeField] private Button showAlternativesButton;
    
    [Header("Alternatives Panel")]
    [SerializeField] private GameObject alternativesPanel;
    [SerializeField] private Transform alternativesContainer;
    [SerializeField] private GameObject alternativeItemPrefab;
    
    private Camera mainCamera;
    private PartData currentlyHighlighted;
    private PartData currentlySelected;
    private Material[] originalMaterials;

    void Start()
    {
        mainCamera = Camera.main;

        // Hide UI panels initially
        if (objectInfoPanel != null)
            objectInfoPanel.SetActive(false);

        if (alternativesPanel != null)
            alternativesPanel.SetActive(false);

        // Setup alternatives button
        if (showAlternativesButton != null)
        {
            showAlternativesButton.onClick.AddListener(ShowAlternatives);
        }
    }
    void Update()
    {
        // Check if mouse is over UI elements
        if (EventSystem.current != null && EventSystem.current.IsPointerOverGameObject())
            return;

        HandleHighlighting();
        HandleSelection();
    }
    void HandleHighlighting()
    {
        Ray ray = mainCamera.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        // If we hit something with our ray
        if (Physics.Raycast(ray, out hit))
        {
            GameObject hitObject = hit.collider.gameObject;

            // If we're hovering over a new object
            if (hitObject != currentlyHighlighted)
            {
                // Unhighlight previous object
                UnhighlightCurrentObject();

                // Highlight new object
                currentlyHighlighted = hitObject.GetComponent<PartData>();
                HighlightObject(currentlyHighlighted);
            }
        }
        else
        {
            // If we're not hovering over anything, unhighlight the current object
            UnhighlightCurrentObject();
        }
    }
    void HandleSelection()
    {
        // When left mouse button is clicked
        if (Input.GetMouseButtonDown(0) && currentlyHighlighted != null)
        {
            // When left mouse button is clicked
            if (Input.GetMouseButtonDown(0) && currentlyHighlighted != null)
            {
                // Deselect previous object if there is one
                if (currentlySelected != null && currentlySelected != currentlyHighlighted)
                {
                    // Perform any deselection logic here
                }

                // Select new object
                currentlySelected = currentlyHighlighted;

                // Show UI panel
                if (objectInfoPanel != null)
                {
                    objectInfoPanel.SetActive(true);

                    // Populate UI with object info
                    PopulateObjectInfo(currentlySelected.partStats);

                    // Show alternatives button only if there are alternatives
                    UpdateAlternativesButton();
                }
            }

            // Close panel when clicking elsewhere
            if (Input.GetMouseButtonDown(0) && currentlyHighlighted == null && objectInfoPanel != null && objectInfoPanel.activeSelf)
            {
                CloseInfoPanel();
            }
        }
    }
    public void CloseInfoPanel()
    {
        if (objectInfoPanel != null)
            objectInfoPanel.SetActive(false);

        if (alternativesPanel != null)
            alternativesPanel.SetActive(false);

        currentlySelected = null;
    }
    void HighlightObject(PartData part)
    {
        Renderer renderer = part.GetComponent<Renderer>();
        if (renderer != null)
        {
            // Store original materials
            originalMaterials = renderer.materials;

            // Apply highlight material to all material slots
            Material[] highlightMaterials = new Material[originalMaterials.Length];
            for (int i = 0; i < highlightMaterials.Length; i++)
            {
                highlightMaterials[i] = highlightMaterial;
            }

            renderer.materials = highlightMaterials;
        }
    }
    void UnhighlightCurrentObject()
    {
        if (currentlyHighlighted != null)
        {
            Renderer renderer = currentlyHighlighted.GetComponent<Renderer>();
            if (renderer != null && originalMaterials != null)
            {
                // Restore original materials
                renderer.materials = originalMaterials;
            }

            currentlyHighlighted = null;
        }
    }
    public void PopulateObjectInfo(PartStats part)
    {
        if (part != null)
        {
            // Set basic info
            if (nameText != null)
                nameText.text = part.partName;

            if (descriptionText != null)
                descriptionText.text = part.description;

            // Set price
            if (priceText != null)
                priceText.text = "$" + part.price.ToString();

            // Set durability slider
            if (durabilitySlider != null)
            {
                durabilitySlider.maxValue = 100f;
                durabilitySlider.value = part.durability;
            }

            // Set icon
            if (objectIconImage != null && part.icon != null)
                objectIconImage.sprite = part.icon;

            if (typeName != null)
                typeName.text = part.partType;

            // Update alternatives button
            UpdateAlternativesButton();
        }
    }
    private void UpdateAlternativesButton()
    {
        if (showAlternativesButton == null || currentlySelected == null)
            return;

        AlternativeParts altComponent = currentlySelected.GetComponent<AlternativeParts>();
        if (altComponent == null || altComponent.GetAlternativesCount() == 0)
        {
            showAlternativesButton.gameObject.SetActive(false);
        }
        else
        {
            showAlternativesButton.gameObject.SetActive(true);
        }
    }
    public void SetSelectedObject(PartData part)
    {
        currentlySelected = part;

        // If we need to highlight it as well
        if (currentlyHighlighted != null)
        {
            UnhighlightCurrentObject();
        }
        currentlyHighlighted = part;
        HighlightObject(currentlyHighlighted);
    }
    public void ShowAlternatives()
    {
        if (currentlySelected == null || alternativesPanel == null || alternativesContainer == null)
            return;

        AlternativeParts altComponent = currentlySelected.GetComponent<AlternativeParts>();
        if (altComponent == null)
            return;

        List<PartStats> alternatives = altComponent.GetAlternatives();
        if (alternatives.Count == 0)
            return;

        // Clear previous alternatives
        foreach (Transform child in alternativesContainer)
        {
            Destroy(child.gameObject);
        }

        // Populate alternatives
        for (int i = 0; i < alternatives.Count; i++)
        {
            PartStats alt = alternatives[i];
            CreateAlternativeItem(alt, i);
        }

        // Show the panel
        alternativesPanel.SetActive(true);
    }
    private void CreateAlternativeItem(PartStats alternative, int index)
    {
        if (alternativeItemPrefab == null || alternativesContainer == null)
            return;

        // Instantiate item UI
        GameObject itemUI = Instantiate(alternativeItemPrefab, alternativesContainer);

        // Set up the UI elements
        AlternativePartUI partUI = itemUI.GetComponent<AlternativePartUI>();
        partUI.Setup(alternative, index);

        // Add button functionality
        
        if (partUI.buyButton != null)
        {
            int capturedIndex = index;
            partUI.buyButton.onClick.AddListener(() => BuyPart(capturedIndex));
        }

        if (partUI.infoButton != null)
        {
            int capturedIndex = index;
            partUI.infoButton.onClick.AddListener(() => ShowPartInformation(capturedIndex));
        }
    }
    private void BuyPart(int index)
    {
        if (currentlySelected == null)
            return;

        AlternativeParts altComponent = currentlySelected.GetComponent<AlternativeParts>();
        if (altComponent == null)
            return;

        // Switch to the alternative
        altComponent.SwitchToAlternative(index);

        // Update the UI
        PopulateObjectInfo(currentlySelected.partStats);
    }
    private void ShowPartInformation(int index)
    {
        if (currentlySelected == null)
            return;

        AlternativeParts altComponent = currentlySelected.GetComponent<AlternativeParts>();
        if (altComponent == null)
            return;

        PopulateObjectInfo(altComponent.GetAAlternative(index));
    }
    public void CloseAlternativesPanel()
    {
        if (alternativesPanel != null)
            alternativesPanel.SetActive(false);
    }
}
