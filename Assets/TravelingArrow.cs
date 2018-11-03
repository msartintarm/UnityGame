using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TravelingArrow : MonoBehaviour {

    private Rigidbody2D body;

    private bool isTraveling = false;
    private Vector2 velocity;

    void Start () {
        body = GetComponent<Rigidbody2D>();
    }

    public void TriggerShot(float x, float y)
    {
        isTraveling = true;
        velocity = new Vector2(x, y);
    }

    void FixedUpdate () {
		if (isTraveling)
        {
            body.velocity = velocity;
        }
	}


}
