using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraLogic : MonoBehaviour {

    public GameObject player;

    private Vector3 offset;

    private enum State { CHASING_PLAYER, IDLE};
    private State state = State.IDLE;

    // below this threshold the camera should move towards player. threshold will be modified dynamically
    // in order to center player, then drop in sensitivity and let move towards edge again.
    private static float CLOSE_THRESHOLD = 0.3f;
    private static float FAR_THRESHOLD = 1.3f;
    private float player_closeness_threshold = FAR_THRESHOLD;


	void Start () {
        player = GameObject.FindWithTag("Player");
        offset = transform.position - player.transform.position;		
	}
	
	void FixedUpdate () {
        Vector3 newPosition = transform.position;

        float playerX = player.transform.position.x + offset.x;
        float playerY = player.transform.position.y + offset.y;

        // only update Y position if player position is beyond a threshold
        if (Mathf.Abs(transform.position.y - playerY) > 2)
        {
            newPosition.y = playerY;
        }

        // only update X position beyond a certain threshold
        if (Mathf.Abs(transform.position.x - playerX) > player_closeness_threshold)
        {
            if (state != State.CHASING_PLAYER)
            {
                state = State.CHASING_PLAYER;
                player_closeness_threshold = CLOSE_THRESHOLD;
            }
            newPosition.x = Mathf.Lerp(transform.position.x, playerX, 0.03f);
        } else
        {
            if (state == State.CHASING_PLAYER)
            {
                state = State.IDLE;
                player_closeness_threshold = FAR_THRESHOLD;
            }

        }


        transform.position = newPosition;
    }
}
