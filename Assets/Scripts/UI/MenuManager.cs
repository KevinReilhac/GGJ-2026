using System.Collections;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using UnityEngine;
using UnityEngine.UI;

public class MenuManager : MonoBehaviour
{
    public void onPlayClicked()
    {
        StartCoroutine(WaitDelay(3f));
        UnityEngine.SceneManagement.SceneManager.LoadScene("ProceduralGeneration");
    }

    IEnumerator WaitDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
    }

    public void onQuitClicked()
    {
        StartCoroutine(WaitDelay(3f));
        Application.Quit();
    }

}
