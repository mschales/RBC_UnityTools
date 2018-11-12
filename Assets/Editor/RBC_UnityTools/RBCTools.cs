using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

class RBCTools : EditorWindow {
    int c_select = 0;
    string[] c_options = new string[] { "Change name", "Update tags", "Update Static option", "Remove Component" };
    int curSearchOption = 0;
    string[] searchOption = new string[] { "Exact name", "Contains string" };
    int curComponent = 0;
    public string[] component_list = new string[] { "AudioSource", "Animator", "BoxCollider"};
    string ObjecToSearch = "";
    string NewName = "";
    string NewTag = "";
    bool ExcludeParents = false;
    string CustomSuffix = "";
    bool SetStatic = false;
    GameObject[] FoundGameObjects;
    List<Transform> TempObjects = new List<Transform> ();
    [MenuItem ("RBC Tools/Open Window")]

    public static void ShowWindow () {
        GetWindow (typeof (RBCTools));
    }

    void OnGUI () {
        GUILayout.Space (20);
        c_select = EditorGUILayout.Popup ("Select Function", c_select, c_options);
        GUILayout.Space (10);
        curSearchOption = EditorGUILayout.Popup ("Search Options", curSearchOption, searchOption);
        GUILayout.Space (10);

        if (c_select == 0) {
            GUILayout.Label ("UPDATE GAMEOBJECT(S) NAME", EditorStyles.boldLabel);
            GUILayout.Space (10);
            ObjecToSearch = EditorGUILayout.TextField ("Search by name", ObjecToSearch);
            GUILayout.Space (5);
            NewName = EditorGUILayout.TextField ("Set new name", NewName);
            GUILayout.Space (5);
            ExcludeParents = EditorGUILayout.Toggle ("Exclude parents", ExcludeParents);
            GUILayout.Space (5);
            CustomSuffix = EditorGUILayout.TextField ("Custom Suffix", CustomSuffix);
            GUILayout.Space (10);
            GUI.backgroundColor = Color.grey;
        }

        else if (c_select == 1) {
            GUILayout.Label ("UPDATE GAMEOBJECT(S) TAG", EditorStyles.boldLabel);
            GUILayout.Space (10);
            ObjecToSearch = EditorGUILayout.TextField ("Search by name", ObjecToSearch);
            GUILayout.Space (5);
            ExcludeParents = EditorGUILayout.Toggle ("Exclude parents", ExcludeParents);
            GUILayout.Space (5);
            NewTag = EditorGUILayout.TagField ("Set new tag", NewTag);
            GUILayout.Space (10);
            GUI.backgroundColor = Color.grey;
        }

        else if (c_select == 2) {
            GUILayout.Label ("UPDATE GAMEOBJECT(S) STATIC OPION", EditorStyles.boldLabel);
            GUILayout.Space (10);
            ObjecToSearch = EditorGUILayout.TextField ("Search by name", ObjecToSearch);
            GUILayout.Space (5);
            ExcludeParents = EditorGUILayout.Toggle ("Exclude parents", ExcludeParents);
            GUILayout.Space (5);
            SetStatic = EditorGUILayout.Toggle ("Set object as static", SetStatic);
            GUILayout.Space (10);
            GUI.backgroundColor = Color.grey;
        }
       
        else if (c_select == 3) {
            GUILayout.Label ("REMOVE COMPONENT FROM GAMEOBJECT(S)", EditorStyles.boldLabel);
            GUILayout.Space (10);
            ObjecToSearch = EditorGUILayout.TextField ("Search by name", ObjecToSearch);
            GUILayout.Space (5);
            ExcludeParents = EditorGUILayout.Toggle ("Exclude parents", ExcludeParents);
            GUILayout.Space (5);
            curComponent = EditorGUILayout.Popup ("Select Function", curComponent, component_list);
            GUILayout.Space (10);
            GUI.backgroundColor = Color.grey;
        }

        if (GUILayout.Button ("START", GUILayout.Width (96), GUILayout.Height (36))) {
            if (c_select == 0) UpdateItemName (ObjecToSearch, NewName, ExcludeParents, CustomSuffix);
            else if (c_select == 1) UpdateTags (ObjecToSearch, NewTag, ExcludeParents);
            else if (c_select == 2) UpdateStatic (ObjecToSearch, SetStatic, ExcludeParents);
            else if (c_select == 3) UpdateComponent (ObjecToSearch, curComponent, ExcludeParents);
        }
    }

    public void UpdateItemName (string itemSearch, string newName, bool excludeParents, string customSuffix) {
        TempObjects.Clear (); // Let's clear temporary item list
        FoundGameObjects = FindObjectsOfType<GameObject> (); // Add scene gameobjects into list

        if (curSearchOption == 0) {
            if (itemSearch.Length < 2) {
                Debug.LogError ("Item name must be at least 2 characters"); return;
            }

            for (int x = 0; x < FoundGameObjects.Length; x++) {
                if (FoundGameObjects[x].transform.name == itemSearch && FoundGameObjects[x].transform.childCount == 0) {
                    if (excludeParents) {
                        if (FoundGameObjects[x].transform.parent == null) TempObjects.Add (FoundGameObjects[x].transform); // Add found items (without parent) into temporary item list
                    }

                    else {
                        TempObjects.Add (FoundGameObjects[x].transform);  // Add found items into temporary item list
                    }
                }
            }
        }

        else if (curSearchOption == 1) {
            if (itemSearch.Length < 2) {
                Debug.LogError ("Item name must be at least 2 characters"); return;
            }

            for (int x = 0; x < FoundGameObjects.Length; x++) {
                if (FoundGameObjects[x].transform.name.Contains (itemSearch) && FoundGameObjects[x].transform.childCount == 0) {
                    if (excludeParents) {
                        if (FoundGameObjects[x].transform.parent == null) TempObjects.Add (FoundGameObjects[x].transform);
                    }

                    else {
                        TempObjects.Add (FoundGameObjects[x].transform);  // Add found items into temporary item list
                    }
                }
            }
        }

        for (int x = 0; x < TempObjects.Count; x++) { // Loop items from temporary item list
            TempObjects[x].gameObject.transform.name = newName; // Change GameObject name with the new defined name
            if (CustomSuffix.Length > 1) {
                TempObjects[x].gameObject.transform.name = NewName + " " + customSuffix.Replace ("%n", x.ToString ());
            }
        }
    }

    public void UpdateTags (string itemSearch, string newTag, bool excludeParents) {
        TempObjects.Clear (); // Let's clear temporary item list
        FoundGameObjects = FindObjectsOfType<GameObject> (); // Add scene gameobjects into list

        if (curSearchOption == 0) {
            for (int x = 0; x < FoundGameObjects.Length; x++) {
                if (FoundGameObjects[x].transform.name == itemSearch && FoundGameObjects[x].transform.childCount == 0) {
                    if (excludeParents) {
                        if (FoundGameObjects[x].transform.parent == null) TempObjects.Add (FoundGameObjects[x].transform); // Add found items (without parent) into temporary item list
                    }

                    else {
                        TempObjects.Add (FoundGameObjects[x].transform);  // Add found items into temporary item list
                    }
                }
            }
        }

        else if (curSearchOption == 1) {
            if (itemSearch.Length < 2) {
                Debug.LogWarning ("Item name must be at least 2 characters"); return;
            }

            for (int x = 0; x < FoundGameObjects.Length; x++) {
                if (FoundGameObjects[x].transform.name.Contains (itemSearch) && FoundGameObjects[x].transform.childCount == 0) {
                    if (excludeParents) {
                        if (FoundGameObjects[x].transform.parent == null) TempObjects.Add (FoundGameObjects[x].transform); // Add found items (without parent) into temporary item list
                    }

                    else {
                        TempObjects.Add (FoundGameObjects[x].transform);  // Add found items into temporary item list
                    }
                }
            }
        }

        for (int x = 0; x < TempObjects.Count; x++) { // Loop items from temporary item list
            TempObjects[x].gameObject.transform.tag = newTag; // Change GameObject (defined as go) name with the new defined name
        }
    }

    public void UpdateStatic (string itemSearch, bool setStatic, bool excludeParents) {
        TempObjects.Clear (); // Let's clear temporary item list
        FoundGameObjects = FindObjectsOfType<GameObject> (); // Add scene gameobjects into list

        if (curSearchOption == 0) {
            if (itemSearch.Length < 2) {
                Debug.LogError ("Item name must be at least 2 characters"); return;
            }

            for (int x = 0; x < FoundGameObjects.Length; x++) {
                if (FoundGameObjects[x].transform.name == itemSearch && FoundGameObjects[x].transform.childCount == 0) {
                    if (excludeParents) {
                        if (FoundGameObjects[x].transform.parent == null) TempObjects.Add (FoundGameObjects[x].transform); // Add found items (without parent) into temporary item list
                    }

                    else {
                        TempObjects.Add (FoundGameObjects[x].transform);  // Add found items into temporary item list
                    }
                }
            }
        }

        else if (curSearchOption == 1) {
            if (itemSearch.Length < 2) {
                Debug.LogWarning ("Item name must be at least 2 characters"); return;
            }

            for (int x = 0; x < FoundGameObjects.Length; x++) {
                if (FoundGameObjects[x].transform.name.Contains (itemSearch) && FoundGameObjects[x].transform.childCount == 0) {
                    if (excludeParents) {
                        if (FoundGameObjects[x].transform.parent == null) TempObjects.Add (FoundGameObjects[x].transform); // Add found items (without parent) into temporary item list
                    }

                    else {
                        TempObjects.Add (FoundGameObjects[x].transform);  // Add found items into temporary item list
                    }
                }
            }
        }

        for (int x = 0; x < TempObjects.Count; x++) { // Loop items from temporary item list
            TempObjects[x].gameObject.isStatic = setStatic; // Change GameObject (defined as go) name with the new defined name
        }
    }

    public void UpdateComponent (string itemSearch, int curComponent, bool excludeParents) {
        TempObjects.Clear (); // Let's clear temporary item list
        FoundGameObjects = FindObjectsOfType<GameObject> (); // Add scene gameobjects into list

        if (curSearchOption == 0) {
            if (itemSearch.Length < 2) {
                Debug.LogError ("Item name must be at least 2 characters"); return;
            }

            for (int x = 0; x < FoundGameObjects.Length; x++) {
                if (FoundGameObjects[x].transform.name == itemSearch && FoundGameObjects[x].transform.childCount == 0) {
                    if (excludeParents) {
                        if (FoundGameObjects[x].transform.parent == null) TempObjects.Add (FoundGameObjects[x].transform); // Add found items (without parent) into temporary item list
                    }

                    else {
                        TempObjects.Add (FoundGameObjects[x].transform);  // Add found items into temporary item list
                    }
                }
            }
        }

        else if (curSearchOption == 1) {
            if (itemSearch.Length < 2) {
                Debug.LogWarning ("Item name must be at least 2 characters"); return;
            }

            for (int x = 0; x < FoundGameObjects.Length; x++) {
                if (FoundGameObjects[x].transform.name.Contains (itemSearch) && FoundGameObjects[x].transform.childCount == 0) {
                    if (excludeParents) {
                        if (FoundGameObjects[x].transform.parent == null) TempObjects.Add (FoundGameObjects[x].transform); // Add found items (without parent) into temporary item list
                    }

                    else {
                        TempObjects.Add (FoundGameObjects[x].transform);  // Add found items into temporary item list
                    }
                }
            }
        }

        for (int x = 0; x < TempObjects.Count; x++) { // Loop items from temporary item list
            DestroyImmediate (TempObjects[x].gameObject.GetComponent (component_list[curComponent])); // Change GameObject name with the new defined name
        }
    }
}