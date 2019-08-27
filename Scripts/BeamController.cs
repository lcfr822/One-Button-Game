using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class BeamController : MonoBehaviour
{
    public Bounds captureBounds;
    public SpriteRenderer beam, lhsBeamHolder, rhsBeamHolder;
    public Image[] lives = new Image[3];

    private int lifeCount = 2;
    private int consecutiveScores = 0;
    
    // Start is called before the first frame update
    void Start()
    {
        StartCoroutine(GrowBeam(20.0f));
        var beamScaledSize = GlobalFunctions.ScaleSpriteToScreensize(beam);
        beam.transform.localScale = new Vector3(beamScaledSize.x * 0.96f, beamScaledSize.y * 0.25f, 1.0f);

        captureBounds = new Bounds(Vector3.zero, beam.bounds.size);
    }

    // Update is called once per frame
    void Update()
    {

    }

    /// <summary>
    /// Increase the vertical size of the Light Beam and the size and positions Emitters.
    /// </summary>
    /// <param name="value">The value to grow the Beam size by.</param>
    /// <returns>Null.</returns>
    public IEnumerator GrowBeam(float value)
    {
        if (value <= 0) { yield break; }

        beam.size = new Vector2(beam.size.x, beam.size.y + value);
        Vector3 beamTopPosition = new Vector3(0.0f, beam.bounds.extents.y, 0.0f) + beam.transform.position;
        beam.transform.GetChild(0).position = new Vector3(beam.transform.GetChild(0).position.x, beamTopPosition.y);
        beam.transform.GetChild(1).position = new Vector2(beam.transform.GetChild(1).position.x, -beamTopPosition.y);
        beam.transform.GetChild(2).position = new Vector2(beam.transform.GetChild(2).position.x, beamTopPosition.y);
        beam.transform.GetChild(3).position = new Vector2(beam.transform.GetChild(3).position.x, -beamTopPosition.y);
        Vector3 rhsBeamHolderTopPosition = new Vector3(0.0f, rhsBeamHolder.bounds.extents.y, 0.0f) + rhsBeamHolder.transform.position;

        var endTime = Time.time + 3.0f;
        var timer = Time.time;
        while (timer < endTime && rhsBeamHolderTopPosition.y < beamTopPosition.y)
        {
            rhsBeamHolder.size += new Vector2(0, 1.0f);
            lhsBeamHolder.size += new Vector2(0, 1.0f);
            captureBounds = new Bounds(Vector3.zero, beam.bounds.size);
            rhsBeamHolderTopPosition = new Vector3(0.0f, rhsBeamHolder.bounds.extents.y, 0.0f) + rhsBeamHolder.transform.position;
            timer += Time.deltaTime;
        }

        if (lifeCount > 0 && FindObjectOfType<GameController>().GameRunning)
        {
            var color = lives[lifeCount].color;
            color.a = 0;
            lives[lifeCount].color = color;
            lifeCount--;
            consecutiveScores = 0;
        }
        else if (lifeCount == 0 && FindObjectOfType<GameController>().GameRunning)
        {
            var color = lives[lifeCount].color;
            color.a = 0;
            lives[lifeCount].color = color;
            FindObjectOfType<GameController>().GameOver();
        }
    }

    /// <summary>
    /// Decrease the vertical size of the Light Beam and the size and positions Emitters.
    /// </summary>
    /// <param name="value">The value to shrink the Beam size by.</param>
    /// <returns>Null.</returns>
    public IEnumerator ShrinkBeam(float value)
    {
        if(value <= 0) { yield break; }

        beam.size = new Vector2(beam.size.x, beam.size.y - value);
        Vector3 beamTopPosition = new Vector3(0.0f, beam.bounds.extents.y, 0.0f) + beam.transform.position;
        beam.transform.GetChild(0).position = new Vector3(beam.transform.GetChild(0).position.x, beamTopPosition.y);
        beam.transform.GetChild(1).position = new Vector2(beam.transform.GetChild(1).position.x, -beamTopPosition.y);
        beam.transform.GetChild(2).position = new Vector2(beam.transform.GetChild(2).position.x, beamTopPosition.y);
        beam.transform.GetChild(3).position = new Vector2(beam.transform.GetChild(3).position.x, -beamTopPosition.y);
        Vector3 rhsBeamHolderTopPosition = new Vector3(0.0f, rhsBeamHolder.bounds.extents.y, 0.0f) + rhsBeamHolder.transform.position;

        var endTime = Time.time + 3.0f;
        var timer = Time.time;
        while (timer < endTime && rhsBeamHolderTopPosition.y > beamTopPosition.y)
        {
            rhsBeamHolder.size -= new Vector2(0.0f, 1.0f);
            lhsBeamHolder.size -= new Vector2(0.0f, 1.0f);
            captureBounds = new Bounds(Vector3.zero, beam.bounds.size);
            rhsBeamHolderTopPosition = new Vector3(0.0f, rhsBeamHolder.bounds.extents.y, 0.0f) + rhsBeamHolder.transform.position;
            timer += Time.deltaTime;
        }

        consecutiveScores++;
        if (consecutiveScores == 5)
        {
            consecutiveScores = 0;
            if (lifeCount < 2) { lifeCount++; }
        }
    }
}
