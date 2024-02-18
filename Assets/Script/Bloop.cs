using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Bloop : MonoBehaviour
{
    [Header(" Elements")]
    [SerializeField] private SpriteRenderer spriteRenderer;

    [Header(" Data")]
    [SerializeField] private BloopType bloopType;
    private bool hasCollided;
    private bool canBeMerged;

    [Header(" Actions")]
    public static Action<Bloop, Bloop> onCollisionWithBloop;

    [Header(" Effects ")]
    [SerializeField] private ParticleSystem mergeParticles;

    // Start is called before the first frame update
    void Start()
    {
        Invoke("AllowMerge", .25f);
    }

    private void AllowMerge()
    {
        canBeMerged = true;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void MoveTo(Vector2 targetPosition)
    {
        transform.position = targetPosition;
    }

    public void EnablePhysics()
    {
        GetComponent<Rigidbody2D>().bodyType = RigidbodyType2D.Dynamic;
        GetComponent<Collider2D>().enabled = true;
    }

    

    private void OnCollisionEnter2D(Collision2D collision)
    {
        ManageCollision(collision);
    }

    private void OnCollisionStay2D(Collision2D collision)
    {
        ManageCollision(collision);
    }

    private void ManageCollision(Collision2D collision)
    {
        hasCollided = true;

        if (!canBeMerged)
            return;

        if (collision.collider.TryGetComponent(out Bloop otherBloop))
        {
            if (otherBloop.GetBloopType() != bloopType)
                return;

            if (!otherBloop.CanBeMerged())
                return;

            onCollisionWithBloop?.Invoke(this, otherBloop);
        }
    }


    public void Merge()
    {
        if (mergeParticles != null)
        {
            mergeParticles.transform.SetParent(null);
            mergeParticles.Play();

        }


        Destroy(gameObject);
    }
    public BloopType GetBloopType()
    {
        return bloopType;
    }

    public Sprite GetSprite()
    {
        return spriteRenderer.sprite;
    }

    public bool HasCollided()
    {
        return hasCollided;
    }

    public bool CanBeMerged()
    {
        return canBeMerged;
    }
}
