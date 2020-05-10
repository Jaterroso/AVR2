using System;
using System.Collections;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using UnityEngine;
using UnityEngine.UI;

public class WarriorsLoader : MonoBehaviour
{
    #region Private fields
    private string path = null;
    #endregion

    #region Public properties
    [Header("Warriors")]
    public Warriors warriors;
    public Transform malesRoot;
    public Transform femalesRoot;

    [Header("User interface")]
    public Text titleLabel;
    public Text nameLabel;
    public Text healthLabel;
    public Text strengthLabel;
    public Text speedLabel;
    public Text positionLabel;
    public Text rotationLabel;
    #endregion

    private Warrior selectedWarrior;

    private void Awake()
    {
        path = Application.dataPath + "/Warriors/warriors.txt";
    }

    // Start is called before the first frame update
    void Start()
    {
        StreamReader reader = new StreamReader(path);
        string result = reader.ReadToEnd();
        reader.Close();
        warriors = JsonUtility.FromJson<Warriors>(result);

        foreach (Warrior warrior in warriors.warriors)
        {
            string _prefabPath = "Prefabs/" + warrior.gender + "s/" + warrior.name;
            var _prefab = Resources.Load<GameObject>(_prefabPath) as GameObject;
            warrior.prefab = _prefab;
            warrior.SetPosition();
            warrior.SetRotation();
            var root = (warrior.gender == "Male") ? malesRoot : femalesRoot;
            var instantiatedWarrior = Instantiate(warrior.prefab, root);
            instantiatedWarrior.tag = "Warrior";
            instantiatedWarrior.transform.position = warrior.position;
            instantiatedWarrior.transform.rotation = warrior.rotation;
        }
    }

    // Update is called once per frame
    void Update()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit = new RaycastHit();
        if (Physics.Raycast(ray, out hit))
        {
            if (Input.GetMouseButtonDown(0))
            {
                if (hit.collider.CompareTag("Warrior"))
                {
                    foreach (Warrior warrior in warriors.warriors)
                    {
                        if (hit.collider.name.Contains(warrior.name))
                        {
                            UpdateUI(warrior);
                        }
                    }
                }
                else
                {
                    Debug.Log(hit.collider.name);
                    ResetUI();
                }
            }
        } 
    }

    private void UpdateUI(Warrior _warrior)
    {
        titleLabel.text = "Selected warrior:";
        nameLabel.text = "Name: " + _warrior.name;
        healthLabel.text = "Health: " + _warrior.health.ToString();
        strengthLabel.text = "Strength: " + _warrior.strength.ToString();
        speedLabel.text = "Speed: " + _warrior.speed.ToString();
        positionLabel.text = "Position: " + _warrior.positionStr;
        rotationLabel.text = "Orientation: " + _warrior.rotationStr;

        //titleLabel.gameObject.SetActive(true);
        nameLabel.gameObject.SetActive(true);
        healthLabel.gameObject.SetActive(true);
        strengthLabel.gameObject.SetActive(true);
        speedLabel.gameObject.SetActive(true);
        positionLabel.gameObject.SetActive(true);
        rotationLabel.gameObject.SetActive(true);
    }

    private void ResetUI()
    {
        titleLabel.text = "Select a warrior to display his/her properties.";
        nameLabel.gameObject.SetActive(false);
        healthLabel.gameObject.SetActive(false);
        strengthLabel.gameObject.SetActive(false);
        speedLabel.gameObject.SetActive(false);
        positionLabel.gameObject.SetActive(false);
        rotationLabel.gameObject.SetActive(false);
    }
}