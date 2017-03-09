﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveListController : MonoBehaviour
{

    public GameObject ListItemPrefab;
    public GameObject ContentPanel;

    // Use this for initialization
    void Start()
    {
        CheckPoint cp = transform.parent.parent.gameObject.GetComponent<CheckPoint>();
        Debug.Log(cp);
        foreach (string name in SaveLoad.savedGames())
        {
            GameObject listItem = Instantiate(ListItemPrefab) as GameObject;
            listItem.transform.parent = ContentPanel.transform;
            listItem.transform.localScale = Vector3.one;
            int start = name.IndexOf("savedGame") + "savedGame".Length;
            int end = name.IndexOf(".gd");
            string croppedName = name.Substring(start, end - start);
            listItem.GetComponent<Text>().text = croppedName;
            Button button = listItem.GetComponent<Button>();
            button.onClick.AddListener(delegate { cp.SaveGame(croppedName); });
        }
    }

}
