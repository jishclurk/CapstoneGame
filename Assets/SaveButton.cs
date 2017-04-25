using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class SaveButton : MonoBehaviour {

    public Text Date { get; set; }

    public Text Level { get; set; }

    public Text Title { get; set; }

    private SaveSlotScript saveSlotScript;

    public int slotNumber;

    // Use this for initialization
    void Awake () {
        saveSlotScript = transform.parent.gameObject.GetComponent<SaveSlotScript>();
        Date = transform.FindChild("Date").GetComponent<Text>();
        Level = transform.FindChild("Level").GetComponent<Text>();
        Title = transform.FindChild("Text").GetComponent<Text>();

    }

}
