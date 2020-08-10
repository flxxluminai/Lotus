using UnityEngine;
using UnityEngine.UI;
using System;
using Firebase.Database;
using Firebase.Auth;
using UnityEngine.SceneManagement;

public class AccountCreation : MonoBehaviour
{
    private Text error;
    private string[] errorParts = { "", "", ""};
    private bool errorFlag = true;

    private InputField DOB;
    private int lastLengthDOB;

    private InputField phone;
    private int lastLengthPhone;

    private FirebaseUser user;
    private DatabaseReference database;

    public void Start()
    { 
        database = FirebaseDatabase.DefaultInstance.RootReference;

        error = GameObject.Find("error").GetComponent<Text>();

        DOB = GameObject.Find("date of birth").GetComponent<InputField>();
        DOB.onValueChanged.AddListener(delegate { formatDOB(); });
        lastLengthDOB = 0;

        phone = GameObject.Find("phone number").GetComponent<InputField>();
        phone.onValueChanged.AddListener(delegate { formatPhone(); });
        lastLengthPhone = 0;

    }

    public void cancelCreation()
    {
        SceneManager.LoadScene("Login", LoadSceneMode.Single);
    }

    public void formatDOB()
    {
        if (lastLengthDOB < DOB.text.Length)
        {
            switch (DOB.text.Length)
            {
                case 2:
                    DOB.text += "/";
                    break;
                case 3:
                    if (DOB.text[2] != '/')
                        DOB.text = DOB.text.Insert(2, "/");
                    break;
                case 5:
                    DOB.text += "/";
                    break;
                case 6:
                    if (DOB.text[5] != '/')
                        DOB.text = DOB.text.Insert(5, "/");
                    break;
                default:
                    break;
            }
        }
        DOB.caretPosition = DOB.text.Length;
        lastLengthDOB = DOB.text.Length;
    }

    public void validateDOB()
    {
        int[] daysAmount = {31, 28, 31, 30, 31, 30, 31, 31, 30, 31, 30, 31};
        string[] parts = DOB.text.Split('/');
        if (parts.Length != 3)
        {
            setError(0, "Invalid date of birth");
        }
        else
        {
            int year = 0;
            int.TryParse(parts[2], out year);
            if (year < 1900 || year >= DateTime.UtcNow.Year)
            {
                setError(0, "Invalid date of birth");
                return;
            }

            if (year % 4 == 0)
                daysAmount[1] = 29;

            int month = 0;
            int.TryParse(parts[0], out month);
            if (month <= 0 || month > 12)
            {
                setError(0, "Invalid date of birth");
                return;
            }

            int day = 0;
            int.TryParse(parts[1], out day);
            if (day <= 0 || day > daysAmount[month - 1])
            {
                setError(0, "Invalid date of birth");
                return;
            }

            setError(0, "");
        }
    }

    public void formatPhone()
    {
        if (lastLengthPhone < phone.text.Length)
        {
            switch (phone.text.Length)
            {
                case 3: 
                    phone.text += '-';
                    break;
                case 4:
                    if (phone.text[3] != '-')
                        phone.text = phone.text.Insert(3, "-");
                    break;
                case 7:
                    phone.text += '-';
                    break;
                case 8:
                    if (phone.text[7] != '-')
                        phone.text = phone.text.Insert(7, "-");
                    break;
            }
        }
        phone.caretPosition = phone.text.Length;
        lastLengthPhone = phone.text.Length;
    }

    public void validatePhone()
    {
        if (phone.text.Length != 12)
            setError(1, "Invalid phone number");
        else
            setError(1, "");
    }

    public void validatePassword()
    {
        InputField password = GameObject.Find("password").GetComponent<InputField>();
        InputField passwordConfirm = GameObject.Find("password confirmation").GetComponent<InputField>();

        if (password.text != passwordConfirm.text)
            setError(2, "Passwords do not match");
        else
            setError(2, "");
    }

    private void setError(int errorNum, string message)
    {
        errorParts[errorNum] = message;
        error.text = "";
        for (int i = 0; i < errorParts.Length; i++)
        {
            if (errorParts[i].Length > 0)
                error.text += "**" + errorParts[i] + "\n";
        }

        if (error.text == "")
            errorFlag = false;
        else
            errorFlag = true;
    }

    public void sectionChoice(Text newText)
    {
        Text choice = GameObject.Find("choice").GetComponent<Text>();
        choice.text = newText.text;
    }

    public void createAccount()
    {
        if (!errorFlag)
        {
            Authorization auth = new Authorization();

            InputField firstName = GameObject.Find("firstName").GetComponent<InputField>();
            InputField middleName = GameObject.Find("middleName").GetComponent<InputField>();
            InputField lastName = GameObject.Find("lastName").GetComponent<InputField>();
            InputField DOB = GameObject.Find("date of birth").GetComponent<InputField>();

            InputField phone = GameObject.Find("phone number").GetComponent<InputField>();
            InputField email = GameObject.Find("email").GetComponent<InputField>();

            InputField password = GameObject.Find("password").GetComponent<InputField>();
            Text section = GameObject.Find("choice").GetComponent<Text>();

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
                        string id = user.UserId;
                        database = database.Child("users").Child(id);
                        database.Child("firstName").SetValueAsync(firstName.text.ToString());
                        database.Child("middleName").SetValueAsync(middleName.text.ToString());
                        database.Child("lastName").SetValueAsync(lastName.text.ToString());
                        database.Child("DOB").SetValueAsync(DOB.text.ToString());
                        database.Child("phone").SetValueAsync(phone.text.ToString());
                        database.Child("section").SetValueAsync(section.text.ToString());

                        if (section.text.ToString() == "Boys")
                            Handbook.initializeEmptyBook(database.Child("users").Child(id).Child("handbook"));
                        SceneManager.LoadScene("Login", LoadSceneMode.Single);

                        
                    }
                });


            }
        }
    }
}
