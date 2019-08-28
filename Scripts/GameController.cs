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
    public CanvasGroup menuGroup, instructionGroup;
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
        GameRunning = false;
        SetBorder();
        menuGroup.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Start";
#if UNITY_WEBGL
        menuGroup.transform.GetChild(2).GetChild(0).GetComponent<Text>().text = "Restart";
        menuGroup.transform.GetChild(2).gameObject.SetActive(false);
#endif
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
        if (menuGroup.transform.GetChild(0).GetChild(0).GetComponent<Text>().text.Equals("Start"))
        {
            menuGroup.transform.GetChild(0).GetChild(0).GetComponent<Text>().text = "Resume";
        }
        if (!GameRunning)
        {
            GameRunning = true;
            menuGroup.alpha = 0;
            menuGroup.blocksRaycasts = false;
            menuGroup.interactable = false;
        }
        else
        {
            GameRunning = false;
            menuGroup.alpha = 1;
            menuGroup.blocksRaycasts = true;
            menuGroup.interactable = true;
        }
    }

    public void RestartQuit()
    {
        GameObject.Find("CaptureButton").GetComponent<Button>().interactable = false;
        menuGroup.transform.GetChild(0).gameObject.SetActive(false);

#if UNITY_WEBGL
        menuGroup.transform.GetChild(2).gameObject.SetActive(true);
#endif
        menuGroup.alpha = 1;
        menuGroup.blocksRaycasts = true;
        menuGroup.interactable = true;
    }

    public void ToggleInstructionGroup(bool value)
    {
        if (value)
        {
            instructionGroup.alpha = 1;
            instructionGroup.blocksRaycasts = true;
            instructionGroup.interactable = true;
        }
        else
        {
            instructionGroup.alpha = 0;
            instructionGroup.blocksRaycasts = false;
            instructionGroup.interactable = false;
        }
    }

    public void GameOver()
    {
        gameRunning = false;
        RestartQuit();
    }

    public void Quit()
    {
#if UNITY_STANDALONE_WIN || UNITY_EDITOR
        Application.Quit();
        return;
#endif
#if UNITY_WEBGL
        UnityEngine.SceneManagement.SceneManager.LoadScene(0);
#endif
    }
}
