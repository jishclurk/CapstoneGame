using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ImageSlideShow : MonoBehaviour {

    public float timeVisible;
    public float timeTransition;

    private List<Image> images;
    private float timeCount;
    private bool transitioning;
    private int shownImage;
    private int nextImage;

	// Use this for initialization
	void Start () {
        images = new List<Image> (GetComponentsInChildren<Image>());
        foreach (Image image in images) {
            image.gameObject.SetActive(false);
        }

        timeCount = 0;
        transitioning = false;
        shownImage = 0;
        nextImage = 1 % images.Count;
        StartCoroutine(IterateOverImages());
    }
	
    void Update()
    {
        /*timeCount += Time.deltaTime;
        if (!transitioning && timeCount >= timeVisible)
        {
            timeCount = 0;
            transitioning = true;
            
        }
        else if (transitioning && timeCount >= timeTransition)
        {
            images[shownImage].color = new Color(images[shownImage].color.r,
                images[shownImage].color.g, images[shownImage].color.b, 0);
            images[nextImage].color = new Color(images[nextImage].color.r,
                images[nextImage].color.g, images[nextImage].color.b, 255);

            shownImage = (shownImage + 1) % images.Count;
            nextImage = (shownImage + 1) % images.Count;
            timeCount = 0;
            transitioning = false;
        }*/
    }

    IEnumerator IterateOverImages()
    {
        while (true)
        {
            images[shownImage].gameObject.SetActive(true);
            yield return new WaitForSeconds(timeVisible);

            images[nextImage].gameObject.SetActive(true);
            images[nextImage].canvasRenderer.SetAlpha(0.01f);
            images[shownImage].CrossFadeAlpha(0.01f, timeTransition, false);
            images[nextImage].CrossFadeAlpha(1, timeTransition, false);

            yield return new WaitForSeconds(timeTransition);
            images[shownImage].gameObject.SetActive(false);
            shownImage = (shownImage + 1) % images.Count;
            nextImage = (shownImage + 1) % images.Count;
        }
    }

}
