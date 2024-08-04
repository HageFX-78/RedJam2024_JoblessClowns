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

    [Header("Extra")]
    [SerializeField] private Prop specifiedProp;

    private Collider2D thisCollider;
    public Prop propData;
    private IEnumerator resetColor = null;

    void Start()
    {   
        if(specifiedProp != null)
            InitializeProp(specifiedProp);
    }
    public void InitializeProp(Prop prop)
    {
        spriteRenderer.color = prop.propColorOverlay;
        spriteRenderer.size = prop.propSpriteSize;
        spriteRenderer.sprite = prop.propSprite;
        propData = prop;

        if (prop.colliderType == ColliderType.Circle)
        {
            CircleCollider2D circleCollider = gameObject.AddComponent<CircleCollider2D>();
            circleCollider.radius = prop.propColliderRadius;
            thisCollider = circleCollider;
        }
        else
        {
            BoxCollider2D boxCollider = gameObject.AddComponent<BoxCollider2D>();
            boxCollider.size = prop.propColliderSize;
            thisCollider = boxCollider;
        }
        thisCollider.isTrigger = prop.propType == PropType.Trigger;
        thisCollider.sharedMaterial = prop.physicsMaterial;
    }
    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            if(propData.propType == PropType.Normal)
            {
                SFXManager.Instance.PlaySoundFXClip(AudioStorage.Instance.getAudioClip("hit"), transform, 1f);
                spriteAnimator.SetTrigger("Collide");
            }
            else if(propData.propType == PropType.Buds)
            {
                if(gameObject.name == "Bam")
                {
                    SFXManager.Instance.PlaySoundFXClip(AudioStorage.Instance.getAudioClip("bam"), transform, 1f);
                }
                else if(gameObject.name == "Ogu")
                {
                    SFXManager.Instance.PlaySoundFXClip(AudioStorage.Instance.getAudioClip("ogu"), transform, 1f);
                }
                else if(gameObject.name == "Tappy")
                {
                    SFXManager.Instance.PlaySoundFXClip(AudioStorage.Instance.getAudioClip("tappy"), transform, 1f);
                } 
                
                spriteAnimator.SetTrigger("Buds");
            }else
            {
                SFXManager.Instance.PlaySoundFXClip(AudioStorage.Instance.getAudioClip("hit"), transform, 1f);
            }

            ps.Play();

            if (resetColor != null)
            {
                StopCoroutine(resetColor);
            }
            resetColor = ResetColor();
            StartCoroutine(resetColor);

            GameManager.instance.AddMoney(propData.propBaseCoinValue);
            GameManager.instance.ShakeCamera();
        }
    }
    void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            thisCollider.enabled = false;
            spriteAnimator.SetTrigger("Trigger");
            GameManager.instance.AddMoney(propData.propBaseCoinValue);
            ps.Play();

            Invoke("DestroyProp", 0.5f);
        }
    }
    private void DestroyProp()
    {
        Destroy(gameObject);
    }

    private IEnumerator ResetColor()
    {
        yield return new WaitForSeconds(0.3f);
        spriteRenderer.color = Color.white;
    }
}
