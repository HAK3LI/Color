using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
    void GoLevels()
    {
        StartCoroutine(LoadYourAsyncScene("Scenes/Choose Level"));
    }
    // Start is called before the first frame update
    void Start()
    {
        GameObject levels = GameObject.Find("Levels");
        Button lvlbtn = levels.GetComponent<Button>();
        lvlbtn.onClick.AddListener(GoLevels);
        GameObject menu = GameObject.Find("Menu");
        UpdateLevel lvlmem = menu.GetComponent<UpdateLevel>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    IEnumerator LoadYourAsyncScene(string scenename)
    {

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync(scenename);

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
