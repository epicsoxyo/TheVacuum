using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FadeInScript : MonoBehaviour
{

    Image blackScreen;
    [SerializeField] private float fadeTime;

    private void Awake()
    {

        blackScreen = GetComponent<Image>();
        blackScreen.color = new Color(0f, 0f, 0f, 1f);

    }

    private void Start()
    {
    
        StartCoroutine("FadeIn");
    
    }

    private IEnumerator FadeIn()
    {

        blackScreen.CrossFadeAlpha(0f, fadeTime, false);

        yield return new WaitForSeconds(fadeTime);

        Destroy(gameObject);

    }

}
