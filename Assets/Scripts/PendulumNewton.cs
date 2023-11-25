using UnityEngine;
using Unity.Mathematics;

public class PendulumNewton : MonoBehaviour
{
    [SerializeField, Range(1, 2000)] private uint m_SimulationLoops;
    [SerializeField] private float3 m_Gravity;
    [SerializeField] private float m_Damping = 10.0f;
    [SerializeField] private float m_Distance;
    [SerializeField] private float m_SpringK = 1000.0f;
    [SerializeField] private Particle[] m_Particles;

    private int[] m_Edges;

    // Apply force
    // Apply gravity
    // Apply constraint
    // Calculate velocity
    // Apply damping

    private void SimulateOneTimeStep(float deltaTime)
    {
        for (int p = 1; p < this.m_Particles.Length; p++)
        {
            ParticleData particleData = this.m_Particles[p].ParticleData;

            // Apply gravity
            if (particleData.InvMass > 0.0f)
            {
                PhysicsUtil.ApplyAcceleration(
                    ref particleData.Velocity,
                    this.m_Gravity,
                    deltaTime
                );
            }

            // Update particle
            this.m_Particles[p].ParticleData = particleData;
        }

        int edgeCount = this.m_Edges.Length / 2;

        for (int e = 0; e < edgeCount; e++)
        {
            int edgeIndex = e * 2;
            int p0Index = this.m_Edges[edgeIndex];
            int p1Index = this.m_Edges[edgeIndex + 1];

            ParticleData particleData0 = this.m_Particles[p0Index].ParticleData;
            ParticleData particleData1 = this.m_Particles[p1Index].ParticleData;

            PhysicsUtil.DistanceConstraintSpring(
                // Particle 0
                ref particleData0.Force,
                particleData0.PredPosition,

                // Particle 1
                ref particleData1.Force,
                particleData1.PredPosition,

                this.m_Distance, this.m_SpringK
            );

            // Update particle
            this.m_Particles[p0Index].ParticleData = particleData0;
            this.m_Particles[p1Index].ParticleData = particleData1;
        }

        for (int p = 1; p < this.m_Particles.Length; p++)
        {
            ParticleData particleData = this.m_Particles[p].ParticleData;

            PhysicsUtil.ApplyForce(ref particleData.Velocity, particleData.InvMass, particleData.Force, deltaTime);

            PhysicsUtil.ApplyVelocity(
                ref particleData.PredPosition,
                particleData.Velocity,
                deltaTime
            );

            PhysicsUtil.VelocityDamping(
                ref particleData.Velocity,
                particleData.InvMass,
                this.m_Damping,
                deltaTime
            );

            // Update current particle position to the predicted position
            particleData.Position = particleData.PredPosition;

            // Resets force
            particleData.Force = 0.0f;

            // Update particle
            this.m_Particles[p].ParticleData = particleData;
        }
    }

    private void Start()
    {
        int edgeCount = this.m_Particles.Length - 1;
        this.m_Edges = new int[edgeCount * 2];

        for (int e = 0; e < edgeCount; e++)
        {
            int edgeIndex = e * 2;
            this.m_Edges[edgeIndex] = e;
            this.m_Edges[edgeIndex + 1] = e + 1;
        }
    }

    private void FixedUpdate()
    {
        // Special case for the first particle as we want to control it
        this.m_Particles[0].ParticleData.Position = this.m_Particles[0].transform.position;
        this.m_Particles[0].ParticleData.PredPosition = this.m_Particles[0].ParticleData.Position;

        float deltaTime = Time.deltaTime / this.m_SimulationLoops;

        for (int s = 0; s < this.m_SimulationLoops; s++)
        {
            this.SimulateOneTimeStep(deltaTime);
        }
    }
}
