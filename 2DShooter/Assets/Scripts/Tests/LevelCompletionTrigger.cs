using UnityEngine;
using Firebase.Auth;
using Firebase.Database;

public class LevelCompleteTrigger : MonoBehaviour
{
    public string playerTag = "Player";

    public GameTimer timer;
    public Tierlists tierlists;

    public string leaderboardPath = "leaderboards/global";

    private bool triggered = false;

    private void Awake()
    {
        timer = FindObjectOfType<GameTimer>();
        tierlists = FindObjectOfType<Tierlists>();
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        Debug.Log("Trigger touched by: " + other.name);

        if (triggered) return;

        if (other.CompareTag(playerTag))
        {
            triggered = true;

            CompleteLevel();
        }
    }

    private void CompleteLevel()
    {
        timer.StopLevelTimer();

        float completionTime = timer.LevelTime;

        // Convert time → score (faster = higher)
        long score = CalculateScore(completionTime);

        Debug.Log("Time: " + completionTime + " | Score: " + score);

        // Get current user
        var user = FirebaseAuth.DefaultInstance.CurrentUser;

        if (user != null)
        {
            DatabaseReference leaderboardRef =
                FirebaseDatabase.DefaultInstance.GetReference(leaderboardPath);

            tierlists.AddScoreToLeaders(user.Email, score, leaderboardRef);
        }
        else
        {
            Debug.LogWarning("No user signed in, score not saved.");
        }
    }

    private long CalculateScore(float time)
    {
        return (long)(10000 - time * 100);
    }
}
