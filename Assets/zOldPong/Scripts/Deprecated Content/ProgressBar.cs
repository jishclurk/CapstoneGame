using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProgressBar : MonoBehaviour {

    public RectTransform barImageRect;

    private Vector2 startRectPos;

    void Start ()
    {
        if (barImageRect != null)
        {
            startRectPos = barImageRect.anchoredPosition;
            barImageRect.anchoredPosition = new Vector2(startRectPos.x - barImageRect.rect.width, startRectPos.y);
        }
    }

	public void SetProgress (float percent)
    {
        percent = (100.0f - percent) / 100.0f;
        barImageRect.anchoredPosition = new Vector2(startRectPos.x - (percent * barImageRect.rect.width), startRectPos.y);
    }
}
