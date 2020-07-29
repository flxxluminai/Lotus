using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Networking;

using Firebase.Auth;

public class Profile : MonoBehaviour
{
    FirebaseUser user;

    Text error;
    Button changeProfileButton, changePasswordButton, changePicButton,
            saveProfile, savePass;

    InputField firstNameInput, middleNameInput, lastNameInput, DOBInput, phoneInput, emailInput;
    InputField oldPassword, newPassword, confirmPassword;

    Image profilePic;
    Text userName, DOB, phone, email, money, view;

    // Start is called before the first frame update
    void Start()
    {
        user = FirebaseAuth.DefaultInstance.CurrentUser;

        error = GameObject.Find("error").GetComponent<Text>();
        changeProfileButton = GameObject.Find("changeInfoButton").GetComponent<Button>();
        changePasswordButton = GameObject.Find("changePassword").GetComponent<Button>();
        changePicButton = GameObject.Find("changePicButton").GetComponent<Button>();

        firstNameInput = GameObject.Find("firstNameInput").GetComponent<InputField>();
        firstNameInput.enabled = false;
        middleNameInput = GameObject.Find("middleNameInput").GetComponent<InputField>();
        middleNameInput.enabled = false;
        lastNameInput = GameObject.Find("lastNameInput").GetComponent<InputField>();
        lastNameInput.enabled = false;
        DOBInput = GameObject.Find("DOBInput").GetComponent<InputField>();
        DOBInput.enabled = false;
        phoneInput = GameObject.Find("phoneInput").GetComponent<InputField>();
        phoneInput.enabled = false;
        emailInput = GameObject.Find("emailInput").GetComponent<InputField>();
        emailInput.enabled = false;

        profilePic = GameObject.Find("profilePic").GetComponent<Image>();
        userName = GameObject.Find("nameData").GetComponent<Text>();
        DOB = GameObject.Find("DOBData").GetComponent<Text>();
        phone = GameObject.Find("phoneData").GetComponent<Text>();
        email = GameObject.Find("phoneData").GetComponent<Text>();
        money = GameObject.Find("moneyData").GetComponent<Text>();
        view = GameObject.Find("viewTitle").GetComponent<Text>();

        saveProfile = GameObject.Find("saveProfile").GetComponent<Button>();
        saveProfile.enabled = false;
        savePass = GameObject.Find("savePassword").GetComponent<Button>();
        savePass.enabled = false;

        oldPassword = GameObject.Find("oldPassword").GetComponent<InputField>();
        newPassword = GameObject.Find("newPassword").GetComponent<InputField>();
        confirmPassword = GameObject.Find("confirmPassword").GetComponent<InputField>();
        oldPassword.enabled = false;

        loadInfo();
    }

    void loadInfo()
    {
        if (Navigation.userSnapshot.Child("section").Value.ToString() != "Boys")
        {
            Text moneyTitle = GameObject.Find("money").GetComponent<Text>();
            moneyTitle.enabled = false;
            money.enabled = false;
            view.enabled = false;
        }
        else
        {
            money.text = Navigation.userSnapshot.Child("money").Value.ToString();
        }

        StartCoroutine(loadProfilePic(user.PhotoUrl.ToString()));

        userName.text = Navigation.userSnapshot.Child("firstName").Value.ToString() + " " +
                        Navigation.userSnapshot.Child("middleName").Value.ToString() + " " +
                        Navigation.userSnapshot.Child("lastName").Value.ToString();
        DOB.text = Navigation.userSnapshot.Child("DOB").Value.ToString();
        phone.text = Navigation.userSnapshot.Child("phone").Value.ToString();
        email.text = user.Email;
    }

    IEnumerator loadProfilePic(string url)
    {
        UnityWebRequest request = UnityWebRequestTexture.GetTexture(url);
        yield return request.SendWebRequest();
        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
            profilePic.sprite = Sprite.Create(((DownloadHandlerTexture)request.downloadHandler).texture, 
                                profilePic.sprite.rect, new Vector2(0.5f, 0.5f));
    }

    public void loadChangeProfile()
    {
        changeProfileButton.enabled = false;
        changePasswordButton.enabled = false;
        changePicButton.enabled = false;
        view.enabled = false;

        string[] name = userName.text.Split(' ');

        firstNameInput.enabled = true;
        firstNameInput.text = name[0];
        middleNameInput.enabled = true;
        middleNameInput.text = name[1];
        for (int i = 2; i < name.Length - 1; i++)
            middleNameInput.text += name[i];
        lastNameInput.enabled = true;
        lastNameInput.text = name[name.Length - 1];
        DOBInput.enabled = true;
        DOBInput.text = DOB.text;
        phoneInput.enabled = true;
        phoneInput.text = phone.text;
        emailInput.enabled = true;
        emailInput.text = email.text;

        userName.enabled = false;
        DOB.enabled = false;
        phone.enabled = false;
        email.enabled = false;
        Text moneyTitle = GameObject.Find("money").GetComponent<Text>();
        moneyTitle.enabled = false;

        saveProfile.enabled = true;
    }

    public void unloadChangeProfile()
    {
        userName.enabled = true;
        DOB.enabled = true;
        phone.enabled = true;
        email.enabled = true;
        if (Navigation.userSnapshot.Child("section").Value.ToString() == "Boys")
        {
            Text moneyTitle = GameObject.Find("money").GetComponent<Text>();
            moneyTitle.enabled = true;
            money.enabled = true;
            view.enabled = true;
        }

        firstNameInput.enabled = false;
        middleNameInput.enabled = false;
        lastNameInput.enabled = false;
        DOBInput.enabled = false;
        phoneInput.enabled = false;
        emailInput.enabled = false;
        saveProfile.enabled = false;
    }

    public void saveProfileInfo()
    {
        userName.text = firstNameInput.text + middleNameInput.text + lastNameInput.text;
        Navigation.userDatabase.Child("firstName").SetValueAsync(firstNameInput.text);
        Navigation.userDatabase.Child("middleName").SetValueAsync(middleNameInput.text);
        Navigation.userDatabase.Child("lastname").SetValueAsync(lastNameInput.text);
        DOB.text = DOBInput.text;
        Navigation.userDatabase.Child("DOB").SetValueAsync(DOBInput.text);
        phone.text = phoneInput.text;
        Navigation.userDatabase.Child("phone").SetValueAsync(phoneInput.text);
        email.text = emailInput.text;
        user.UpdateEmailAsync(emailInput.text);

        unloadChangeProfile();
    }

    public void loadChangePassword()
    {
        changeProfileButton.enabled = false;
        changePasswordButton.enabled = false;
        changePicButton.enabled = false;
        view.enabled = false;
        Text moneyTitle = GameObject.Find("money").GetComponent<Text>();
        moneyTitle.enabled = false;
        Text nameTitle = GameObject.Find("name").GetComponent<Text>();
        nameTitle.enabled = false;
        Text DOBTitle = GameObject.Find("DOB").GetComponent<Text>();
        DOBTitle.enabled = false;
        Text phoneTitle = GameObject.Find("phone").GetComponent<Text>();
        phoneTitle.enabled = false;
        Text emailTitle = GameObject.Find("email").GetComponent<Text>();
        emailTitle.enabled = false;

        oldPassword.enabled = true;
        savePass.enabled = true;
    }

    public void unloadChangePassword()
    {
        changeProfileButton.enabled = true;
        changePasswordButton.enabled = true;
        changePicButton.enabled = true;
        view.enabled = true;
        Text moneyTitle = GameObject.Find("money").GetComponent<Text>();
        moneyTitle.enabled = true;
        Text nameTitle = GameObject.Find("name").GetComponent<Text>();
        nameTitle.enabled = true;
        Text DOBTitle = GameObject.Find("DOB").GetComponent<Text>();
        DOBTitle.enabled = true;
        Text phoneTitle = GameObject.Find("phone").GetComponent<Text>();
        phoneTitle.enabled = true;
        Text emailTitle = GameObject.Find("email").GetComponent<Text>();
        emailTitle.enabled = true;
        savePass.enabled = false;
        oldPassword.enabled = false;
    }

    public void savePassword()
    {
        Authorization auth = new Authorization();
        if (auth.authorizeSignIn(user.Email, oldPassword.text) == 2)
        {
            if (newPassword.text == confirmPassword.text)
            {
                user.UpdatePasswordAsync(newPassword.text);
                unloadChangePassword();
            }
            else
            {
                error.text = "** New Password and Confirmation Password do not match **";
            }
        }
        else
        {
            error.text = "** Invalid Old Password **";
        }
    }
    
}
