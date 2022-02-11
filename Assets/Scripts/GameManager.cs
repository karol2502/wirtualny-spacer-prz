using System.Collections;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    public PlayerMovement playerMovementScript;
    public MouseLook mouseLookScript;
    public FocusSwitcher focusSwitcher;
    public GameObject examineLight;

    public GameObject hoverUI;
    public GameObject crosshairUI;
    public GameObject examineUI;

    private GameObject device;
    private GameObject deviceClone;
    private HoverObject hoverObject;
    public bool hovering { get; set; }
    public bool examining { get; set; }

    void SetStatic(Transform trans, bool value)
    {
        trans.gameObject.isStatic = value;

        foreach (Transform child in trans)
        {
            child.gameObject.isStatic = value;

            if (child.childCount != 0)
            {
                SetStatic(child, value);
            }
        }
    }

    void Start()
    {
        QualitySettings.vSyncCount = 0;
        Application.targetFrameRate = 60;

        hovering = false;
        device = null;
        hoverObject = GetComponent<HoverObject>();
    }

    private void Update()
    {
        if(hovering && Input.GetKeyDown(KeyCode.Mouse0))
        {
            examineUI.SetActive(true);
            crosshairUI.SetActive(false);

            examineLight.SetActive(true);

            hoverObject.enabled = false;
            playerMovementScript.enabled = false;
            mouseLookScript.enabled = false;

            hovering = false;

            examining = true;
            //focusSwitcher.SetFocused(device);

            // bez odleglosci
            //device.GetComponent<OutlineInProximity>().enabled = false;

            SetStatic(device.transform, false);
            deviceClone = Instantiate(device, transform);

            SetStatic(device.transform, true);

            focusSwitcher.SetFocused(deviceClone);

            

            deviceClone.GetComponent<ExamineItemController>().enabled = true;

            //device.GetComponent<ExamineItemController>().enabled = true;            
        }

        if(examining && Input.GetKeyDown(KeyCode.Mouse1))
        {
            examineUI.SetActive(false);
            crosshairUI.SetActive(true);

            examineLight.SetActive(false);

            hoverObject.enabled = true;
            playerMovementScript.enabled = true;
            mouseLookScript.enabled = true;

            examining = false;
            focusSwitcher.SetFocused(null);

            //device.GetComponent<OutlineInProximity>().enabled = true;


            deviceClone.GetComponent<ExamineItemController>().enabled = false;
            //SetStatic(device.transform, true);

            Destroy(deviceClone);
        }
                
        if(hovering)
        {
            hoverUI.SetActive(true);
        }
        else
        {
            hoverUI.SetActive(false);
        }        
    }

    public void SetDevice(GameObject gameObject)
    {
        device = gameObject;
    }

    public GameObject GetDevice()
    {
        return device;
    }       

    public void HoverObjectSetActive(bool active)
    {
        hoverObject.enabled = active;
    }

    public void SetHovering(bool active)
    {
        hovering = active;
    }

    public bool GetHovering()
    {
        return hovering;
    }
}
