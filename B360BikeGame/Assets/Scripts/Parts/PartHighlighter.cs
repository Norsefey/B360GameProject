using System.Collections.Generic;
using UnityEngine;

public class PartHighlighter : MonoBehaviour
{
    // List of renderers currently being tracked
    private List<MeshRenderer> trackedRenderers = new List<MeshRenderer>();

    [Header("Highlighting Settings")]
    [SerializeField] private Material highlightMaterial;

    private Dictionary<MeshRenderer, Material[]> originalMaterials = new Dictionary<MeshRenderer, Material[]>();

    private void Awake()
    {
        MeshRenderer[] renderers = GetComponentsInChildren<MeshRenderer>();
        AddMultipleRenderers(renderers);
    }

    public void AddRenderer(MeshRenderer renderer)
    {
        if (renderer == null || trackedRenderers.Contains(renderer))
            return;

        // Store original materials
        Material[] materials = renderer.materials;
        originalMaterials[renderer] = materials;
        trackedRenderers.Add(renderer);
    }
    public void AddMultipleRenderers(MeshRenderer[] renderers)
    {
        foreach (MeshRenderer renderer in renderers)
        {
            AddRenderer(renderer);
        }
    }
    public void RemoveRenderer(MeshRenderer renderer)
    {
        if (renderer == null || !trackedRenderers.Contains(renderer))
            return;

        // Restore original materials if currently highlighted
        if (renderer.sharedMaterial == highlightMaterial)
        {
            renderer.materials = originalMaterials[renderer];
        }

        originalMaterials.Remove(renderer);
        trackedRenderers.Remove(renderer);
    }
    public void ClearAllRenderers()
    {
        // restore all materials to original state, to avoid highlight material sticking on asset
        RestoreAll();

        originalMaterials.Clear();
        trackedRenderers.Clear();

        Debug.Log(trackedRenderers.Count);
    }
    public void SetNewRenders(GameObject newPart)
    {
        MeshRenderer[] renderers = newPart.GetComponentsInChildren<MeshRenderer>();
        AddMultipleRenderers(renderers);
    }
    public void HighlightAll()
    {
        foreach (MeshRenderer renderer in trackedRenderers)
        {
            if (renderer == null)
                continue;

            // Create a new array of materials all set to the highlight material
            Material[] highlightMaterials = new Material[renderer.materials.Length];
            for (int i = 0; i < highlightMaterials.Length; i++)
            {
                highlightMaterials[i] = highlightMaterial;
            }

            renderer.materials = highlightMaterials;
        }
    }
    public void RestoreAll()
    {
        if(trackedRenderers.Count <= 0)
            return;
        foreach (MeshRenderer renderer in trackedRenderers)
        {
            if (renderer == null)
                continue;

            renderer.materials = originalMaterials[renderer];
        }
    }
    public void HighlightSingleRenderer(MeshRenderer renderer)
    {
        if (renderer == null || !trackedRenderers.Contains(renderer))
            return;

        // Create a new array of materials all set to the highlight material
        Material[] highlightMaterials = new Material[renderer.materials.Length];
        for (int i = 0; i < highlightMaterials.Length; i++)
        {
            highlightMaterials[i] = highlightMaterial;
        }

        renderer.materials = highlightMaterials;
    }
    public void RestoreSingleRenderer(MeshRenderer renderer)
    {
        if (renderer == null || !trackedRenderers.Contains(renderer))
            return;

        renderer.materials = originalMaterials[renderer];
    }
}
