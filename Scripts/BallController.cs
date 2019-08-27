using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BallController : MonoBehaviour
{
    public float lerpTime = 1.0f;

    private bool isLerping = false;
    private float lerpStartTime = 0.0f;
    private float pausedTime = 0.0f;
    private Vector3 pointA = new Vector3(0.0f, 4.28f, 0.0f);
    private Vector3 pointB = new Vector3(0.0f, -4.28f, 0.0f);

    // Start is called before the first frame update
    void Start()
    {
        InitiateLerp();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void FixedUpdate()
    {
        if (isLerping && FindObjectOfType<GameController>().GameRunning) // Percent based Lerp that only operates while the game is running.
        {
            if(pausedTime != 0.0f) // Update lerp start time to handle time passed during pause.
            {
                lerpStartTime += pausedTime;
                pausedTime = 0.0f;
            }
            float timeSinceStarted = Time.time - lerpStartTime;
            float percentComplete = timeSinceStarted / lerpTime;

            transform.position = Vector3.Lerp(pointA, pointB, percentComplete);

            if(percentComplete >= 1.0f) {
                GetComponent<AudioSource>().PlayOneShot(GetComponent<AudioSource>().clip);
                isLerping = false;
                FlipEndpoints();
                InitiateLerp(); }
        }
        else if (isLerping && !FindObjectOfType<GameController>().GameRunning)
        {
            pausedTime += Time.deltaTime;
        }
    }

    /// <summary>
    /// Set initial values of Lerp and flip the endpoint to create a ping-pong effect.
    /// </summary>
    private void InitiateLerp()
    {
        isLerping = true;
        lerpStartTime = Time.time;
    }

    private void FlipEndpoints()
    {
        Vector3 temp = pointB;
        pointB = pointA;
        pointA = temp;
    }
}
