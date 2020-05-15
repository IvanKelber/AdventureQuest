using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(PlayerController))]
public class Player : MonoBehaviour
{
    [SerializeField]
    private float walkSpeed = 5;

    [SerializeField]
    private float runSpeed = 8;

    private Vector2 velocity = Vector2.zero;
    private PlayerController controller;

    [SerializeField]
    private Stats playerStats;

    // Start is called before the first frame update
    void Awake()
    {
        controller = GetComponent<PlayerController>();
    }

    // Update is called once per frame
    void Update()
    {
        velocity = GetInputVector() * Time.deltaTime;
        controller.Move(velocity);
    }

    Vector2 GetInputVector() {
        Vector2 v = new Vector2(Input.GetAxis("Horizontal"), Input.GetAxis("Vertical"));
        float speedScalar = Input.GetKey(KeyCode.Space) ? runSpeed : walkSpeed;
        v = Vector2.ClampMagnitude(v*speedScalar,speedScalar);
        return v;
    }
}
