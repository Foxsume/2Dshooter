using UnityEngine;
using Firebase.Database;
using System;
using System.Linq;
using System.Threading.Tasks;
using System.Collections.Generic;
using Newtonsoft.Json;

public class Tierlists : MonoBehaviour
{
    public int maxLeaderboardScores = 100;

    private void Awake()
    {
        // Make the script live accross scenes
        DontDestroyOnLoad(this);
    }

    private void Start()
    {
        // Get the root reference location of the database
        DatabaseReference reference = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public async void AddScoreToLeaders(string email, long score, DatabaseReference leaderBoardRef)
    {
        Task<DataSnapshot> updateLeaderboards = leaderBoardRef.RunTransaction(mutableData =>
        {
            List<object> leaders = mutableData.Value as List<object>;

            if (leaders == null)
            {
                leaders = new List<object>();
            }
            // Leaderboard full (maxLeaderboardScores reached), check if new entry can be added:
            else if (mutableData.ChildrenCount >= maxLeaderboardScores)
            {
                // type-cast hell to get the object with lowest score and the actual lowest score.

                // LINQ makes finding the object with min score a bit easier: order by scores and return first
                object entryWithLowestScore = leaders.OrderBy(entry => (long)((Dictionary<string, object>)entry)["score"]).FirstOrDefault();

                // Getting the actual score integer value requires obfuscating type-casts:
                long minScore = (long)((Dictionary<string, object>)entryWithLowestScore)["score"];

                // Either abort transaction without changing anything
                // or remove the object with lowest score from the 'leaders' list
                if (minScore > score)
                {
                    Debug.Log("The new score is lower than the existing scores, abort.");
                    return TransactionResult.Abort();
                }
                else
                {
                    Debug.Log("Remove the lowest score.");
                    leaders.Remove(entryWithLowestScore);
                }
            }

            // Now we can add the new high score, because:
            // 1. There wasn't enough scores yet
            // 2. New score wasn't good enough, and we aborted, and never reached this point
            // 3. We removed the lowest score
            Dictionary<string, object> newScoreMap = new Dictionary<string, object>();

            newScoreMap["score"] = score;
            newScoreMap["email"] = email;
            leaders.Add(newScoreMap);
            mutableData.Value = leaders;

            return TransactionResult.Success(mutableData);
        });

        try
        {
            await updateLeaderboards;
        }
        catch (AggregateException ae)
        {
            foreach (Exception ex in ae.InnerExceptions)
            {
                Debug.Log(ex.ToString());
            }
        }

        DataSnapshot leaderboardUpdateResult = updateLeaderboards.Result;
        string resultingJSON = leaderboardUpdateResult.GetRawJsonValue();

        // Format JSON properly so that Newtonsoft JSON can parse it:
        resultingJSON = "{\"leaderboards\":" + resultingJSON + "}";

        Debug.Log("JSON: " + resultingJSON);

        // NOTICE: Did not work with Unity's JsonUtility, that's why Newtonsoft JSON:
        Leaderboard leaderboard = JsonConvert.DeserializeObject<Leaderboard>(resultingJSON);

        // Local ordering and printing the results:
        leaderboard.leaderboards = leaderboard.leaderboards.OrderByDescending(entry => entry.score).ToList();
        leaderboard.leaderboards.ForEach(entry => Debug.Log("Email: " + entry.email + " | Score: " + entry.score));
    }
}

public class Leaderboard
{
    public List<LeaderboardEntry> leaderboards = new List<LeaderboardEntry>();
}

[Serializable]
public class LeaderboardEntry
{
    public string email;
    public int score;
}
