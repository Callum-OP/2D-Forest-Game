using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SetParentSortLayer : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        this.GetComponent<MeshRenderer> ().sortingLayerName = "Layer 3";
        this.GetComponent<MeshRenderer> ().sortingOrder = 2;
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
