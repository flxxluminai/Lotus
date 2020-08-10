using System.Collections;
using System;
using UnityEngine.Networking;
using UnityEngine;

public class Connection
{
    private const string siteURL = "https://christophervuhoang.wixsite.com/hoalu";
    private const string urlProfiles = siteURL + "/_functions-dev/profiles";
    private const string announcements = siteURL + "/_functions/announcements";

    /*
     * Check if connected to internet 
     */
    public bool checkConnected()
    {
        UnityWebRequest request = new UnityWebRequest(urlProfiles);
        if (request.isHttpError || request.isNetworkError)
        {
            Debug.Log("no connection");
            return false;
        }
        else
        {
            Debug.Log("connection");
            return true;
        }
    }

    /*
     * Checks Internet connection type:
     *  0: not connected
     *  1: connected via wifi
     *  2: connected via mobile data or LAN
     */
    public int checkConnectionType()
    {
        if (checkConnected())
        {
            switch(Application.internetReachability)
            {
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    return 1;
                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    return 2;
                default:
                    return 0;
            }
        }
        else
        {
            return 0;
        }
    }

    public IEnumerator getData()
    {
        UnityWebRequest request = new UnityWebRequest(urlProfiles);
        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            Debug.Log(request.downloadHandler.text);
        }
        
    }
}
