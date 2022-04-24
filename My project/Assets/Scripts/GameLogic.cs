using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameLogic : MonoBehaviour
{
    public Text text;
    public GameObject scaleRef;
    private Animator anim;
    private SpriteRenderer sprite;
    private float H, S, V;
    List<GameObject> close;

    public int currentclicks;
    public int clicks = 20;
    public GameObject block, parent;
    private GameObject new_block, new_star;
    public static int width = 10;
    public static int height = 10;
    public GameObject star;
    public Color[] colors;
    private float scale;
    private int[,] info = new int[width, height];
    // Start is called before the first frame update
    GameObject[] control = new GameObject[4];
    
    public void disableControl() {
        foreach (GameObject cntrl in control)
        {
            Button cntrlbtn = cntrl.GetComponent<Button>();
            cntrlbtn.interactable = false;
        }
    }
    public void ButtonSelected(int i)
    {
        Animator anim = control[i].GetComponent<Animator>();
        anim.SetTrigger("trans");
        anim.GetCurrentAnimatorStateInfo(0);
        info = search(info, 10 + i);
        changeData(info, 10 + i, Time.time);
        currentclicks--;
        if (currentclicks == 0)
        {
            disableControl();
        }
        text.text = "Кликов осталось:" + currentclicks;
    }

    public void loadGame(string l_info, int l_currentclicks, int l_width, int l_height)
    {
        for (int y = 0; y < l_height; ++y)
        {
            for (int x = 0; x < l_width; ++x)
            {
                info[x, y] = l_info[x + y * l_height] - '0';
            }
        }
        currentclicks = l_currentclicks;
        width = l_width;
        height = l_height;
    }
    public void PrintEvent() 
    {
        Debug.Log("PrintEvent: " + " called at: " + Time.time);
    }
    void setData(int[,] field)
    {
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("tile");
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                Data data = blocks[x + y * height].GetComponent<Data>();
                data.setNumber(field[x, y]);
            }
        }
    }

    int[,] search(int[,] field, int value)
    {
        void lookAround(int i, int j)
        {
            if (i != 0 && value == field[i - 1, j] + 10)
            {
                field[i - 1, j] = value;
                lookAround(i - 1, j);
            }
            if (i != width - 1 && value == field[i + 1, j] + 10)
            {
                field[i + 1, j] = value;
                lookAround(i + 1, j);
            }
            if (j != 0 && value == field[i, j - 1] + 10)
            {
                field[i, j - 1] = value;
                lookAround(i, j - 1);
            }
            if (j != height - 1 && value == field[i, j + 1] + 10)
            {
                field[i, j + 1] = value;
                lookAround(i, j + 1);
            }
        }

        for (int i = 0; i < width; ++i)
        {
            for (int j = 0; j < height; ++j)
            {
                if (field[i, j] >= 10)
                {
                    lookAround(i, j);
                }
            }
        }
        return field;
    }


    void Start()
    {
        text.text = "Кликов осталось:" + clicks;
        GameObject menu = GameObject.Find("Game Field");
        UpdateLevel lvlmem = menu.GetComponent<UpdateLevel>();
        int i = 0;
        control = GameObject.FindGameObjectsWithTag("control");
        foreach (GameObject cntrl in control)
        {
            int x = i;
            i++;
            Button cntrlbtn = cntrl.GetComponent<Button>();
            cntrlbtn.onClick.AddListener(delegate { ButtonSelected(x); });
        }
        //PlayerPrefs.DeleteKey("level");
        bool load = false;
        string level = PlayerPrefs.GetString("level");
        if (level.Length > 0)
        {
            int getWidth = PlayerPrefs.GetInt("width");
            int getHeight = PlayerPrefs.GetInt("height");
            loadGame(level, 20, getWidth, getHeight);
            load = true;
        }
        if (load)
        //Load game
        {
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    Vector3 cord = new Vector3(x - width / 2 + (0.5f * (1 - width % 2)), y - height / 2 + (0.5f * (1 - height % 2)), 0);
                    new_block = Instantiate(block, cord, Quaternion.identity, parent.transform);
                    sprite = new_block.GetComponent<SpriteRenderer>();
                    Data data = new_block.GetComponent<Data>();
                    sprite.color = colors[info[x, y]];
                }
            }
        }
        else
        //New game
        {
            for (int y = 0; y < height; ++y)
            {
                for (int x = 0; x < width; ++x)
                {
                    Vector3 cord = new Vector3(x - width / 2 + (0.5f * (1 - width % 2)), y - height / 2 + (0.5f * (1 - height % 2)), 0);
                    new_block = Instantiate(block, cord, Quaternion.identity, parent.transform);
                    sprite = new_block.GetComponent<SpriteRenderer>();
                    Data data = new_block.GetComponent<Data>();
                    int V;
                    V = Random.Range(0, 4);
                    sprite.color = colors[V];
                    info[x, y] = V;
                }
            }
        }
        info[0, height - 1] = info[0, height - 1] + 10;
        info = search(info, info[0, height - 1]);
        setData(info);
        scale = 6.0f / width * 0.8f;
        parent.transform.localScale = new Vector3(scale, scale, scale);
    }

    void changeData(int[,] field, int value, float startTime)
    {
        bool animated = false;
        int completed = 0;
        GameObject[] blocks = GameObject.FindGameObjectsWithTag("tile");
        for (int y = 0; y < height; ++y)
        {
            for (int x = 0; x < width; ++x)
            {
                Data data = blocks[x + y * height].GetComponent<Data>();
                sprite = blocks[x + y * height].GetComponent<SpriteRenderer>();
                anim = blocks[x + y * height].GetComponent<Animator>();
                data.setNumber(field[x, y]);
                if (field[x, y] >= 10)
                {
                    completed++;
                    field[x, y] = value;
                    float delay = x + height - y;
                    StartCoroutine(AnimateImageColor(sprite, sprite.color, colors[value - 10], delay, anim));
                }
            }
        }
        if (completed == height * width && animated == false)
        //check win
        {
            animated = true;
            disableControl();
            GameObject fin = GameObject.Find("Game over");
            GameObject fin_text = GameObject.Find("Win text");
            Animator fin_bg = fin.GetComponent<Animator>();
            Animator fin_text_bg = fin_text.GetComponent<Animator>();
            fin_bg.SetBool("Game over", true);
            fin_text_bg.SetBool("Game over", true);
            int levelnum = PlayerPrefs.GetInt("levelnum");
            Level infolvl = new Level("Level_0_"+levelnum+".json");
            if (infolvl.highscore > (clicks-currentclicks+1))
            {
                infolvl.UpdateHighScore(clicks-currentclicks+1);
                infolvl.SaveIntoJson("Level_0_"+levelnum);
            }    
            for (int i=0;i<3;i++) {
                StartCoroutine(Win(i));
            }    
        }
        if (currentclicks == 1 && animated == false)
        //check lose
        {
            animated = true;
            Debug.Log("lose");
            GameObject fin = GameObject.Find("Game over");
            GameObject fin_text = GameObject.Find("Lose text");
            Animator fin_bg = fin.GetComponent<Animator>();
            Animator fin_text_bg = fin_text.GetComponent<Animator>();
            fin_bg.SetBool("Game over", true);
            fin_text_bg.SetBool("Game over", true);
        }
    }

    // Update is called once per frame
    void Update()
    {

    }

    public IEnumerator AnimateImageColor(SpriteRenderer sprite, Color from, Color to, float delay, Animator anim)
    {
        bool animated = false;
        float ElapsedTime = 0.0f - delay * 0.03f;
        float TotalTime = 0.25f;
        while (ElapsedTime < TotalTime)
        {
            ElapsedTime += Time.deltaTime;
            if (ElapsedTime >= 0)
            {
                if (animated == false)
                {
                    anim.SetTrigger("trans");
                    animated = true;
                }
                if (ElapsedTime / TotalTime <= 0.6)
                {
                    sprite.color = Color.Lerp(from, Color.Lerp(to, Color.white, 1), (ElapsedTime / TotalTime));
                }
                else
                {
                    sprite.color = Color.Lerp(from, to, (ElapsedTime / TotalTime));
                }

            }
            yield return null;
        }
    }
    public IEnumerator Win(int i)
    {
        yield return new WaitForSeconds(i*0.8f);
        int target = PlayerPrefs.GetInt("target"+(2-i));
        Debug.Log(target);
        parent = GameObject.Find("UI");       
        Vector3 cord = new Vector3(-1.75f+1.75f*i,1.5f,100);
        new_star = Instantiate(star, cord, Quaternion.identity, parent.transform);
        if (target >= clicks-currentclicks) {
            GameObject bloom = GameObject.Find("Global Volume");
            Animator bloomanim = bloom.GetComponent<Animator>();
            Animator staranim = new_star.GetComponent<Animator>();
            bloomanim.SetTrigger("anim");
            staranim.SetTrigger("anim");
        }          
    }
}
