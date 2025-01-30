using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{

    [SerializeField] private string nextScene;

    // Start is called before the first frame update
    void Start()
    {
        SceneManager.LoadScene(nextScene);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
