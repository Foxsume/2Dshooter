using UnityEngine;

public class GameTimer : MonoBehaviour
{
    // LEVEL TIME
    public float LevelTime { get; private set; }

    // TOTAL PLAY TIME
    public float TotalPlayTime { get; private set; }

    private bool levelRunning = false;
    private bool totalRunning = false;

    private void Awake()
    {
        // Make the script live accross scenes
        DontDestroyOnLoad(this);
    }

    private void Update()
    {
        if (levelRunning)
            LevelTime += Time.deltaTime;

        if (totalRunning)
            TotalPlayTime += Time.deltaTime;
    }

    // ---- LEVEL ----
    public void StartLevelTimer()
    {
        LevelTime = 0f;
        levelRunning = true;
    }

    public void StopLevelTimer()
    {
        levelRunning = false;
    }

    // ---- TOTAL ----
    public void StartSession()
    {
        totalRunning = true;
    }

    public void StopSession()
    {
        totalRunning = false;
    }
}
