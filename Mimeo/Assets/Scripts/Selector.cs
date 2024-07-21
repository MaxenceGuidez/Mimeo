using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;

public class Selection : MonoBehaviour
{
    public Camera playerCamera;
    public Material highlightMaterial;
    public Material selectionMaterial;
    public TextMeshProUGUI textName;
    public PanelInfos panelInfos;

    private Material[] _originalMaterialHighlight;
    private Material[] _originalMaterialSelection;
    private Transform _highlight;
    private Transform _selection;
    private RaycastHit _raycastHit;
    private MainInputs _mainInputs;

    private void Awake()
    {
        _mainInputs = new MainInputs();
    }

    private void OnEnable()
    {
        _mainInputs.Selector.Enable();
        _mainInputs.Selector.Select.performed += Select;
    }

    private void OnDisable()
    {
        _mainInputs.Selector.Disable();
        _mainInputs.Selector.Select.performed -= Select;
    }

    void Update()
    {
        if (_highlight)
        {
            _highlight.GetComponent<MeshRenderer>().materials = _originalMaterialHighlight;
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
                textName.text = "NO NAME";
            }
        }
    }

    private void Select(InputAction.CallbackContext context)
    {
        RaycastHit _hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out _hit))
        {
            if (!_hit.transform.CompareTag("Selectable")) return;
        }

        if (!_highlight) return;
        if (_selection) _selection.GetComponent<MeshRenderer>().materials = _originalMaterialSelection;
            
        _selection = _raycastHit.transform;
        MeshRenderer renderer = _selection.GetComponent<MeshRenderer>();
        if (renderer.material != selectionMaterial)
        {
            _originalMaterialSelection = _originalMaterialHighlight;
            Material[] newMaterials = new Material[renderer.materials.Length];
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = selectionMaterial;
            }
            renderer.materials = newMaterials;

            SelectableElement selectableElement = _selection.GetComponent<SelectableElement>();
            if (selectableElement)
            {
                panelInfos.Open(selectableElement);
            }
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        _highlight = null;
    }

    private void HighlightObject(Transform obj)
    {
        MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
        if (renderer)
        {
            if (renderer.material != highlightMaterial)
            {
                _originalMaterialHighlight = renderer.materials;
                Material[] newMaterials = new Material[renderer.materials.Length];
                for (int i = 0; i < newMaterials.Length; i++)
                {
                    newMaterials[i] = highlightMaterial;
                }
                renderer.materials = newMaterials;
                
                SelectableElement selectableElement = obj.GetComponent<SelectableElement>();
                if (selectableElement)
                {
                    textName.text = selectableElement.name;
                }
            }
        }
    }
}