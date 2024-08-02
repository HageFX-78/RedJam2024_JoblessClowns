using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Collections;
using Unity.Mathematics;
using UnityEngine;
using UnityEngine.EventSystems;

public class PlayerMovement : MonoBehaviour
{
    [SerializeField] private GameObject dragPoint;
    [SerializeField] private Rigidbody2D rb;
    [SerializeField] private GameObject sprite;


    [SerializeField] private float dragDistance = 1.5f;
    [SerializeField] private float shootForce = 100f;
    [SerializeField] private float damping = 0.99f;
    [SerializeField] private float wallDamping = 0.9f;

    public bool isMoving = false;
    private Vector2 lastVelocity = Vector2.zero;

    private IEnumerator spriteShake = null;
    private IEnumerator squishAndSquash = null;
    private Vector2 lastContactNormal = Vector2.zero;

    // ------------------------------------------------------- Unity functions

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
    private void OnMouseDown()
    {
        //dragPoint.SetActive(true);
    }

    private void OnMouseDrag()
    {
        if (isMoving) return;

        if(spriteShake == null)
        {
            spriteShake = ShakeSprite();
            StartCoroutine(spriteShake);
        }

        dragPoint.transform.position = 
            (Vector2)transform.position - Vector2.ClampMagnitude( 
                (Vector2)transform.position - (Vector2)Camera.main.ScreenToWorldPoint(Input.mousePosition)
                , dragDistance
                );
    }

    private void OnMouseUp()
    {
        if (isMoving) return;

        rb.AddForce(-(Vector2)dragPoint.transform.localPosition * ((Vector2)dragPoint.transform.localPosition).magnitude * shootForce);
        dragPoint.transform.localPosition = Vector2.zero;

        if(spriteShake != null)
        {
            StopCoroutine(spriteShake);
            spriteShake = null;
            sprite.transform.localPosition = Vector3.zero;
        }
        Invoke("TriggerMoving", 0.1f);
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Bounce off the wall
        lastContactNormal = other.contacts[0].normal;
        Debug.Log(lastContactNormal);
        Vector2 direction = Vector2.Reflect(lastVelocity.normalized, lastContactNormal);
        rb.velocity = direction * lastVelocity.magnitude * wallDamping;

        if(squishAndSquash == null)
        {
            squishAndSquash = SquishAndSquash();
            StartCoroutine(squishAndSquash);
        }else
        {
            
            StopCoroutine(squishAndSquash);
            squishAndSquash = null;
            ResetScale();

            squishAndSquash = SquishAndSquash();
            StartCoroutine(squishAndSquash);
        }
    }


    // ------------------------------------------------------- External functions
    private void TriggerMoving()
    {
        isMoving = true;
    }
    private void ResetScale()
    {
        sprite.transform.localScale = Vector2.one;
    }

    // ------------------------------------------------------- Coroutines
    public IEnumerator ShakeSprite()
    {
        Vector3 originalPos = sprite.transform.position;
        float shakeAmount = 0.05f;
        while (true)
        {
            sprite.transform.position = originalPos + UnityEngine.Random.insideUnitSphere * shakeAmount;
            yield return new WaitForSeconds(0.05f);
        }
    }

    public IEnumerator SquishAndSquash()
    {

        float duration = 0.3f;
        float currentTime = 0f; 
        Vector2 flipDirection = new Vector2(
            math.abs(lastContactNormal.x) > math.abs(lastContactNormal.y) ? 1 : math.abs(lastContactNormal.y) * 0.9f, 
            math.abs(lastContactNormal.y) > math.abs(lastContactNormal.x) ? 1 : math.abs(lastContactNormal.x) * 0.9f);
        // size sprite by 1.5 and go back to 1 with sine wave
        while (currentTime < duration)
        {
            
            sprite.transform.localScale = Vector2.Lerp(Vector2.one, Vector2.one * flipDirection, Mathf.Sin(currentTime / duration * Mathf.PI));
            currentTime += Time.deltaTime;
            yield return null;
        }
        ResetScale();
        squishAndSquash = null;
    }
}
