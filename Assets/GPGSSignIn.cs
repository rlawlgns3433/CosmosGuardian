using GooglePlayGames;
using GooglePlayGames.BasicApi;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GPGSSignIn : MonoBehaviour
{
    void Awake()
    {
        PlayGamesPlatform.DebugLogEnabled = true;
        PlayGamesPlatform.Activate();
        SignIn();
    }
    public void SignIn()
    {
        PlayGamesPlatform.Instance.Authenticate(OnAuthentication);
    }

    void OnAuthentication(SignInStatus result)
    {
        if (result == SignInStatus.Success)
        {
            //log.text = "Signed in successfully.";
            // Signed in successfully, we can now proceed with saving or loading
        }
        else
        {
            //log.text = "Failed to sign in.";
        }
    }

}
