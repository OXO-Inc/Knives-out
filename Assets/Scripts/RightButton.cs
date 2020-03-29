using UnityEngine;
using UnityEngine.EventSystems;

public class RightButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isRightPressed = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isRightPressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isRightPressed = false;
    }
}