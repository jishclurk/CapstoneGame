using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CursorReset : MonoBehaviour {

	// Use this for initialization
	void Start () {
        Cursor.SetCursor(null, Vector2.zero, CursorMode.Auto);
	}
	
}
