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
    
    private BuildElement _actualHighlight;
    private BuildElement _actualSelection;
    private BuildElement _previousHighlightForSFX;
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
            BuildElement buildElementTouched = _raycastHit.transform.GetComponent<BuildElement>();
            if (!buildElementTouched)
            {
                _previousHighlightForSFX = null;
                return;
            }
            
            if (buildElementTouched.state == BuildElement.ElementState.UNUSED)
            {
                _actualHighlight = buildElementTouched;
                Highlight();
            }
        }
    }

    private void Highlight()
    {
        _isHighlighting = true;

        if (_actualHighlight.state != BuildElement.ElementState.UNUSED) return;
        _actualHighlight.state = BuildElement.ElementState.HIGHLIGHTED;
        
        MeshRenderer highlightRenderer = _actualHighlight.GetComponent<MeshRenderer>();
        if (highlightRenderer)
        {
            Material[] originalMaterials = highlightRenderer.materials;
            _originalHighlightMaterials = new Material[originalMaterials.Length];
            for (int i = 0; i < originalMaterials.Length; i++)
            {
                _originalHighlightMaterials[i] = new Material(originalMaterials[i]);
            }
        }

        if (!_previousHighlightForSFX)
        {
            if (AudioManager.instance) AudioManager.instance.PlayClipAt(soundHighlight, transform.position);
        }
        else if (_actualHighlight.transform != _previousHighlightForSFX.transform)
        {
            if (AudioManager.instance) AudioManager.instance.PlayClipAt(soundHighlight, transform.position);
        }
        _previousHighlightForSFX = _actualHighlight;
        
        MeshRenderer actualHighlightRenderer = _actualHighlight.GetComponent<MeshRenderer>();
        if (actualHighlightRenderer)
        {
            Material[] newMaterials = actualHighlightRenderer.materials;
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i].color = highlightMaterial.color;
            }
            actualHighlightRenderer.materials = newMaterials;
        }
        
        textName.text = _actualHighlight.name;
    }

    public void Unhighlight()
    {
        if (!_isHighlighting) return;
        
        _actualHighlight.state = BuildElement.ElementState.UNUSED;
        
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
        
        BuildElement previousSelection = _actualSelection;
        
        if (previousSelection)
        {
            if (_raycastHit.transform != previousSelection.transform) Unselect();
        }
        
        _isSelecting = true;
        
        _actualSelection = _raycastHit.transform.GetComponent<BuildElement>();
        if (!_actualSelection) return;
        
        if (_actualSelection.state == BuildElement.ElementState.SELECTED) return;
        if (_actualSelection.state == BuildElement.ElementState.HIGHLIGHTED)
        {
            _originalSelectionMaterials = _originalHighlightMaterials;
            if (AudioManager.instance) AudioManager.instance.PlayClipAt(soundSelect, transform.position);
        }
        
        InputsManager.instance.EnableSelectionMode();
        PanelInfos.instance.Open(_actualSelection);
        
        _actualSelection.state = BuildElement.ElementState.SELECTED;

        MeshRenderer actualSelectionRenderer = _actualSelection.GetComponent<MeshRenderer>();
        if (actualSelectionRenderer)
        {
            Material[] newMaterials = actualSelectionRenderer.materials;
            for (int i = 0; i < newMaterials.Length; i++)
            {
                newMaterials[i].color = selectionMaterial.color;
            }
            actualSelectionRenderer.materials = newMaterials;
        }
        
        _actualHighlight = null;
    }

    public void Unselect()
    {
        if (!_isSelecting) return;
        
        _actualSelection.state = BuildElement.ElementState.UNUSED;
        
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
