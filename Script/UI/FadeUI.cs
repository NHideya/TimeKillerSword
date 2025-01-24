using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class FadeUI : MonoBehaviour
{
    CanvasGroup canvasGroup;
    [SerializeField] float maxAlpha = 1.0f;

    [SerializeField] GameObject initalizeSelect;
    private Coroutine coroutine;
    private void Awake()
    {
        canvasGroup = GetComponent<CanvasGroup>();
    }

    IEnumerator FadeOut(float _seconds)
    {
        if( canvasGroup.alpha == 0) yield break;
        canvasGroup.interactable = false;
        canvasGroup.blocksRaycasts = false;
        canvasGroup.alpha = maxAlpha;
        while(canvasGroup.alpha > 0)
        {
            canvasGroup.alpha -= maxAlpha * Time.unscaledDeltaTime / _seconds;
            yield return null;

        }
        canvasGroup.alpha = 0;
        yield return null;

    }

    public void FadeUIOut(float _seconds)
    {
        if(coroutine != null)
        StopCoroutine(coroutine);
        
        coroutine = StartCoroutine(FadeOut(_seconds));
    }
      public void FadeUIIn(float _seconds)
    {
         if(coroutine != null)
        StopCoroutine(coroutine);
         coroutine = StartCoroutine(FadeIn(_seconds));
    }

    IEnumerator FadeIn(float _seconds)
    {
             if( canvasGroup.alpha == maxAlpha) yield break;
        canvasGroup.alpha = 0;
        while(canvasGroup.alpha < maxAlpha)
        {
            canvasGroup.alpha += maxAlpha * Time.unscaledDeltaTime / _seconds;
            yield return null;

        }
        canvasGroup.alpha = maxAlpha;
         canvasGroup.interactable = true;
        canvasGroup.blocksRaycasts = true;
        if(initalizeSelect != null)
         EventSystem.current.SetSelectedGameObject(initalizeSelect);
        yield return null;

    }
    
}
