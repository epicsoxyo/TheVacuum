using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;

public class PlayerScript : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI scoreUI;
    [SerializeField] private AudioSource whine;

    private int score = 0;
    private int totalToys;

    private void Awake()
    {

        whine = GetComponent<AudioSource>();

    }

    private void Start()
    {

        GameObject[] toys = GameObject.FindGameObjectsWithTag("DogToy");
        foreach(GameObject toy in toys)
        {
            toy.GetComponent<DogToy>().Player = this;
        }
        totalToys = toys.Length;

        scoreUI.SetText("Toys Found: 0/" + totalToys);

    }

    public void IncrementScore()
    {
        score++;

        if(score == totalToys)
            scoreUI.SetText("Go to Bed");
        else
            scoreUI.SetText("Toys Found: " + score + "/" + totalToys);

    }

    private void OnCollisionEnter(Collision other)
    {
        
        if(other.gameObject.tag == "Enemy") StartCoroutine("TriggerDeath");

    }

    private IEnumerator TriggerDeath()
    {

        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(1);

    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.CompareTag("DogBed"))
        {
            if(score == totalToys) StartCoroutine("TriggerWin");
            else
            {
                whine.Play();
                scoreUI.GetComponent<Animator>().SetTrigger("Breathe");
            }
        }

    }

    private IEnumerator TriggerWin()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(2);
    }

}