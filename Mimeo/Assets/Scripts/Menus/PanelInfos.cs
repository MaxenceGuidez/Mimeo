using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

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

    private SelectableElement _selectedElement;
    private Vector3 _closedPosition;
    private Vector3 _openPosition;
    private bool _isAnimating;
    private Material _combinedMaterial;
    
    void Start()
    {
        _closedPosition = transform.localPosition;
        _openPosition = _closedPosition - new Vector3(0, 650, 0);
        _combinedMaterial = new Material(Shader.Find("Standard"));
        
        PopulateDropdowns();

        textFieldName.onValueChanged.AddListener(delegate {OnNameChange(); });
        textFieldDescription.onValueChanged.AddListener(delegate {OnDescriptionChange(); });
        dropdownColor.onValueChanged.AddListener(delegate { UpdateMaterial(); });
        dropdownTexture.onValueChanged.AddListener(delegate { UpdateMaterial(); });
    }

    public void Open(SelectableElement selected)
    {
        if (!selected) return;
        _selectedElement = selected;

        textFieldName.text = _selectedElement.name;
        textFieldDescription.text = _selectedElement.description;
        
        if (!_isAnimating)
        {
            StartCoroutine(TranslateUIElementTo(transform, _openPosition));
            lineNameDisplay.SetActive(false);
        }
    }

    public void Close()
    {
        if (!_isAnimating)
        {
            StartCoroutine(TranslateUIElementTo(transform, _closedPosition));
            lineNameDisplay.SetActive(true);
        }
    }

    public void CloseDirectly()
    {
        transform.localPosition = _closedPosition;
        lineNameDisplay.SetActive(true);
    }

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

    void OnNameChange()
    {
        _selectedElement.name = textFieldName.text;
    }

    void OnDescriptionChange()
    {
        _selectedElement.description = textFieldDescription.text;
    }
    
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
    
    void UpdateMaterial()
    {
        if (dropdownColor.value == 0) return;
        if (dropdownTexture.value == 0) return;
        
        if (dropdownColor.value == 0 && dropdownTexture.value == 0)
        {
            _selectedElement.ResetMaterial();
        }
        
        Material selectedColorMaterial = colorMaterials[dropdownColor.value - 1];
        Material selectedTextureMaterial = textureMaterials[dropdownTexture.value - 1];

        _combinedMaterial.color = selectedColorMaterial.color;
        _combinedMaterial.mainTexture = selectedTextureMaterial.mainTexture;

        _selectedElement.SetMaterial(_combinedMaterial);
    }
}