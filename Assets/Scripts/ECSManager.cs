using UnityEngine;
using Unity.Entities;
using Unity.Mathematics;
using Unity.Transforms;

public class ECSManager : MonoBehaviour
{
    public static EntityManager manager;

    public GameObject virusPrefab;
    public GameObject redBloodPrefab;
    public GameObject whiteBloodPrefab;
    public GameObject bulletPrefab;
    public GameObject player;

    [SerializeField] private int numVirus = 500;
    [SerializeField] private int numBlood = 500;
    [SerializeField] private int numBullets = 10;

    Entity virus;
    Entity redBlood;
    Entity bullet;
    public static Entity whiteBlood;

    BlobAssetStore store;

    void Start()
    {
        store = new BlobAssetStore();
        manager = World.DefaultGameObjectInjectionWorld.EntityManager;
        var settings = GameObjectConversionSettings.FromWorld(World.DefaultGameObjectInjectionWorld, store);

        virus = GameObjectConversionUtility.ConvertGameObjectHierarchy(virusPrefab, settings);
        redBlood = GameObjectConversionUtility.ConvertGameObjectHierarchy(redBloodPrefab, settings);
        whiteBlood = GameObjectConversionUtility.ConvertGameObjectHierarchy(whiteBloodPrefab, settings);
        bullet = GameObjectConversionUtility.ConvertGameObjectHierarchy(bulletPrefab, settings);

        CreateFloatingEntities(numVirus, virus);
        CreateFloatingEntities(numBlood, redBlood);
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            ShootBullets();
        }
    }

    private void OnDestroy()
    {
        store.Dispose();
    }

    private void CreateFloatingEntities(int numEnt, Entity ent)
    {
        for (int i = 0; i < numEnt; i++)
        {
            var instance = manager.Instantiate(ent);

            float x = UnityEngine.Random.Range(-50, 50);
            float y = UnityEngine.Random.Range(-50, 50);
            float z = UnityEngine.Random.Range(-50, 50);

            float3 pos = new float3(x, y, z);
            manager.SetComponentData(instance, new Translation { Value = pos });

            float rSpeed = UnityEngine.Random.Range(1, 10) / 10f;
            manager.SetComponentData(instance, new FloatData { speed = rSpeed });
        }
    }

    private void ShootBullets()
    {
        for (int i = 0; i < numBullets; i++)
        {
            var instance = manager.Instantiate(bullet);
            var startPos = player.transform.position + UnityEngine.Random.insideUnitSphere * 2;
            manager.SetComponentData(instance, new Translation { Value = startPos });
            manager.SetComponentData(instance, new Rotation { Value = player.transform.rotation });
        }
    }
}