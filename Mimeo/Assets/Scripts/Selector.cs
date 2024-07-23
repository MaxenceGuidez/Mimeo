using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

/// <summary>
/// Manages the selection and highlighting of build elements in the app world.
/// This class handles visual feedback, audio cues, and state changes for elements that the player interacts with.
/// </summary>
/// <author>GUIDEZ Maxence</author>
/// <date>2024-07-23</date>
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
    
    /// <summary>
    /// Initializes the Selector instance.
    /// If another instance of Selector exists, it destroys the new one to maintain a single instance.
    /// </summary>
    private void Awake()
    {
        if (instance && instance != this) Destroy(this); 
        else instance = this;
    }

    /// <summary>
    /// Performs an update each frame to handle highlighting of build elements.
    /// Casts a ray from the center of the screen to detect build elements and highlight them if necessary.
    /// </summary>
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

    /// <summary>
    /// Highlights the currently hovered build element by changing its material and playing a highlight sound.
    /// Updates the textName to display the name of the highlighted element.
    /// </summary>
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

    /// <summary>
    /// Removes the highlight from the currently highlighted build element.
    /// Restores the original materials and resets the highlighted status.
    /// Resets textName to "NO NAME".
    /// </summary>
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
    
    /// <summary>
    /// Selects the currently highlighted build element.
    /// If an element was previously selected, it is unselected.
    /// Updates the appearance of the selected element and plays the selection sound.
    /// Opens the PanelInfos UI with the selected element and enable selection mode.
    /// </summary>
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
            Material[] originalHighlightMaterials = _originalHighlightMaterials;
            _originalSelectionMaterials = new Material[originalHighlightMaterials.Length];
            for (int i = 0; i < originalHighlightMaterials.Length; i++)
            {
                _originalSelectionMaterials[i] = new Material(originalHighlightMaterials[i]);
            }
            
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

    /// <summary>
    /// Unselects the currently selected build element.
    /// Resets the appearance of the element, stops selection mode, and closes the PanelInfos UI.
    /// Plays the unselect sound.
    /// </summary>
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

    /// <summary>
    /// Updates the stored original materials for the currently selected build element.
    /// This is used to restore the original appearance of the selected element when unselecting.
    /// </summary>
    public void UpdateOriginalMaterial()
    {
        MeshRenderer actualSelectionRenderer = _actualSelection.GetComponent<MeshRenderer>();
        if (actualSelectionRenderer && _actualSelection.state == BuildElement.ElementState.SELECTED)
        {
            _originalSelectionMaterials = actualSelectionRenderer.materials;
        }
    }
    
    /// <summary>
    /// Checks if the mouse pointer is currently over a UI element.
    /// This is used to prevent interaction with build elements while interacting with the UI.
    /// </summary>
    /// <returns>True if the pointer is over a UI element; otherwise, false.</returns>
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
