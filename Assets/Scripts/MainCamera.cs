using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainCamera : MonoBehaviour
{
    public static new Transform transform;
    public static Vector3 basePos;

    private static readonly float CRIZATUS_SHAKE_MAGNITUDE = 0.05f;
    private static readonly float NERVOSUS_SHAKE_MAGNITUDE = 0.25f;
    private static readonly float HLIZATUS_SHAKE_MAGNITUDE = 0.5f;

    void Start()
    {
        transform = GetComponent<Transform>();
        basePos = transform.localPosition;
    }

    void Update()
    {
        if(Deprimus.hlizatusCount > 0)
            transform.localPosition = basePos + (Vector3) (Random.insideUnitCircle * HLIZATUS_SHAKE_MAGNITUDE);
        else if(Deprimus.nervosusCount > 0)
            transform.localPosition = basePos + (Vector3)(Random.insideUnitCircle * NERVOSUS_SHAKE_MAGNITUDE);
        else if(Deprimus.crizatusCount > 0)
            transform.localPosition = basePos + (Vector3)(Random.insideUnitCircle * CRIZATUS_SHAKE_MAGNITUDE);
    }
}
