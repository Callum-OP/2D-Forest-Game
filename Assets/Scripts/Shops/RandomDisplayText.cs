using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RandomDisplayText : MonoBehaviour
{
    [Header("Target")]
    // This is the target that we are waiting on getting closer
	public Transform target;

    [Header("Text to show")]
    // The text prefab to be used when player is near
    public GameObject prefabToSpawn1;
    public GameObject prefabToSpawn2;
    public GameObject prefabToSpawn3;
    private GameObject prefabToSpawn;
    private GameObject newText;
    private bool display;

    [Header("Speech bubble")]
    public GameObject speechBubble;
    private GameObject textBackground;

    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
		// Do nothing if the target hasn't been assigned or it was detroyed for some reason
		if(target == null)
			return;
    }

    // Update is called once per frame
    void Update()
    {
        target = GameObject.FindWithTag("Player").GetComponent<Transform>();
		// Do nothing if the target hasn't been assigned or it was detroyed for some reason
		if(target == null)
			return;

        // If player target is close enough
        if(Vector2.Distance(transform.position, target.position) <= 2 && display != true){
            if(Random.value > 0.66) {
                prefabToSpawn = prefabToSpawn1;
            } else if(Random.value < 0.33) {
                prefabToSpawn = prefabToSpawn2;
            } else {
                prefabToSpawn = prefabToSpawn3;
            }
            newText = Instantiate<GameObject>(prefabToSpawn);
			newText.transform.position = this.transform.position;
            newText.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 2f, this.transform.position.z);
            newText.GetComponent<Renderer>().sortingLayerID = this.GetComponent<Renderer>().sortingLayerID;
            newText.GetComponent<MeshRenderer> ().sortingOrder = 2;

            textBackground = Instantiate<GameObject>(speechBubble);
            textBackground.transform.position = this.transform.position;
            textBackground.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 1.75f, this.transform.position.z);

            display = true;
        }

        if(Vector2.Distance(transform.position, target.position) > 3){
            Destroy(newText);
            Destroy(textBackground);

            display = false;
        }

    }
}
