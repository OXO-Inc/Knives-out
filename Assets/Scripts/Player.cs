using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public LeftButton[] leftButton;
    public RightButton[] rightButton;

    public GameObject player;

    public GameObject pauseButton;
    public GameObject playButton;

    void Start()
    {
        Application.targetFrameRate = 60;
    }

    void Update()
    {
        if (leftButton[0].isLeftPressed || leftButton[1].isLeftPressed)
            player.transform.Translate(Vector3.left * Time.deltaTime * 3f);
        if (rightButton[0].isRightPressed || rightButton[1].isRightPressed)
            player.transform.Translate(Vector3.right * Time.deltaTime * 3f);
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void pauseGame()
    {
        playButton.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0;
    }

    public void playGame()
    {
        playButton.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
    }
}
