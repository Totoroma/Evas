using UnityEngine;

public class Recolector : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Collectible"))
        {
            Debug.Log("Recogido: " + other.name);
            GameManager.Instance.Recoger();
            Destroy(other.gameObject);
        }
    }
}