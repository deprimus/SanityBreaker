using System.Collections;
using UnityEngine;

public class StaticCoroutine : MonoBehaviour
{
    private static StaticCoroutine singleton;

    private void OnDestroy() => singleton.StopAllCoroutines();
    private void OnApplicationQuit() => singleton.StopAllCoroutines();

    private static StaticCoroutine GetInstance()
    {
        if (singleton != null)
            return singleton;

        singleton = (StaticCoroutine) FindObjectOfType(typeof(StaticCoroutine));

        if (singleton != null)
            return singleton;

        GameObject instanceObject = new GameObject("StaticCoroutine");
        instanceObject.AddComponent<StaticCoroutine>();
        singleton = instanceObject.GetComponent<StaticCoroutine>();

        if (singleton != null)
            return singleton;

        Debug.LogError("Cannot get the StaticCoroutine instance");

        return null;
    }

    public static void Start(string methodName)               => GetInstance().StartCoroutine(methodName);
    public static void Start(string methodName, object value) => GetInstance().StartCoroutine(methodName, value);
    public static void Start(IEnumerator routine)             => GetInstance().StartCoroutine(routine);
}