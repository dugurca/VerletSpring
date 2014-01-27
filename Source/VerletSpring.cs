using UnityEngine;
using System.Collections;

public class VerletSpring : MonoBehaviour
{
    public GameObject sphere;

    Vector3 anchorPos;
    Vector3 springEnd;

    private static Vector3 sforce;
    private static Spring spring;

    public static Vector3 cubePosition;
    public static Vector3 lastCubePosition;

    RaycastHit hit;

    private static float mass = 1f;
    private static float dt = 1f / 60f;

    private LineRenderer lineRenderer;

    private void InitPositions()
    {
        springEnd = new Vector3(0, 3, 0);
        anchorPos = new Vector3(0, 4, 0);

        cubePosition = springEnd;
        lastCubePosition = springEnd;
    }

    private void InitSpring()
    {
        spring = new Spring();
        spring.P1 = 0;
        spring.P2 = 1;
        spring.Ks = 25f;
        spring.Kd = -0.35f;
        spring.RestLength = 3f;
    }

    // Use this for initialization
    void Start()
    {
        Time.timeScale = 2.5f;
        InitPositions();
        InitSpring();
        InitLineRenderer();
    }

    private void InitLineRenderer()
    {
        lineRenderer = this.gameObject.AddComponent<LineRenderer>();
        lineRenderer.enabled = true;
        lineRenderer.SetWidth(0.1f, 0.1f);
    }

    private void ComputeDefaultForce()
    {
        sforce = Vector3.zero;
        Vector3 verletVelocity = VerletUtil.GetVerletVelocity(cubePosition, lastCubePosition, dt);
        sforce = VerletUtil.AddGravityForce(sforce, mass);
        sforce = VerletUtil.AddDampingForce(sforce, verletVelocity);
    }

    private void ComputeSpringForces()
    {
        Vector3 p1 = cubePosition;
        Vector3 p1Last = lastCubePosition;

        Vector3 verletVelocity1 = VerletUtil.GetVerletVelocity(p1, p1Last, dt);

        Vector3 deltaPosition = p1 - anchorPos;
        Vector3 deltaVelocity = verletVelocity1;

        float dist = Vector3.Magnitude(deltaPosition);
        if (dist <= spring.RestLength) return;
        float leftTerm = -spring.Ks * (dist - spring.RestLength);
        float rightTerm = spring.Kd * (Vector3.Dot(deltaVelocity, deltaPosition) / dist);
        Vector3 springForce = (leftTerm + rightTerm) * Vector3.Normalize(deltaPosition);
        sforce += springForce;
    }

    private void IntegrateVerlet()
    {
        float ddm = (dt * dt) / mass;
        Vector3 buffer = cubePosition;
        cubePosition = cubePosition + (cubePosition - lastCubePosition) + ddm * sforce;
        lastCubePosition = buffer;
    }

    void FixedUpdate()
    {
        dt = Time.deltaTime;

        if (Input.GetMouseButton(0))
        {
            ManipulateSphere();
        }
        else
        {
            ComputeVerlet();
        }
        sphere.transform.position = cubePosition;
        RenderLine();
    }

    private void ManipulateSphere()
    {
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        Vector3 newPos = new Vector3(ray.origin.x + ray.direction.x * 10, ray.origin.y + ray.direction.y * 10, 0);
        cubePosition = newPos;
        lastCubePosition = newPos;
    }

    private void ComputeVerlet()
    {
        ComputeDefaultForce();
        ComputeSpringForces();
        IntegrateVerlet();
    }

    private void RenderLine()
    {
        lineRenderer.SetPosition(0, sphere.transform.position);
        lineRenderer.SetPosition(1, anchorPos);
    }
}
