using System.Collections;
using System.Collections.Generic;

using UnityEngine;

[System.Serializable]
public class Save
{
    bool[] settings = { };

    private Save createSave()
    {
        Save save = new Save();
        save.settings = Settings.settings;
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
    }
}
