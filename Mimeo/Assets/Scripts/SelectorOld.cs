using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class SelectorOld : MonoBehaviour
{
    public Camera playerCamera;
    public Material highlightMaterial;
    public Material selectionMaterial;
    public TextMeshProUGUI textName;
    public PanelInfos panelInfos;
    public AudioClip soundHighlight;
    public AudioClip soundSelect;
    public AudioClip soundUnselect;

    private Material[] _originalMaterialsHighlight;
    private Material[] _originalMaterialsSelection;
    private Transform _highlight;
    private Transform _selection;
    private RaycastHit _raycastHit;
    private MeshRenderer _previousSelectedRenderer;
    private MeshRenderer _previousHighlightedRenderer;
    private float _oldLookSpeed;
    private bool _isSelecting;
    private bool _isHighlighting;
    
    public static SelectorOld instance { get; private set; }
    
    private void Awake()
    {
        if (instance && instance != this) Destroy(this); 
        else instance = this;
    }

    void Update()
    {
        if (_highlight)
        {
            _highlight.GetComponent<MeshRenderer>().materials = _originalMaterialsHighlight;
            _highlight = null;
        }
        
        Ray ray = playerCamera.ScreenPointToRay(new Vector3(Screen.width / 2, Screen.height / 2, 0));
        if (Physics.Raycast(ray, out _raycastHit))
        {
            _highlight = _raycastHit.transform;
            if (_highlight.CompareTag("Selectable") && _highlight != _selection)
            {
                Highlight();
            }
            else
            {
                _highlight = null;
                textName.text = "NO NAME";
            }
        }
    }

    private void Highlight()
    {
        MeshRenderer renderer = _highlight.GetComponent<MeshRenderer>();
        if (renderer)
        {
            if (renderer.material != highlightMaterial)
            {
                _isHighlighting = true;

                if (!_previousHighlightedRenderer)
                {
                    _previousHighlightedRenderer = renderer;
                    _originalMaterialsHighlight = renderer.materials;
                    if (AudioManager.instance) AudioManager.instance.PlayClipAt(soundHighlight, transform.position);
                }
                else if (_highlight != _previousHighlightedRenderer.transform)
                {
                    _previousHighlightedRenderer = renderer;
                    _originalMaterialsHighlight = renderer.materials;
                    if (AudioManager.instance) AudioManager.instance.PlayClipAt(soundHighlight, transform.position);
                }
                
                Material[] newMaterials = new Material[renderer.materials.Length];
                for (int i = 0; i < newMaterials.Length; i++)
                {
                    newMaterials[i] = highlightMaterial;
                }
                renderer.materials = newMaterials;
                
                SelectableElement selectableElement = _highlight.GetComponent<SelectableElement>();
                if (selectableElement) textName.text = selectableElement.name;
            }
        }
    }

    public void Unhighlight()
    {
        if (!_isHighlighting) return;
        
        _previousHighlightedRenderer.materials = _originalMaterialsHighlight;
        _highlight = null;
        
        _isHighlighting = false;
    }
    
    public void Select()
    {
        if (IsPointerOverUIElement()) return;
        if (!_highlight) return;
        if (_selection) _selection.GetComponent<MeshRenderer>().materials = _originalMaterialsSelection;
            
        _selection = _raycastHit.transform;
        MeshRenderer renderer = _selection.GetComponent<MeshRenderer>();
        if (renderer.material != selectionMaterial)
        {
            _isSelecting = true;
            if (AudioManager.instance) AudioManager.instance.PlayClipAt(soundSelect, transform.position);

            SelectableElement selectableElement = _selection.GetComponent<SelectableElement>();
            if (selectableElement) panelInfos.Open(selectableElement);
            
            _previousSelectedRenderer = renderer;
            _originalMaterialsSelection = _originalMaterialsHighlight;
            Material[] newMaterials = new Material[renderer.materials.Length];
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = selectionMaterial;
            }
            renderer.materials = newMaterials;
            
            InputsManager.instance.mainInputs.FPSController.Move.Disable();

            float actualLookSpeed = FPSController.instance.lookSpeed;
            if (!Mathf.Approximately(actualLookSpeed, 2f)) _oldLookSpeed = actualLookSpeed;
            FPSController.instance.lookSpeed = 2f;
            
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        _highlight = null;
    }

    public void Unselect()
    {
        if (!_isSelecting) return;
        
        if (AudioManager.instance) AudioManager.instance.PlayClipAt(soundUnselect, transform.position);
        
        panelInfos.Close();

        bool isPreviousSelectedElementMaterialChanged = false;
        foreach (Material material in _previousSelectedRenderer.materials)
        {
            if (material != selectionMaterial) isPreviousSelectedElementMaterialChanged = true;
        }
        
        if (!isPreviousSelectedElementMaterialChanged) _previousSelectedRenderer.materials = _originalMaterialsSelection;
        
        MeshRenderer selectRenderer = _selection.GetComponent<MeshRenderer>();
        selectRenderer.materials = _originalMaterialsSelection;
        _selection = null;
        
        InputsManager.instance.mainInputs.FPSController.Move.Enable();
        FPSController.instance.lookSpeed = _oldLookSpeed;
        
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        _isSelecting = false;
    }
    
    private bool IsPointerOverUIElement()
    {
        PointerEventData eventDataCurrentPosition = new PointerEventData(EventSystem.current)
        {
            position = new Vector2(Input.mousePosition.x, Input.mousePosition.y)
        };
        
        List<RaycastResult> results = new List<RaycastResult>();
        EventSystem.current.RaycastAll(eventDataCurrentPosition, results);
        
        foreach (RaycastResult result in results)
        {
            if (result.gameObject.name != "Crosshair")
            {
                return true;
            }
        }
        
        return false;
    }
}