using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using TMPro;
using UnityEditor;
using UnityEngine;
using UnityEngine.UI;

public class ObjectiveManager : MonoBehaviour
{
    public static ObjectiveManager Instance = null;
    private Objective[] allObjectives = null;
    [SerializeField] private GameObject objectiveListPanel = null, winPanel;
    [SerializeField] private TextMeshProUGUI objectiveText = null;

    [Header("Chet Console")]
    [SerializeField] private GameObject chetConsole = null;
    [SerializeField] private TMP_InputField input = null;
    [SerializeField] private Button enterButton;

    // Start is called before the first frame update
    private void Awake()
    {
        if (Instance is null) Instance = this;
        allObjectives = Resources.LoadAll<Objective>("Data/Objectives");
        for (int i = 0; i < allObjectives.Length; i++)
        {
            allObjectives[i].objectiveStatus = false;
        }
        chetConsole.SetActive(false);
        winPanel.SetActive(false);
    }

    private void Start()
    {
        if (objectiveListPanel != null)
        {
            for (int i = 0; i < allObjectives.Length; i++)
            {
                var thisObj = Instantiate(objectiveText, objectiveListPanel.transform);
                thisObj.text = allObjectives[i].objectiveName;
                thisObj.name = allObjectives[i].objectiveName;
                thisObj.fontSize = 12f;
            }
        }
        
        if(enterButton != null)
            enterButton.onClick.AddListener(() => SetObjectiveStatus(input.text, true));
    }

    public void SetObjectiveStatus(string objectiveName, bool status)
    {
        foreach (var t in allObjectives)
        {
            if (t.objectiveName != objectiveName) continue;
            t.objectiveStatus = status;
            for (int i = 0; i < objectiveListPanel.transform.childCount; i++)
            {
                if (objectiveListPanel.transform.GetChild(i).name == t.objectiveName)
                    objectiveListPanel.transform.GetChild(i).GetComponent<TextMeshProUGUI>().fontStyle =
                        FontStyles.Strikethrough;
            }
        }
    }

    private void WinState()
    {
        foreach (var t in allObjectives)
        {
            if (!(t.objectiveStatus is true)) continue;
            Time.timeScale = 0;
            winPanel.SetActive(true);
        }
    }
#if UNITY_EDITOR
    private void Update()
    {
        if (Input.GetKeyUp(KeyCode.KeypadEnter))
        {
            chetConsole.SetActive(true);
            Cursor.lockState = CursorLockMode.None;
            Cursor.visible = true;
        }
        
    }
#endif
}