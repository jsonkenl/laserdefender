using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerShip : MonoBehaviour
{
    [SerializeField] float moveSpeed = 35f;
    [SerializeField] float paddingLeft;
    [SerializeField] float paddingRight;
    [SerializeField] float paddingTop;
    [SerializeField] float paddingBottom;
    Vector2 rawInput;
    Vector2 minBounds;
    Vector2 maxBounds;
    Vector2 previousPos;
    Shooter shooter;
    GameObject playerChild;
    Animator childAnimator;
    SpriteRenderer childSprite;

    void Awake()
    {
        shooter = GetComponent<Shooter>();
    }

    void Start()
    {
        InitBounds();
        playerChild = gameObject.transform.Find("Player Exhaust").gameObject;
        childAnimator = playerChild.GetComponent<Animator>();
        childSprite = playerChild.GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        previousPos = transform.position;
        Move();
    }

    void InitBounds()
    {
        Camera mainCamera = Camera.main;
        minBounds = mainCamera.ViewportToWorldPoint(new Vector2(0, 0));
        maxBounds = mainCamera.ViewportToWorldPoint(new Vector2(1, 1));
    }

    void Move()
    {
        Vector2 delta = rawInput * moveSpeed * Time.deltaTime;
        Vector2 newPos = new Vector2();
        newPos.x = Mathf.Clamp(transform.position.x + delta.x, minBounds.x + paddingLeft, maxBounds.x - paddingRight);
        newPos.y = Mathf.Clamp(transform.position.y + delta.y, minBounds.y + paddingBottom, maxBounds.y - paddingTop);
        transform.position = newPos;

        bool isMovingForward = newPos.y > previousPos.y;
        childSprite.enabled = isMovingForward;
        childAnimator.SetBool("isMovingForward", isMovingForward);
    }

    void OnMove(InputValue value)
    {
        rawInput = value.Get<Vector2>();
    }

    void OnFire(InputValue value)
    {
        if (shooter != null)
        {
            shooter.isFiring = value.isPressed;
        }
    }
}
