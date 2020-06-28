using UnityEngine;
using Firebase.Auth;

public class Authorization : MonoBehaviour
{
    private static FirebaseAuth auth;
    public static FirebaseUser user;

    public void Start()
    {
        auth = FirebaseAuth.DefaultInstance;
        user = FirebaseAuth.DefaultInstance.CurrentUser;
    }

    public int authorizeSignIn(string email, string password)
    {
        if (user != null)
        {
            if (user.IsEmailVerified)
                return 2;
            else
                return 1;
        }
        else
        {
            int valid = 2;
            auth.SignInWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
            {
                if (task.IsCanceled || task.IsFaulted)
                {
                    valid = 0;
                    return;
                }

                user = task.Result;
            });
            return valid;
        }
    }

    public bool createAccount(string email, string password)
    {
        auth = FirebaseAuth.DefaultInstance;
        /*
        auth.CreateUserWithEmailAndPasswordAsync(email, password).ContinueWith(task =>
        {
            if (task.IsCanceled || task.IsFaulted)
            {
                failed = true;
                return;
            }

            user = task.Result;
        }).RunSynchronously();
        */

        user = auth.CreateUserWithEmailAndPasswordAsync(email, password).Result;

        return (user == null);
    }

    public void signOut()
    {
        auth.SignOut();
    }

}
