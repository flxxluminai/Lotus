using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Database;
using Firebase.Auth;

public class Handbook : MonoBehaviour
{
    private DatabaseReference database;
    private DatabaseReference userDatabase;
    private FirebaseUser user;
    private DataSnapshot userSnapshot;
    private DataSnapshot dataSnapshot;
    private string sceneName;

    // Start is called before the first frame update
    void Start()
    {
        sceneName = SceneManager.GetActiveScene().ToString(); 
        database = FirebaseDatabase.DefaultInstance.RootReference.Child("handbook").Child(sceneName);
        user = FirebaseAuth.DefaultInstance.CurrentUser;
        userDatabase = FirebaseDatabase.DefaultInstance.RootReference.Child("accounts").Child(user.Email.Replace(".", " "))
            .Child("handbook").Child(sceneName);

        bool flag = false;
        userDatabase.GetValueAsync().ContinueWith(task =>
        {
            if (task.IsFaulted || task.IsCanceled)
                return;
            else
            {
                userSnapshot = task.Result;
                if ((int) userSnapshot.Child("year").Value == 0)
                {
                    flag = true;
                }
                else
                {
                    database = database.Child(userSnapshot.Child("year").Value.ToString());
                }
            }
        });
        if (flag)
        {
            dataSnapshot = database.OrderByKey().LimitToLast(1).GetValueAsync().Result;
            userDatabase.Child("requirements").SetValueAsync(dataSnapshot.Children);
            userDatabase.Child("year").SetValueAsync(dataSnapshot.Key);

            userDatabase = userDatabase.Child("requirements");
            foreach (DataSnapshot i in dataSnapshot.Children)
            {
                userDatabase.Child(i.Key).SetValueAsync(" ");
            }
            userSnapshot = userDatabase.GetValueAsync().Result;
        }
        else
        {
            dataSnapshot = database.GetValueAsync().Result;
        }
        
        loadRequirements();
    }

    void loadRequirements()
    {
        Image model = GameObject.Find("Model").GetComponent<Image>();
        int items = 0;
        foreach (DataSnapshot req in dataSnapshot.Children)
        {
            items++;
            Vector3 position = new Vector3(50, 40+(items*1300), 0);
            Image copy = Image.Instantiate(model);
            copy.transform.position = position;

            Text reqNum = copy.transform.Find("Text").GetComponent<Text>();
            reqNum.text = req.Key;

            Text description = copy.transform.Find("Description").transform.Find("Text").GetComponent<Text>();
            description.text = req.Value.ToString();

            Text signature = copy.transform.Find("Signature").transform.Find("Text").GetComponent<Text>();
            signature.text = userSnapshot.Child(req.Key).Value.ToString();
        }
    }
}
