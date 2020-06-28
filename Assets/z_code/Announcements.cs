using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Firebase.Database;

public class Announcements : MonoBehaviour
{
    private DatabaseReference database;

    public void Start()
    {
        database = FirebaseDatabase.DefaultInstance.RootReference.Child("announcements");
    }

    public void addContent()
    {
        database.GetValueAsync().ContinueWith(task => 
        { 
            if (task.IsFaulted)
            {

            }
            else if (task.IsCompleted)
            {
                RectTransform scrollView = GameObject.Find("Scroll View").GetComponent<RectTransform>();
                DataSnapshot snapshot = task.Result;
                IEnumerable<DataSnapshot> values = snapshot.Children;
                GameObject item = GameObject.Find("Content Template").GetComponent<GameObject>();
                GameObject end = GameObject.Find("Ending").GetComponent<GameObject>();

                foreach (DataSnapshot value in values)
                {
                    GameObject copy = RectTransform.Instantiate(item);
                    GameObject child = copy.transform.GetChild(0).gameObject;

                    child.transform.Find("When").GetComponent<Text>().text = value.Child("when").Value.ToString();
                    child.transform.Find("Where").GetComponent<Text>().text = value.Child("where").Value.ToString();
                    child.transform.Find("Description").GetComponent<Text>().text = value.Child("description").Value.ToString();

                    if (value.Child("type").Value.ToString() == "camp")
                        child.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load("z_images/ic_camping") as Sprite;
                    else if (value.Child("type").Value.ToString() == "volunteer")
                        child.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load("z_images/ic_volunteer") as Sprite;
                    else
                        child.transform.Find("Image").GetComponent<Image>().sprite = Resources.Load("z_images/ic_activity") as Sprite;

                    copy.transform.SetParent(scrollView);
                }
                end.transform.SetParent(scrollView);
            }
        });
    }
}
