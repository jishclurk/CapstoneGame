using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class LoadListController : MonoBehaviour
{

    public GameObject ListItemPrefab;
    public GameObject ContentPanel;

    // Use this for initialization
    void Start()
    {
        MainMenu mainMenuScript = GameObject.Find("StartMenu").GetComponent<MainMenu>();
        foreach(string name in SaveLoad.savedGames())
        {
            Debug.Log(name);
        }
        foreach (string name in SaveLoad.savedGames())
        {
            Debug.Log(name);
            GameObject listItem = Instantiate(ListItemPrefab) as GameObject;
            listItem.transform.parent = ContentPanel.transform;
            listItem.transform.localScale = Vector3.one;
            int start = name.IndexOf("savedGame") + "savedGame".Length;
            int end = name.IndexOf(".gd");
            string croppedName = name.Substring(start, end - start);
            listItem.GetComponent<Text>().text = croppedName;
            Button button = listItem.GetComponent<Button>();
            button.onClick.AddListener(delegate { mainMenuScript.LoadSavedGame(croppedName); });
        }
    }

}
