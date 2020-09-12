using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BrickManager : MonoBehaviour
{
    private static new Transform transform;

    public GameObject prefab;
    private static Renderer prefabRenderer;

    private static GameObject _internalPrefab;

    public static byte rowCount = 6;
    public static byte columnCount = 14;
    public static float padding = 0.05f;

    private static Vector2 basePos;

    public static List<GameObject> bricks;

    public static List<Vector2> freePositions;

    void Start()
    {
        _internalPrefab = prefab;
        bricks = new List<GameObject>();
        freePositions = new List<Vector2>();

        transform = GetComponent<Transform>();

        Camera camera = FindObjectOfType<Camera>();

        float prefabWidth = (camera.orthographicSize * camera.aspect * 2f - (columnCount + 1) * padding) / columnCount;

        Transform prefabTransform = _internalPrefab.GetComponent<Transform>();
        prefabRenderer = _internalPrefab.GetComponent<SpriteRenderer>();

        prefabTransform.localScale = new Vector3(prefabTransform.localScale.x * (prefabWidth / prefabRenderer.bounds.size.x), prefabTransform.localScale.y, prefabTransform.localScale.z);

        transform.position = new Vector3(-camera.orthographicSize * camera.aspect + prefabWidth / 2f + padding, transform.position.y, transform.position.z);

        basePos = transform.position;

        Reset();
    }

    public static void Reset()
    {
        foreach(GameObject brick in bricks)
            Destroy(brick);
        bricks.Clear();

        for(byte column = 0; column < columnCount; ++column)
        {
            for(byte row = 0; row < rowCount; ++row)
            {
                Vector2 pos = (Vector2) basePos + new Vector2(column * (prefabRenderer.bounds.size.x + padding), -row * (prefabRenderer.bounds.size.y + padding));
                CreateBrick(pos);
            }
        }

        freePositions.Clear();
    }

    public static void CreateBrick(Vector2 pos)
    {
        GameObject brick = Instantiate(_internalPrefab, pos, Quaternion.identity);
        bricks.Add(brick);
    }

    public static void DestroyBrick(Brick brick)
    {
        DestroyBrick(brick.gameObject);
    }

    public static void DestroyBrick(GameObject brick)
    {
        bricks.Remove(brick);
        Destroy(brick);

        if (bricks.Count == 0)
        {
            GameManager.Reset();
            GameManager.ToggleStarted();
        }

        freePositions.Add(brick.transform.position);
    }

    public static void RecreateBrick(int index)
    {
        CreateBrick(freePositions[index]);
        freePositions.RemoveAt(index);
    }

    public static float GetTopLimit()
    {
        return ((Vector2) basePos).y - rowCount * (prefabRenderer.bounds.size.y + padding);
    }
}
