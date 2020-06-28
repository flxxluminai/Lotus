using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    static int failedCounter;
    Authorization auth;

    public void Start()
    {
        failedCounter = 0;
        auth = new Authorization();
    }

    public void verify()
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
                SceneManager.LoadScene("Home", LoadSceneMode.Single);
                break;
        }
    }

    public void accountCreation()
    {
        SceneManager.LoadScene("Account_Creation", LoadSceneMode.Single);
    }
}
