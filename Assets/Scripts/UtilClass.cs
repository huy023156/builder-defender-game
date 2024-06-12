using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class UtilClass
{
    private static Camera camera;

    public static Vector3 GetMouseWorldPosition()
    {
        if (camera == null) camera = Camera.main;

        Vector3 mouseWorldPosition = camera.ScreenToWorldPoint(Input.mousePosition);
        mouseWorldPosition.z = 0;

        return mouseWorldPosition;
    }

    public static Vector3 GetRandomDir()
    {
        return new Vector3(
                Random.Range(-1f, 1f),
                Random.Range(-1f, 1f)
            ).normalized;
    }

    public static float GetAngleFromVector(Vector3 vector)
    {
        float radians = Mathf.Atan2(vector.y, vector.x);
        float degrees = radians * Mathf.Rad2Deg;

        return degrees;
    }
}
