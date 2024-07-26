using System;
using UnityEngine;

/// <summary>
/// Represents an element in the build system with configurable visual properties and states.
/// This class handles the appearance of the element by allowing the modification of colors and textures
/// and manages the element's state within the build system.
/// </summary>
/// <author>GUIDEZ Maxence</author>
/// <date>2024-07-23</date>
public class BuildElement : MonoBehaviour
{
    public string description = "NO DESCRIPTION";

    private MeshRenderer _renderer;
    private Material[] _originalMaterials;
    public ElementState state = ElementState.UNUSED;

    /// <summary>
    /// Enum representing the possible states of the build element.
    /// </summary>
    public enum ElementState
    {
        HIGHLIGHTED,
        SELECTED,
        UNUSED
    }

    /// <summary>
    /// Initializes the build element.
    /// Sets the tag of the game object to "Selectable", initializes the renderer and original materials,
    /// and sets the initial state of the element to UNUSED.
    /// </summary>
    private void Start()
    {
        gameObject.tag = "Selectable";
        
        _renderer = GetComponent<MeshRenderer>();
        if (_renderer)
        {
            _originalMaterials = new Material[_renderer.materials.Length];
            for (int i = 0; i < _originalMaterials.Length; i++)
            {
                _originalMaterials[i] = new Material(_renderer.materials[i]);
            }
        }
        else _originalMaterials = Array.Empty<Material>();

        state = ElementState.UNUSED;
    }

    /// <summary>
    /// Sets the color of the build element's materials.
    /// Updates each material's color to the provided material's color. If the provided material is null,
    /// the original color of each material is retained.
    /// </summary>
    /// <param name="materialColor">The material containing the color to apply. If null, the original colors are used.</param>
    public void SetColor(Material materialColor)
    {
        if (state != ElementState.SELECTED) return;
            
        Material[] newMaterials = _renderer.materials;
        for (int i = 0; i < newMaterials.Length; i++)
        {
            newMaterials[i].color = materialColor == null ? _originalMaterials[i].color : materialColor.color; // TODO : GetMaterialWithIndex() to PanelInfos and store here the index
        }
        _renderer.materials = newMaterials;
    }

    /// <summary>
    /// Sets the texture of the build element's materials.
    /// Updates each material's texture to the provided material's main texture and normal map. If the provided material is null,
    /// the original texture and normal map of each material are retained.
    /// </summary>
    /// <param name="materialTexture">The material containing the texture and normal map to apply. If null, the original textures are used.</param>
    public void SetTexture(Material materialTexture)
    {
        Material[] newMaterials = _renderer.materials;

        for (int i = 0; i < newMaterials.Length; i++)
        {
            if (materialTexture != null)
            {
                newMaterials[i].SetTexture("_BumpMap", materialTexture.GetTexture("_BumpMap"));
                newMaterials[i].mainTexture = materialTexture.mainTexture;
            }
            else
            {
                newMaterials[i].SetTexture("_BumpMap", _originalMaterials[i].GetTexture("_BumpMap"));
                newMaterials[i].mainTexture = _originalMaterials[i].mainTexture;
            }
        }

        _renderer.materials = newMaterials;
    }

}
