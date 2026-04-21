using System;
using System.Threading.Tasks;
using UnityEngine;
using Firebase.Database;
using Firebase.Auth;
using System.Threading;

public class UserAccountDetails : MonoBehaviour
{
    private DatabaseReference dbRootReference;
    private Firebase.Auth.FirebaseAuth auth;
    private GameTimer timer;

    private void Awake()
    {
        // Make the script live accross scenes
        DontDestroyOnLoad(this);

        timer = FindObjectOfType<GameTimer>();

        // Get the root reference location of the database.
        dbRootReference = FirebaseDatabase.DefaultInstance.RootReference;

        // We can get current user from FirebaseAuth instance CurrentUser
        auth = FirebaseAuth.DefaultInstance;
    }

    private void OnEnable()
    {
        // Subscribe to global auth state event
        UserInfo.OnUserAuthStateChanged += UserInfo_OnUserAuthStateChanged;
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid duplicate calls
        UserInfo.OnUserAuthStateChanged -= UserInfo_OnUserAuthStateChanged;
    }

    /// <summary>
    /// Handle user signing in or out event
    /// </summary>
    /// <param name="isSignedIn">Signed in = true or out = false</param>
    private void UserInfo_OnUserAuthStateChanged(FirebaseUser user)
    {
        if (user != null)
        {
            string email = user.Email;
            float playTime = timer.TotalPlayTime;
            ReadWriteUserDetails(email, playTime, false, true);
        }
        else
            Debug.Log("User signed out, no data access");
    }

    private async void ReadWriteUserDetails(string username, float totalPlayTime, bool updateUsername, bool updateTotalPlayTime)
    {
        UserDetails userDetails = null;

        Task<DataSnapshot> task = FirebaseDatabase.DefaultInstance
            .GetReference("users/" + auth.CurrentUser.UserId + "/").OrderByChild(auth.CurrentUser.UserId)
            .GetValueAsync();

        await task;

        try
        {
            // Firebase returns DataSnapshot...
            DataSnapshot snapshot = task.Result;

            // And because we were basically checking if 'users/some-long-user-id/' already is
            // in database, we can implement logic to add user if she doesn't exist...
            if (snapshot.Exists)
            {
                if (updateUsername || updateTotalPlayTime)
                {
                    if (updateUsername)
                        await dbRootReference.Child("users").Child(auth.CurrentUser.UserId).Child("Username").SetValueAsync(username);

                    if (updateTotalPlayTime)
                        await dbRootReference.Child("users").Child(auth.CurrentUser.UserId).Child("TotalPlayTime").SetValueAsync(totalPlayTime);
                }
                else
                {
                    // Serialize to class(es) with matching semantics:
                    userDetails = JsonUtility.FromJson<UserDetails>(snapshot.GetRawJsonValue());
                }
            }
            else // add new user if existing wasn't found
            {
                userDetails = new UserDetails(username, totalPlayTime);
                string json = JsonUtility.ToJson(userDetails);

                await dbRootReference.Child("users").Child(auth.CurrentUser.UserId).SetRawJsonValueAsync(json);
            }
        }
        catch (AggregateException ae)
        {
            foreach (var ex in ae.InnerExceptions)
                Debug.LogErrorFormat("{0}: {1}", ex.GetType().Name, ex.Message);
        }

        // If there was no exceptions 'userDetails' is either just added new user or details of existing user:
        if (userDetails != null)
        {
            Debug.Log("Username: " + userDetails.Username + " | Playtime: " + userDetails.TotalPlayTime);
        }
    }

    /// <summary>
    /// Class for saving/retrieving user details from Firebase
    /// </summary>
    public class UserDetails
    {
        public string Username;
        public float TotalPlayTime;

        public UserDetails(string username, float totalPlayTime)
        {
            Username = username;
            TotalPlayTime = totalPlayTime;
        }
    }
}