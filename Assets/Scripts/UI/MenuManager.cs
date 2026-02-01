using System.Collections;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class MenuManager : MonoBehaviour
{
    private bool _hasClicked = false;

    IEnumerator LoadLevelAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        SceneManager.LoadScene("ProceduralGeneration");
    }

    public void onPlayClicked()
    {
        if (_hasClicked)
            return;

        _hasClicked = true;
        StartCoroutine(LoadLevelAfterDelay(2f));
    }

    IEnumerator QuitAfterDelay(float delay)
    {
        yield return new WaitForSeconds(delay);
        Application.Quit();
    }

    public void onQuitClicked()
    {
        if (_hasClicked)
            return;

        _hasClicked = true;
        StartCoroutine(QuitAfterDelay(2f));
    }

}
