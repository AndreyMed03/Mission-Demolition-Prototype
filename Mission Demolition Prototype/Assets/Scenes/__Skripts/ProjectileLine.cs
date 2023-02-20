using System.Collections.Generic;
using UnityEngine;

public class ProjectileLine : MonoBehaviour
{
    static public ProjectileLine S;

    [Header("Set in Inspector")]
    public float minDist = 0.1f; // Дистанция между точек для построения линий с помощью LineRenderer

    private LineRenderer line; // Компонент LineRenderer
    private GameObject _poi; // Ссылка на объект
    private List<Vector3> points; // Список точек, по которым будет строиться траектория с помощью LineRenderer

    private void Awake()
    {
        S = this;
        line = GetComponent<LineRenderer>(); // Получить ссылку на LineRenderer
        line.enabled = false; // Временное выключение LineRenderer
        points = new List<Vector3>();
    }

    public GameObject poi // Метод, маскирующийся под поле
    {
        get
        {
            return (_poi);
        }
        set
        {
            _poi = value;
            if (_poi != null)
            {
                line.enabled = false;
                points = new List<Vector3>();
                AddPoint();
            }
        }
    }

    public void Clear() // Можно вызвать для удаления линии (очищение списка точек)
    {
        _poi = null;
        line.enabled = false;
        points = new List<Vector3>();
    }

    public void AddPoint()
    {
        Vector3 pt = _poi.transform.position; // Вызывается для добавления точки
        if (points.Count > 0 && (pt - lastPoint).magnitude < minDist) // Если точка недостаточно далека от предыдущей, выходим
        {
            return;
        }

        if (points.Count == 0) // Если это начальная точка запуска, надо добавить дополнительный фрагмент линии, что бы было легче прицеливаться в будущем
        {
            Vector3 launchPosDiff = pt - SlingShot.LAUNCH_POS;
            // Добавляем в список первые две точки
            points.Add(pt + launchPosDiff);
            points.Add(pt);
            line.positionCount = 2;
            // Установим первые две точки
            line.SetPosition(0, points[0]);
            line.SetPosition(1, points[1]);
            line.enabled = true; // Включим LineRenderer
        }
        else // Обычная последовательность добавления точки
        {
            points.Add(pt);
            line.positionCount = points.Count;
            line.SetPosition(points.Count-1, lastPoint); // Устанавливаем позицию (предыдущая точка, новая позиция)
            line.enabled = true; // Включим LineRenderer
        }
    }

    public Vector3 lastPoint // Возвращает местоположение последней добавленной точки
    {
        get
        {
            if (points == null)
            {
                return (Vector3.zero);
            }

            return (points[points.Count - 1]);
        }
    }

    private void FixedUpdate()
    {
        if (poi == null)
        {
            // Если свойство poi содержит пустое значение, найти интересующий объект
            if (FollowCam.POI != null)
            {
                if (FollowCam.POI.tag == "Projectile") // Проверка объекта на то что он действительно снаряд
                {
                    poi = FollowCam.POI; // Следование за снарядом
                }
                else
                {
                    return; // Выйти, если интересующий объект не найден
                }
            }
            else // Выйти, если интересующий объект не найден
            {
                return;
            }
        }
        // Если интересующий объект найден, попытаться добавить точку с его координатами в каждом кадре FixedUpdate
        AddPoint(); 
        if (FollowCam.POI == null)
        {
            poi = null;
        }
    }
}
