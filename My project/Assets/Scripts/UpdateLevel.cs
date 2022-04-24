using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;

public class UpdateLevel : MonoBehaviour
{

    
}

[System.Serializable]
public class Level
{
    public string data;
    public int width;
    public int height;
    public int colors;
    public int[] target = new int[3];
    public int highscore;

    public Level(string data, int width, int height, int colors, int[] target)
    {
        this.data = data;
        this.width = width;
        this.height = height;
        this.colors = colors;
        this.target = target;
        this.highscore = -1;
    }
    public Level(string name)
    {
        string filePath = Application.dataPath + "/Levels/"+name;
        string jsonString = File.ReadAllText(filePath);
        Level from = JsonUtility.FromJson<Level>(jsonString);
        this.data = from.data;
        this.width = from.width;
        this.height = from.height;
        this.colors = from.colors;
        this.target = from.target;
        this.highscore = from.highscore;
    }
    public void loadGame() {
        PlayerPrefs.SetString("level", this.data);
        PlayerPrefs.SetInt("width", this.width);
        PlayerPrefs.SetInt("height", this.height);
        PlayerPrefs.SetInt("colors", this.colors);
        PlayerPrefs.SetInt("target0", this.target[0]);
        PlayerPrefs.SetInt("target1", this.target[1]);
        PlayerPrefs.SetInt("target2", this.target[2]);
    }
    public void UpdateHighScore(int highscore) 
    {
        this.highscore = highscore;
    }
    public void SaveIntoJson(string name){
        string level = JsonUtility.ToJson(this, true);
        System.IO.File.WriteAllText(Application.dataPath + "/Levels/"+name+".json", level);
    }
    
}