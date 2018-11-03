using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ArrowContainer : MonoBehaviour {

    public enum ArrowDirection {LEFT, RIGHT, UP, DOWN};

    private TravelingArrow arrow;
    private Collider2D arrowCollider;
    private bool arrowShot = false;

    public ArrowDirection arrowDir = ArrowDirection.RIGHT;

	// Use this for initialization
	void Start () {
        GameObject arrowObj = transform.Find("arrow").gameObject;
        arrow = arrowObj.GetComponent<TravelingArrow>();
        arrowCollider = arrowObj.GetComponent<Collider2D>();
        if (arrowDir == ArrowDirection.LEFT)
        {
            arrow.transform.rotation = Quaternion.Euler(0, 0, 180f);
        }
	}

    private void TriggerArrowShot()
    {
        if (!arrowShot)
        {
            arrow.TriggerShot(1, 0);
            arrowShot = true;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        TriggerArrowShot();
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (!arrowCollider.gameObject != collision.gameObject)
        {
            TriggerArrowShot();
        }
    }
}
