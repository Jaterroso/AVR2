using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using UnityEngine;

public class Controller : MonoBehaviour
{
    [DllImport("__Internal")]
    private static extern void Hello();

    public GameObject[] spheres;

    // Start is called before the first frame update
    void Start()
    {
        Hello();
        SelectSphere("-1");
    }

    public void SelectSphere(string _sphereNumber)
    {
        if (_sphereNumber != "-1")
        {
            // Disabling all the spheres
            foreach (GameObject _sphere in spheres)
                _sphere.SetActive(false);

            spheres[int.Parse(_sphereNumber)].SetActive(true);
        }
        else
            foreach (GameObject _sphere in spheres) _sphere.SetActive(true);

    }
}
