using Unity.Mathematics;

public static class PhysicsUtil
{
    // Annotations
    // t: time
    // F: force
    // a: acceleration
    // v: velocity
    // d: displacement

    public static void ApplyForce(ref float3 velocity, float3 invMass, float3 force, float deltaTime)
    {
        // F = m*a
        // a = F/m
        // a = v/t
        // v = a*t
        float3 acceleration = force * invMass;
        velocity += acceleration * deltaTime;
    }

    public static void ApplyAcceleration(ref float3 velocity, float3 acceleration, float deltaTime)
    {
        // a = v/t
        // v = a*t
        velocity += acceleration * deltaTime;
    }

    public static void ApplyVelocity(ref float3 position, float3 velocity, float deltaTime)
    {
        // v = d/t
        // d = v*t
        position += velocity * deltaTime;
    }

    public static void CalculateVelocity(ref float3 velocity, float3 displacement, float deltaTime)
    {
        // v = d/t
        velocity = displacement / deltaTime;
    }

    public static void VelocityDamping(ref float3 velocity, float invMass, float damping, float deltaTime)
    {
        velocity -= velocity * math.min(1.0f, damping * deltaTime * invMass);
    }

    public static void CalculateDisplacement(ref float3 position, float3 velocity, float deltaTime)
    {
        // v = d/t
        // d = v*t
        position += velocity * deltaTime;
    }

    public static void DistanceConstraintPBD(
        ref float3 position0, float invMass0,
        ref float3 position1, float invMass1,
        float distance, float strength
    )
    {
        // Calculate the difference vector
        float3 difference = position1 - position0;
        // Calculate the magnitude of the difference vector
        float magnitude = math.length(difference);

        // Distance between positions are too small, we risk dividing by zero
        if (magnitude <= math.EPSILON)
        {
            return;
        }

        // Normalize difference to get direction
        float3 direction = difference / magnitude;

        float invMassSum = invMass0 + invMass1;
        // The difference between the actual distance and the desired distance
        float distanceCorrection = magnitude - distance;

        // Apply the position correction based on inverse mass
        // The smaller the inverse mass, the lesser it is being effected
        // Infinite mass can be denoted using 0 as the invMass
        position0 += strength * direction * distanceCorrection * invMass0 / invMassSum;
        position1 -= strength * direction * distanceCorrection * invMass1 / invMassSum;
    }

    public static void DistanceConstraintSpring(
        ref float3 force0, float3 position0,
        ref float3 force1, float3 position1,
        float distance, float springK
    )
    {
        // Calculate the difference vector
        float3 difference = position1 - position0;
        // Calculate the magnitude of the difference vector
        float magnitude = math.length(difference);

        // Distance between positions are too small, we risk dividing by zero
        if (magnitude <= math.EPSILON)
        {
            return;
        }

        // Normalize difference to get direction
        float3 direction = difference / magnitude;

        // The difference between the actual distance and the desired distance (the x in the equation)
        float distanceCorrection = magnitude - distance;

        // F = kx
        float3 totalForce = springK * distanceCorrection * direction;

        force0 += totalForce;
        force1 -= totalForce;
    }
}
