using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public LeftButton leftButton;
    public RightButton rightButton;

    public GameObject player;

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if (leftButton.isLeftPressed)
            player.transform.Translate(Vector3.left * Time.deltaTime * 3f);
        if (rightButton.isRightPressed)
            player.transform.Translate(Vector3.right * Time.deltaTime * 3f);
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene(0);
    }
}
