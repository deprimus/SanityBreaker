using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeprimusManager : MonoBehaviour
{
    public GameObject prefab;
    private static GameObject _internalPrefab;
    private static Renderer prefabRenderer;

    private class Limits
    {
        public float left;
        public float right;
        public float top;
        public float bottom;
    }

    private static Limits spawnLimits;

    private static float clock;
    public static readonly float SPAWN_PROB_PER_SEC = 0.3f;

    public static float actualSpawnProb;

    private class EffectInfo
    {
        public float remaining;
        public Delegates.ShallowDelegate untrigger;

        public EffectInfo(float remaining, Delegates.ShallowDelegate untrigger) { this.remaining = remaining; this.untrigger = untrigger; }
    }

    private static List<EffectInfo> effects;

    public static List<Deprimus> deprimuses;

    void Start()
    {
        _internalPrefab = prefab;
        effects = new List<EffectInfo>();
        deprimuses = new List<Deprimus>();
        prefabRenderer = _internalPrefab.GetComponent<Renderer>();
        clock = 0f;

        Player player = FindObjectOfType<Player>();
        Camera camera = FindObjectOfType<Camera>();

        spawnLimits = new Limits();
        spawnLimits.left = -camera.orthographicSize * camera.aspect + prefabRenderer.bounds.size.x / 2f;
        spawnLimits.right = camera.orthographicSize * camera.aspect - prefabRenderer.bounds.size.x / 2f;
        spawnLimits.top = BrickManager.GetTopLimit() - prefabRenderer.bounds.size.y / 2f;
        spawnLimits.bottom = player.transform.position.y + player.GetComponent<Renderer>().bounds.size.y / 2f + prefabRenderer.bounds.size.y / 2f;

        actualSpawnProb = SPAWN_PROB_PER_SEC;
    }

    void Update()
    {
        TickEffects();
        TickClock();
    }

    void TickEffects()
    {
        for (int i = 0; i < effects.Count;)
        {
            effects[i].remaining -= Time.deltaTime;

            if (effects[i].remaining <= 0f)
            {
                effects[i].untrigger();
                effects.RemoveAt(i);
            }
            else ++i;
        }
    }

    void TickClock()
    {
        clock += Time.deltaTime;

        while(clock > 1.0f)
        {
            clock -= 1.0f;

            // Integer range, because it's [min, max), while float range is [min, max]. I've worked with LibGDX so much that I can't trust fully inclusive ranges.
            if(actualSpawnProb > UnityEngine.Random.Range(0, 100) / 100f)
                Spawn();
        }
    }

    public static void Spawn()
    {
        Array pool = Enum.GetValues(typeof(Deprimus.Type));
        SpawnOfType((Deprimus.Type) pool.GetValue(UnityEngine.Random.Range(0, pool.Length)));
    }

    public static void SpawnOfType(Deprimus.Type type)
    {
        Vector2 pos = new Vector2(UnityEngine.Random.Range(spawnLimits.left, spawnLimits.right), UnityEngine.Random.Range(spawnLimits.bottom, spawnLimits.top));

        Deprimus deprimus = Instantiate(_internalPrefab, pos, Quaternion.identity).GetComponent<Deprimus>();
        deprimus.Construct(type);

        deprimuses.Add(deprimus);

        SoundManager.Play(SoundManager.Clip.SPAWN);
    }

    public static void Reset()
    {
        foreach(EffectInfo effect in effects)
            effect.untrigger();
        effects.Clear();

        while(deprimuses.Count > 0)
        {
            Destroy(deprimuses[0].gameObject);
            deprimuses.RemoveAt(0);
        }

        actualSpawnProb = SPAWN_PROB_PER_SEC;
    }

    public static void Consume(Deprimus deprimus, float time, Delegates.ShallowDelegate onTrigger, Delegates.ShallowDelegate onUntrigger)
    {
        onTrigger();

        if(time != 0f)
            effects.Add(new EffectInfo(time, onUntrigger));

        deprimuses.Remove(deprimus);

        StaticCoroutine.Start(deprimus.FadeOut());
    }
}
