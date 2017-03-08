using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ListController : MonoBehaviour {

    public GameObject ListItemPrefab;
    public GameObject ContentPanel;

    // Use this for initialization
    void Start() {
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
            MainMenu mainMenuScript = GameObject.Find("StartMenu").GetComponent<MainMenu>();
            button.onClick.AddListener(delegate { mainMenuScript.LoadSavedGame(croppedName); });
        }
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
