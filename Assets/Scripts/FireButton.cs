using UnityEngine;
using UnityEngine.EventSystems;

public class FireButton : MonoBehaviour, IPointerDownHandler, IPointerUpHandler
{
    public bool isFirePressed = false;

    public void OnPointerDown(PointerEventData eventData)
    {
        isFirePressed = true;
    }

    public void OnPointerUp(PointerEventData eventData)
    {
        isFirePressed = false;
    }
}
