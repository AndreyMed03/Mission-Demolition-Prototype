using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class Cloud : MonoBehaviour
{
    [Header("Set in Inspector")] 
    public GameObject cloudSphere;
    public int numSpheresMin = 6; // Минимальное количество сфер в облаке
    public int numSpheresMax = 10; // Максимальное количество сфер в облаке
    public Vector3 sphereOffsetScale = new Vector3(5,2,1); // Максимальное расстояние сферы от центра облака вдоль каждой оси
    // Диапазон масштаба сферы вдоль каждой оси
    public Vector2 sphereScaleRangeX = new Vector2(4, 8); 
    public Vector2 sphereScaleRangeY = new Vector2(3, 4);
    public Vector2 sphereScaleRangeZ = new Vector2(2, 4);
    public float scaleYMin = 2f; // Уменьшение масштаба сферы по оси Y в зависимости от расстояния удалённости от центра облака
    private List<GameObject> spheres;
    void Start()
    {
        spheres = new List<GameObject>();
        // Пышность облака (от 6 до 10)
        int num = Random.Range(numSpheresMin, numSpheresMax);
        for (int i = 0; i < num; i++)
        {
            GameObject sp = Instantiate<GameObject>(cloudSphere);
            spheres.Add(sp);
            Transform spTrans = sp.transform;
            spTrans.SetParent(this.transform);
            // Случайное расположение сфер в большом облаке
            Vector3 offset = Random.insideUnitSphere;
            offset.x *= sphereOffsetScale.x;
            offset.z *= sphereOffsetScale.z;
            offset.y *= sphereOffsetScale.y;
            spTrans.localPosition = offset;
            // Случайный масштаб сфер в облаке
            Vector3 scale = Vector3.one;
            scale.x = Random.Range(sphereScaleRangeX.x, sphereScaleRangeX.y);
            scale.y = Random.Range(sphereScaleRangeY.x, sphereScaleRangeX.y);
            scale.z = Random.Range(sphereScaleRangeZ.x, sphereScaleRangeX.y);
            // Чем дальше сфера от центра облака, тем меньше масштаб сферы по оси Y 
            scale.y *= 1 - (Mathf.Abs(offset.x) / sphereOffsetScale.x);
            scale.y = Mathf.Max(scale.y, scaleYMin);

            spTrans.localScale = scale;
        }
    }
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            Restart();
        }
    }

    void Restart()
    {
        foreach (GameObject sp in spheres)
        {
            Destroy(sp);
        }
        Start();
    }
}
