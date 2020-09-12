using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private new Camera camera;

    public new SpriteRenderer renderer;

    public bool isConfused = false;
    public bool isDisgusted = false;
    public bool isObsessed = false;

    void Start()
    {
        camera = FindObjectOfType<Camera>();
        renderer = GetComponent<SpriteRenderer>();
    }

    void Update()
    {
        if(!GameManager.paused && GameManager.started)
            transform.position = new Vector2((isConfused ? -1 : 1) * Mathf.Max(-camera.orthographicSize * camera.aspect, Mathf.Min(camera.orthographicSize * camera.aspect, camera.ScreenToWorldPoint(Input.mousePosition).x)), transform.position.y);
    }

    public void Reset()
    {
        isConfused = false;
        isDisgusted = false;
    }
}
