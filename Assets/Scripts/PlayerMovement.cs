using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Mathematics;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject dragPoint;
    [SerializeField] private Rigidbody2D rb;



    [SerializeField] private float dragDistance = 1.5f;
    [SerializeField] private float shootForce = 100f;
    [SerializeField] private float damping = 0.99f;
    [SerializeField] private float wallDamping = 0.9f;

    public bool isMoving = false;
    private Vector2 lastVelocity = Vector2.zero;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        lastVelocity = rb.velocity;
        rb.velocity *= damping;
        if(isMoving && lastVelocity.magnitude <= 0.2f)
        {
            isMoving = false;
            rb.velocity = Vector2.zero;
        }
    }

    Vector2 difference = Vector2.zero;
    private void OnMouseDown()
    {
        //dragPoint.SetActive(true);
    }

    private void OnMouseDrag()
    {
        if (isMoving) return;
        dragPoint.transform.position = 
            (Vector2)transform.position - Vector2.ClampMagnitude( 
                (Vector2)transform.position - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition)
                , dragDistance
                );
    }

    private void OnMouseUp()
    {
        if (isMoving) return;
        
        //dragPoint.SetActive(false);
        rb.AddForce(-(Vector2)dragPoint.transform.localPosition * ((Vector2)dragPoint.transform.localPosition).magnitude * shootForce);
        dragPoint.transform.localPosition = Vector2.zero;

        Invoke("TriggerMoving", 0.1f);
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        Vector2 direction = Vector2.Reflect(lastVelocity.normalized, other.contacts[0].normal);
        rb.velocity = direction * lastVelocity.magnitude * wallDamping;
    }

    private void TriggerMoving()
    {
        isMoving = true;
    }
}
