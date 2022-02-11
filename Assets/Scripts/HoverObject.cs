using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HoverObject : MonoBehaviour
{
    private GameManager gameManager;
    private GameObject device;

    public float distance = 3.5f;

    void Start()
    {
        gameManager = GameObject.Find("GameManager").GetComponent<GameManager>();
    }

    // Update is called once per frame
    void Update()
    {
        RaycastHit hit;
        device = gameManager.GetDevice();

        if (Physics.Raycast(Camera.main.transform.position, Camera.main.transform.forward, out hit, distance))
        {
            if (hit.collider.tag == "Device")
            {
                gameManager.SetHovering(true);
                device = hit.collider.gameObject;                
                //device.GetComponent<Outline>().enabled = true;
                //device.GetComponent<Outline>().OutlineColor = Color.blue;                
            }
            else if (device != null)
            {
                gameManager.SetHovering(false);
                //device.GetComponent<Outline>().enabled = false;
                device = null;
            }            
        }
        else
        {
            if (device != null)
            {
                gameManager.SetHovering(false);
                //device.GetComponent<Outline>().enabled = false;
                device = null;
            }
        }

        gameManager.SetDevice(device);
    }
    
    private void OnDisable()
    {
        gameManager.SetHovering(false);
        if (device != null)
        {
            //device.GetComponent<Outline>().enabled = false;
            device = null;
        }        
    }
    
}
