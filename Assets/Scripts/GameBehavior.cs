using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameBehavior : MonoBehaviour
{

    [SerializeField] private GameObject secretDoor;
    private int maxItem = 4;
    public static int playerDeaths = 0;
    private int itemCollected = 0;
    
    public static GameBehavior gameBehavior;
    public static GameBehavior GetInstance() { return gameBehavior; }
    


    public int Items
    {
        get { return itemCollected; }
        set { itemCollected = value;
            if (itemCollected>=maxItem)
            {
                gameObject.GetComponent<RenderUIText>().InsertlabelTextAndTimeScale( "You've found all the items!",true,false); 
            }
            else
            {
                gameObject.GetComponent<RenderUIText>().InsertlabelTextAndTimeScale("Item found, only " + (maxItem - itemCollected) + " more to go!");
                if (itemCollected==3)
                {
                    secretDoor.GetComponent<AudioSource>().Play();
                    Invoke(nameof(SecretDoorOpen), 3f); 
                }
            }
        }
    }

    private int playerHP=10;

    public int PlayerHP
    {
        get { return playerHP; }
      private  set { playerHP = value;
            if (playerHP<=0)
            {
                gameObject.GetComponent<RenderUIText>().InsertlabelTextAndTimeScale("You want another life with that?",false,true);
                
            }
            else
            {
                gameObject.GetComponent<RenderUIText>().InsertlabelTextAndTimeScale("Ouch.. that's got hurt.");
            };
        }
    }
    private float volume=0.5f;

    public float Volume
    {
        get { return volume; }
        set
        {
            if (value<=1f)
            {
                volume = value;
            }  
        }
    }
    private float sensetivity=250f;

    public float Sensetivity
    {
        get { return sensetivity; }
        set {
            if (value <= 500f&&value>0)
            {
                sensetivity = value;
            }
        }
    }


    private void Awake()
    {
        //DontDestroyOnLoad(gameObject); // ≈сли нужны будут другие уровни - убрать обьекты из манагера(придумать другие зависимости) и включить это
        if (gameBehavior == null)
        {
            gameBehavior = this;
            
        }
        //else if (gameBehavior != this)
        //{
            
        //    Destroy(gameObject);
            
        //}
    }
    public void RestartLevel(int sceneIndex=0)
    {
        itemCollected = 0;
        playerHP = 10;
        gameObject.GetComponent<RenderUIText>().InsertlabelTextAndTimeScale("Collect all 4 item and win your freedom!");
        SceneManager.LoadScene(sceneIndex);
        Time.timeScale = 1f;
    }
    public void DecrementPlayerHP()
    {
        PlayerHP--;
    }
    public void IncrementItemCollected()
    {
        Items++;
    }
    private void SecretDoorOpen()
    {
        secretDoor.SetActive(false);
    }

}
