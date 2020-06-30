using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    public void navigate(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Additive);
    }

    public void pageUnload(string name)
    {
        SceneManager.UnloadSceneAsync(name);
    }
}
