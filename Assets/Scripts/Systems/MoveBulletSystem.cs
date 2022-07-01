using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;
using Unity.Physics;

public class MoveBulletSystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float dT = Time.DeltaTime;

        var jobHandle = Entities
                        .WithName("moveBulletSystem")
                        .ForEach((ref PhysicsVelocity physics,
                                  ref Translation position,
                                  ref Rotation rotation,
                                  ref BulletData bulletData) =>
                        {
                            physics.Angular = float3.zero;
                            physics.Linear = dT * bulletData.speed * math.forward(rotation.Value);
                        })
                        .Schedule(inputDeps);

        jobHandle.Complete();

        return jobHandle;
    }
}
