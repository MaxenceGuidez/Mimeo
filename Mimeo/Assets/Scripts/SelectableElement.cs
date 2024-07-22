using UnityEngine;

public class SelectableElement : MonoBehaviour
{
    public string description = "NO DESCRIPTION";

    private MeshRenderer _renderer;
    private Material[] _originalMaterials;
    
    [HideInInspector]
    public bool isHighlighted { get; set; }
    [HideInInspector]
    public bool isSelected { get; set; }

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _originalMaterials = _renderer.materials;
    }

    public void SetColor(Material materialColor)
    {
        Material[] newMaterials = _renderer.materials;
        for (int i = 0; i < newMaterials.Length; i++)
        {
            newMaterials[i].color = materialColor == null ? _originalMaterials[i].color : materialColor.color;
        }
        _renderer.materials = newMaterials;
    }

    public void SetTexture(Material materialTexture)
    {
        Material[] newMaterials = _renderer.materials;
        for (int i = 0; i < newMaterials.Length; i++)
        {
            newMaterials[i].mainTexture = materialTexture == null ? _originalMaterials[i].mainTexture : materialTexture.mainTexture;
        }
        _renderer.materials = newMaterials;
    }
}
