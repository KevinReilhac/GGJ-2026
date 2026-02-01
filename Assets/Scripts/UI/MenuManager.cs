using System.Collections;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    IEnumerator LoadLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("ProceduralGeneration");
    }

    public void onPlayClicked()
    {
        StartCoroutine(LoadLevelAfterDelay(2f));
    }

    IEnumerator QuitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Application.Quit();
    }

    public void onQuitClicked()
    {
        StartCoroutine(LoadLevelAfterDelay(2f));
    }

}
