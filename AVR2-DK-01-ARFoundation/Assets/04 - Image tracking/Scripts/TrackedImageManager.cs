using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.XR.ARFoundation;
using UnityEngine.UI;

public class TrackedImageManager : MonoBehaviour
{
    private ARTrackedImageManager arTrackedImageManager;

    [SerializeField]
    private GameObject[] arObjectsToPlace;
    [SerializeField]
    private Dictionary<string, GameObject> arObjects = new Dictionary<string, GameObject>();
    [SerializeField]
    private Text trackedImageText;
    [SerializeField]
    private Vector3 scaleFactor = new Vector3(0.1f, 0.1f, 0.1f);


    // Start is called before the first frame update
    private void Awake()
    {
        arTrackedImageManager = GetComponent<ARTrackedImageManager>();

        foreach (GameObject _arObject in arObjectsToPlace)
        {
            GameObject newARObject = Instantiate(_arObject, Vector3.zero, Quaternion.identity);
            newARObject.name = _arObject.name;
            arObjects.Add(_arObject.name, newARObject);
        }
    }

    private void OnEnable()
    {
        arTrackedImageManager.trackedImagesChanged += OnTrackedImagesChanged;
    }

    private void OnDisable()
    {
        arTrackedImageManager.trackedImagesChanged -= OnTrackedImagesChanged;
    }



    private void OnTrackedImagesChanged(ARTrackedImagesChangedEventArgs eventArgs)
    {
        foreach (ARTrackedImage trackedImage in eventArgs.added)
            UpdateARImage(trackedImage);

        foreach (ARTrackedImage trackedImage in eventArgs.updated)
            UpdateARImage(trackedImage);

        foreach (ARTrackedImage trackedImage in eventArgs.removed)
            arObjects[trackedImage.name].SetActive(false);
        
    }

    private void UpdateARImage(ARTrackedImage _trackedImage)
    {
        trackedImageText.text = _trackedImage.referenceImage.name;
        AssignGameObject(_trackedImage.referenceImage.name, _trackedImage.transform.position);
    }

    private void AssignGameObject(string _name, Vector3 _newPosition)
    {
        if (arObjectsToPlace != null)
        {
            GameObject _go = arObjects[_name];
            _go.SetActive(true);
            _go.transform.position = _newPosition;
            //_go.transform.localScale = scaleFactor;
            foreach( GameObject go in arObjects.Values)
            {
                if (go.name != _name)
                {
                    go.SetActive(false);
                }
            }
        }
    }

}
