using UnityEngine;

/// <summary>
/// Represents an element in the build system with configurable visual properties and states.
/// This class handles the appearance of the element by allowing the modification of colors and textures
/// and manages the element's state within the build system.
/// </summary>
/// <author>GUIDEZ Maxence</author>
/// <date>2024-07-22</date>
public class BuildElement : MonoBehaviour
{
    public string description = "NO DESCRIPTION";

    private MeshRenderer _renderer;
    private Material[] _originalMaterials;
    public ElementState state;

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
        _originalMaterials = _renderer.materials;

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
        Material[] newMaterials = _renderer.materials;
        for (int i = 0; i < newMaterials.Length; i++)
        {
            newMaterials[i].color = materialColor == null ? _originalMaterials[i].color : materialColor.color;
        }
        _renderer.materials = newMaterials;
    }

    /// <summary>
    /// Sets the texture of the build element's materials.
    /// Updates each material's main texture to the provided material's main texture. If the provided material is null,
    /// the original texture of each material is retained.
    /// </summary>
    /// <param name="materialTexture">The material containing the texture to apply. If null, the original textures are used.</param>
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
