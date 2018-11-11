using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

class RBCTools : EditorWindow
{
    int c_select = 0;
    string[] c_options = new string[]
    { "Change name",
      "Update tags",
      "Material change",
      "Add component",
      "Remove component",
    };

    int curSearchOption = 0;
    string[] searchOption = new string[]
    {"Exact name", "Contains string"};

    string ObjecToSearch = "";
    string NewName = "";
    bool IncludeChilds = false;
    int SelectChild = 0;

    bool korjaus = true;
    bool skorjaus = true;
    public GameObject[] EsineHaku;
    public List<Transform> TempObjects = new List<Transform>();

    [MenuItem("RunByCoffee Tools/RBC Tools")]

    public static void ShowWindow()
    {
        GetWindow(typeof(RBCTools));
    }

    void OnGUI()
    {
        GUILayout.Space(20);
        c_select = EditorGUILayout.Popup("Select Function", c_select, c_options);
        GUILayout.Space(10);
        curSearchOption = EditorGUILayout.Popup("Search Options", curSearchOption, searchOption);
        GUILayout.Space(10);

        if (c_select == 0) 
        {
            GUILayout.Label("CHANGE GAMEOBJECT(S) NAME", EditorStyles.boldLabel);
            GUILayout.Space(10);
            ObjecToSearch = EditorGUILayout.TextField("Search by name", ObjecToSearch);
            GUILayout.Space(5);
            NewName = EditorGUILayout.TextField("Set new name", NewName);
            GUILayout.Space(5);
            IncludeChilds = EditorGUILayout.Toggle("Has childs ?", IncludeChilds);
            GUILayout.Space(5);
            SelectChild = EditorGUILayout.IntField("Child index", SelectChild);

            GUILayout.Space(10);
            GUI.contentColor = Color.white;
            GUI.backgroundColor = Color.grey;
        }

        if (GUILayout.Button("START", GUILayout.Width(96), GUILayout.Height(36)))
        {
            if (c_select == 0) UpdateItemName(ObjecToSearch, NewName, IncludeChilds, SelectChild);
        }
    }

    public void UpdateItemName(string itemSearch, string newName, bool includeChilds, int selectChild)
    {
        if (c_select == 0) 
        {            
            TempObjects.Clear(); // Let's clear temporary item list
            EsineHaku = FindObjectsOfType<GameObject>();

            if (includeChilds) 
            {
                if (curSearchOption == 0) 
                {
                    foreach (GameObject go in EsineHaku) 
                    {
                        if (go.transform.name == itemSearch && go.transform.childCount == 0) 
                        {
                            Debug.Log("Muffinssi: " + go.transform.name);
                            // TempObjects.Add(go.transform);  // Add found items into temporary item list
                        }
                    }
                }

                else if (curSearchOption == 1) 
                {
                    if (itemSearch.Length < 4)
                    {
                        Debug.LogWarning("Item name must be at least 4 characters"); return;
                    }

                    foreach (GameObject go in EsineHaku) 
                    {
                        if (go.transform.name.Contains(itemSearch) && go.transform.childCount == 0) 
                        {

                            Debug.Log("Muffinssi: " + go.transform.name);
                            // TempObjects.Add(go.transform);  // Add found items into temporary item list
                        }
                    }
                }

                foreach (Transform go in TempObjects) { // Loop items from temporary item list
                    go.gameObject.transform.name = newName; // Change GameObject (defined as go) name with the new defined name
                }

                return;
            }

            else if (!includeChilds) 
            {
                if (curSearchOption == 0)
                {
                    foreach (GameObject go in EsineHaku)
                    {
                        if (go.transform.name == itemSearch && go.transform.childCount == 0)
                        {
                            Debug.Log("Objekti: " + go.transform.name);
                            // TempObjects.Add(go.transform);  // Add found items into temporary item list
                        }
                    }
                }

                if (curSearchOption == 1)
                {
                    if (itemSearch.Length < 4) 
                    {
                        Debug.LogWarning("Item name must be at least 4 characters"); return;
                    }


                    foreach (GameObject go in EsineHaku)
                    {
                        if (go.transform.name.Contains(itemSearch) && go.transform.childCount == 0)
                        {
                            Debug.Log("Objekti: " + go.transform.name);
                            // TempObjects.Add(go.transform);  // Add found items into temporary item list
                        }
                    }
                }

                foreach (Transform go in TempObjects)
                { // Loop items from temporary item list
                    Debug.Log("Objekti: " + go.transform.name);
                    // TempObjects.Add(go.transform);  // Add found items into temporary item list
                }
            }

            return;
        }
    }
}