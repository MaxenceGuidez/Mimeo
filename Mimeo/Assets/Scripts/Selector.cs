using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Selector : MonoBehaviour
{
    public static Selector instance { get; private set; }
    
    private void Awake()
    {
        if (instance && instance != this) Destroy(this); 
        else instance = this;
    }

    void Update()
    {
        
    }

    private void Highlight()
    {
        
    }

    public void Unhighlight()
    {
        
    }
    
    public void Select()
    {
        
    }

    public void Unselect()
    {
        
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