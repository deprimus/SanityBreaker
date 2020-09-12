using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public float speed = 5.0f;

    public Rigidbody2D rigidBody;
    public new Transform transform;

    private Transform target;
    public Vector2 baseVelocity;

    void Start()
    {
        rigidBody = GetComponent<Rigidbody2D>();
        transform = GetComponent<Transform>();
        Reset();
    }

    void Update()
    {
        if(Deprimus.curiosusCount > 0)
        {
            if(Deprimus.ordinariusCount <= 0)
            {
                if(target == null)
                    FindNearestTarget();

                if(target != null)
                {
                    Vector3 diff = target.position - transform.position;
                    diff.Normalize();

                    transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg + 90);

                    rigidBody.velocity = -transform.up * speed;
                }
            }
        }
        else if(Deprimus.ordinariusCount > 0)
        {
            FindNearestTarget();

            if(target != null)
            {
                Vector2 diff = target.position - transform.position;
                diff.Normalize();

                transform.eulerAngles = new Vector3(0, 0, Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg + 90);

                rigidBody.velocity = transform.up * speed;
            }
        }
        else if(rigidBody.velocity.x == 0f || rigidBody.velocity.y == 0f)
        {
            baseVelocity = Random.insideUnitCircle.normalized;

            rigidBody.velocity = baseVelocity * speed;
        }
    }

    void FindTarget()
    {
        if(DeprimusManager.deprimuses.Count > 0)
            target = DeprimusManager.deprimuses[Random.Range(0, DeprimusManager.deprimuses.Count)].GetComponent<Transform>();
    }

    void FindNearestTarget()
    {
        Vector2 minDiff = Vector2.zero;

        foreach(Deprimus deprimus in DeprimusManager.deprimuses)
        {
            if (!deprimus.consumed)
            {
                Vector2 diff = deprimus.transform.position - transform.position;

                if (minDiff == Vector2.zero || minDiff.magnitude > diff.magnitude)
                {
                    minDiff = diff;
                    target = deprimus.transform;
                }
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(GameManager.player.isDisgusted && other.gameObject.tag.Equals("Player"))
            GameManager.Reset();
        else SoundManager.Play(SoundManager.Clip.HIT);
    }

    public void Reset()
    {
        transform.position = Vector3.zero;
        transform.rotation = Quaternion.identity;
        baseVelocity = Random.insideUnitCircle.normalized;

        rigidBody.velocity = baseVelocity * speed;
    }
}
