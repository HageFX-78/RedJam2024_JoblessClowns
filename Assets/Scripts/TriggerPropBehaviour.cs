using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TriggerPropBehaviour : MonoBehaviour
{
    [SerializeField] private Animator spriteAnimator;
    [SerializeField] private SpriteRenderer spriteRenderer;
    [SerializeField] private Collider2D collider2D;

    [SerializeField] private PropType propType;

    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            collider2D.enabled = false;
            spriteAnimator.SetTrigger("Trigger");
            GameManager.instance.AddMoney(propType);

            Invoke("DestroyProp", 0.5f);
        }
    }
    private void DestroyProp()
    {
        Destroy(gameObject);
    }
}
