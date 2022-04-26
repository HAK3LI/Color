using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine.SceneManagement;
using UnityEngine;
using UnityEngine.UI;

public class LevelSelect : MonoBehaviour
{

    public GameObject block;
    public Sprite fillSprite;
    public GameObject parent;
    private GameObject new_block;
    private string filePath;
    // Start is called before the first frame update
    void TaskOnClick(int levelnum)
    {
        Level gamelvl = new Level("Level_0_"+levelnum);
        gamelvl.loadGame();
        PlayerPrefs.SetInt("levelnum", levelnum);
        StartCoroutine(LoadYourAsyncScene());
    }
    void GetFiles()
    {
        Object[] dir = Resources.LoadAll("Levels");
        int i = 0;
        foreach (Object f in dir)
        {
            Vector3 cord = new Vector3((i % 4 - 1.5f)*1.2f, (2.35f - i / 4) * 1.2f, 0);
            new_block = Instantiate(block, cord, Quaternion.identity, parent.transform);
            Text block_text = new_block.GetComponentInChildren<Text>();
            block_text.text = (i + 1).ToString();
            Level infolvl = new Level("Level_0_"+i);
            for (int j = 0; j < 3; j++) {
                if (infolvl.highscore <= infolvl.target[j] && infolvl.highscore > 0)
                {
                    GameObject target = new_block.transform.Find("target"+(2-j)).gameObject;
                    SpriteRenderer targetsprite = target.GetComponent<SpriteRenderer>();
                    targetsprite.sprite = fillSprite; 
                }
            }
            Button btn = new_block.GetComponentInChildren<Button>();
            Data level = new_block.GetComponentInChildren<Data>();
            level.setNumber(i);
            btn.onClick.AddListener(delegate { TaskOnClick(level.getNumber()); }); ;
            i++;
        }
    }
    void Start()
    {
        GameObject menu = GameObject.Find("Menu");
        UpdateLevel lvlmem = menu.GetComponent<UpdateLevel>();
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
