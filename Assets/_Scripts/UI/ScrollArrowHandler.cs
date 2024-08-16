using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollArrowHandler : MonoBehaviour
{
    [SerializeField] GameObject UI_Arrow;
    [SerializeField] RectTransform scrollableContent;
    [SerializeField] Vector3 maxScroll;

    private void Update()
    {
        if (scrollableContent.localPosition.y > maxScroll.y)
            UI_Arrow.SetActive(false);
        else
            UI_Arrow.SetActive(true);
    }
}
