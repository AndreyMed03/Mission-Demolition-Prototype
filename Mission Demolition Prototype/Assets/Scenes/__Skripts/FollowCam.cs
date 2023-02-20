using UnityEngine;

public class FollowCam : MonoBehaviour
{
    static public GameObject POI; // Ссылка на интересующий объект
    [Header("Set in Inspector")] 
    public float easing = 0.1f; // Скорость плавной привязки камеры к объекту
    public Vector2 minXY = Vector2.zero; // Вектор для ограничений перемещений камеры 
    [Header("Set Dynamically")] 
    public float camZ; // Желаемая координата Z камеры

    private void Awake()
    {
        camZ = this.transform.position.z; // Координате Z камеры присваевается её позиция
    }
    void FixedUpdate()
    {
        Vector3 destination; // Позиция интересующего объекта
        if (POI == null)
        {
            destination = Vector3.zero;
        }
        else
        {
            destination = POI.transform.position; // Получить позицию интересующего объекта
            if (POI.tag == "Projectile") // Проверка объекта на то что он действительно снаряд
            {
                if (POI.GetComponent<Rigidbody>().IsSleeping()) // Если он стоит на месте
                {
                    POI = null; // Возвращаемся в начальную позицию камеры
                    return;
                }
            }
        }
        destination.x = Mathf.Max(minXY.x, destination.x); // Максимальная граница камеры по оси X
        destination.y = Mathf.Max(minXY.y, destination.y); // Максимальная граница камеры по оси Y
        destination = Vector3.Lerp(transform.position, destination, easing); // Находится среднее значение расстояния между камерой и объектом на основании "easing" 
        destination.z = camZ;
        transform.position = destination;
        Camera.main.orthographicSize = destination.y + 10; // Земля всегда должна оставаться в поле зрения камеры
    }
}
