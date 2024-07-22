using UnityEngine;
using UnityEngine.Serialization;

public class SelectableElement : MonoBehaviour
{
    public string description = "NO DESCRIPTION";

    private MeshRenderer _renderer;
    private Material[] _originalMaterials;
    public SelectableElementState state;

    public enum SelectableElementState
    {
        HIGHLIGHTED,
        SELECTED,
        UNUSED
    }

    private void Start()
    {
        gameObject.tag = "Selectable";
        
        _renderer = GetComponent<MeshRenderer>();
        _originalMaterials = _renderer.materials;

        state = SelectableElementState.UNUSED;
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
