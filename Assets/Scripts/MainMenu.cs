using System.Collections;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public GameObject sliderGameObject;
	public GameObject gameModePanel;
	public GameObject scoreBoardPanel;
	public GameObject knifeChangePanel;
	public GameObject soundPanel;
	public GameObject checkImage;
	public GameObject[] knives;

	public Text casualScore;
	public Text expertScore;
	public Text timedScore;

	public Slider slider;
	public Slider soundSlider;

	public AudioSource panelClick;
	public AudioSource gameClick;

	private bool isSoundPanelOpen = false;
	private bool isGameModePanelOpen = false;
	private bool isScoreBoardPanelOpen = false;
	private bool isKnifeChangePanelOpen = false;
	private bool playSound = true;

	void Start()
    {
		Time.timeScale = 1;
		Application.targetFrameRate = 60;
		chooseGameMode(PlayerPrefs.GetInt("Gamemode",1));
		setHighScores();
		soundSlider.value = PlayerPrefs.HasKey("volumeLevel") ? PlayerPrefs.GetFloat("volumeLevel") : 0.5f;
	}

    /* Load Game Screen */
	public void loadScene()
	{
		playGameSound();
		sliderGameObject.SetActive(true);
		StartCoroutine(loadAsync());
	}

	IEnumerator loadAsync()
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(1);
		while (!operation.isDone)
		{
			slider.value = operation.progress;
			yield return null;
		}
	}

    /* Game mode Setting */
    public void chooseGameMode(int mode)
	{
		if (playSound)
			playSound = false;
		else
			playGameSound();

		RectTransform rectTransform = checkImage.GetComponent<RectTransform>();
		if (mode == 1)
		{
			rectTransform.localPosition = new Vector3(0, -290, 0);
			PlayerPrefs.SetInt("Gamemode", 1);
		}
		else if (mode == 2)
		{
			rectTransform.localPosition = new Vector3(-220, -290, 0);
			PlayerPrefs.SetInt("Gamemode", 2);
		}
		else if (mode == 3)
		{
			rectTransform.localPosition = new Vector3(240, -290, 0);
			PlayerPrefs.SetInt("Gamemode", 3);
		}
	}

	/* Sound Setting */
	public void soundSetting(float volume)
	{
		AudioListener.volume = volume;
		PlayerPrefs.SetFloat("volumeLevel", AudioListener.volume);
	}

    /* Panels */
	public void gameMode()
    {
		playPanelSound();
		if (isGameModePanelOpen) {
			closePanel("gamemode");
			return;
		}
		if (isScoreBoardPanelOpen)
		{
			closePanel("scoreboard");
		}
		gameModePanel.SetActive(true);
		isGameModePanelOpen = true;
    }

	public void scoreBoard()
	{
		playPanelSound();
		if (isScoreBoardPanelOpen)
		{
			closePanel("scoreboard");
			return;
		}
        if(isGameModePanelOpen)
        {
			closePanel("gamemode");
        }
		isScoreBoardPanelOpen = true;
		scoreBoardPanel.SetActive(true);
	}

	public void knifeBoard()
	{
		playPanelSound();
		playSound = true;
		changeKnife(PlayerPrefs.GetInt("knifeNumber", 0));
		if (isKnifeChangePanelOpen)
		{
			closePanel("knife");
			return;
		}

		isKnifeChangePanelOpen = true;
		knifeChangePanel.SetActive(true);
	}

	public void sound()
    {
		playPanelSound();
		if (isSoundPanelOpen)
		{
			closePanel("sound");
			return;
		}
		isSoundPanelOpen = true;
		soundPanel.SetActive(true);
	}

	public void closePanel(string menuName)
    {
		playPanelSound();
		if (menuName == "gamemode")
		{
			isGameModePanelOpen = false;
			gameModePanel.SetActive(false);
		}
		if (menuName == "scoreboard")
		{
			isScoreBoardPanelOpen = false;
			scoreBoardPanel.SetActive(false);
		}
		if (menuName == "sound")
		{
			isSoundPanelOpen = false;
			soundPanel.SetActive(false);
		}
		if (menuName == "knife")
		{
			isKnifeChangePanelOpen = false;
			knifeChangePanel.SetActive(false);
		}
	}

    /* Knife */
    public void changeKnife(int i)
    {
		if (playSound)
			playSound = false;
		else
			playGameSound();

		GameObject knife = knives[i];
        GameObject selected = knife.transform.GetChild(0).gameObject;
        GameObject locked = knife.transform.GetChild(1).gameObject;

		if (locked.active)
			return;
        else
        {
			selected.SetActive(true);
			GameObject previouslySelected = knives[PlayerPrefs.GetInt("knifeNumber", 0)];
			GameObject check = previouslySelected.transform.GetChild(0).gameObject;
			check.SetActive(false);
			PlayerPrefs.SetInt("knifeNumber", i);
		}
    }

	/* Sound Effect */
	private void playPanelSound()
	{
		panelClick.Play();
	}

	private void playGameSound()
	{
		gameClick.Play();
	}

	/* Set High Scores */
	private void setHighScores()
    {
		int timed = PlayerPrefs.GetInt("timedScore", -1);
		int casual = PlayerPrefs.GetInt("casualScore", -1);
		int expert = PlayerPrefs.GetInt("expertScore", -1);

		if (timed != -1)
			timedScore.text = "Timed: " + timed.ToString();

		if (casual != -1)
			casualScore.text = "Casual: " + casual.ToString();

		if (expert != -1)
			expertScore.text = "Expert: " + expert.ToString();
	}

    /* Quit Game */
	public void quitGame()
	{
		playGameSound();
		Application.Quit();
	}
}
