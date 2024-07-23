using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

/// <summary>
/// Manages the panel displaying information and controls for the currently selected build element.
/// Allows for editing the name, description, color, and texture of the element. Handles UI animations for opening and closing the panel.
/// </summary>
/// <author>GUIDEZ Maxence</author>
/// <date>2024-07-23</date>
public class PanelInfos : MonoBehaviour
{
    public GameObject lineNameDisplay;
    public TMP_InputField textFieldName;
    public TMP_InputField textFieldDescription;
    public TMP_Dropdown dropdownColor;
    public TMP_Dropdown dropdownTexture;
    public float animationDuration = .2f;
    public Material[] colorMaterials;
    public Material[] textureMaterials;

    private BuildElement _selectedElement;
    private Vector3 _closedPosition;
    private Vector3 _openPosition;
    private bool _isAnimating;
    
    public static PanelInfos instance { get; private set; }
    
    /// <summary>
    /// Initializes the singleton instance of the PanelInfos class.
    /// Ensures only one instance exists by destroying duplicates.
    /// </summary>
    private void Awake()
    {
        if (instance && instance != this) Destroy(this); 
        else instance = this;
    }
    
    /// <summary>
    /// Sets up the panel with initial positions and populates the dropdown menus with color and texture options.
    /// Adds listeners to input fields and dropdowns for handling changes.
    /// </summary>
    void Start()
    {
        _closedPosition = transform.localPosition;
        _openPosition = _closedPosition - new Vector3(0, 650, 0);
        
        PopulateDropdowns();

        textFieldName.onValueChanged.AddListener(delegate {OnNameChange(); });
        textFieldDescription.onValueChanged.AddListener(delegate {OnDescriptionChange(); });
        dropdownColor.onValueChanged.AddListener(delegate { UpdateMaterial(); });
        dropdownTexture.onValueChanged.AddListener(delegate { UpdateMaterial(); });
    }

    /// <summary>
    /// Opens the panel and displays information about the selected build element.
    /// Sets the name, description, color, and texture based on the selected element's properties.
    /// Animates the panel to the open position.
    /// </summary>
    /// <param name="selected">The build element to display information for.</param>
    public void Open(BuildElement selected)
    {
        if (!selected) return;
        _selectedElement = selected;

        textFieldName.text = _selectedElement.name;
        textFieldDescription.text = _selectedElement.description;
        
        MeshRenderer selectedRenderer = _selectedElement.GetComponent<MeshRenderer>();
        dropdownColor.value = GetIndexOfColor(selectedRenderer.material.color);
        dropdownTexture.value = GetIndexOfTexture(selectedRenderer.material.mainTexture);
        
        StartCoroutine(TranslateUIElementTo(transform, _openPosition));
        lineNameDisplay.SetActive(false);
    }

    /// <summary>
    /// Closes the panel and returns it to the closed position.
    /// </summary>
    public void Close()
    {
        if (!_isAnimating)
        {
            StartCoroutine(TranslateUIElementTo(transform, _closedPosition));
            lineNameDisplay.SetActive(true);
        }
    }

    /// <summary>
    /// Closes the panel immediately without animation and resets its position to the closed state.
    /// </summary>
    public void CloseDirectly()
    {
        transform.localPosition = _closedPosition;
        lineNameDisplay.SetActive(true);
    }

    /// <summary>
    /// Smoothly animates the UI element to a target position over a specified duration.
    /// </summary>
    /// <param name="UIElement">The UI element to animate.</param>
    /// <param name="targetPosition">The target position to animate the UI element to.</param>
    /// <returns>An IEnumerator for coroutine execution.</returns>
    private IEnumerator TranslateUIElementTo(Transform UIElement, Vector3 targetPosition)
    {
        _isAnimating = true;
        Vector3 startPosition = UIElement.localPosition;
        float elapsedTime = 0;

        while (elapsedTime < animationDuration)
        {
            UIElement.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        UIElement.localPosition = targetPosition;
        _isAnimating = false;
    }

    /// <summary>
    /// Updates the name of the selected build element based on the input field value.
    /// </summary>
    void OnNameChange()
    {
        _selectedElement.name = textFieldName.text;
    }

    /// <summary>
    /// Updates the description of the selected build element based on the input field value.
    /// </summary>
    void OnDescriptionChange()
    {
        _selectedElement.description = textFieldDescription.text;
    }
    
    /// <summary>
    /// Populates the color and texture dropdown menus with options.
    /// Adds options for all available colors and textures, including a default "Original" option.
    /// </summary>
    void PopulateDropdowns()
    {
        dropdownColor.ClearOptions();
        dropdownTexture.ClearOptions();

        List<string> colorOptions = new List<string>();
        colorOptions.Add("Original color");
        foreach (Material mat in colorMaterials)
        {
            colorOptions.Add(mat.name);
        }
        dropdownColor.AddOptions(colorOptions);
        dropdownColor.value = 0;

        List<string> textureOptions = new List<string>();
        textureOptions.Add("Original texture");
        foreach (Material mat in textureMaterials)
        {
            textureOptions.Add(mat.name);
        }
        dropdownTexture.AddOptions(textureOptions);
        dropdownTexture.value = 0;
    }
    
    /// <summary>
    /// Updates the color and texture of the selected build element based on the dropdown menu selections.
    /// Applies the selected color and texture materials or reverts to the original if the default option is chosen.
    /// </summary>
    void UpdateMaterial()
    {
        if (dropdownColor.value == 0) _selectedElement.SetColor(null);
        else _selectedElement.SetColor(colorMaterials[dropdownColor.value - 1]);

        if (dropdownTexture.value == 0) _selectedElement.SetTexture(null);
        else _selectedElement.SetTexture(textureMaterials[dropdownTexture.value - 1]);
        
        Selector.instance.UpdateOriginalMaterial();
    }
    
    /// <summary>
    /// Gets the index of the color material in the colorMaterials array.
    /// </summary>
    /// <param name="colorToFind">The color to find in the array.</param>
    /// <returns>The index of the color material or 0 if not found.</returns>
    int GetIndexOfColor(Color colorToFind)
    {
        for (int i = 1; i < colorMaterials.Length; i++)
        {
            if (colorMaterials[i].color == colorToFind)
            {
                return i;
            }
        }
        return 0;
    }
    
    /// <summary>
    /// Gets the index of the texture material in the textureMaterials array.
    /// </summary>
    /// <param name="textureToFind">The texture to find in the array.</param>
    /// <returns>The index of the texture material or 0 if not found.</returns>
    int GetIndexOfTexture(Texture textureToFind)
    {
        for (int i = 1; i < textureMaterials.Length; i++)
        {
            if (textureMaterials[i].mainTexture == textureToFind)
            {
                return i;
            }
        }
        return 0;
    }
}