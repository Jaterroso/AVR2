using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectManipulator : MonoBehaviour
{
    private GameObject selectedObject;
    private Camera arCamera;
    private float rotationSpeedModifier = 0.2f;
    private bool wasScaledLastFrame;
    private Vector2[] lastScalePositions;
    private float scaleSpeed = 0.001f;

    [SerializeField]
    private GameObject manipulatorObject;

    // Start is called before the first frame update
    void Awake()
    {
        arCamera = FindObjectOfType<Camera>();
        manipulatorObject.SetActive(false);
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.touches.Length == 0)
            return;

        if (Input.touches.Length == 1)
        {
            Touch touch = Input.GetTouch(0);
            Ray ray = arCamera.ScreenPointToRay(touch.position);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit))
            {
                if (touch.phase == TouchPhase.Began)
                {
                    if (hit.collider.gameObject.CompareTag("Character"))
                    {
                        selectedObject = hit.collider.gameObject;
                        manipulatorObject.transform.position = selectedObject.transform.position;
                        manipulatorObject.SetActive(true);
                    }
                }
                if (touch.phase == TouchPhase.Moved && selectedObject != null)
                {
                    var rotationY = Quaternion.Euler(
                        0.0f,
                        -touch.deltaPosition.x * rotationSpeedModifier,
                        0.0f
                    );
                    selectedObject.transform.rotation = rotationY * selectedObject.transform.rotation;
                }
            }
        }
        if (Input.touches.Length == 2) 
        {
            Touch[] scaleTouches = Input.touches;
            if (scaleTouches[0].phase == TouchPhase.Moved &&
                scaleTouches[1].phase == TouchPhase.Moved &&
                selectedObject != null)
            {
                Vector2[] newPositions = new Vector2[]
                {
                    scaleTouches[0].position,
                    scaleTouches[1].position
                };
                if (!wasScaledLastFrame)
                {
                    lastScalePositions = newPositions;
                    wasScaledLastFrame = true;
                } else
                {
                    float newDistance = Vector2.Distance(newPositions[0], newPositions[1]);
                    float oldDistance = Vector2.Distance(lastScalePositions[0], lastScalePositions[1]);
                    float offset = newDistance - oldDistance;

                    ScaleObject(selectedObject.transform, offset, scaleSpeed);

                    lastScalePositions = newPositions;

                }
            }
        }
    }

    private void ScaleObject(Transform _transform, float _offset, float _speed)
    {
        _transform.localScale += transform.localScale * _offset * _speed;
    }

}
