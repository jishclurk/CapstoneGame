
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPointTrigger : MonoBehaviour
{

    public Canvas CheckPointSceen;
    private Button yes;
    private Button no;
    //public GameManager gm;
    public bool checkpointReached;



    public void Start()
    {
        CheckPointSceen = Instantiate(CheckPointSceen) as Canvas;
        CheckPointSceen = CheckPointSceen.GetComponent<Canvas>();

        CheckPointSceen.enabled = false;
        checkpointReached = false;

        yes = CheckPointSceen.transform.GetChild(2).GetComponent<Button>();
        no = CheckPointSceen.transform.GetChild(3).GetComponent<Button>();

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            checkpointReached = true;
            Collider col = gameObject.GetComponent<Collider>();
            col.enabled = false;
            yes.onClick.AddListener(GameManager.manager.OpenSaveScreen);
            no.onClick.AddListener(GameManager.manager.nextLevel);
            yes.onClick.AddListener(closeCheckpointScreen);
            no.onClick.AddListener(closeCheckpointScreen);
        }
    }

    private void closeCheckpointScreen()
    {
        CheckPointSceen.enabled = false;
    }

    public void openCheckPointScreen()
    {
        // Time.timeScale = 0;
        CheckPointSceen.enabled = true;
    }
}
