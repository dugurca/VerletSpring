using UnityEngine;
using System.Collections;

public static class C
{
    public const float GRAVITY = -9.81f;
    public const float DAMPING = -0.0125f;
    public static Vector3 GravityVector = new Vector3(0f, GRAVITY, 0f);

    public const float KS = 0.5f;
    public const float KD = -0.05f;
}
