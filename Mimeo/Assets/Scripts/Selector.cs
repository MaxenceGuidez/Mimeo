using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour
{
    public Camera playerCamera;
    public Material highlightMaterial;
    public Material selectionMaterial;
    public TextMeshProUGUI textName;
    public AudioClip soundHighlight;
    public AudioClip soundSelect;
    public AudioClip soundUnselect;
    
    private SelectableElement _actualHighlight;
    private SelectableElement _actualSelection;
    private SelectableElement _originalHighlight;
    private SelectableElement _originalSelection;
    private RaycastHit _raycastHit;
    private bool _isHighlighting;
    private bool _isSelecting;
    
    public static Selector instance { get; private set; }
    
    private void Awake()
    {
        if (instance && instance != this) Destroy(this); 
        else instance = this;
    }

    void Update()
    {
        Vector2 middleScreen = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = playerCamera.ScreenPointToRay(middleScreen);
        if (Physics.Raycast(ray, out _raycastHit))
        {
            SelectableElement selectableElementTouched = _raycastHit.transform.GetComponent<SelectableElement>();
            if (selectableElementTouched) {
                _actualHighlight = selectableElementTouched;
                
                if (_actualHighlight.CompareTag("Selectable") && _actualHighlight != _actualSelection)
                {
                    Highlight();
                }
                else
                {
                    Unhighlight();
                }
            }
        }
    }

    private void Highlight()
    {
        bool isAlreadyHighlighted = _actualHighlight.isHighlighted;
        bool isAlreadySelected = _actualHighlight.isSelected;
        if (!isAlreadyHighlighted && !isAlreadySelected)
        {
            _originalHighlight = _actualHighlight;
            if (AudioManager.instance) AudioManager.instance.PlayClipAt(soundHighlight, transform.position);
        }

        _isHighlighting = true;
        _actualHighlight.isHighlighted = true;
        
        MeshRenderer actualHighlightRenderer = _actualHighlight.GetComponent<MeshRenderer>();
        if (actualHighlightRenderer)
        {
            Material[] newMaterials = new Material[actualHighlightRenderer.materials.Length];
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = highlightMaterial;
            }
            actualHighlightRenderer.materials = newMaterials;
        }
        
        textName.text = _actualHighlight.name;
    }

    public void Unhighlight()
    {
        if (!_isHighlighting) return;
        
        _actualHighlight.isHighlighted = false;
        
        MeshRenderer actualHighlightRenderer = _actualHighlight.GetComponent<MeshRenderer>();
        MeshRenderer originalHighlightRenderer = _originalHighlight.GetComponent<MeshRenderer>();
        if (actualHighlightRenderer && originalHighlightRenderer)
        {
            Material[] originalMaterials = new Material[originalHighlightRenderer.materials.Length];
            for (int i = 0; i < originalMaterials.Length; i++)
            {
                originalMaterials[i] = originalHighlightRenderer.materials[i];
            }
            actualHighlightRenderer.materials = originalMaterials;
        }
        
        _actualHighlight = null;
        _isHighlighting = false;
        
        textName.text = "NO NAME";
    }
    
    public void Select()
    {
        if (IsPointerOverUIElement()) return;
        if (!_actualHighlight) return;
        
        bool isAlreadyHighlighted = _actualSelection.isHighlighted;
        bool isAlreadySelected = _actualSelection.isSelected;
        if (!isAlreadyHighlighted && !isAlreadySelected)
        {
            _originalSelection = _actualSelection;
            if (AudioManager.instance) AudioManager.instance.PlayClipAt(soundSelect, transform.position);
        }

        _isSelecting = true;
        _actualSelection.isHighlighted = false;
        _actualSelection.isSelected = true;

        MeshRenderer actualSelectionRenderer = _actualSelection.GetComponent<MeshRenderer>();
        if (actualSelectionRenderer)
        {
            Material[] newMaterials = new Material[actualSelectionRenderer.materials.Length];
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i] = selectionMaterial;
            }
            actualSelectionRenderer.materials = newMaterials;
        }
        
        InputsManager.instance.EnableSelectionMode();
        PanelInfos.instance.Open(_actualSelection);
        
        _actualHighlight = null;
    }

    public void Unselect()
    {
        if (!_isSelecting) return;
        
        _actualSelection.isSelected = false;
        _actualSelection.isHighlighted = true;
        
        if (AudioManager.instance) AudioManager.instance.PlayClipAt(soundUnselect, transform.position);
        
        PanelInfos.instance.Close();
        InputsManager.instance.DisableSelectionMode();
        
        MeshRenderer actualSelectionRenderer = _actualSelection.GetComponent<MeshRenderer>();
        MeshRenderer originalSelectionRenderer = _originalSelection.GetComponent<MeshRenderer>();
        if (actualSelectionRenderer && originalSelectionRenderer)
        {
            Material[] originalMaterials = new Material[originalSelectionRenderer.materials.Length];
            for (int i = 0; i < originalMaterials.Length; i++)
            {
                originalMaterials[i] = originalSelectionRenderer.materials[i];
            }
            actualSelectionRenderer.materials = originalMaterials;
        }

        _actualSelection = null;
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