using System.Collections;
using System.Collections.Generic;
using UnityEngine;

#if UNITY_ANDROID
using UnityEngine.Android;
#endif

#if UNITY_IOS
using UnityEngine.iOS;
#endif

public class Permissions : MonoBehaviour
{
    GameObject dialog = null;

    // Start is called before the first frame update
    void Start()
    {

    }

    void askPermission()
    {
#if UNITY_ANDROID
        if (!Permission.HasUserAuthorizedPermission(Permission.Camera) ||
            !Permission.HasUserAuthorizedPermission(Permission.ExternalStorageRead) ||
            !Permission.HasUserAuthorizedPermission(Permission.ExternalStorageWrite))
        {
            Permission.RequestUserPermission(Permission.Camera);
            Permission.RequestUserPermission(Permission.ExternalStorageRead);
            Permission.RequestUserPermission(Permission.ExternalStorageWrite);
            dialog = new GameObject();
        }
#endif


#if UNITY_IOS
        if (!Application.HasUserAuthorization(UserAuthorization.WebCam))
        {
            Application.RequestUserAuthorization(UserAuthorization.WebCam);
        }
#endif
    }
}

