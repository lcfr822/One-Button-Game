using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameController : MonoBehaviour
{
    private bool gameRunning = false;
    public bool GameRunning
    {
        get
        {
            return gameRunning;
        }
        set
        {
            gameRunning = value;
            if (gameRunning) { GameObject.Find("CaptureButton").GetComponent<Button>().interactable = true; }
            else
            {
                GameObject.Find("CaptureButton").GetComponent<Button>().interactable = false;
            }
        }
    }
    public CanvasGroup menuGroup, pauseGroup;
    public SpriteRenderer border;
    public Text scoreText;

    private int score = 0;
    public int Score
    {
        get { return score; }
        set
        {
            score = value;
            scoreText.text = "Score: " + score.ToString();
        }
    }

    // Start is called before the first frame update
    void Start()
    {
        SetBorder();
        pauseGroup.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Start";
    }

    // Update is called once per frame
    void Update()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        if (Input.GetKeyDown(KeyCode.Escape)) { PlayPause(); }
#endif
    }

    private void SetBorder()
    {
        float frustrumHeight = 2.0f * 10.0f * Mathf.Tan(Camera.main.fieldOfView * 0.5f * Mathf.Deg2Rad);
        float frustrumWidth = frustrumHeight * Camera.main.aspect;
        border.size = new Vector3(frustrumWidth - 1.0f, frustrumHeight - 1.0f, 0.0f);
    }

    public void AttemptCapture()
    {
        if (FindObjectOfType<BallController>().gameObject.transform.position.y > FindObjectOfType<BeamController>().captureBounds.min.y
            && FindObjectOfType<BallController>().gameObject.transform.position.y < FindObjectOfType<BeamController>().captureBounds.max.y)
        {
            Score += 3;
            StartCoroutine(FindObjectOfType<BeamController>().ShrinkBeam(2.5f));
        }
        else
        {
            Score -= 2;
            StartCoroutine(FindObjectOfType<BeamController>().GrowBeam(2.5f));
        }
    }

    public void PlayPause()
    {
        Debug.Log("Call: " + GameRunning);
        if (pauseGroup.transform.GetChild(0).GetChild(0).GetComponent<Text>().text.Equals("Start"))
        {
            pauseGroup.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Resume";
        }
        if (!GameRunning)
        {
            GameRunning = true;
            menuGroup.alpha = 0;
            menuGroup.blocksRaycasts = false;
            menuGroup.interactable = false;

            pauseGroup.alpha = 0;
            pauseGroup.blocksRaycasts = false;
            pauseGroup.interactable = false;
        }
        else
        {
            GameRunning = false;
            menuGroup.alpha = 1;
            menuGroup.blocksRaycasts = true;
            menuGroup.interactable = true;

            pauseGroup.alpha = 1;
            pauseGroup.blocksRaycasts = true;
            pauseGroup.interactable = true;
        }
        Debug.Log("Termination: " + GameRunning);
    }

    public void RestartQuit()
    {
        menuGroup.alpha = 1;
        menuGroup.blocksRaycasts = true;
        menuGroup.interactable = true;

        pauseGroup.alpha = 1;
        pauseGroup.blocksRaycasts = true;
        pauseGroup.interactable = true;

        pauseGroup.transform.GetChild(0).gameObject.SetActive(false);
    }

    public void GameOver()
    {
        Debug.LogWarning("Game Ending");
        gameRunning = false;
        RestartQuit();
    }

    public void Quit()
    {
#if UNITY_STANDALONE_WIN
        Application.Quit();
        return;
#endif
#if UNITY_WEBGL
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
#endif
    }
}
