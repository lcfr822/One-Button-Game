using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class GlobalFunctions
{
    public static Vector2 ScaleSpriteToScreensize(SpriteRenderer spriteRenderer)
    {
        spriteRenderer.transform.localScale = Vector3.one;

        var width = spriteRenderer.bounds.size.x;
        var height = spriteRenderer.bounds.size.y;

        var worldScreenHeight = Camera.main.orthographicSize * 2.0f;
        var worldScreenWidth = worldScreenHeight / Screen.height * Screen.width;

        return new Vector2(worldScreenWidth / width, worldScreenHeight / height);
    }
}
