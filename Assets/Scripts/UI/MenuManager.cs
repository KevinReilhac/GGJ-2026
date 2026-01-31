using System.Collections;
using System.Collections.Generic;
using System.Configuration.Assemblies;
using UnityEngine;

public class MenuManager : MonoBehaviour
{
    private Canvas menuCanvas;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void onPlayClicked()
    {
        UnityEngine.SceneManagement.SceneManager.LoadScene("GameScene");
    }

    public void onQuitClicked()
    {
        Application.Quit();
    }

}
