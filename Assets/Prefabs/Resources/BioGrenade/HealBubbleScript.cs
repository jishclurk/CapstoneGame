using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HealBubbleScript : MonoBehaviour {


    //set by instantiator (abilityHelper)
    public float healRate;
    public float healHP;
    public float healLength;
    public GameObject target;

	// Use this for initialization
	void Start () {
        StartCoroutine(ScaleOverTime(0.3f));
        StartCoroutine(HealOverTime());
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    IEnumerator ScaleOverTime(float time)
    {
        Vector3 originalScale = transform.localScale;
        float scaleTarget = target.transform.localScale.x;
        Vector3 destinationScale = new Vector3(scaleTarget/1.67f, scaleTarget / 3f, scaleTarget / 1.67f);

        float currentTime = 0.0f;

        do
        {
            transform.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        } while (currentTime <= time);

    }

    private IEnumerator HealOverTime()
    {
        float tracker = 0.0f;
        while(tracker < healLength)
        {
            yield return new WaitForSeconds(healRate);
            AOETargetController aoeController = target.GetComponent<AOETargetController>();
            foreach (GameObject player in aoeController.affectedPlayers)
            {
                player.GetComponent<PlayerResources>().Heal(healHP);
                Debug.Log(name + " on " + target.name + " does " + healHP + " heal.");
            }
            tracker += healRate;
        }

    }

}
