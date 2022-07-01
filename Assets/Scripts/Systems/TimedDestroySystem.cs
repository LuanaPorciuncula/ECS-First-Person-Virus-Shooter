using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;
using Unity.Jobs;
using Unity.Collections;
using Unity.Physics;

public class TimedDestroySystem : JobComponentSystem
{
    protected override JobHandle OnUpdate(JobHandle inputDeps)
    {
        float dT = Time.DeltaTime;

        Entities.WithoutBurst().WithStructuralChanges()
                .ForEach((Entity entity,
                         ref Translation position,
                         ref LifeTimeData lifeTimeData) =>
                         {
                            lifeTimeData.lifeLeft -= dT;
                            if (lifeTimeData.lifeLeft <= 0f) EntityManager.DestroyEntity(entity);
                         })
                        .Run();

        Entities.WithoutBurst().WithStructuralChanges()
                   .ForEach((Entity entity,
                            ref Translation position,
                            ref VirusData virusData) =>
                            {
                                if (!virusData.isAlive)
                                {
                                    for (int i = 0; i < 100; i++)
                                    {
                                        float3 offset = (float3)UnityEngine.Random.insideUnitSphere * 2f;
                                        var splat = ECSManager.manager.Instantiate(ECSManager.whiteBlood);

                                        float x = UnityEngine.Random.Range(-1, 1);
                                        float y = UnityEngine.Random.Range(-1, 1);
                                        float z = UnityEngine.Random.Range(-1, 1);
                                        float3 randomDir = new float3(x, y, z);

                                        ECSManager.manager.SetComponentData(splat, new Translation { Value = position.Value + offset });
                                        ECSManager.manager.SetComponentData(splat, new PhysicsVelocity { Linear = randomDir * 2f });
                                    }

                                    EntityManager.DestroyEntity(entity);
                                }
                           })
                           .Run();

        return inputDeps;
    }
}
