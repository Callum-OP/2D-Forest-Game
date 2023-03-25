using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// Class for buying an extra heart in shop to increase max health
public class BuyHeart : MonoBehaviour
{
    [Header("Target")]
    // This is the target that we are waiting on getting closer
	public Transform player;
    public Transform item;

    [Header("Text to show")]
    // The text prefabs to be used when player is near
    public GameObject itemDescription;
    public GameObject notEnoughGold;
    public GameObject fullHearts;
    public GameObject itemToBuy;
    public GameObject itemSold;
    private GameObject newText;
    private bool display;

    [Header("Speech bubble")]
    public GameObject speechBubble;
    private GameObject textBackground;

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
        if(Vector2.Distance(item.position, player.position) <= 1.5f && display != true){
            newText = Instantiate<GameObject>(itemDescription);
			newText.transform.position = this.transform.position;
            newText.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z);
            newText.GetComponent<Renderer>().sortingLayerID = this.GetComponent<Renderer>().sortingLayerID;
            newText.GetComponent<MeshRenderer> ().sortingOrder = 2;

            textBackground = Instantiate<GameObject>(speechBubble);
            textBackground.transform.position = this.transform.position;
            textBackground.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1.75f, this.transform.position.z);
            
            display = true;
        }

        if(Vector2.Distance(item.position, player.position) > 1.5f){
            Destroy(newText);
            Destroy(textBackground);

            display = false;
        }

        // If player target is close enough
        if(Vector2.Distance(item.position, player.position) <= 1.5f){
            if (Input.GetKeyDown("b")) {
                if (Player.maxHealth < 10) {
                    if (Scores.score >= 500) {
                        GameObject buyObject = Instantiate<GameObject>(itemToBuy);
                        buyObject.transform.position = new Vector3(player.transform.position.x, player.transform.position.y - 2f, player.transform.position.z);
                        Destroy(newText);
                        Player.maxHealth = Player.maxHealth + 1;
                        Scores.score = Scores.score - 500;
                        newText.GetComponent<Renderer>().sortingLayerID = this.GetComponent<Renderer>().sortingLayerID;
                        newText.GetComponent<MeshRenderer> ().sortingOrder = 2;
                    } else {
                        Destroy(newText);
                        newText = Instantiate<GameObject>(notEnoughGold);
                        newText.transform.position = this.transform.position;
                        newText.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z);
                    }
                } else {
                    Destroy(newText);
                    newText = Instantiate<GameObject>(fullHearts);
                    newText.transform.position = this.transform.position;
                    newText.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z);
                }
                newText.GetComponent<Renderer>().sortingLayerID = this.GetComponent<Renderer>().sortingLayerID;
                newText.GetComponent<MeshRenderer> ().sortingOrder = 2;
            }
        }

    }
}
