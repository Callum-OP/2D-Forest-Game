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
    private GameObject newObject;
    private bool display;

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
        if(Vector2.Distance(transform.position, target.position) <= 0.3 && display != true){
            if(Random.value > 0.66) {
                prefabToSpawn = prefabToSpawn1;
            } else if(Random.value < 0.33) {
                prefabToSpawn = prefabToSpawn2;
            } else {
                prefabToSpawn = prefabToSpawn3;
            }
            newObject = Instantiate<GameObject>(prefabToSpawn);
			newObject.transform.position = this.transform.position;
            newObject.transform.position = new Vector3(this.transform.position.x, this.transform.position.y + 0.2f, this.transform.position.z);
            newObject.GetComponent<Renderer>().sortingLayerID = this.GetComponent<Renderer>().sortingLayerID;
            newObject.GetComponent<MeshRenderer> ().sortingLayerName = "Layer 1";
            newObject.GetComponent<MeshRenderer> ().sortingOrder = 3;
            display = true;
        }

        if(Vector2.Distance(transform.position, target.position) > 0.3){
            Destroy(newObject);
            display = false;
        }

    }
}
