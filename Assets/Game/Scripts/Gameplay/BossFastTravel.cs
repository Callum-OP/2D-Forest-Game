using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossFastTravel : MonoBehaviour
{
    [Header("Areas")]
    public Transform Entrance;
    public Transform Exit;

    [Header("Player")]
    public Transform Player;

    public float newZoom = 2;
    public float originalZoom;
    public Camera MainCamera;

    // Update is called once per frame
    void Update () {
        Player = GameObject.FindWithTag("Player").GetComponent<Transform>();
        MainCamera = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        originalZoom = MainCamera.orthographicSize;
    }

    private void OnTriggerEnter2D(Collider2D other)
    {
        // Teleport player to exit
        if(this.gameObject.CompareTag("Entrance") && other.gameObject.CompareTag("Player")) {
            Player.position = Exit.position;
            // Run Disable() coroutine
            StartCoroutine (Disable());
            MainCamera.orthographicSize = newZoom;
        }

        // Teleport player to entrance
        if(this.gameObject.CompareTag("Exit") && other.gameObject.CompareTag("Player")) {
            Player.position = Entrance.position;
            // Run Disable() coroutine
            StartCoroutine (Disable());
            MainCamera.orthographicSize = originalZoom;
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
