using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brick : MonoBehaviour
{
    public new Transform transform;

    void Start() => transform = GetComponent<Transform>();

    private void OnCollisionEnter2D(Collision2D other)
    {
        if(!GameManager.player.isObsessed)
            BrickManager.DestroyBrick(this);
    }
}
