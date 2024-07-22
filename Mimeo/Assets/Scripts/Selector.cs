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
    private Material[] _originalHighlightMaterials;
    private Material[] _originalSelectionMaterials;
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
        if (_actualHighlight) Unhighlight();
        
        Vector2 middleScreen = new Vector2(Screen.width / 2, Screen.height / 2);
        Ray ray = playerCamera.ScreenPointToRay(middleScreen);
        if (Physics.Raycast(ray, out _raycastHit))
        {
            SelectableElement selectableElementTouched = _raycastHit.transform.GetComponent<SelectableElement>();
            if (!selectableElementTouched) return;
            
            if (selectableElementTouched.state == SelectableElement.SelectableState.UNUSED)
            {
                _actualHighlight = selectableElementTouched;
                Highlight();
            }
        }
    }

    private void Highlight()
    {
        _isHighlighting = true;

        if (_actualHighlight.state != SelectableElement.SelectableState.UNUSED) return;
        _actualHighlight.state = SelectableElement.SelectableState.HIGHLIGHTED;
        
        MeshRenderer highlightRenderer = _actualHighlight.GetComponent<MeshRenderer>();
        if (highlightRenderer) _originalHighlightMaterials = highlightRenderer.materials;
        
        if (AudioManager.instance) AudioManager.instance.PlayClipAt(soundHighlight, transform.position);
        
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
        
        _actualHighlight.state = SelectableElement.SelectableState.UNUSED;
        
        MeshRenderer actualHighlightRenderer = _actualHighlight.GetComponent<MeshRenderer>();
        if (actualHighlightRenderer)
        {
            Material[] originalMaterials = new Material[_originalHighlightMaterials.Length];
            for (int i = 0; i < originalMaterials.Length; i++)
            {
                originalMaterials[i] = _originalHighlightMaterials[i];
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
        
        SelectableElement previousSelection = _actualSelection;
        
        if (previousSelection)
        {
            if (_raycastHit.transform != previousSelection.transform) Unselect();
        }
        
        _isSelecting = true;
        
        _actualSelection = _raycastHit.transform.GetComponent<SelectableElement>();
        if (!_actualSelection) return;

        
        if (_actualSelection.state == SelectableElement.SelectableState.SELECTED) return;
        if (_actualSelection.state == SelectableElement.SelectableState.HIGHLIGHTED)
        {
            _originalSelectionMaterials = _originalHighlightMaterials;
            if (AudioManager.instance) AudioManager.instance.PlayClipAt(soundSelect, transform.position);
        }

        _actualSelection.state = SelectableElement.SelectableState.SELECTED;

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
        
        _actualSelection.state = SelectableElement.SelectableState.UNUSED;
        
        if (AudioManager.instance) AudioManager.instance.PlayClipAt(soundUnselect, transform.position);
        
        PanelInfos.instance.Close();
        InputsManager.instance.DisableSelectionMode();
        
        MeshRenderer actualSelectionRenderer = _actualSelection.GetComponent<MeshRenderer>();
        if (actualSelectionRenderer)
        {
            Material[] originalMaterials = new Material[_originalSelectionMaterials.Length];
            for (int i = 0; i < originalMaterials.Length; i++)
            {
                originalMaterials[i] = _originalSelectionMaterials[i];
            }
            actualSelectionRenderer.materials = originalMaterials;
        }

        _actualSelection = null;
        _isSelecting = false;
    }

    public void UpdateOriginalMaterial()
    {
        MeshRenderer actualSelectionRenderer = _actualSelection.GetComponent<MeshRenderer>();
        if (actualSelectionRenderer)
        {
            _originalSelectionMaterials = actualSelectionRenderer.materials;
        }
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