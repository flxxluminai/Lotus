using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class Login : MonoBehaviour
{
    private Connection connection = new Connection();

    public void verify()
    {
        InputField email = GameObject.Find("Username").GetComponent<InputField>();
        InputField password = GameObject.Find("Password").GetComponent<InputField>();

        this.connection = new Connection("login");
        StartCoroutine(connection.getData(string.Format("email={0}&password={1}",email.text,password.text), this.login));
    }

    private void login()
    {
        if (this.connection.json["approved"])
        {
            Save save = new Save();
            save.loadData();

            SceneManager.LoadScene("Navigation", LoadSceneMode.Single);
        }
        else
        {
            Text error = GameObject.Find("Error").GetComponent<Text>();
            error.text = "** Email and/or password is invalid **";
        }
    }

    public void resetPassword()
    {
        if (this.connection.checkConnected())
            SceneManager.LoadScene("Account_Creation", LoadSceneMode.Single);
        else
        {
            Text error = GameObject.Find("Error").GetComponent<Text>();
            error.text = "** Need Internet connection to reset password **";
        }
    }

    public void accountCreation()
    {
        if (this.connection.checkConnected())
            SceneManager.LoadScene("Account_Creation", LoadSceneMode.Single);
        else
        {
            Text error = GameObject.Find("Error").GetComponent<Text>();
            error.text = "** Need Internet connection to create account **";
        }
    }
}
