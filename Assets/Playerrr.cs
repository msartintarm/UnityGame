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
    public static float JUMP_SPEED = 4.0f;
    public static float MOVE_INERTIA = 0.09f;

    public LayerMask groundLayer;

    private Rigidbody2D body;
    private ScissorsCut scissors;

    private enum DirectionState { LEFT, RIGHT };
    private DirectionState direction = DirectionState.LEFT;

    private enum MovingState { STILL, MOVING_LR };
    private MovingState movingState = MovingState.STILL;

    private enum JumpingState { NO_JUMP, JUMP };
    private JumpingState jumpingState = JumpingState.NO_JUMP;

    private static int walk_frames = 0;
    private int MAX_WALK_FRAMES = 5;

    public bool enableTiltControls = false;

    void Start()
    {
        spriteRenderer = GetComponent<SpriteRenderer>();
        body = GetComponent<Rigidbody2D>();
        scissors = GetComponentInChildren<ScissorsCut>();
        scissors.RegisterPlayerScissors();
        scissors.Activate(false);
    }

    bool IsGrounded()
    {
        Vector2 position = transform.position;
        Vector2 direction = Vector2.down;
        float distance = 1.0f;

        RaycastHit2D hit = Physics2D.Raycast(position, direction, distance, groundLayer);
        if (hit.collider != null)
        {
            Debug.Log("Wow!");
            Debug.Log(hit.collider.gameObject);
            return true;
        }

        return false;
    }

    public void ActivateScissors()
    {
        scissors.Activate();
    }

    int getInputX()
    {
        float keypadX = Input.GetAxisRaw("Horizontal");
        int inputX = (keypadX > 0) ? 1 : (keypadX < 0) ? -1 : 0;
        if (enableTiltControls)
        {
            float accelX = Input.acceleration.x;
            inputX = (accelX > 0) ? 1 : (accelX < 0) ? -1 : 0;
        }
        return inputX;
    }

    int getInputY()
    {
        float keypadY = Input.GetAxisRaw("Vertical");
        int inputY = (keypadY > 0) ? 1 : (keypadY < 0) ? -1 : 0;
        if (enableTiltControls)
        {
            float accelY = Input.acceleration.y;
            inputY = (accelY > 0) ? 1 : (accelY < 0) ? -1 : 0;
        }
        return inputY;
    }

    private void CheckSpriteDirectionChange(int inputX)
    {
        if (direction == DirectionState.LEFT && inputX > 0)
        {
            transform.rotation = Quaternion.Euler(0, 180f, 0);
            direction = DirectionState.RIGHT;
        }
        if (direction == DirectionState.RIGHT && inputX < 0)
        {
            transform.rotation = Quaternion.Euler(0, 0, 0);
            direction = DirectionState.LEFT;
        }
    }

    private void UpdateWalkSprite(bool init)
    {
        walk_frames = (init || walk_frames == MAX_WALK_FRAMES) ? 0 : walk_frames + 1;
        if (walk_frames == 0)
        {
            spriteRenderer.sprite = (spriteRenderer.sprite == player_back_foot_raised) ?
                player_front_foot_raised : player_back_foot_raised;
        }
    }

        // FixedUpdate is called once per physics calculation
        private void FixedUpdate()
    {
        int inputX = getInputX();
        int inputY = getInputY();

        CheckSpriteDirectionChange(inputX);

        Vector2 moveDirection = body.velocity;
        if (inputX != 0) {
            moveDirection.x = Mathf.Lerp(body.velocity.x, SPEED * inputX, MOVE_INERTIA);
            UpdateWalkSprite(movingState == MovingState.STILL);
            if (movingState == MovingState.STILL) {
                movingState = MovingState.MOVING_LR;
            }
        } else {
            if (movingState == MovingState.MOVING_LR) {
                movingState = MovingState.STILL;
                spriteRenderer.sprite = player_still;
            }
        }

        if (inputY != 0 && jumpingState == JumpingState.NO_JUMP && IsGrounded())
        {
            moveDirection.y = (JUMP_SPEED * inputY);
            jumpingState = JumpingState.JUMP;
        } else if (jumpingState == JumpingState.JUMP && IsGrounded())
        {
            jumpingState = JumpingState.NO_JUMP;
        }
//        body.AddForce(moveDirection, ForceMode2D.Impulse);
        body.velocity = moveDirection;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Money"))
        {
            collision.gameObject.SetActive(false);
        }
    }

}
