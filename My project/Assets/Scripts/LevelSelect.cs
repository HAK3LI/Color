using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{

    public GameObject block;
    public GameObject parent;
    private GameObject new_block;
    private string filePath;
    // Start is called before the first frame update
    void TaskOnClick(int levelnum)
    {
        string readText = File.ReadAllText(filePath + "Level_0_" + levelnum + ".dat");
        PlayerPrefs.SetString("level", readText);
        StartCoroutine(LoadYourAsyncScene());
    }
    void GetFiles()
    {
        DirectoryInfo dir = new DirectoryInfo(filePath);
        FileInfo[] info = dir.GetFiles("*.dat");
        int i = 0;
        foreach (FileInfo f in info)
        {
            Vector3 cord = new Vector3((i % 5 - 2f), (i / 5 + 3) * 0.8f, 0);
            new_block = Instantiate(block, cord, Quaternion.identity, parent.transform);
            Text block_text = new_block.GetComponentInChildren<Text>();
            block_text.text = (i + 1).ToString();
            Button btn = new_block.GetComponent<Button>();
            Data level = new_block.GetComponent<Data>();
            level.setNumber(i);
            btn.onClick.AddListener(delegate { TaskOnClick(level.getNumber()); }); ;
            i++;
        }
    }
    void Start()
    {
        filePath = Application.dataPath + "/Levels/";
        GetFiles();
    }

    // Update is called once per frame


    void Update()
    {

    }
   IEnumerator LoadYourAsyncScene()
    {
        // The Application loads the Scene in the background as the current Scene runs.
        // This is particularly good for creating loading screens.
        // You could also load the Scene by using sceneBuildIndex. In this case Scene2 has
        // a sceneBuildIndex of 1 as shown in Build Settings.

        AsyncOperation asyncLoad = SceneManager.LoadSceneAsync("Scenes/Game");

        // Wait until the asynchronous scene fully loads
        while (!asyncLoad.isDone)
        {
            yield return null;
        }
    }
}
