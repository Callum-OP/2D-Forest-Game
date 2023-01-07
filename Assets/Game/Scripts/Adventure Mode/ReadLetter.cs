using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ReadLetter : MonoBehaviour
{
    [Header("Target")]
    // This is the player that we are waiting on getting closer
	public Transform player;
    public Transform area;

    [Header("Text to show")]
    // The text prefab to be used when player is near
    public GameObject letterClosed;
    public GameObject letterOpened;
    private GameObject newText;
    private GameObject letterObject;
    private bool display;
    private bool open;
    private float speed;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
		// Do nothing if the player hasn't been assigned or it was detroyed for some reason
		if(player == null)
			return;
        speed = Player.speed;
        open = true;
        letterObject = Instantiate<GameObject>(letterOpened);
        letterObject.transform.position = player.transform.position;
        letterObject.GetComponent<Renderer>().sortingLayerID = this.GetComponent<Renderer>().sortingLayerID;
        letterObject.GetComponent<SpriteRenderer> ().sortingLayerName = "Layer 3";
        letterObject.GetComponent<SpriteRenderer> ().sortingOrder = 1;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
		// Do nothing if the player hasn't been assigned or it was detroyed for some reason
		if(player == null)
			return;

        // If player target is close enough
        if(Vector2.Distance(transform.position, player.position) <= 0.2 && display != true){
            newText = Instantiate<GameObject>(letterClosed);
			newText.transform.position = area.transform.position;
            newText.transform.position = new Vector3(area.transform.position.x, area.transform.position.y + 0.15f, area.transform.position.z);
            newText.GetComponent<Renderer>().sortingLayerID = this.GetComponent<Renderer>().sortingLayerID;
            newText.GetComponent<MeshRenderer> ().sortingLayerName = "Layer 1";
            newText.GetComponent<MeshRenderer> ().sortingOrder = 3;
            display = true;
        }

        if(Vector2.Distance(transform.position, player.position) > 0.2){
            Destroy(newText);
            display = false;
        }

        // If player target is close enough
        if(Vector2.Distance(transform.position, player.position) <= 0.2){
            if (Input.GetKeyDown("r") && open == false) {
                open = true;
                letterObject = Instantiate<GameObject>(letterOpened);
                letterObject.transform.position = player.transform.position;
                letterObject.GetComponent<Renderer>().sortingLayerID = this.GetComponent<Renderer>().sortingLayerID;
                letterObject.GetComponent<SpriteRenderer> ().sortingLayerName = "Layer 3";
                letterObject.GetComponent<SpriteRenderer> ().sortingOrder = 1;
            } 
        }

        if (open == true) {
            Destroy(newText);
            Player.speed = 0;
            if (Input.GetKeyDown("x") && open == true) {
                Player.speed = speed;
                Destroy(letterObject);
                open = false;
            }
        }

    }
    
}

