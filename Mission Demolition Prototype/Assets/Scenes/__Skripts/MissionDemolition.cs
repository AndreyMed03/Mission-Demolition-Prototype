using UnityEngine;
using UnityEngine.UI;

public enum GameMode // Экземпляр перечислений (способ определения именованных чисел)
{
    idle, // 1-я ситуация - бездействующий
    playing, // 2-я ситуация - играющий
    levelEnd // 3-я ситуация - конец уровня
}
public class MissionDemolition : MonoBehaviour
{
    public GameMode Idle = GameMode.idle;
    static private MissionDemolition S;
    [Header("Set in Inspector")]
    public Text uitLevel; // Ссылка на текстовый объект с подсчётом уровня
    public Text uitShots; // Ссылка на текстовый объект с подсчётом количества использованных выстрелов
    public Text uitButton; // Ссылка на кнопку
    public Vector3 castlePos; // Координаты замка
    public GameObject[] castles; // Массив замков
    
    [Header("Set Dynamically")] 
    public int level; // Текущий уровень
    public int levelMax; // Количество уровней
    public int shotsTaken; // Подсчёт количества потраченных выстрелов
    public GameObject castle; // Текущий замок
    public GameMode mode = GameMode.idle; // Объявление переменной с типом ситуации (перечисления)
    public string showing = "Show Slingshot"; // Режим FollowCam
    void Start()
    {
        S = this; // Определить объект одиночку
        level = 0;
        levelMax = castles.Length;
        StartLevel();
    }

    void StartLevel()
    {
        // При запуске уровня уничтожить старый замок, если он существует
        if (castle != null)
        {
            Destroy(castle);
        }
        // Уничтожить прежние снаряды, если они существуют
        GameObject[] destroyedShells = GameObject.FindGameObjectsWithTag("Projectile");
        foreach (GameObject destroyedProjectile in destroyedShells)
        {
            Destroy(destroyedProjectile);
        }
        // Создать новый замок
        castle = Instantiate<GameObject>(castles[level]);
        castle.transform.position = castlePos;
        shotsTaken = 0;
        // Переместить камеру в начальную позицию
        SwitchView("Show Both");
        ProjectileLine.S.Clear();

        Goal.goalMet = false; // Сбросить цель
        UpdateGUI();
        mode = GameMode.playing; // Переключение состояния игры на "играющий"
    }

    void UpdateGUI() // Обновление графического интерфейса
    {
        uitLevel.text = "Level: " + (level + 1) + " of " + levelMax;
        uitShots.text = "Shots Taken: " + shotsTaken;
    }
    void Update()
    {
        UpdateGUI();
        // Проверить завершение уровня
        if ((mode == GameMode.playing) && Goal.goalMet)
        {
            mode = GameMode.levelEnd; // Переключение состояния игры на "завершённый"
            SwitchView("Show Both"); // Отдалить камеру, что бы увидеть в поле зрения замок
            Invoke("NextLevel", 2f); // Начало нового уровня через 2 секунды
        }
    }

    void NextLevel() // Перемещение на новый уровень
    {
        level++;
        if (level == levelMax)
        {
            level = 0;
        }
        StartLevel();
    }

    public void SwitchView(string eView = "") // Переключить вид камеры
    {
        if (eView == "") // Если где-то был вызван метод SwitchView() без параметра, записываем в "eView" текущий текст кнопки
        {
            eView = uitButton.text;
        }

        showing = eView;
        switch (showing)
        {
            case "Show Slingshot": // Если захотели увидеть только рогатку 
                FollowCam.POI = null;
                uitButton.text = "Show Castle"; // Появившаяся надпись на кнопке предлагает рассмотреть замок крупным планом (будет виден только замок)
                break;
            case "Show Castle": // Если захотели увидеть только замок
                FollowCam.POI = S.castle;
                uitButton.text = "Show Both"; // Появившаяся надпись на кнопке предлагает отдалить камеру (будут видны и замок, и рогатка)
                break;
            case "Show Both": // Если захотели увидеть оба объекта
                FollowCam.POI = GameObject.Find("ViewBoth");
                uitButton.text = "Show Slingshot"; // Появившаяся надпись предлагает вернуться в начальное положение камеры (будет видна только рогатка)
                break;
        }
    }
    public static void ShotFired() // Метод, позволяющий из любого кода увеличить "shotsTaken" (количество сделанных выстрелов)
    {
        S.shotsTaken++;
    }
    
}
