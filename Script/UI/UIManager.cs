using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UIManager : MonoBehaviour
{
    public SceneFader sceneFader;
    public static UIManager Instance;

    [SerializeField] GameObject deathScreen;
    public GameObject mapHandler;

    private void Awake()
    {
        if (Instance != null && Instance != this)
        {
            Destroy(gameObject);
        }
        else
        {
            Instance = this;
        }
        DontDestroyOnLoad(gameObject);
        sceneFader = GetComponentInChildren<SceneFader>();
        deathScreen.SetActive(false);

    }
    public IEnumerator ActivateDeathScreen()
    {
        yield return new WaitForSeconds(0.1f);
        StartCoroutine(sceneFader.Fade(SceneFader.FadeDirection.In));
        yield return new WaitForSeconds(0.5f);
        deathScreen.SetActive(true);
    }

    public IEnumerator DeactivateDeathScreen()
    {
        yield return new WaitForSeconds(0.5f);
        deathScreen.SetActive(false);
        StartCoroutine(sceneFader.Fade(SceneFader.FadeDirection.Out));
    }

}
