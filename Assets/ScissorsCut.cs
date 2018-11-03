using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public class ScissorsCut : MonoBehaviour {


    private SpriteRenderer spriteRenderer;
    public Sprite scissors_open;
    public Sprite scissors_cutting;
    public Sprite scissors_closed;

    public enum ScissorsState { OPEN, CUTTING, CLOSED};
    public ScissorsState state = ScissorsState.OPEN;

    private int framesToCut;
    private bool is_player_scissors = false;

	void Start () {
        spriteRenderer = GetComponent<SpriteRenderer>();
        if (state != ScissorsState.OPEN)
        {
            spriteRenderer.sprite = scissors_closed;
        }
    }

    /** lets you register / deregister player scissor input */
    public void RegisterPlayerScissors(bool toRegister = true)
    {
        is_player_scissors = toRegister;
    }

    /** lets you activate scissors */
    public void Activate(bool toActivate = true)
    {
        gameObject.SetActive(toActivate);
    }

    /** Called as part of FixedUpdate */
    private void Cut()
    {
        framesToCut = 8;
        state = ScissorsState.CUTTING;
        spriteRenderer.sprite = scissors_cutting;
    }

    private void Close()
    {
        state = ScissorsState.CLOSED;
        spriteRenderer.sprite = scissors_closed;
    }

    private void Open()
    {
        state = ScissorsState.OPEN;
        spriteRenderer.sprite = scissors_open;
    }

    private void Update()
    {
        if (is_player_scissors)
        {
            if (Input.GetButtonUp("Fire1") && state != ScissorsState.OPEN)
            {
                Open();
            }
            else if (Input.GetButtonDown("Fire1") && state == ScissorsState.OPEN)
            {
                Debug.Log("inputX@WWW");
                Cut();
            }
        }
    }

    private void FixedUpdate()
    {
        if (state == ScissorsState.CUTTING)
        {
            if (framesToCut == 0)
            {
                Close();
            }
            else
            {
                framesToCut -= 1;
            }
        }
    }

    private void OnTriggerStay2D(Collider2D other)
    {
        if (state != ScissorsState.CUTTING) {
            return;
        }
        Open();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player") && !is_player_scissors)
        {
            collision.gameObject.GetComponent<Playerrr>().ActivateScissors();
        }
    }
}
