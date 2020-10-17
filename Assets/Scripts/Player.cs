using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Player : MonoBehaviour
{
    private new Camera camera;
    private Transform ballTransform;

    public new SpriteRenderer renderer;
    public new Transform transform;

    public bool isAI = false;

    public bool isConfused = false;
    public bool isDisgusted = false;
    public bool isObsessed = false;

    public Vector3 desiredPos;

    void Start()
    {
        camera = FindObjectOfType<Camera>();
        ballTransform = FindObjectOfType<Ball>().GetComponent<Transform>();
        renderer = GetComponent<SpriteRenderer>();
        transform = GetComponent<Transform>();
    }

    void Update()
    {
        UpdatePos();   
    }

    void UpdatePos()
    {
        if (!isAI)
        {
            if(!GameManager.paused && GameManager.started)
                transform.position = new Vector2((isConfused ? -1 : 1) * Mathf.Max(-camera.orthographicSize * camera.aspect, Mathf.Min(camera.orthographicSize * camera.aspect, camera.ScreenToWorldPoint(Input.mousePosition).x)), transform.position.y);
        }
        else
        {
            desiredPos = new Vector3(ballTransform.position.x, transform.position.y, transform.position.z);

            if(isDisgusted)
                desiredPos = new Vector3((ballTransform.position.x < 0 ? 1 : -1)* camera.orthographicSize * camera.aspect, desiredPos.y, desiredPos.z);

            transform.position = desiredPos;
        }
    }

    public void Reset()
    {
        isConfused = false;
        isDisgusted = false;
    }
}
