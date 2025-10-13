using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class GoGame : MonoBehaviour
{
    public void SceneChanges()
    {
        SceneManager.LoadScene("gameSceneObject_BG");
    }

    public void AIScene()
    {
        SceneManager.LoadScene("gameSceneObject_AI");
    }
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
