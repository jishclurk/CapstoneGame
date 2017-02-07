using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FloatingDamageText : MonoBehaviour {

    public float lifetime = 0.8f;

    [Range(-100, 0)]
    public float minXOffset = -10f;

    [Range(0, 100)]
    public float maxXOffset = 10f;

    [Range(0, 100)]
    public float endHeightOffset = 25f;

    private float time;
    private float startHeight;
    private float endHeight;
    private Text dmgText;

    // Use this for initialization
    void Start () {
        startHeight = transform.localPosition.y;
        endHeight = startHeight + endHeightOffset;
        transform.localPosition = new Vector3(Random.Range(minXOffset, maxXOffset), startHeight);
        dmgText = GetComponent<Text>();
        dmgText.CrossFadeAlpha(0, lifetime, false);
        Destroy(gameObject, lifetime);
	}
	
	// Update is called once per frame
	void FixedUpdate () {
        time += Time.deltaTime;

        transform.localPosition = new Vector3(transform.localPosition.x, Mathf.Lerp(startHeight, endHeight, time));
    }
}
