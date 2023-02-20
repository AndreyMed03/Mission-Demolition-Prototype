using UnityEngine;

public class Goal : MonoBehaviour
{
    static public bool goalMet = false;

    private void OnTriggerEnter(Collider other)
    {
        if (other.gameObject.tag == "Projectile") // Когда в область действия триггера попадает что-то, проверить, является ли это "что-то" снарядом
        {
            Goal.goalMet = true; // Если это снаряд, присвоим полю "goalMet" значение true
            // А так же увеличим непрозрачность
            Material mat = GetComponent<Renderer>().material;
            Color c = mat.color;
            c.a = 1;
            mat.color = c;
        }
    }
}
