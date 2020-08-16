using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Navigation
{

    public void navigate(string name)
    {
        SceneManager.LoadScene(name, LoadSceneMode.Additive);
    }

    public void pageUnload()
    {
        SceneManager.UnloadSceneAsync(SceneManager.GetActiveScene());
    }
}
