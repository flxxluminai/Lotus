using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

using Firebase.Auth;

public class Login : MonoBehaviour
{
    static int failedCounter;
    Authorization auth;
    FirebaseUser user;

    public void Start()
    {
        Connection connection = new Connection();
        connection.checkConnected();
        StartCoroutine(connection.getData());

        failedCounter = 0;
        auth = new Authorization();
        user = FirebaseAuth.DefaultInstance.CurrentUser;
    }

    public void verify()
    {
        Save save = new Save();
        save.loadData();
        Connection connection = new Connection();
        if (!Settings.settings[4] && !connection.checkConnected())
        {
            Text error = GameObject.Find("Error").GetComponent<Text>();
            error.text = "** Offline sync is not enabled and there is not Internet connection **";
        }
        else if (user != null)
        {
            SceneManager.LoadScene("Navigation", LoadSceneMode.Single);
        }
        else 
        {
            InputField username = GameObject.Find("Username").GetComponent<InputField>();
            InputField password = GameObject.Find("Password").GetComponent<InputField>();
            Text error = GameObject.Find("Error").GetComponent<Text>();

            int valid = auth.authorizeSignIn(username.text, password.text);
            switch (valid)
            {
                case 0:
                    error.text = "** Email and/or password is invalid **";
                    failedCounter++;
                    break;
                case 1:
                    error.text = "** Please verify your email before signing in **";
                    break;
                default:
                    SceneManager.LoadScene("Navigation", LoadSceneMode.Single);
                    break;
            }
        }
    }

    public void resetPassword()
    {
        Connection connection = new Connection();
        if (connection.checkConnected())
            SceneManager.LoadScene("Password Reset Request", LoadSceneMode.Single);
        else
        {
            Text error = GameObject.Find("Error").GetComponent<Text>();
            error.text = "** Need Internet connection to reset password **";
        }
    }

    public void accountCreation()
    {
        Connection connection = new Connection();
        if (connection.checkConnected())
            SceneManager.LoadScene("Account_Creation", LoadSceneMode.Single);
        else
        {
            Text error = GameObject.Find("Error").GetComponent<Text>();
            error.text = "** Need Internet connection to create account **";
        }
    }
}
