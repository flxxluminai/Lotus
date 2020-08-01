using System.Collections;
using System.Collections.Generic;

using Firebase.Database;
using UnityEngine;

[System.Serializable]
public class Save
{
    string settings = "";
    DataSnapshot userData;
    DataSnapshot handbookData;
    DataSnapshot announcementsData;

    private Save createSave()
    {
        Save save = new Save();
        save.userData = Navigation.userSnapshot;
        save.handbookData = Handbook.dataSnapshot;
        save.announcementsData = Announcements.snapshot;
        return save;
    }

    public void saveData()
    {
        Save save = createSave();

        string jsonData = JsonUtility.ToJson(save);
        PlayerPrefs.SetString("UserData", jsonData);
        PlayerPrefs.Save();
    }

    public void loadData()
    {
        string jsonData = PlayerPrefs.GetString("UserData");
        Save save = JsonUtility.FromJson<Save>(jsonData);

        Navigation.userSnapshot = save.userData;
        Handbook.dataSnapshot = save.handbookData;
        Announcements.snapshot = save.announcementsData;
    }
}
