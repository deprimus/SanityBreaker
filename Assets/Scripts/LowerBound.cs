using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LowerBound : MonoBehaviour
{
    private void OnCollisionEnter2D(Collision2D other)
    {
        if(!GameManager.player.isDisgusted)
            GameManager.Reset();
    }
}
