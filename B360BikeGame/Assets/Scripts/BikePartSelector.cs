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
    [SerializeField] private TextMeshProUGUI categoryText;

    private Camera mainCamera;
    private GameObject currentlyHighlighted;
    private GameObject currentlySelected;
    private Material[] originalMaterials;

    void Start()
    {
        mainCamera = Camera.main;

        // Hide UI panel initially
        if (objectInfoPanel != null)
            objectInfoPanel.SetActive(false);
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
                currentlyHighlighted = hitObject;
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
            // Deselect previous object if there is one
            if (currentlySelected != null && currentlySelected == currentlyHighlighted)
            {
                // Perform any deselection logic here
                objectInfoPanel.SetActive(false);
                currentlySelected = null;
            }
            else
            {
                // Select new object
                currentlySelected = currentlyHighlighted;

                // Show UI panel
                if (objectInfoPanel != null)
                {
                    objectInfoPanel.SetActive(true);

                    // Populate UI with object info
                    PopulateObjectInfo(currentlySelected);
                }
            }
        }
    }

    void HighlightObject(GameObject obj)
    {
        Renderer renderer = obj.GetComponent<Renderer>();
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

    void PopulateObjectInfo(GameObject obj)
    {
         PartData data = obj.GetComponent<PartData>();
        if (data != null)
        {
            nameText.text = data.partName;
            descriptionText.text = data.partDescription;
            priceText.text = "$" + data.partStats.price.ToString();

           /* // Set durability slider
            if (durabilitySlider != null)
            {
                durabilitySlider.maxValue = 100f;
                durabilitySlider.value = data.GetDurabilityPercentage();
            }*/

            // Set icon
            if (objectIconImage != null && data.partIcon != null)
                objectIconImage.sprite = data.partIcon;

            // Set category
            if (categoryText != null)
                categoryText.text = data.category + " - " + data.partType;
        }
    }
}
