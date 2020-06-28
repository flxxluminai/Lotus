﻿using Firebase.Database;
using Firebase.Auth;

public class Person
{
    protected string name, DOB, email, phone, password;
    protected int clearance;
    protected FirebaseUser user = FirebaseAuth.DefaultInstance.CurrentUser;
    protected DatabaseReference database;
    public Person(string name, string DOB, string email, string phone, int clearance = 0)
    {
        this.name = name;
        this.DOB = DOB;
        this.email = email;
        this.phone = phone;
        this.clearance = clearance;
        this.database = FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(email.Replace('.', ' '));
    }

    public void changeEmail(string newEmail)
    {
        email = newEmail;
        database = FirebaseDatabase.DefaultInstance.RootReference.Child("users").Child(email.Replace('.', ' '));
        user.UpdateEmailAsync(newEmail).ContinueWith(task => 
        {
            if (task.IsCanceled || task.IsFaulted)
                return;

            updateDatabase();
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
}
