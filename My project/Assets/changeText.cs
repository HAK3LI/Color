using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class changeText : MonoBehaviour
{
    public Text text;
    public GameObject scaleRef;
    private Animator anim;
    private SpriteRenderer sprite;
    private float H,S,V;
    List<GameObject> close;

    public int clicks = 20;
    public GameObject block;
    private GameObject new_block;
    public GameObject parent;
    public static int width = 10;
    public static int height = 10;
    public Color color1,color2,color3,color4;
    private float scale;
    private int[,] info = new int[width, height];
    // Start is called before the first frame update

    

    void setData(int[,] field) {
            GameObject[] blocks = GameObject.FindGameObjectsWithTag("tile");
            for (int y =0; y < height; ++y) {
                for (int x =0; x < width; ++x) {
                    Data data = blocks[x+y*height+1].GetComponent<Data>();
                    data.setNumber(field[x,y]);
                }
            } 
        }

    int[,] search(int[,] field,int value) {
        void lookAround(int i, int j) {
            if (i!=0) {
                if (value == field[i-1,j]+10) {field[i-1,j] = value; lookAround(i-1,j);}    
            } 
            if (i!=width-1) {
                if (value == field[i+1,j]+10) {field[i+1,j] = value; lookAround(i+1,j);} 
            } 
            if (j!=0) {
                if (value == field[i,j-1]+10) {field[i,j-1] = value; lookAround(i,j-1);} 
            } 
            if (j!=height-1) {
                if (value == field[i,j+1]+10) {field[i,j+1] = value; lookAround(i,j+1);} 
            } 
        }
            
        for (int i =0; i < width; ++i) {
            for (int j =0; j < height; ++j) {
                if (field[i,j]>=10) {
                    lookAround(i,j);
                }
            }
        }
        return field;
    } 


    void Start()
    {
               
        
        for (int y=0; y<height; ++y)
        {
            for (int x=0; x<width; ++x)
            {
                new_block = Instantiate(block, new Vector3(x-width/2+(0.5f*(1-width % 2)),y-height/2+(0.5f*(1-height % 2)),0), Quaternion.identity, parent.transform);
                sprite = new_block.GetComponent<SpriteRenderer>();
                Color randomColor = new Color(0,0,0);
                Data data = new_block.GetComponent<Data>();
                int V;
                switch(Random.Range(0,4)) {
                    case 0:
                        randomColor = color1;
                        V = 0;
                        info[x,y] = V;
                        data.setNumber(V);
                        break;
                        
                    case 1:
                        randomColor = color2;
                        V = 1;
                        info[x,y] = V;
                        data.setNumber(V);
                        break;
                        
                    case 2:
                        randomColor = color3;
                        V = 2;
                        info[x,y] = V;
                        data.setNumber(V);
                        break;
                        
                    case 3:
                        randomColor = color4;
                        V = 3;
                        info[x,y] = V;
                        data.setNumber(V);
                        break;
                    
                    default:
                        break;
                }
                sprite.color = randomColor;
            }
        }
        info[0,height-1] = info[0,height-1]+10;
        info = search(info, info[0,height-1]);
        setData(info);
        scale = 6.0f/width*0.8f;
        parent.transform.localScale = new Vector3(scale, scale, scale);
    }
    
    

    // Update is called once per frame
    void Update()
    {
        void changeData(int[,] field, int value, float startTime) {
            bool animated=false;
            int completed = 0; 
            GameObject[] blocks = GameObject.FindGameObjectsWithTag("tile");
            for (int y =0; y < height; ++y) {
                for (int x =0; x < width; ++x) {
                    Data data = blocks[x+y*height+1].GetComponent<Data>();
                    sprite = blocks[x+y*height+1].GetComponent<SpriteRenderer>();
                    anim = blocks[x+y*height+1].GetComponent<Animator>();
                    data.setNumber(field[x,y]);
                    if (field[x,y]>=10) {
                        completed++;
                        field[x,y] = value;
                        Color randomColor = new Color(0,0,0);
                        float delay = x+height-y;                       
                        switch(field[x,y]) {
                            case 10:
                                StartCoroutine(AnimateImageColor(sprite,sprite.color,color1,delay,anim));
                                break;
                                    
                            case 11:
                                StartCoroutine(AnimateImageColor(sprite,sprite.color,color2,delay,anim));
                                
                                break;
                                    
                            case 12:
                                StartCoroutine(AnimateImageColor(sprite,sprite.color,color3,delay,anim));
                                
                                break;
                                    
                            case 13:
                                StartCoroutine(AnimateImageColor(sprite,sprite.color,color4,delay,anim));
                                
                                break;
                                
                            default:
                                break;
                        }
                        if (completed == height*width && animated==false) {
                            animated=true;
                            GameObject fin = GameObject.Find("Game over");
                            GameObject fin_text = GameObject.Find("Win text");
                            Animator fin_bg = fin.GetComponent<Animator>();
                            Animator fin_text_bg = fin_text.GetComponent<Animator>();
                            fin_bg.SetBool("Game over", true);
                            fin_text_bg.SetBool("Game over", true);
                        }
                    }
                }
            } 
        }
        

        if(Input.GetMouseButtonDown(0)){
            Vector2 mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition);
            GameObject[] gos;
            close = new List<GameObject>();
            gos = GameObject.FindGameObjectsWithTag("control");
            for (int i=0;i < gos.Length;i++) {
                if (Mathf.Abs(gos[i].transform.position.x - mousePosition[0]) < 0.5*scaleRef.transform.localScale.x && Mathf.Abs(gos[i].transform.position.y - mousePosition[1]) < 0.5*scaleRef.transform.localScale.y) {
                    close.Add(gos[i]);
                }
            }
            if (close.Capacity>0 && clicks>0) {
                clicks--;
                text.text = "Кликов осталось:"+clicks;
                Data but = close[0].GetComponent<Data>();
                int butnum = but.getNumber();
                anim = close[0].GetComponent<Animator>();
                anim.SetTrigger("trans");
                if (butnum == 0) {
                    
                    info = search(info, 10);
                    changeData(info, 10, Time.time);
                }
                if (butnum == 1) {
 
                    info = search(info, 11);
                    changeData(info, 11, Time.time);
                }
                if (butnum == 2) {

                    info = search(info, 12);
                    changeData(info, 12, Time.time);
                }
                if (butnum == 3) {

                    info = search(info, 13);
                    changeData(info, 13, Time.time);
                }
                anim.GetCurrentAnimatorStateInfo(0);
            }
        }    
    }

    public IEnumerator AnimateImageColor(SpriteRenderer sprite, Color from,Color to,float delay, Animator anim)
    {
          bool animated=false;
          float ElapsedTime = 0.0f-delay*0.03f;
          float TotalTime = 0.25f;
          while (ElapsedTime < TotalTime) {
              ElapsedTime += Time.deltaTime;
              if (ElapsedTime>=0) {
                  if (animated == false) {
                    anim.SetTrigger("trans");
                    animated=true;
                  }
                  if (ElapsedTime / TotalTime<=0.6) {
                      sprite.color = Color.Lerp(from, Color.Lerp(to, Color.white,1), (ElapsedTime / TotalTime));
                  } else {
                      sprite.color = Color.Lerp(from, to, (ElapsedTime / TotalTime));
                  }
                  
              }
              yield return null;
          }
    }

}
