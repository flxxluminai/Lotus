using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using Firebase.Database;

public class Handbook : MonoBehaviour
{
    private DataSnapshot dataSnapshot;
    private DatabaseReference userDatabase;
    private DataSnapshot userSnapshot;

    // Start is called before the first frame update
    void Start()
    {
        string sceneName = SceneManager.GetActiveScene().ToString(); 
        DatabaseReference database = FirebaseDatabase.DefaultInstance.RootReference.Child("handbook").Child(sceneName);
        userDatabase = Navigation.userDatabase.Child("handbook").Child(sceneName);

        userSnapshot = Navigation.userSnapshot.Child("handbook").Child(sceneName);
        if ((int)userSnapshot.Child("year").Value == 0)
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
            database = database.Child(userSnapshot.Child("year").Value.ToString());
            dataSnapshot = database.GetValueAsync().Result;
        }
        
        loadRequirements();
    }

    void loadRequirements()
    {
        Text model = GameObject.Find("DescripText").GetComponent<Text>();

        Vector3 position = new Vector3(194, 32, 0);
        foreach (DataSnapshot req in dataSnapshot.Children)
        {
            Text copy = Text.Instantiate(model);
            copy.transform.position = position;

            Text reqNum = copy.transform.Find("ReqText").GetComponent<Text>();
            reqNum.text = req.Key;

            model.text = req.Value.ToString();

            Text signature = copy.transform.Find("SignatureText").transform.Find("Text").GetComponent<Text>();
            signature.text = userSnapshot.Child(req.Key).Value.ToString();

            position = new Vector3(194, copy.rectTransform.offsetMin.y, 0);
        }
    }

    public static void initializeEmptyBook(DatabaseReference data)
    {
        data.Child("Scout").Child("year").SetValueAsync(0);
        data.Child("Tenderfoot").Child("year").SetValueAsync(0);
        data.Child("Second Class").Child("year").SetValueAsync(0);
        data.Child("First Class").Child("year").SetValueAsync(0);
        data.Child("Star").Child("year").SetValueAsync(0);
        data.Child("Life").Child("year").SetValueAsync(0);
        data.Child("Eagle").Child("year").SetValueAsync(0);
    }
}
