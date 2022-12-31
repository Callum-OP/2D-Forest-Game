using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for buying hearts in shop to heal health
public class BuyHealth : MonoBehaviour
{
    [Header("Target")]
    // This is the target that we are waiting on getting closer
	public Transform player;
    public Transform item;

    [Header("Text to show")]
    // The text prefabs to be used when player is near
    public GameObject itemDescription;
    public GameObject notEnoughGold;
    public GameObject fullHealth;
    public GameObject itemToBuy;
    public GameObject itemSold;
    private GameObject newText;
    private bool display;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
		// Do nothing if the target hasn't been assigned or it was detroyed for some reason
		if(player == null)
			return;
    }

    // Update is called once per frame
    void Update()
    {
        player = GameObject.FindWithTag("Player").GetComponent<Transform>();
		// Do nothing if the target hasn't been assigned or it was detroyed for some reason
		if(player == null)
			return;

        // If player target is close enough
        if(Vector2.Distance(item.position, player.position) <= 0.2 && display != true){
            newText = Instantiate<GameObject>(itemDescription);
			newText.transform.position = this.transform.position;
            newText.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.2f, this.transform.position.z);
            newText.GetComponent<Renderer>().sortingLayerID = this.GetComponent<Renderer>().sortingLayerID;
            newText.GetComponent<MeshRenderer> ().sortingLayerName = "Layer 1";
            newText.GetComponent<MeshRenderer> ().sortingOrder = 3;
            display = true;
        }

        if(Vector2.Distance(item.position, player.position) > 0.2){
            Destroy(newText);
            display = false;
        }

        // If player target is close enough
        if(Vector2.Distance(item.position, player.position) <= 0.2){
            if (Input.GetKeyDown("b")) {
                if (Player.health < Player.maxHealth) {
                    if (Scores.score >= 50) {
                        GameObject buyObject = Instantiate<GameObject>(itemToBuy);
                        buyObject.transform.position = player.transform.position;
                        Destroy(newText);
                        Scores.score = Scores.score - 50;
                        newText = Instantiate<GameObject>(itemSold);
                        newText.transform.position = this.transform.position;
                        newText.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.2f, this.transform.position.z);
                        newText.GetComponent<Renderer>().sortingLayerID = this.GetComponent<Renderer>().sortingLayerID;
                        newText.GetComponent<MeshRenderer> ().sortingLayerName = "Layer 1";
                        newText.GetComponent<MeshRenderer> ().sortingOrder = 3;
                    } else {
                        Destroy(newText);
                        newText = Instantiate<GameObject>(notEnoughGold);
                        newText.transform.position = this.transform.position;
                        newText.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.2f, this.transform.position.z);
                        newText.GetComponent<Renderer>().sortingLayerID = this.GetComponent<Renderer>().sortingLayerID;
                        newText.GetComponent<MeshRenderer> ().sortingLayerName = "Layer 1";
                        newText.GetComponent<MeshRenderer> ().sortingOrder = 3;
                    }
                } else {
                    Destroy(newText);
                    newText = Instantiate<GameObject>(fullHealth);
                    newText.transform.position = this.transform.position;
                    newText.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.2f, this.transform.position.z);
                    newText.GetComponent<Renderer>().sortingLayerID = this.GetComponent<Renderer>().sortingLayerID;
                    newText.GetComponent<MeshRenderer> ().sortingLayerName = "Layer 1";
                    newText.GetComponent<MeshRenderer> ().sortingOrder = 3;
                }
            }
        }

    }
}

