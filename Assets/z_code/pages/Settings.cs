using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Settings : MonoBehaviour
{
    public static bool[] settings = { }; /* Index:
                                          *     0: push notifications
                                          *     1: email notifications
                                          *     2: camera usage
                                          *     3: mobile data usage
                                          *     4: offline data sync
                                          */

    Toggle[] toggles = { };

    // Start is called before the first frame update
    void Start()
    {
        toggles[0] = GameObject.Find("pushNotifications").GetComponent<Toggle>();
        toggles[1] = GameObject.Find("emailNotifications").GetComponent<Toggle>();
        toggles[2] = GameObject.Find("cameraUsage").GetComponent<Toggle>();
        toggles[3] = GameObject.Find("mobileDataUsage").GetComponent<Toggle>();
        toggles[4] = GameObject.Find("offlineUsage").GetComponent<Toggle>();

        loadToggle();
    }

    private void loadToggle()
    {
        if (settings.Length != 5)
        {
            settings[0] = true;
            settings[1] = true;
            settings[2] = false;
            settings[3] = false;
            settings[4] = false;
        }
        
        for (int i = 0; i < toggles.Length; i++)
        {
            toggles[i].isOn = settings[i];
        }   
    }

    public void toggleButton(int num)
    {
        if (settings[num])
        {
            settings[num] = false;
            toggles[num].isOn = false;
        }
        else
        {
            settings[num] = true;
            toggles[num].isOn = true;
        }
    }
}
