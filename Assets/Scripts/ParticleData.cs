using Unity.Mathematics;

/// <summary>Data that is needed for particle simulation.</summary>
[System.Serializable]
public struct ParticleData
{
    public float3 Position;
    public float3 PredPosition;
    public float3 Velocity;
    public float InvMass;
}
