using System.Collections;
using System.Collections.Generic;
using UnityEditor.SearchService;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MM_UI : MonoBehaviour
{
    public string mainScene;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void StartGame()
    {
        SceneManager.LoadScene(mainScene, LoadSceneMode.Single);
    }

    public void ExitGame()
    {
        Application.Quit();
    }
}
