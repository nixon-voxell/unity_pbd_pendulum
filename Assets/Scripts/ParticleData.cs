using Unity.Mathematics;

/// <summary>Data that is needed for particle simulation.</summary>
[System.Serializable]
public struct ParticleData
{
    public float3 Position;
    public float3 PredPosition;
    public float3 Force;
    public float3 Velocity;
    public float InvMass;

    // public float Mass
    // {
    //     get
    //     {
    //         if (this.InvMass == 0.0)
    //         {
    //             return 99999999999.0f;
    //         }
    //         else
    //         {
    //             return 1.0f / this.InvMass;
    //         }
    //     }
    // }
}
