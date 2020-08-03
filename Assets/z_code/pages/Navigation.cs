using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

using Firebase.Auth;
using Firebase.Database;

public class Navigation : MonoBehaviour
{
    FirebaseUser user;
    public static DatabaseReference userDatabase;
    public static DataSnapshot userSnapshot;

    // Start is called before the first frame update
    void Start()
    {
        FirebaseDatabase.DefaultInstance.SetPersistenceEnabled(Settings.settings[4]);

        user = FirebaseAuth.DefaultInstance.CurrentUser;
        userDatabase = FirebaseDatabase.DefaultInstance.RootReference.Child("accounts").Child(user.UserId);
        userSnapshot = userDatabase.GetValueAsync().Result;
        userDatabase.ValueChanged += HandleValueChanged;
    }

    public void navigate(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Additive);
    }

    public void pageUnload()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }

    void HandleValueChanged(object sender, ValueChangedEventArgs args)
    {
        if (args.DatabaseError != null)
        {
            Debug.LogError(args.DatabaseError.Message);
            return;
        }

        userSnapshot = args.Snapshot;
    }
}
