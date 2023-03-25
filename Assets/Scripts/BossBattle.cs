using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossBattle : MonoBehaviour
{
    [Header("Target")]
    // This is the target that we are waiting on getting closer
	public Transform target;

    [Header("Text to show")]
    // The text prefab to be used when player is near
    public GameObject prefabToSpawn;
    private GameObject newText;
    private bool display;

    [Header("Speech bubble")]
    public GameObject speechBubble;
    private GameObject textBackground;

    [Header("Areas")]
    public Transform Entrance;
    public Transform Exit;


    // Start is called before the first frame update
    void Start()
    {
        target = GameObject.FindWithTag("target").GetComponent<Transform>();
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

        if(Scores.gems != Scores.gemsToGet) {
            // If player target is close enough
            if(Vector2.Distance(transform.position, target.position) <= 2 && display != true){
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
        }

        if(Vector2.Distance(transform.position, target.position) > 3){
            Destroy(newText);
            Destroy(textBackground);

            display = false;
        }

    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        if(Scores.gems == Scores.gemsToGet) {
            // Teleport player to exit
            if(this.gameObject.CompareTag("Entrance") && other.gameObject.CompareTag("Player")) {
                target.position = Exit.position;
                // Run Disable() coroutine
                StartCoroutine (Disable());
            }

            // Teleport player to entrance
            if(this.gameObject.CompareTag("Exit") && other.gameObject.CompareTag("Player")) {
                target.position = Entrance.position;
                // Run Disable() coroutine
                StartCoroutine (Disable());
            }

            // Disable entrance/exit temporarily
            IEnumerator Disable()
            {
                Entrance.localScale = new Vector3(0f, 0f, 0f);
                Exit.localScale = new Vector3(0f, 0f, 0f);
                yield return new WaitForSeconds(1.5f);
                Entrance.localScale = new Vector3(2f, 0.5f, 1f);
                Exit.localScale = new Vector3(2f, 0.5f, 1f);
            }
        }
    }
}
