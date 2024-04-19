using System.Collections.Generic;
using UnityEngine;

[DisallowMultipleComponent]
public class Outline : MonoBehaviour
{
    [SerializeField, Range(0f, 1f)]
    private float outlineOpacity = 0.5f; // New property for controlling outline opacity

    private Renderer[] renderers;
    private Material outlineMaskMaterial;
    private Material outlineFillMaterial;

    void Awake()
    {
        // Cache renderers
        renderers = GetComponentsInChildren<Renderer>();

        // Instantiate outline materials
        outlineMaskMaterial = new Material(Shader.Find("Hidden/OutlineMask"));
        outlineFillMaterial = new Material(Shader.Find("Hidden/OutlineFill"));

        // Apply material properties immediately
        ApplyOutlineOpacity();
    }

    void OnEnable()
    {
        foreach (var renderer in renderers)
        {
            // Append outline shaders
            
              List<Material> materials = new List<Material> (renderer.sharedMaterials);

            materials.Add(outlineMaskMaterial);
            materials.Add(outlineFillMaterial);

            renderer.materials = materials.ToArray();
        }
    }

    // Function to apply outline opacity to outline materials
    private void ApplyOutlineOpacity()
    {
        outlineMaskMaterial.SetFloat("_Opacity", outlineOpacity);
        outlineFillMaterial.SetFloat("_Opacity", outlineOpacity);
    }

    // Function to update outline opacity when it changes
    private void OnValidate()
    {
        ApplyOutlineOpacity();
    }
}