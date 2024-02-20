using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DogToy : MonoBehaviour
{

    [SerializeField] private MeshRenderer foundToyMesh;

    private bool isInReach = false;

    private PlayerScript player;
    public PlayerScript Player
    {
        set{player = value;}
    }

    private void Start()
    {
        
        foundToyMesh.enabled = false;

    }

    private void Update()
    {
    
        if(isInReach && Input.GetAxis("Interact") > 0.01f) AddToFoundList();

    }

    private void AddToFoundList()
    {
        
        foundToyMesh.enabled = true;

        player.IncrementScore();

        Destroy(gameObject);

    }

    private void OnTriggerEnter(Collider other)
    {

        isInReach = true;

    }

    private void OnTriggerExit(Collider other)
    {

        isInReach = false;

    }


}
