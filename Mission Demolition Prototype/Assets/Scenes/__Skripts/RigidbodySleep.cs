using UnityEngine;
// Предотвращение преждевременного падения стен замка ещё до того, как снаряд попал в них.
public class RigidbodySleep : MonoBehaviour
{
    void Start()
    {
        // Заставляет стены предполагать, что они не должны никуда двигаться (обеспечивает устойчивость замка)
        Rigidbody rb = GetComponent<Rigidbody>();
        if (rb != null) rb.Sleep();
    }

}
