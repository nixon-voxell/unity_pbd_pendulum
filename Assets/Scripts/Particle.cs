using UnityEngine;
using Unity.Mathematics;

public class Particle : MonoBehaviour
{
    public bool DontUpdateParticle;
    public ParticleData ParticleData;

    // Initialize the data
    private void Start()
    {
        this.ParticleData.Position = this.transform.position;
        this.ParticleData.PredPosition = this.transform.position;
        this.ParticleData.Force = float3.zero;
        this.ParticleData.Velocity = float3.zero;
    }

    // Transfer position data to the game object's transform
    private void Update()
    {
        if (DontUpdateParticle)
        {
            return;
        }

        this.transform.position = this.ParticleData.Position;
    }
}
