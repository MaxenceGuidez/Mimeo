using System.Collections;
using UnityEngine;

public class PanelInfos : MonoBehaviour
{
    public float animationDuration = 0.5f;

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
            StartCoroutine(AnimatePanel(_openPosition));
        }
    }

    public void Close()
    {
        if (!_isAnimating)
        {
            StartCoroutine(AnimatePanel(_closedPosition));
        }
    }

    private IEnumerator AnimatePanel(Vector3 targetPosition)
    {
        _isAnimating = true;
        Vector3 startPosition = transform.localPosition;
        float elapsedTime = 0;

        while (elapsedTime < animationDuration)
        {
            transform.localPosition = Vector3.Lerp(startPosition, targetPosition, elapsedTime / animationDuration);
            elapsedTime += Time.deltaTime;
            yield return null;
        }

        transform.localPosition = targetPosition;
        _isAnimating = false;
    }
}