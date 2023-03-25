using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;

public class DayNightCycle : MonoBehaviour
{
    public GameObject darknessLevel1;
    public GameObject darknessLevel2;
    public float targetTime = 1000.0f;

    void Start() 
    {
        darknessLevel1.SetActive(false);
        darknessLevel2.SetActive(false);
    }
    
    void Update() 
    {
        targetTime -= Time.deltaTime;
        
        // Gradually get darker overtime
        if (targetTime <= 500.0f)
        {
            darknessLevel1.SetActive(true);
        }
        if (targetTime <= 400.0f)
        {
            darknessLevel2.SetActive(true);
        }
        // Gradually get lighter overtime
        if (targetTime <= 200.0f)
        {
            darknessLevel2.SetActive(false);
        }
        if (targetTime <= 100.0f)
        {
            darknessLevel1.SetActive(false);
        }
        // Reset timer and repeat the cycle
        if (targetTime <= 0.0f)
        {
            targetTime = 900.0f;
        }
    
    }
}
