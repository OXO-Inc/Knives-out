using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using System.Collections;
using UnityEngine.Advertisements;

public class Player : MonoBehaviour, IUnityAdsListener
{
    public LeftButton[] leftButton;
    public RightButton[] rightButton;

    public GameObject player;
    public GameObject playerPrefab;
    public GameObject[] casualPlayerIcon;
    public GameObject pauseButton;
    public GameObject playButton;
    public GameObject timedMode;
    public GameObject casualMode;
    public GameObject expertMode;
    public GameObject background;
    public GameObject plusScore;
    public GameObject scorePanel;
    public GameObject casualRewardedPanel;
    public GameObject[] handController;

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
    public AudioSource timer;
    public AudioSource levelUpSound;

    public int numberOfKnives = 75;
    public int gameModeVal;

    int fastWaveMultiplier = -1;
    int level = 0;
    int knife;
    int controller;

    public bool gameOver = false;

    bool testMode = false;
    bool actionExecuted = false;
    bool soundPlayed = false;

    float fastWaveTime = 15f;
    float secondCount = 60f;
    float timeElapsed = 0f;
    float playerSpeedMultiplier = 3f;
    float knifeSpeedMultiplier = 10f;
    float backToNormalTime;
    float prev = 1f;
    float next = 0.93f;

    string gameId = "3550261";
    string myPlacementId = "rewardedVideo";

    Vector3 accel;

    void Start()
    {
        Time.timeScale = 1;
        accel = Input.acceleration;
        Advertisement.Initialize(gameId, testMode);
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

        controller = PlayerPrefs.GetInt("controller", 1);
        if(controller == 1)
        {
            handController[0].SetActive(false);
            handController[1].SetActive(false);
        }

    }

    void setPlayer(int knife)
    {
        SpriteRenderer spriteRenderer = player.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[knife];
        spriteRenderer = playerPrefab.GetComponent<SpriteRenderer>();
        spriteRenderer.sprite = sprites[knife];

        Image image = casualPlayerIcon[0].GetComponent<Image>();
        image.sprite = sprites[knife];
        image = casualPlayerIcon[1].GetComponent<Image>();
        image.sprite = sprites[knife];
    }

    void FixedUpdate()
    {
        //Game over
        if (gameOver)
            return;

        //Hands Free
        if (controller == 1)
        {
            var dir = new Vector3(Input.acceleration.x, 0, 0);
            if (dir.sqrMagnitude > 1) dir.Normalize();
            player.transform.Translate(dir * knifeSpeedMultiplier * Time.deltaTime);
        }
       
        //Tap Control
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
            if((int)secondCount % 10 == 0 && actionExecuted == false)
            {
                levelUp("timed");
                actionExecuted = true;
            }

            if((int)secondCount == 10 && !soundPlayed)
            {
                timer.Play();
                soundPlayed = true;
            }

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

        //Expert Mode
        if (gameModeVal == 3)
        {
            if ((int)timeElapsed % 10 == 0 && actionExecuted == false)
            {
                levelUp("expert");
                actionExecuted = true;
            }
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

    private void levelUp(string mode)
    {
        levelUpSound.Play();
        if (mode == "casual")
        {
            level += 1;
            if (level < 10)
            {
                knifeSpeedMultiplier = 10f + level * .75f;
                playerSpeedMultiplier = 3f + level * .15f;
                fruitSpawner.speedMultiplier = .7f + level * .1f;
                fruitSpawner.spawnDelay = .85f - level * .05f;
            }
            else
            {
                knifeSpeedMultiplier = 17.5f;
                playerSpeedMultiplier = 4f;
                fruitSpawner.speedMultiplier = .8f;
                fruitSpawner.spawnDelay = .5f;
            }

            fruitSpawner.callInvoke();
            StartCoroutine(fade(.65f, 1f, 3f));
        }
        else if(mode == "timed")
        {
            level += 1;
            knifeSpeedMultiplier = 10f + level * .5f;
            playerSpeedMultiplier = 3f + level * .075f;
            fruitSpawner.speedMultiplier = .7f + level * .05f;
            fruitSpawner.spawnDelay = .85f - level * .05f;
            fruitSpawner.callInvoke();
            StartCoroutine(fade(prev, next, 2f));
            prev = next;
            next = next - 0.07f;
        }
        else
        {
            level += 1;
            if (level < 20)
            {
                knifeSpeedMultiplier = 10f + level * .4f;
                playerSpeedMultiplier = 3f + level * .075f;
                fruitSpawner.speedMultiplier = .7f + level * .05f;
                fruitSpawner.spawnDelay = .85f - level * .025f;
                prev = next;
                next = next - 0.035f;
                StartCoroutine(fade(prev, next, 2f));
            }
            else
            {
                knifeSpeedMultiplier = 17.5f;
                playerSpeedMultiplier = 4f;
                fruitSpawner.speedMultiplier = .8f;
                fruitSpawner.spawnDelay = .5f;
            }
            fruitSpawner.callInvoke();            
        }
    }

    private void fastWave()
    {
        fastWaveMultiplier += 1;
        backToNormalTime = 5 + Random.Range(1.15f, 1.25f) * fastWaveMultiplier;
        fastWaveTime = fastWaveTime + Random.Range(15, 20) + 1.25f * fastWaveMultiplier;

        knifeSpeedMultiplier = 15f;
        playerSpeedMultiplier = 4f;
        fruitSpawner.speedMultiplier = .8f;
        fruitSpawner.spawnDelay = .5f;

        fruitSpawner.callInvoke();
        //fruitSpawner.spawnKnife();

        StartCoroutine(fade(1f, .65f, 1.5f));
        StartCoroutine(backToNormal(backToNormalTime));
    }

    IEnumerator backToNormal(float time)
    {
        yield return new WaitForSeconds(time);
        levelUp("casual");
    }

    IEnumerator fade(float from, float to, float time)
    {
        for (float t = 0.0f; t < 1.0f; t += Time.deltaTime / time)
        {
            Color newColor = new Color(1, Mathf.Lerp(from, to, t), Mathf.Lerp(from, to, t), 1);
            background.GetComponent<Image>().color = newColor;
            yield return null;
        }
        actionExecuted = false;
    }

    private void gameOverTimedMode()
    {
        Time.timeScale = 0;
        gameOver = true;
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
        gameOver = true;
        gameScore.text = "Score: " + ScoreKeeper.score.ToString();
        int highScore = PlayerPrefs.GetInt("casualScore", -1);
        if (ScoreKeeper.score > highScore)
        {
            PlayerPrefs.SetInt("casualScore", ScoreKeeper.score);
            bestScore.text = "Best: " + ScoreKeeper.score.ToString() + " *";
        }
        else
            bestScore.text = "Best: " + highScore.ToString();
        
        pauseButton.SetActive(false);
        if (Advertisement.IsReady(myPlacementId))
            casualRewardedPanel.SetActive(true);
        else
            skip();
    }

    public void gameOverExpertMode()
    {
        Time.timeScale = 0;
        gameOver = true;
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

    public void watchAd()
    {
        Advertisement.AddListener(this);
        OnUnityAdsReady(myPlacementId);
    }

    public void skip()
    {
        Advertisement.RemoveListener(this);
        casualRewardedPanel.SetActive(false);
        scorePanel.SetActive(true);
        casualMode.SetActive(false);
    }

    private void resumeGame()
    {
        fruitSpawner.callInvoke();
        Advertisement.RemoveListener(this);
        numberOfKnives += 25;
        gameOver = false;
        casualRewardedPanel.SetActive(false);
        scorePanel.SetActive(false);
        casualMode.SetActive(true);
        pauseButton.SetActive(true);
        Time.timeScale = 1;  
    }

    private void playGameSound()
    {
        gameClick.Play();
    }

    /* Rewarded Ads */

    public void OnUnityAdsReady(string placementId)
    {
        // If the ready Placement is rewarded, show the ad:
        if (placementId == myPlacementId)
        {
            Advertisement.Show(myPlacementId);
        }
    }

    public void OnUnityAdsDidFinish(string placementId, ShowResult showResult)
    {
        if (showResult == ShowResult.Finished)
        {
            resumeGame();
        }
        else if (showResult == ShowResult.Skipped)
        {
            skip();
        }
        else if (showResult == ShowResult.Failed)
        {
            skip();
        }
    }

    public void OnUnityAdsDidError(string message)
    {
        skip();
    }

    public void OnUnityAdsDidStart(string placementId)
    {
        // Optional actions to take when the end-users triggers an ad.
    }
}
