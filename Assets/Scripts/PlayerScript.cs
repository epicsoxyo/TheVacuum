using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.AI;
using TMPro;

public class PlayerScript : MonoBehaviour
{

    [SerializeField] private TextMeshProUGUI scoreUI;
    private AudioSource whine;

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

    private void OnTriggerEnter(Collider other)
    {

        if(other.CompareTag("Enemy")) StartCoroutine(TriggerDeath(other.gameObject));

        else if(other.CompareTag("DogBed"))
        {
            if(score == totalToys) StartCoroutine("TriggerWin");
            else
            {
                whine.Play();
                scoreUI.GetComponent<Animator>().SetTrigger("Breathe");
            }
        }

    }

    private IEnumerator TriggerDeath(GameObject lookTarget)
    {

        Destroy(lookTarget.GetComponent<VacuumAI>());
        NavMeshAgent nav = lookTarget.transform.parent.gameObject.GetComponent<NavMeshAgent>();
        nav.speed = 0;
        nav.isStopped = true;

        float lerpSpeed = 0.95f;
        float timeElapsed = 0f;

        while(timeElapsed * lerpSpeed < 1f)
        {
            Quaternion targetRotation = Quaternion.LookRotation(lookTarget.transform.position - transform.position);
            timeElapsed += Time.deltaTime;
            transform.rotation = Quaternion.Lerp(transform.rotation, targetRotation, timeElapsed * lerpSpeed);
            yield return null;
        }

        yield return new WaitForSeconds(0.2f);
        SceneManager.LoadScene(1);

    }

    private IEnumerator TriggerWin()
    {
        yield return new WaitForSeconds(0.3f);
        SceneManager.LoadScene(2);
    }

}