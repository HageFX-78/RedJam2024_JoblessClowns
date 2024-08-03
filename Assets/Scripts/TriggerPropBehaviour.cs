using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPropBehaviour : MonoBehaviour
{
    [SerializeField] private Animator spriteAnimator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Color32 color32;
    [SerializeField] private Color32 original;

    private IEnumerator resetColor = null;
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            spriteAnimator.SetTrigger("Trigger");

        }
    }
}
