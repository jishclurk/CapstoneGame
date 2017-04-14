using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinimapZoom : MonoBehaviour {

    private MinimapCameraController minimapCameraController;

	// Use this for initialization
	void Start () {
        GameObject gameObject = GameObject.FindGameObjectWithTag("Minimap");
        minimapCameraController = gameObject.GetComponent<MinimapCameraController>();
	}
	
	public void ZoomIn()
    {
        minimapCameraController.ZoomIn();
    }

    public void ZoomOut()
    {
        minimapCameraController.ZoomOut();
    }
}
