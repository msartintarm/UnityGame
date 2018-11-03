using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Rope : MonoBehaviour {

    private EdgeCollider2D edge;

    // Use this for initialization
    void Start () {
        edge = GetComponent<EdgeCollider2D>();
	}
	
	// Update is called once per frame
	void Update () {
		
	}

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Arrow"))
        {
            Object.Destroy(this.gameObject);
            return;
            Debug.Log("Hello");

            // Get the start and end points of the rope.
            var edgeCenter = (Vector2)edge.transform.position + edge.offset;
            var xLeft = (edgeCenter.x - (edge.bounds.size.x / 2));
            var xRight = (edgeCenter.x + (edge.bounds.size.x / 2));
            var yLeft = (edgeCenter.y - (edge.bounds.size.y / 2));
            var yRight = (edgeCenter.y + (edge.bounds.size.y / 2));
            Debug.Log(xLeft);
            Debug.Log(xRight);
            Debug.Log(yLeft);
            Debug.Log(yRight);

            Vector2 collisionObjCenter = collision.gameObject.transform.position;

            RaycastHit2D result = Physics2D.Raycast(collisionObjCenter, new Vector2(collisionObjCenter.x + 0.1f, collisionObjCenter.y));

            if (result.centroid != null)
            {
                Debug.Log("Hooray!");
                Debug.Log(result.centroid.x);
                Debug.Log(result.centroid.y);

                Instantiate(gameObject, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);

            }


            // Get position of new segment. We have bounding box, so now do complicated logic.

            // Determine whether the new segment would be long enough.
            //            if (collision.GetContac) {
            //          }

            // Manipulate position / scale of this rope to shrink it
            // Add new rope

            //            Instantiate(gameObject, new Vector3(transform.position.x, transform.position.y, 0), Quaternion.identity);
        }
    }
}
