using UnityEngine;
using UnityEngine.EventSystems;

// Credits : https://www.youtube.com/watch?v=A0Kd6lnBNRE
public class Selection : MonoBehaviour
{
    public Camera playerCamera;
    public Material highlightMaterial;
    public Material selectionMaterial;

    private Material _originalMaterialHighlight;
    private Material _originalMaterialSelection;
    private Transform _highlight;
    private Transform _selection;
    private RaycastHit _raycastHit;

    void Update()
    {
        #region Highlight
        if (_highlight != null)
        {
            _highlight.GetComponent<MeshRenderer>().sharedMaterial = _originalMaterialHighlight;
            _highlight = null;
        }
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out _raycastHit))
        {
            _highlight = _raycastHit.transform;
            if (_highlight.CompareTag("Selectable") && _highlight != _selection)
            {
                HighlightObject(_highlight);
            }
            else
            {
                _highlight = null;
            }
        }
        #endregion

        #region Selection
        if (Input.GetMouseButtonDown(0))
        {
            if (_highlight)
            {
                if (_selection != null)
                {
                    _selection.GetComponent<MeshRenderer>().material = _originalMaterialSelection;
                }
                _selection = _raycastHit.transform;
                if (_selection.GetComponent<MeshRenderer>().material != selectionMaterial)
                {
                    _originalMaterialSelection = _originalMaterialHighlight;
                    _selection.GetComponent<MeshRenderer>().material = selectionMaterial;
                }
                _highlight = null;
            }
            else
            {
                if (_selection)
                {
                    _selection.GetComponent<MeshRenderer>().material = _originalMaterialSelection;
                    _selection = null;
                }
            }
        }
        #endregion
    }
    private void HighlightObject(Transform obj)
    {
        // Check and apply highlight to the object itself
        MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
        if (renderer != null)
        {
            if (renderer.material != highlightMaterial)
            {
                _originalMaterialHighlight = renderer.material;
                renderer.material = highlightMaterial;
            }
        }
        
        // Check and apply highlight to all children
        foreach (GameObject child in obj)
        {
            renderer = child.GetComponent<MeshRenderer>();
            if (renderer != null)
            {
                if (renderer.material != highlightMaterial)
                {
                    _originalMaterialHighlight = renderer.material;
                    renderer.material = highlightMaterial;
                }
            }
        }
    }

}