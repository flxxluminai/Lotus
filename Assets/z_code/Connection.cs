using System.Collections;
using System;
using UnityEngine.Networking;
using UnityEngine;

public class Connection
{
    private const string siteURL = "https://christophervuhoang.wixsite.com/hoalu";

    private string functionName;
    public JSONObject json;

    public Connection()
    {
        this.functionName = "";
    }

    public Connection(string functionName)
    {
        this.functionName = functionName;
    }

    /*
     * Check if connected to internet 
     */
    public bool checkConnected()
    {
        UnityWebRequest request = new UnityWebRequest(siteURL);
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

    /*
     * Gets data from http request
     * Afterwards, completes action (function) 
     */
    public IEnumerator getData(string query, Action action = null)
    {
        UnityWebRequest request;
        if (query.Length > 0)
            request = new UnityWebRequest(siteURL + "/_functions-dev/" + functionName + "?" + query);
        else
            request = new UnityWebRequest(siteURL + "/_functions-dev/" + functionName);

        request.downloadHandler = new DownloadHandlerBuffer();

        yield return request.SendWebRequest();

        if (request.isNetworkError || request.isHttpError)
            Debug.Log(request.error);
        else
        {
            json = JSONObject.Create(request.downloadHandler.text);
            Debug.Log(json.Print(true));
        }

        if (action != null)
        {
            action();
        }
        
    }
}
