using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PropBehaviour : MonoBehaviour
{
    [SerializeField] private Animator spriteAnimator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private ParticleSystem ps;
    [SerializeField] private Color32 color32;
    [SerializeField] private Color32 original;

    [SerializeField] private PropType propType;
    private IEnumerator resetColor = null;



    public void InitializeProp(Prop prop)
    {
        spriteRenderer.color = prop.propColorOverlay;
        spriteRenderer.size = prop.propSpriteSize;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            spriteAnimator.SetTrigger("Collide");
            spriteRenderer.color = color32;
            ps.Play();

            if (resetColor != null)
            {
                StopCoroutine(resetColor);
            }
            resetColor = ResetColor();
            StartCoroutine(resetColor);

            GameManager.instance.AddMoney(propType);
        }
    }

    private IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.color = original;
    }
}
