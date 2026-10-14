using UnityEngine;
using TMPro;

public class GameManager : MonoBehaviour
{
    public static GameManager Instance;
    public int actual = 0;
    public int total = 5;
    public TextMeshProUGUI texto;

    void Awake() { Instance = this; }

    void Start() { ActualizarUI(); }

    public void Recoger()
    {
        actual++;
        ActualizarUI();
    }

    void ActualizarUI()
    {
        texto.text = "coins: " + actual + "/" + total;
    }
}