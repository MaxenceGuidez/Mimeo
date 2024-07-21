using System.Collections;
using TMPro;
using UnityEngine;

public class PanelInfos : MonoBehaviour
{
    public GameObject lineNameDisplay;
    public TMP_InputField textFieldName;
    public TMP_InputField textFieldDescription;
    public TMP_Dropdown dropdownColor;
    public TMP_Dropdown dropdownTexture;
    public float animationDuration = .2f;

    private SelectableElement _selectedElement;
    private Vector3 _closedPosition;
    private Vector3 _openPosition;
    private bool _isAnimating;
    
    void Start()
    {
        _closedPosition = transform.localPosition;
        _openPosition = _closedPosition - new Vector3(0, 650, 0);

        textFieldName.onValueChanged.AddListener(delegate {OnNameChange(); });
        textFieldDescription.onValueChanged.AddListener(delegate {OnDescriptionChange(); });
    }

    public void Open(SelectableElement selected)
    {
        if (!selected) return;
        _selectedElement = selected;

        textFieldName.text = _selectedElement.name;
        textFieldDescription.text = _selectedElement.description;
        
        if (!_isAnimating)
        {
            StartCoroutine(TranslateUIElementTo(transform, _openPosition));
            lineNameDisplay.SetActive(false);
        }
    }

    public void Close()
    {
        if (!_isAnimating)
        {
            StartCoroutine(TranslateUIElementTo(transform, _closedPosition));
            lineNameDisplay.SetActive(true);
        }
    }

    public void CloseDirectly()
    {
        transform.localPosition = _closedPosition;
        lineNameDisplay.SetActive(true);
    }

    private IEnumerator TranslateUIElementTo(Transform UIElement, Vector3 targetPosition)
    {
        _isAnimating = true;
        Vector3 startPosition = UIElement.localPosition;
        float elapsedTime = 0;

        while (elapsedTime < animationDuration)
        {
            UIElement.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        UIElement.localPosition = targetPosition;
        _isAnimating = false;
    }

    void OnNameChange()
    {
        _selectedElement.name = textFieldName.text;
    }

    void OnDescriptionChange()
    {
        _selectedElement.description = textFieldDescription.text;
    }
}