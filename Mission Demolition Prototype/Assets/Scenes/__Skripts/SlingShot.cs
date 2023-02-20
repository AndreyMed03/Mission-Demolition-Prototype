using UnityEngine;

public class SlingShot : MonoBehaviour
{
    static private SlingShot S; // Статический скрытый экземпляр SlingShot, который играет роль объекта-одиночки
    [Header("Set in Inspector")]
    public GameObject prefabProjectile; // Экземпляр "Projectile"

    public float velocityMult = 8f; // Скорость полёта снаряда
    [Header("Set Dynamically")]
    public GameObject projectile; // Ссылка на созданный экземпляр "Projectile"
    public Vector3 launchPos; // Хранит трёхмерные координаты "launchPoint"
    public bool aimingMode; // Режим прицеливания
    private Rigidbody projectileRigidbody;
    public GameObject launchPoint;

    static public Vector3 LAUNCH_POS
    {
        get // Открытие доступа только для чтения к полю launchPos класса SlingShot
        {
            if (S == null)
            {
                return Vector3.zero;
            }
            return S.launchPos;
        }
    }
    private void Awake()
    {
        S = this;
        Transform launchPointTrans = transform.Find("LaunchPoint"); // Находим дочерний объект с именем "launch Point"
        launchPoint = launchPointTrans.gameObject;
        launchPoint.SetActive(false); // Сообщает игре, должна ли она игнорировать "launchPoint"
        launchPos = launchPointTrans.position; // Устанавливаем координаты "launchPos"
    }

    private void OnMouseEnter() // Если указатель мыши находится в зоне прицеливания
    {
        launchPoint.SetActive(true); // Сообщает игре, должна ли она игнорировать "launchPoint"
    }

    private void OnMouseExit() // Если указатель мыши не находится в зоне прицеливания
    {
        launchPoint.SetActive(false); // Сообщает игре, должна ли она игнорировать "launchPoint"
    }

    private void OnMouseDown()
    {
        aimingMode = true; // Включается режим прицеливания
        projectile = Instantiate(prefabProjectile) as GameObject; // Создание снаряда
        projectile.transform.position = launchPos; // Переместить в точку "launchPoint"
        projectile.GetComponent<Rigidbody>().isKinematic = true; // Сделать его кинематическим
        projectileRigidbody = projectile.GetComponent<Rigidbody>();
        projectileRigidbody.isKinematic = true;
    }

    private void Update()
    {
        if (!aimingMode) return; // Если рогатка не в режиме прицеливания, не выполнять этот код
        Vector3 mousePos2D = Input.mousePosition; // Получить текущие экранные координаты курсора
        // Преобразовать координаты указателя мыши из экранных координат в мировые
        mousePos2D.z = -Camera.main.transform.position.z;
        Vector3 mousePos3D = Camera.main.ScreenToWorldPoint(mousePos2D);

        Vector3 mouseDelta = mousePos3D - launchPos; // Найти разность координат между курсором мыши и центром рогатки
        float maxMagnitude = this.GetComponent<SphereCollider>().radius; // Ограничить "mouseDelta" радиусом коллайдера объекта "Slingshot"
        if (mouseDelta.magnitude > maxMagnitude) // Если снаряд во время прицеливания хочет выйти за пределы коллайдера
        {
            mouseDelta.Normalize(); // Нормализуем его (При "нормальном" значении его координаты всегда будут равны 1)
            mouseDelta *= maxMagnitude;
        }
        // Передвинуть снаряд в новую позицию
        Vector3 projPos = launchPos + mouseDelta;
        projectile.transform.position = projPos;
        if (Input.GetMouseButtonUp(0)) // Если снаряд запущен
        {
            // Кнопка мыши отпущена
            aimingMode = false;
            projectileRigidbody.isKinematic = false;
            projectileRigidbody.velocity = -mouseDelta * velocityMult;
            FollowCam.POI = projectile;
            projectile = null;
            MissionDemolition.ShotFired(); // Метод, увеличивающий количество потраченных выстрелов
            ProjectileLine.S.poi = projectile;
        }
    }
}
