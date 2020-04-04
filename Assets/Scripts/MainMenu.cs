using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	public Slider slider;
	public GameObject sliderGameObject;

    void Start()
    {
		Application.targetFrameRate = 60;
    }

    public void loadScene()
	{
		sliderGameObject.SetActive(true);
		StartCoroutine(loadAsync());
	}

	IEnumerator loadAsync()
	{
		AsyncOperation operation = SceneManager.LoadSceneAsync(1);
		while (!operation.isDone)
		{
			float progress = Mathf.Clamp01(operation.progress);
			slider.value = progress;
			yield return null;
		}
	}

}
