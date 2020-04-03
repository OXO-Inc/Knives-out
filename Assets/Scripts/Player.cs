using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    public LeftButton leftButton;
    public RightButton rightButton;

    public GameObject player;

    void Update()
    {
        if (leftButton.isLeftPressed)
            player.transform.Translate(Vector3.left * Time.deltaTime * 2.5f);
        if (rightButton.isRightPressed)
            player.transform.Translate(Vector3.right * Time.deltaTime * 2.5f);
    }
}
