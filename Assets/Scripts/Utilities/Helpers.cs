using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class Helpers
{
    public static void DestroyChildren(this Transform t)
    {
        foreach (Transform child in t) Object.Destroy(child.gameObject);
    }
    public static void Shuffle<T>(this List<T> list)
    {
        if (list.Count == 1) return;
        for (int i = list.Count - 1; i > 0; i--)
        {
            int j = Random.Range(0, i + 1);
            (list[j], list[i]) = (list[i], list[j]);
        }
    }
    public static bool Contains<T>(this List<Ability> abilities) where T : Ability
    {
        return abilities.Any(ability => ability is T);
    }
    public static bool Contains<T>(this List<Ability> abilities, out T ability) where T : Ability
    {
        bool res = abilities.Any(ability => ability is T);
        if (res)
        {
            ability = abilities.Find(ability => ability is T) as T;
        }
        else
        {
            ability = null;
        }
        return res;
    }
    public static Vector3 TranslateScreenToWorld(this Vector3 position)
    {
        Vector3 cameraTranslatePos = Camera.main.ScreenToWorldPoint(position);
        return new Vector3(cameraTranslatePos.x, cameraTranslatePos.y, 0);
    }
    public static Vector2 TranslateScreenToWorld(this Vector2 position)
    {
        Vector3 cameraTranslatePos = Camera.main.ScreenToWorldPoint(position);
        return new Vector3(cameraTranslatePos.x, cameraTranslatePos.y, 0);
    }
}
