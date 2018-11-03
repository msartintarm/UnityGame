using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BackgroundUpdate : MonoBehaviour {

	// Use this for initialization
	void Start () {
		
	}
	
    void LateUpdate()
    {
        float camerax = Camera.main.transform.position.x * 0.9f;
        float cameray = Camera.main.transform.position.y * 0.9f;
        transform.position = new Vector3(camerax, cameray, transform.position.z);
    }
}
