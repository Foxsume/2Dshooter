using UnityEngine;
using UnityEngine.UI;
using System;
using System.Threading.Tasks;
using Firebase.Auth;
using TMPro;

public class UISignInUp : MonoBehaviour
{
    public GameObject SignInUpView;
    public GameObject UserView;

    public Button ButtonSignUp;
    public Button ButtonSignIn;
    public Button ButtonSignOut;
    
    /*** Sign up ***/
    public TMPro.TMP_InputField InputFieldEmail;
    public TMPro.TMP_InputField InputFieldPassword;

    // User id
    public TMP_Text TextUserEmail;

    // Reference to FirebaseAuth instance
    private Firebase.Auth.FirebaseAuth auth;
    private FirebaseUser currentUser;

    public void Awake()
    {
        // Make the script live accross scenes
        DontDestroyOnLoad(this);

        // Get Firebase instance
        auth = FirebaseAuth.DefaultInstance;

        // Handle logging in
        ButtonSignIn.onClick.AddListener(() => SignIn(InputFieldEmail.text, InputFieldPassword.text));

        // Handle new account button click
        ButtonSignUp.onClick.AddListener(() => SignUp(InputFieldEmail.text, InputFieldPassword.text));

        // Handle signing out
        ButtonSignOut.onClick.AddListener(SignOut);
    }

    private void OnEnable()
    {
        // Subscribe to global auth state event
        UserInfo.OnUserAuthStateChanged += HandleAuthStateChanged;
    }

    private void OnDisable()
    {
        // Unsubscribe to avoid duplicate calls
        UserInfo.OnUserAuthStateChanged -= HandleAuthStateChanged;
    }

    // Called whenever login state changes anywhere in the app
    private void HandleAuthStateChanged(FirebaseUser user)
    {
        if (user != null)
        {
            // User is signed in → show user UI
            ShowUserView(user.Email);
        }
        else
        {
            // User is signed out → show login UI
            ShowSignInView();
        }
    }

    // ---------------- UI SWITCHING ----------------
    private void ShowSignInView()
    {
        SignInUpView.SetActive(true);
        UserView.SetActive(false);
    }

    private void ShowUserView(string email)
    {
        SignInUpView.SetActive(false);
        UserView.SetActive(true);

        if (TextUserEmail != null)
            TextUserEmail.text = "Signed in as: " + email;
    }

    // ---------------- AUTH ACTIONS ----------------

    // SIGN IN existing user
    private async void SignIn(string email, string password)
    {
        try
        {
            var task = auth.SignInWithEmailAndPasswordAsync(email, password);
            await task;

            Debug.Log("SIGNED IN: " + task.Result.User.Email);
        }
        catch (Exception ex)
        {
            Debug.LogError("SIGN IN FAILED: " + ex.Message);
        }
    }

    // SIGN UP new user (also logs them in automatically)
    private async void SignUp(string email, string password)
    {
        try
        {
            var task = auth.CreateUserWithEmailAndPasswordAsync(email, password);
            await task;

            Debug.Log("ACCOUNT CREATED: " + task.Result.User.Email);
        }
        catch (Exception ex)
        {
            Debug.LogError("SIGN UP FAILED: " + ex.Message);
        }
    }

    // SIGN OUT current user
    private void SignOut()
    {
        auth.SignOut();

        Debug.Log("SIGNED OUT");
    }
}
