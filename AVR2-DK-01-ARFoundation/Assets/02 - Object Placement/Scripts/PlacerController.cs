using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class PlacerController : MonoBehaviour
{

    private ARRaycastManager arRaycastManager;
    private GameObject placer;
    private Pose hitPose;

    public Button placerButton;
    public List<GameObject> placedObjects;

    [SerializeField]
    private GameObject placedObjectPrefab;
    [SerializeField]
    private GameObject placedObjectsRoot;
    [SerializeField]
    private List<GameObject> objectsInventory;

    private void Awake()
    {
        arRaycastManager = FindObjectOfType<ARRaycastManager>();
        placer = transform.GetChild(0).gameObject;
        placer.SetActive(false);
        placerButton.interactable = false;
        placedObjects = new List<GameObject>();
    }

    private void Update() 
    {
        List<ARRaycastHit> buttonHits = new List<ARRaycastHit>();
        List<ARRaycastHit> screenHits = new List<ARRaycastHit>();
        arRaycastManager.Raycast(new Vector2(Screen.width / 2, Screen.height / 2), buttonHits, TrackableType.PlaneWithinPolygon);
    
        if (buttonHits.Count > 0)
        {
            placerButton.interactable = true;
            placer.transform.position = buttonHits[0].pose.position;
            placer.transform.rotation = buttonHits[0].pose.rotation;

            if (!placer.activeInHierarchy)
                placer.SetActive(true);
        } else
        {
            placerButton.interactable = false;
            placer.SetActive(false);
        }

        if (!TryGetTouchPosition(out Vector2 touchPosition))
            return;

        /*if (Input.GetTouch(0).phase == TouchPhase.Began)
        {
            if (arRaycastManager.Raycast(touchPosition, screenHits, TrackableType.PlaneWithinPolygon))
            {
                hitPose = screenHits[0].pose;
                PlaceObject("FINGER");
            }
        }*/
    }

    private bool TryGetTouchPosition(out Vector2 touchPosition)
    {
        if (Input.touchCount > 0)
        {
            touchPosition = Input.GetTouch(0).position;
            return true;
        }
        touchPosition = default;
        return false;
    }

    public void PlaceObject(string _placementMode)
    {
        Vector3 position = Vector3.zero;
        Quaternion orientation = Quaternion.identity;
        switch (_placementMode)
        {
            case "PLACER":
                position = placer.transform.position;
                orientation = placer.transform.rotation;
                break;
            case "FINGER":
                position = hitPose.position;
                orientation = hitPose.rotation;
                break;
            default:
                break;
        }
        // Randomly select an object in the objects inventory
        System.Random rnd = new System.Random();
        int id = rnd.Next(0, objectsInventory.Count);
        placedObjectPrefab = objectsInventory[id];
        GameObject placedObject = Instantiate(
            placedObjectPrefab,
            position,
            orientation,
            placedObjectsRoot.transform);
        placedObject.transform.parent = placedObjectsRoot.transform;
        placedObjects.Add(placedObject);
    }
}
