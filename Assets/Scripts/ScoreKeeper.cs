using UnityEngine;
using UnityEngine.UI;

public class ScoreKeeper : MonoBehaviour
{
    public Text scoreTextTimedMode;
    public Text scoreTextCasualMode;
    public Text scoreTextExpertMode;

    public static int score = 0;
 
    private void Start()
    {
        score = 0;
    }

    void Update()
    {
        scoreTextTimedMode.text = score.ToString();
        scoreTextCasualMode.text = score.ToString();
        scoreTextExpertMode.text = score.ToString();
    }
}
