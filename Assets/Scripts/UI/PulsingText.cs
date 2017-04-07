using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PulsingText : MonoBehaviour {

    public float speed = 2;

    private Text txt;

    void Awake ()
    {
        txt = GetComponent<Text>();
    }

    void Start ()
    {
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, 1);
    }

	// Update is called once per frame
	void Update () {
        txt.color = new Color(txt.color.r, txt.color.g, txt.color.b, Mathf.PingPong(Time.unscaledTime * speed, 1.0f));
	}
}
