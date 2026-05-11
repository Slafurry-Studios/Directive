using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CreditScene : MonoBehaviour
{
    // Start is called before the first frame update
    private CanvasGroup canvasGroup;
    [SerializeField] private RectTransform _rectTransform;
    private bool isAutoScrolling = true;
    private bool startFade = false;
    [SerializeField] private float scrollSpeed = 0.5f;
    
    [SerializeField] private float scrollTime = 10f;
    [SerializeField] private float visibleTime = 0f;
    void Start()
    {
        canvasGroup = GetComponent<CanvasGroup>();
        visibleTime = 0f;
    }
    
    void Update()
    {
        AutoScroll();
        FadeOut();
    }

    void FadeOut()
    {
        if (startFade)
        {
            canvasGroup.alpha -= Time.deltaTime;
        }
    }

    void AutoScroll()
    {
        if (isAutoScrolling) {
            _rectTransform.anchoredPosition += Vector2.up * scrollSpeed * Time.deltaTime;
            visibleTime += Time.deltaTime;
            if (visibleTime >= scrollTime)
            {
                isAutoScrolling = false;
                startFade = true;
                visibleTime = 0f;
            }
        }
        
    }
}
