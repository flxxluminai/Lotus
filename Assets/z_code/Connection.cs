using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Firebase.Database;

public class Connection
{
    /*
     * Check if connected to internet
     */
    public bool checkConnected()
    {
        DatabaseReference database = FirebaseDatabase.DefaultInstance.GetReference(".info/connected");
        return (bool) database.GetValueAsync().Result.Value;
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
}
