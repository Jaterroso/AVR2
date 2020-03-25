using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

[Serializable]
public class FaceMaterial
{
    public Material material;
    public string name;
}

public class FaceController : MonoBehaviour
{

    [SerializeField]
    private Toggle faceTrackingToggle;
    [SerializeField]
    private Button swapFacesButton;
    [SerializeField]
    private FaceMaterial[] faceMaterials;

    private ARFaceManager arFaceManager;
    private bool faceTrackingOn = true;
    private int swapCounter = 0;

    // Start is called before the first frame update
    void Awake()
    {
        arFaceManager = GetComponent<ARFaceManager>();
        arFaceManager.facePrefab.GetComponent<MeshRenderer>().material = faceMaterials[0].material;
        swapFacesButton.GetComponentInChildren<Text>().text = $"Swap faces ({faceMaterials[0].name})";
        faceTrackingToggle.GetComponentInChildren<Text>().text = $"Face tracking {(arFaceManager.enabled ? "ON" : "OFF")}";
    }

    public void SwapFaces()
    {
        swapCounter = (swapCounter == faceMaterials.Length - 1) ? 0 : swapCounter + 1;
        /*if (swapCounter == faceMaterials.Length - 1)
            swapCounter = 0;
        else
            swapCounter++;
        */
        foreach(ARFace face in arFaceManager.trackables)
            face.GetComponent<MeshRenderer>().material = faceMaterials[swapCounter].material;

        swapFacesButton.GetComponentInChildren<Text>().text = $"Swap faces ({faceMaterials[swapCounter].name})";
        //swapFacesButton.GetComponentInChildren<Text>().text = "Swap faces (" + faceMaterials[swapCounter].name + ")";
    }

    public void ToggleTrackingFaces()
    {
        faceTrackingOn = !faceTrackingOn;
        arFaceManager.enabled = faceTrackingOn;
        faceTrackingToggle.GetComponentInChildren<Text>().text = $"Face tracking {(arFaceManager.enabled ? "ON" : "OFF")}";
    }
}
