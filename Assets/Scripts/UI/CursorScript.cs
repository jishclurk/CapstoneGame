using System.Collections;
using System.Collections.Generic;
using LayerDefinitions;
using UnityEngine;

public class CursorScript : MonoBehaviour {

    public Texture2D navTexture;
    public Texture2D shootTexture;
    public Texture2D abilityTexture;
    public CursorMode cursorMode = CursorMode.ForceSoftware;

    // Use this for initialization
    void Start () {
        
        Cursor.SetCursor(navTexture, Vector2.zero, cursorMode);
    }
	
	// Update is called once per frame
	void Update () {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        RaycastHit hit;

        if (Physics.Raycast(ray, out hit, 100f, Layers.NonWall))
        {
            if (hit.collider.CompareTag("Enemy"))
            {
                Cursor.SetCursor(shootTexture, new Vector2(shootTexture.height/2, shootTexture.width/2), cursorMode);
            }
            else
            {
                Cursor.SetCursor(navTexture, Vector2.zero, cursorMode);
            }


        }



    }
}
