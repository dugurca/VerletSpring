using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VerletUtil
{

    public static Vector3 GetVerletVelocity(Vector3 pos, Vector3 lastPos, float dt)
    {
        return (pos - lastPos) / dt;
    }

    public static Vector3 AddGravityForce(Vector3 forceInput, float mass)
    {
        return forceInput + C.GravityVector * mass;
    }

    public static Vector3 AddDampingForce(Vector3 forceInput, Vector3 verletVelocity)
    {
        return forceInput + C.DAMPING * verletVelocity;
    }
}
