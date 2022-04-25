using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RenderUIText : MonoBehaviour
{
    private bool showWinScreen = false;
    private bool showLossScreen = false;
    public string labelText = "Collect all 4 item and win your freedom!";
    

    private void OnGUI()
    {
        GUI.Box(new Rect(20, 20, 150, 25), "Player Health: " + gameObject.GetComponent<GameBehavior>().PlayerHP);
        GUI.Box(new Rect(20, 50, 150, 25), "Items Collected: " + gameObject.GetComponent<GameBehavior>().Items+"/4");
        GUI.Label(new Rect(Screen.width / 2 - 100, Screen.height - 50, 300, 50), labelText);
        if (showWinScreen)
        {
            Cursor.lockState = CursorLockMode.Confined;
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "YOU WON!!"))//GUI.Button возвращает true если нажата кнопка,
                                                                                                            //которая рисуется прям в условии с пом. new Rect
            {
                gameObject.GetComponent<GameBehavior>().RestartLevel();
            }
        }
        if (showLossScreen)
        {
            Cursor.lockState = CursorLockMode.Confined;
            if (GUI.Button(new Rect(Screen.width / 2 - 100, Screen.height / 2 - 50, 200, 100), "You lose..."))
            {
                gameObject.GetComponent<GameBehavior>().RestartLevel();
            }
        }
    }
    public void InsertlabelTextAndTimeScale(string Text, bool showWinScreen=false, bool showLossScreen=false)
    {
        this.showWinScreen = showWinScreen;
        this.showLossScreen = showLossScreen;
        float timeScale=0f;
        if (showWinScreen)
        {
            labelText = Text;
        }
        else if (showLossScreen)
        {
            labelText = Text;
        }
        else
        {
            labelText = Text;
            timeScale = 1f;
        }
        Time.timeScale = timeScale;
    }
   
}
