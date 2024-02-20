using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class RetryScript : MonoBehaviour
{

    private void Update()
    {
        if(Input.GetAxis("Interact") >= 0.01f) Retry();    
    }

    public void Retry()
    {
        StartCoroutine("ReloadGame");
    }

    private IEnumerator ReloadGame()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(0);
    }

}
