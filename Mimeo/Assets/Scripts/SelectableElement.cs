using UnityEngine;

public class SelectableElement : MonoBehaviour
{
    public string description = "NO DESCRIPTION";

    private MeshRenderer _renderer;
    private Material[] _originalMaterials;

    private void Start()
    {
        _renderer = GetComponent<MeshRenderer>();
        _originalMaterials = _renderer.materials;
    }

    public Material[] GetOriginalMaterials()
    {
        return _originalMaterials;
    }

    public void SetMaterials(Material[] newMaterials)
    {
        _renderer.materials = newMaterials;
    }

    public void SetMaterial(Material newMaterial)
    {
        Material[] newMaterials = new Material[_renderer.materials.Length];
        for (int i = 0; i < newMaterials.Length; i++)
        {
            newMaterials[i] = newMaterial;
        }
        _renderer.materials = newMaterials;
    }

    public void ResetMaterial()
    {
        _renderer.materials = _originalMaterials;
    }
}
