using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public LeftButton leftButton;
    public RightButton rightButton;

    void Update()
    {
        if (leftButton.isLeftPressed)
            transform.Translate(Vector3.left * Time.deltaTime);
        if (rightButton.isRightPressed)
            transform.Translate(Vector3.right * Time.deltaTime);
    }
}
