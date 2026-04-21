using System;
using UnityEngine;
using Firebase.Auth;

// Handles user authentication state tracking using firebase
// Broadcasts an event whenever the authentication state changes
public class UserInfo : MonoBehaviour
{
    // Event triggered whenever the user's authentication state changes
    // Sends the current FirebaseUser (null if signed out)
    public static event Action<FirebaseUser> OnUserAuthStateChanged;

    private FirebaseAuth auth;

    // Sets up Firebase auth and subscribes to state change events
    private void Awake()
    {
        // Make the script live accross scenes
        DontDestroyOnLoad(this);

        // Get the default firebase authentication instance
        auth = FirebaseAuth.DefaultInstance;

        // subscribe to authentication state changes
        auth.StateChanged += Auth_StateChanged;

        // Manually trigger once to initialize current auth state
        Auth_StateChanged(this, null);
    }

    private void Auth_StateChanged(object sender, EventArgs eventArgs)
    {
        // Emit the current user (null = not signed in)
        OnUserAuthStateChanged?.Invoke(auth.CurrentUser);
    }

    private void OnDestroy()
    {
        // Unsubscribe from authentication state changes
        if (auth != null)
            auth.StateChanged -= Auth_StateChanged;
    }
}
