using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Cell : MonoBehaviour
{
    public Transform destination;
    public Light cellLight;
    // Start is called before the first frame update
    void Start()
    {
        cellLight = GetComponentInChildren<Light>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
