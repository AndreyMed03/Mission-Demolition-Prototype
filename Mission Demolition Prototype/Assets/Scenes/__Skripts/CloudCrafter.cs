using UnityEngine;
using Random = UnityEngine.Random;

public class CloudCrafter : MonoBehaviour
{
    [Header("Set in Inspector")] public int numClouds = 40; // Кол-во облаков
    public GameObject cloudPrefab; // шаблон для облаков
    public Vector3 cloudPosMin = new Vector3(-50, -5, 10); // Левая граница, до которой могут двигаться облака
    public Vector3 cloudPosMax = new Vector3(150, 100, 10); // Правая граница, от которой двигаются облака 
    public float cloudScaleMin = 1; // Мин. размер облака
    public float cloudScaleMax = 3; // Макс. размер облака
    public float cloudSpeedMult = 0.5f; // Скорость облаков
    private GameObject[] cloudInstances; // Массив игровых объектов
    private void Awake()
    {
        cloudInstances = new GameObject[numClouds];
        GameObject anchor = GameObject.Find("CloudAnchor"); // Найти родительский игровой объект
        GameObject cloud;
        for (int i = 0; i < numClouds; i++)
        {
            cloud = Instantiate<GameObject>(cloudPrefab); // Создать экземпляр облака
            Vector3 cPos = Vector3.zero;
            cPos.x = Random.Range(cloudPosMin.x, cloudPosMax.x);
            cPos.y = Random.Range(cloudPosMin.y, cloudPosMax.y);
            float scaleU = Random.value; // Рандомное значение между 0 и 1
            float scaleVal = Mathf.Lerp(cloudScaleMin, cloudScaleMax, scaleU); // Найти промежуточное значение между "a" и "b" на основании "t"
            cPos.y = Mathf.Lerp(cloudPosMin.y, cPos.y, scaleU); // Меньшие облака располагаются ближе к земле
            cPos.z = 100 - 90 * scaleU; // Мень шие облака располагаются дальше от камеры
            cloud.transform.position = cPos;
            cloud.transform.localScale = Vector3.one * scaleVal;
            cloud.transform.SetParent(anchor.transform); // Сделать облако дочерним по отношению к "anchor"
            cloudInstances[i] = cloud;

        }
    }
    void Update()
    {
        foreach (GameObject cloud in cloudInstances) // Обход облаков из массива
        {
            float scaleVal = cloud.transform.localScale.x;
            Vector3 cPos = cloud.transform.position;
            cPos.x -= scaleVal * Time.deltaTime * cloudSpeedMult; // Движение облаков
            if (cPos.x <= cloudPosMin.x) // Перемещение до правой границы при достижении левой
            {
                cPos.x = cloudPosMax.x;
            }

            cloud.transform.position = cPos;
        }
    }
}
