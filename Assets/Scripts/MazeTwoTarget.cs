using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MazeTwoTarget : MonoBehaviour
{
    [SerializeField] private GameObject lastItem;
    [SerializeField] private GameObject redDoor;
    private bool appruveDown = false;
    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Bullet")&&!appruveDown)
        {
            appruveDown = true;
            Vector3 pos = lastItem.transform.position;
            pos.y -= 3.4f;
            lastItem.transform.position = pos;
            redDoor.SetActive(false);
            gameObject.GetComponent<AudioSource>().Play();

        }
    }
}
