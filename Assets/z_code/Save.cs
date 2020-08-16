using UnityEngine;

[System.Serializable]
public class Save
{
    bool[] settings = { };
    JSONObject json = JSONObject.Create();

    private Save createSave()
    {
        Save save = new Save();
        save.settings = Settings.settings;
        json.AddField("announcements", Announcements.json);
        json.AddField("profile", Profile.json);
        json.AddField("last login", System.DateTime.Now.ToLongDateString());

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

        Settings.settings = save.settings;
        Announcements.json = save.json["announcements"];
        Profile.json = save.json["profile"];
    }
}
