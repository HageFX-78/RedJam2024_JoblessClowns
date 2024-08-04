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
    [SerializeField] private GameObject pointerAnchor;
    [SerializeField] private GameObject pointerSprite;
    [SerializeField] private Animator indicatorRingAnimator;

    [Header("Movement parameters")]
    [SerializeField] private float dragDistance = 1.5f;
    [SerializeField] private float shootForce = 100f;
    [SerializeField] private float minDamping = 0.9999f;
    [SerializeField] private float maxDampThreshold = 2f;
    [SerializeField] private float maxDamping = 0.995f;
    [SerializeField] private float stopMovementThreshold = 0.2f;
    [SerializeField] private float wallDamping = 0.9f;

    [SerializeField] private float noiseRange = 0.2f;
    [Header("Debug")]
    public bool isMoving = false;
    private Vector2 lastVelocity = Vector2.zero;
    private float damping = 0.99f;

    private IEnumerator spriteShake = null;
    private IEnumerator squishAndSquash = null;
    private IEnumerator triggerMovingState = null;
    private Vector2 lastContactNormal = Vector2.zero;

    // ------------------------------------------------------- Unity functions

    void Update()
    {   if(! isMoving) return;
        lastVelocity = rb.velocity;
        rb.velocity *= damping;
        // Increase  damping when velocity is under threshold
        if(lastVelocity.magnitude <= maxDampThreshold)
        {
            damping = maxDamping;
            // Stops movement when under specified threshold
            if(lastVelocity.magnitude <= stopMovementThreshold && rb.velocity.magnitude <= stopMovementThreshold){
                indicatorRingAnimator.SetBool("active", true);
                isMoving = false;
                rb.velocity = Vector2.zero;
            }
        }
    }
    private void OnMouseDown()
    {
        if (isMoving) return;
        rb.velocity = Vector2.zero; // Stop movement in case
        pointerAnchor.SetActive(true);
        damping = minDamping;
    }
    private void OnMouseDrag()
    {
        if (isMoving) return;
        pointerAnchor.SetActive(true);
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

        // Pointer
        Vector2 dir = (Vector2)(dragPoint.transform.position - transform.position);
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        pointerAnchor.transform.rotation = Quaternion.Euler(0, 0, angle + 90);
        pointerSprite.transform.localScale = new Vector2(dir.magnitude * 2f, 1f);
    }

    private void OnMouseUp()
    {
        if (isMoving) return;
        pointerAnchor.SetActive(false);

        indicatorRingAnimator.SetBool("active", false);
        Vector2 finalForce = -(Vector2)dragPoint.transform.localPosition * ((Vector2)dragPoint.transform.localPosition).magnitude * shootForce;
        lastVelocity = finalForce; // Cache initial velocity

        rb.AddForce(finalForce);
        dragPoint.transform.localPosition = Vector2.zero; // Reset drag point

        if(spriteShake != null)
        {
            StopCoroutine(spriteShake);
            spriteShake = null;
            sprite.transform.localPosition = Vector3.zero;
        }

        if (triggerMovingState != null)
        {
            StopCoroutine(triggerMovingState);
            triggerMovingState = null;
        }
        triggerMovingState = TriggerMovingState();
        StartCoroutine(triggerMovingState);
        
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        // Bounce off the wall
        lastContactNormal = other.contacts[0].normal;
        Vector2 direction = Vector2.Reflect(lastVelocity.normalized, lastContactNormal);

        Vector2 veloNoise = new Vector2(UnityEngine.Random.Range(-noiseRange, noiseRange), UnityEngine.Random.Range(-noiseRange, noiseRange));
        rb.velocity = (direction * lastVelocity.magnitude * wallDamping) + veloNoise;

        Animate();
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Prop"))
        {
            // Rob item
            Animate();
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
    private void Animate()
    {
        if(squishAndSquash != null)
        {
            StopCoroutine(squishAndSquash);
            squishAndSquash = null;
            ResetScale();
        }
        squishAndSquash = SquishAndSquash();
        StartCoroutine(squishAndSquash);
    }

    // ------------------------------------------------------- Coroutines
    public IEnumerator TriggerMovingState()
    {
        yield return new WaitForSeconds(0.1f);
        TriggerMoving();
        triggerMovingState = null;
    }
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
        // size sprite by 0.9 and go back to 1 with sine wave
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
