using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;

public class Player : MonoBehaviour
{
    public LeftButton[] leftButton;
    public RightButton[] rightButton;

    public GameObject player;
    public GameObject playerPrefab;
    public GameObject casualPlayerIcon;
    public GameObject pauseButton;
    public GameObject playButton;
    public GameObject timedMode;
    public GameObject casualMode;
    public GameObject expertMode;
    public GameObject background;
    public GameObject plusScore;
    public GameObject scorePanel;

    public Sprite[] sprites;

    public FruitSpawner fruitSpawner;

    public Text time;
    public Text knives;
    public Text bestScore;
    public Text gameScore;

    public AudioSource gameClick;
    public AudioSource expertModeSound;
    public AudioSource casualModeSound;
    public AudioSource timedModeSound;

    public int numberOfKnives = 75;
    public int gameModeVal;

    int fastWaveMultiplier = -1;
    int level = 0;
    int knife;

    float fastWaveTime = 15f;
    float secondCount = 60f;
    float timeElapsed = 0f;
    float playerSpeedMultiplier = 3f;
    float backToNormalTime;

    void Start()
    {
        Time.timeScale = 1;
        gameModeVal = PlayerPrefs.GetInt("Gamemode");
        knife = PlayerPrefs.GetInt("knifeNumber");
        setPlayer(knife);

        if (gameModeVal == 1)
        {
            casualMode.SetActive(true);
        }
        else if(gameModeVal == 2)
        {
            timedMode.SetActive(true);
        }
        else if(gameModeVal == 3)
        {
            expertMode.SetActive(true);
        }
        
    }

    void setPlayer(int knife)
    {
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[knife];

        spriteRenderer = playerPrefab.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[knife];

        Image image = casualPlayerIcon.GetComponent<Image>();
        image.sprite = sprites[knife];
    }

    void Update()
    {
        //Player Control
        if (leftButton[0].isLeftPressed || leftButton[1].isLeftPressed)
            player.transform.Translate(Vector3.left * Time.deltaTime * playerSpeedMultiplier);
        if (rightButton[0].isRightPressed || rightButton[1].isRightPressed)
            player.transform.Translate(Vector3.right * Time.deltaTime * playerSpeedMultiplier);

        timeElapsed += Time.deltaTime;

        //Casual Mode
        if (gameModeVal == 1)
        {
            knives.text = numberOfKnives.ToString();
            if (numberOfKnives == 0)
                gameOverCasualMode();

            if ((int)timeElapsed == (int)fastWaveTime)
                fastWave();
        }

        //Timed Mode
        if (gameModeVal == 2)
        {
            secondCount -= Time.deltaTime;
            if ((int)secondCount == 0)
                gameOverTimedMode();
            else if (secondCount < 10)
            {
                time.text = "00" + ":0" + ((int)secondCount).ToString();
                time.color = Color.red;
            }
            else if (secondCount > 10)
                time.text = "00" + ":" + ((int)secondCount).ToString();
        }
    }

    public void plusScoreAnimation(Transform transform)
    {
        Transform plusScoreTransform = plusScore.GetComponent<Transform>();
        plusScoreTransform.position = transform.position;
        plusScore.SetActive(true);
        StartCoroutine(plusScoreInactive());
    }

    IEnumerator plusScoreInactive()
    { 
        yield return new WaitForSeconds(.5f);
        plusScore.SetActive(false);
    }

    private void levelUp()
    {
        level += 1;
        if (level < 10)
        {
            playerSpeedMultiplier = 3f + level * .1f;
            fruitSpawner.speedMultiplier = .7f + level * .1f;
            fruitSpawner.spawnDelay = 1f - level * .05f;        
        }
        else
        {
            playerSpeedMultiplier = 4f;
            fruitSpawner.speedMultiplier = .8f;
            fruitSpawner.spawnDelay = .5f;
        }

        fruitSpawner.callInvoke();
        StartCoroutine(fade(.65f, 1f, 3f));
    }

    private void fastWave()
    {
        fastWaveMultiplier += 1;
        backToNormalTime = 5 + Random.Range(1.15f, 1.25f) * fastWaveMultiplier;
        fastWaveTime = fastWaveTime + Random.Range(15, 20) + 1.25f * fastWaveMultiplier;

        playerSpeedMultiplier = 4f;
        fruitSpawner.speedMultiplier = .8f;
        fruitSpawner.spawnDelay = .5f;

        fruitSpawner.callInvoke();

        StartCoroutine(fade(1f, .65f, 1.5f));
        StartCoroutine(backToNormal(backToNormalTime));
    }

    IEnumerator backToNormal(float time)
    {
        yield return new WaitForSeconds(time);
        levelUp();
    }

    IEnumerator fade(float from, float to, float time)
    {
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
        {
            Color newColor = new Color(1, Mathf.Lerp(from, to, t), Mathf.Lerp(from, to, t), 1);
            background.GetComponent<Image>().color = newColor;
            yield return null;
        }
    }

    private void gameOverTimedMode()
    {
        Time.timeScale = 0;
        gameScore.text = "Score: " + ScoreKeeper.score.ToString();
        int highScore = PlayerPrefs.GetInt("timedScore", -1);
        if (ScoreKeeper.score > highScore)
        {
            PlayerPrefs.SetInt("timedScore", ScoreKeeper.score);
            bestScore.text = "Best: " + ScoreKeeper.score.ToString() + " *";
        }
        else
            bestScore.text = "Best: " + highScore.ToString();
        scorePanel.SetActive(true);
        pauseButton.SetActive(false);
        timedMode.SetActive(false);
    }

    private void gameOverCasualMode()
    {
        Time.timeScale = 0;
        gameScore.text = "Score: " + ScoreKeeper.score.ToString();
        int highScore = PlayerPrefs.GetInt("casualScore", -1);
        if (ScoreKeeper.score > highScore)
        {
            PlayerPrefs.SetInt("casualScore", ScoreKeeper.score);
            bestScore.text = "Best: " + ScoreKeeper.score.ToString() + " *";
        }
        else
            bestScore.text = "Best: " + highScore.ToString();
        scorePanel.SetActive(true);
        pauseButton.SetActive(false);
        casualMode.SetActive(false);
    }

    public void gameOverExpertMode()
    {
        Time.timeScale = 0;
        gameScore.text = "Score: " + ScoreKeeper.score.ToString();
        int highScore = PlayerPrefs.GetInt("expertScore", -1);
        if (ScoreKeeper.score > highScore)
        {
            PlayerPrefs.SetInt("expertScore", ScoreKeeper.score);
            bestScore.text = "Best: " + ScoreKeeper.score.ToString() + " *";
        }
        else
            bestScore.text = "Best: " + highScore.ToString();
        scorePanel.SetActive(true);
        pauseButton.SetActive(false);
        expertMode.SetActive(false);
    }

    public void pauseGame()
    {
        playGameSound();
        expertModeSound.Pause();
        casualModeSound.Pause();
        timedModeSound.Pause();
        playButton.SetActive(true);
        pauseButton.SetActive(false);
        Time.timeScale = 0;
    }

    public void playGame()
    {
        playGameSound();
        expertModeSound.Play();
        casualModeSound.Play();
        timedModeSound.Play();
        playButton.SetActive(false);
        pauseButton.SetActive(true);
        Time.timeScale = 1;
    }

    public void goToMainMenu()
    {
        SceneManager.LoadScene(0);
    }

    public void retryGame()
    {
        playGameSound();
        SceneManager.LoadScene(1);
    }

    private void playGameSound()
    {
        gameClick.Play();
    }
}
