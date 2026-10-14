using UnityEngine;
using UnityEngine.SceneManagement;

public class ObjetoPeligroso : MonoBehaviour
{
    private void OnTriggerEnter(Collider other)
    {
        // Solo el jugador (tiene CharacterController) activa el peligro
        if (other.GetComponent<CharacterController>() == null) return;

        // Reiniciar el contador para que la escena arranque limpia
        if (GameManager.Instance != null) GameManager.Instance.actual = 0;

        // Reiniciar la escena actual
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }
}
