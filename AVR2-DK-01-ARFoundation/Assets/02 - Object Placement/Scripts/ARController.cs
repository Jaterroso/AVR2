using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class ARController : MonoBehaviour
{
    private ARPlaneManager arPlaneManager;

    [SerializeField]
    private Toggle arPlaneDetectionToggle;
    [SerializeField]
    private Toggle arPlaneDisplayToggle;

    private void Awake()
    {
        arPlaneManager = FindObjectOfType<ARPlaneManager>();
    }

    public void TogglePlaneDetection()
    {
        arPlaneManager.enabled = !arPlaneManager.enabled;
    }

    public void TogglePlaneDisplay()
    {
        foreach(ARPlane plane in arPlaneManager.trackables)
        {
            plane.gameObject.SetActive(arPlaneDisplayToggle.isOn);
        }
    }
}
