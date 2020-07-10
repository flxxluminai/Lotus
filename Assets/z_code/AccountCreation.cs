using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Auth;
using UnityEngine.SceneManagement;
using System.Text.RegularExpressions;

public class AccountCreation : MonoBehaviour
{
    private static int lastLengthDOB = 0;
    private static int lastLengthPhone = 0;
    private DatabaseReference database;
    private FirebaseUser user;

    public void Start()
    {
        //lastLengthDOB = 0;
        //lastLengthPhone = 0;
        database = FirebaseDatabase.DefaultInstance.RootReference;
    }

    public void cancelCreation()
    {
        SceneManager.LoadScene("Login", LoadSceneMode.Single);
    }

    public void formatDOB()
    {
        Text DOB = GameObject.Find("DOB text").GetComponent<Text>();
        string t = DOB.text.ToString();
        if (lastLengthDOB < t.Length)
        {
            if (t.Length == 2)
                DOB.text = t + '/';
            else if (t.Length == 5)
                DOB.text = t + '/';
        }
        else
        {
            if (t.Length == 3)
                DOB.text = t.Substring(0, 2);
            else if (t.Length == 6)
                DOB.text = t.Substring(0, 5);
        }

        lastLengthDOB = t.Length;
    }

    public void validateDOB()
    {
        InputField DOB = GameObject.Find("date of birth").GetComponent<InputField>();
    }

    public void formatPhone(Text phone)
    {
        string text = phone.text.ToString();
        if (lastLengthPhone < text.Length)
        {
            if (text.Length == 3)
                phone.text = '(' + text + ')';
            else if (text.Length == 8)
                phone.text = text + '-';
        }
    }

    public void validatePassword()
    {
        InputField password = GameObject.Find("password").GetComponent<InputField>();
        InputField passwordConfirm = GameObject.Find("password confirmation").GetComponent<InputField>();
        Text passwordError = GameObject.Find("password error").GetComponent<Text>();

        if (password.text != passwordConfirm.text)
            passwordError.color = new Color(passwordError.color.r, passwordError.color.g, passwordError.color.b, 255);
        else
            passwordError.color = new Color(passwordError.color.r, passwordError.color.g, passwordError.color.b, 0);
    }

    public void sectionChoice(Text newText)
    {
        Text choice = GameObject.Find("choice").GetComponent<Text>();
        choice.text = newText.text;
    }

    public void createAccount()
    {
        Authorization auth = new Authorization();

        InputField name = GameObject.Find("name").GetComponent<InputField>();
        InputField DOB = GameObject.Find("date of birth").GetComponent<InputField>();
        InputField phone = GameObject.Find("phone number").GetComponent<InputField>();
        InputField email = GameObject.Find("email").GetComponent<InputField>();
        InputField password = GameObject.Find("password").GetComponent<InputField>();
        Text section = GameObject.Find("choice").GetComponent<Text>();

        Text error = GameObject.Find("error").GetComponent<Text>();
        if (!auth.createAccount(email.text, password.text))
        {
            error.text = "** Unable to create account **";
        }
        else
        {
            user = FirebaseAuth.DefaultInstance.CurrentUser;
            user.SendEmailVerificationAsync().ContinueWith(task => 
            { 
                if (task.IsFaulted || task.IsCanceled)
                {
                    error.text = "** Unable to send verification email. Please try again later **";
                }
                else
                {
                    string emailTemp = email.text.ToString();
                    emailTemp = emailTemp.Replace('.', ' ');
                    database.Child("users").Child(emailTemp).Child("name").SetValueAsync(name.text.ToString());
                    database.Child("users").Child(emailTemp).Child("DOB").SetValueAsync(DOB.text.ToString());
                    database.Child("users").Child(emailTemp).Child("phone").SetValueAsync(phone.text.ToString());
                    database.Child("users").Child(emailTemp).Child("section").SetValueAsync(section.text.ToString());
                    SceneManager.LoadScene("Login", LoadSceneMode.Single);
                }
            });

            
        }

        
        
    }
}
