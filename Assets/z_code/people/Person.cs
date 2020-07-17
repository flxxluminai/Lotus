using System;

using Firebase.Database;
using Firebase.Auth;

public class Person
{
    protected string name, DOB, email, phone, password;
    protected string id;
    protected int clearance;
    protected static FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
    protected DatabaseReference database;
    public Person(string name, string DOB, string email, string phone, string id, int clearance = 0)
    {
        this.name = name;
        this.DOB = DOB;
        this.email = email;
        this.phone = phone;
        this.id = id;
        this.clearance = clearance;
        this.database = FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(id);
    }

    public void changeEmail(string newEmail)
    {
        user.UpdateEmailAsync(newEmail).ContinueWith(task => 
        {
            if (task.IsCanceled || task.IsFaulted)
                return;

            database.Child("email").SetValueAsync(email);
            email = newEmail;
        });
    }

    public void changePassword(string newPassword)
    {
        user.UpdatePasswordAsync(newPassword);
    }

    public void updateDatabase()
    {
        database.Child("name").SetValueAsync(name);
        database.Child("DOB").SetValueAsync(DOB);
        database.Child("phone").SetValueAsync(phone);
        database.Child("clearance").SetValueAsync(clearance);
    }

    public static void deleteProfile(string id)
    {
        FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(id).RemoveValueAsync();
        user.DeleteAsync();
    }

    public static string createID(string first, string middle, string last, string DOB)
    {
        string time = DateTime.UtcNow.ToString();
        int timeMult = 0;
        for (int i = 0; i < time.Length; i++)
        {
            timeMult += time[i];
        }

        string id = "";
        string[] parts = { first, middle, last, DOB };
        for (int i = 0; i < 4; i++)
        {
            int part = 0;
            for (int j = 0; j < parts[i].Length; j++)
            {
                part += parts[i][j];
            }

            part = (part * timeMult)%100;
            if (part < 10)
                id += "0" + part;
            else
                id += part;
        }
        return id;
    }
}
