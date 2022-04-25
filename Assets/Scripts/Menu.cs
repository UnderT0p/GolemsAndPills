using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Menu : MonoBehaviour
{
    [SerializeField] private GameObject menu;
    [SerializeField] private GameObject settings;
    private bool approveEsc=false;
    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape)&& !approveEsc)
        {
            approveEsc = true;
            Time.timeScale = 0f;
            menu.SetActive(true);
            Cursor.lockState = CursorLockMode.Confined;
        }
        else if(Input.GetKeyDown(KeyCode.Escape) && approveEsc)
        {
            approveEsc = false;
            Time.timeScale = 1f;
            menu.SetActive(false);
            settings.SetActive(false);
            Cursor.lockState = CursorLockMode.Locked;
        }
    }
    public void Resume()
    {
        Time.timeScale = 1f;
        menu.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
    }
    public void Exit()
    {
        Application.Quit();
    }
   
}
