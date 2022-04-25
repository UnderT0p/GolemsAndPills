using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformButton : MonoBehaviour
{
    [SerializeField] private GameObject door;
    private float speed = 1;
    private bool open = false;
    private bool soundPlay = true;
    private void Update()
    {
        if (open)
        {
            Vector3 pos = gameObject.transform.position;
            if (!(pos.y<=-0.2))
            {
                
                pos.y -= speed * Time.deltaTime;
                gameObject.transform.position = pos;
            }
            else
            {
                open = false;
            }
        }
    }
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            open = true;
            door.SetActive(false);
            
            if (soundPlay)
            {
                gameObject.GetComponent<AudioSource>().Play();
                soundPlay = false;
            }
            
        }
    }
}
