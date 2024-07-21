using System.Collections;
using UnityEngine;

public class PanelInfos : MonoBehaviour
{
    public GameObject textName;
    public float animationDuration = .2f;

    private Vector3 _closedPosition;
    private Vector3 _openPosition;
    private bool _isAnimating;

    void Start()
    {
        _closedPosition = transform.localPosition;
        _openPosition = _closedPosition - new Vector3(0, 650, 0);
    }

    public void Open()
    {
        if (!_isAnimating)
        {
            StartCoroutine(TranslateUIElementTo(transform, _openPosition));
            textName.SetActive(false);
        }
    }

    public void Close()
    {
        if (!_isAnimating)
        {
            StartCoroutine(TranslateUIElementTo(transform, _closedPosition));
            textName.SetActive(true);
        }
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
}