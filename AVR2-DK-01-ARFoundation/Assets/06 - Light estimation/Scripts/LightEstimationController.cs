using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.XR.ARFoundation;

public class LightEstimationController : MonoBehaviour
{
    [SerializeField]
    private ARCameraManager arCameraManager;
    [SerializeField]
    private Text brightnessValue;
    [SerializeField]
    private Text temperatureValue;
    [SerializeField]
    private Text colorCorrectionValue;

    private PlacerController placerController;

    public List<Light> lights;

    void Awake()
    {
        placerController = FindObjectOfType<PlacerController>();
    }

    private void OnEnable()
    {
        arCameraManager.frameReceived += FrameUpdated;
    }

    private void OnDisable()
    {
        arCameraManager.frameReceived -= FrameUpdated;
    }

    private void FrameUpdated(ARCameraFrameEventArgs args)
    {
        if (args.lightEstimation.averageBrightness.HasValue)
        {
            brightnessValue.text = $"Brightness: {args.lightEstimation.averageBrightness.Value}";
            placerController.placedObjects[0].GetComponent<Animator>().SetFloat("AverageBrightness", args.lightEstimation.averageBrightness.Value);
            foreach(Light light in lights)
            {
                light.intensity = args.lightEstimation.averageBrightness.Value;
            }
        }
        if (args.lightEstimation.averageColorTemperature.HasValue)
        {
            temperatureValue.text = $"Color temperature: {args.lightEstimation.averageColorTemperature.Value}";
        }
        if (args.lightEstimation.colorCorrection.HasValue)
        {
            colorCorrectionValue.text = $"Color correction: {args.lightEstimation.colorCorrection.Value}";
        }
    }
}
