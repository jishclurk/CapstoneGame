
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CheckPointTrigger : MonoBehaviour
{

    public Canvas CheckPointSceen;
    private Button yes;
    private Button no;
    private Button exit;

    public bool checkpointReached;
	private Collider col;


    public void Start()
    {
        CheckPointSceen = Instantiate(CheckPointSceen) as Canvas;
        CheckPointSceen = CheckPointSceen.GetComponent<Canvas>();

        CheckPointSceen.enabled = false;
        checkpointReached = false;

        yes = CheckPointSceen.transform.GetChild(2).GetComponent<Button>();
        no = CheckPointSceen.transform.GetChild(3).GetComponent<Button>();
        exit = CheckPointSceen.transform.GetChild(4).GetComponent<Button>();

		col = gameObject.GetComponent<Collider>();
		col.enabled = true;
		Debug.Log ("starting trigger");

    }

    public void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag.Equals("Player"))
        {
            Debug.Log("triggered checkpoint");
            checkpointReached = true;
            Collider col = gameObject.GetComponent<Collider>();
            col.enabled = false;
            //yes.onClick.AddListener(GameManager.manager.OpenSaveScreen);
            //no.onClick.AddListener(GameManager.manager.nextLevel);
            yes.onClick.AddListener(closeCheckpointScreen);
            no.onClick.AddListener(closeCheckpointScreen);
            exit.onClick.AddListener(GameManager.manager.MainMenu);
            CheckPointSceen.enabled = true;
        }
    }

    public void closeCheckpointScreen()
    {
        Debug.Log("close");
        CheckPointSceen.enabled = false;
		Destroy (CheckPointSceen);
    }

    //public void openCheckPointScreen()
    //{
    //    Debug.Log("open");
    //    CheckPointSceen.enabled = true;
    //}
}
