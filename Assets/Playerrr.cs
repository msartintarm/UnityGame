using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Playerrr : MonoBehaviour
{

    private SpriteRenderer spriteRenderer;
    public Sprite player_still;
    public Sprite player_back_foot_raised;
    public Sprite player_front_foot_raised;

    public static float SPEED = 6.0f;
    public static float JUMP_SPEED = 8.0F;
    public float gravity = 20.0f;
    public float moveInertia = 0.09f;

//    private CharacterController controller;
    private Rigidbody2D body;
    private ScissorsCut scissors;

    private enum DirectionState { LEFT, RIGHT };
    private DirectionState direction = DirectionState.LEFT;

    private enum MovingState { STILL, MOVING_LR };
    private MovingState movingState = MovingState.STILL;

    private static int walk_frames = 0;
    private int MAX_WALK_FRAMES = 5;

    private bool useTiltControls = false;

    // Use this for initialization
    void Start()
    {
        //       controller = GetComponent<CharacterController>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        scissors = GetComponentInChildren<ScissorsCut>();
        scissors.RegisterPlayerScissors();
        scissors.Activate(false);
    }

    public void ActivateScissors()
    {
        scissors.Activate();
    }

    public void EnableTiltControls()
    {
        useTiltControls = true;
    }

    // FixedUpdate is called once per physics calculation
    private void FixedUpdate()
    {
        float keypadX = Input.GetAxisRaw("Horizontal");
        float keypadY = Input.GetAxisRaw("Vertical");

        float accelX = Input.acceleration.x;
        float accelY = Input.acceleration.y;

        int inputX = (keypadX > 0) ? 1 : (keypadX < 0) ? -1 :
            (accelX > 0) ? 1 : (accelX < 0) ? -1 :
            0;
        int inputY = (keypadY > 0) ? 1 : (keypadY < 0) ? -1 :
            (accelY > 0) ? 1 : (accelY < 0) ? -1 :
            0;

        switch (direction)
        {
            case DirectionState.LEFT:
                if (inputX > 0)
                {
                    transform.rotation = Quaternion.Euler(0, 180f, 0);
                    direction = DirectionState.RIGHT;
                }
                break;
            case DirectionState.RIGHT:
                if (inputX < 0)
                {
                    transform.rotation = Quaternion.Euler(0, 0, 0);
                    direction = DirectionState.LEFT;
                }
                break;
            default:
                break;
        }

        Vector2 moveDirection = Vector2.zero;
        if (inputX != 0) {

            moveDirection.x = Mathf.Lerp(body.velocity.x, SPEED * inputX, moveInertia);
            if (movingState == MovingState.STILL) {
                movingState = MovingState.MOVING_LR;
                walk_frames = 0;
            }
            if (walk_frames == MAX_WALK_FRAMES) {
                walk_frames = 0;
                spriteRenderer.sprite = (spriteRenderer.sprite == player_back_foot_raised) ?
                    player_front_foot_raised : player_back_foot_raised;
            } else {
                walk_frames += 1;
            }
        } else {
            if (movingState == MovingState.MOVING_LR) {
                movingState = MovingState.STILL;
                spriteRenderer.sprite = player_still;
            }
        }

        if (inputY != 0) {
            moveDirection.y = (JUMP_SPEED * inputY);
        }

        if (inputX != 0 || inputY != 0) {
            body.velocity = moveDirection;
        }
    }

}
