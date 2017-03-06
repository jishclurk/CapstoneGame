using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyHoverHighlight : MonoBehaviour {

    public GameObject highlight;

	void Start () {
        if (highlight != null)
            highlight.gameObject.SetActive(false);
	}
	
	void OnMouseEnter()
    {
        if (highlight != null)
            highlight.gameObject.SetActive(true);
    }

    void OnMouseExit()
    {
        if (highlight != null)
            highlight.gameObject.SetActive(false);
    }
}
