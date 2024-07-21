using TMPro;
using UnityEngine;

public class Selector : MonoBehaviour
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
    private FPSController _fpsController;
    private MeshRenderer _previousSelectedElement;
    private MeshRenderer _previousHighlightedElement;
    private float _oldLookSpeed;
    private bool _isSelecting;
    private bool _isHighlighting;

    private void Awake()
    {
        FPSController controller = transform.GetComponent<FPSController>();
        if (controller) _fpsController = controller;
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

    public void Select()
    {
        RaycastHit _hit;
        Ray ray = playerCamera.ScreenPointToRay(Input.mousePosition);
        // TODO : Upgrade to be only the UI that return, not all not selectable elements
        // TODO : Upgrade to catch the sky
        if (Physics.Raycast(ray, out _hit) && !_hit.transform.CompareTag("Selectable")) return;

        if (!_highlight) return;
        if (_selection) _selection.GetComponent<MeshRenderer>().materials = _originalMaterialSelection;
            
        _selection = _raycastHit.transform;
        MeshRenderer renderer = _selection.GetComponent<MeshRenderer>();
        if (renderer.material != selectionMaterial)
        {
            _isSelecting = true;
            _previousSelectedElement = renderer;
            
            _originalMaterialSelection = _originalMaterialHighlight;
            Material[] newMaterials = new Material[renderer.materials.Length];
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = selectionMaterial;
            }
            renderer.materials = newMaterials;

            SelectableElement selectableElement = _selection.GetComponent<SelectableElement>();
            if (selectableElement) panelInfos.Open(selectableElement);
            
            InputsManager.instance.mainInputs.FPSController.Move.Disable();
            _oldLookSpeed = _fpsController.lookSpeed;
            _fpsController.lookSpeed = 2f;
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        _highlight = null;
    }

    public void Unselect()
    {
        if (!_isSelecting) return;
        
        panelInfos.Close();

        _previousSelectedElement.materials = _originalMaterialSelection;
        
        InputsManager.instance.mainInputs.FPSController.Move.Enable();
        _fpsController.lookSpeed = _oldLookSpeed;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        _isSelecting = false;
    }

    private void HighlightObject(Transform obj)
    {
        MeshRenderer renderer = obj.GetComponent<MeshRenderer>();
        if (renderer)
        {
            if (renderer.material != highlightMaterial)
            {
                _isHighlighting = true;
                _previousHighlightedElement = renderer;
                _originalMaterialHighlight = renderer.materials;
                Material[] newMaterials = new Material[renderer.materials.Length];
                for (int i = 0; i < newMaterials.Length; i++)
                {
                    newMaterials[i] = highlightMaterial;
                }
                renderer.materials = newMaterials;
                
                SelectableElement selectableElement = obj.GetComponent<SelectableElement>();
                if (selectableElement) textName.text = selectableElement.name;
            }
        }
    }

    public void UnhighlightObject()
    {
        if (!_isHighlighting) return;
        
        _previousHighlightedElement.materials = _originalMaterialHighlight;
        
        _isHighlighting = false;
    }
}