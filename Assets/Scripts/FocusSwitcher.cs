using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FocusSwitcher : MonoBehaviour
{
    public string focusedLayer = "Focused";

    private GameObject currentlyFocused;
    private int previousLayer;

    void SetChildLayers(Transform trans, int layer)
    {
        foreach (Transform child in trans)
        {
            child.gameObject.layer = layer;

            if (child.childCount != 0)
            {
                SetChildLayers(child, layer);
            }
        }
    }

    public void SetFocused(GameObject obj)
    {
        // enables this camera and the postProcessingVolume which is the child
        gameObject.SetActive(true);

        // if something else was focused before reset it
        if (currentlyFocused)
        {
            SetPreviousLayer();
        }

        // store and focus the new object
        currentlyFocused = obj;

        if (currentlyFocused)
        {     
            previousLayer = currentlyFocused.layer;
            currentlyFocused.layer = LayerMask.NameToLayer(focusedLayer);

            SetChildLayers(currentlyFocused.transform, LayerMask.NameToLayer(focusedLayer));
        }
        else
        {
            // if no object is focused disable the FocusCamera
            // and PostProcessingVolume for not wasting rendering resources
            gameObject.SetActive(false);
        }
    }

    // On disable make sure to reset the current object
    private void OnDisable()
    {
        if (currentlyFocused)
        {
            SetPreviousLayer();
        }            

        currentlyFocused = null;
    }

    private void SetPreviousLayer()
    {        
        currentlyFocused.layer = previousLayer;

        SetChildLayers(currentlyFocused.transform, previousLayer);
    }
}
