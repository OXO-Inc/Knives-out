using UnityEngine;
using UnityEngine.EventSystems;

public class LeftButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isLeftPressed = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isLeftPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isLeftPressed = false;
    }
}