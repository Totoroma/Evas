using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(CharacterController))]
public class FirstPersonController_DobleSalto_Correr_CaminarLento : MonoBehaviour
{
    public float velocidadMovimiento = 5f;   // Velocidad de movimiento normal del jugador
    public float velocidadCarrera = 10f;     // Velocidad de movimiento al correr
    public float velocidadCaminarLento = 2f; // Velocidad de movimiento al caminar lentamente
    public float sensibilidadMouse = 2f;    // Sensibilidad del mouse

    private CharacterController characterController;
    private Transform camara;
    private float rotacionVertical = 0f;
    private float velocidadVertical = 0f;
    private int saltosRestantes = 2;        // Número de saltos restantes (1 para el salto simple y 1 para el doble salto)
    private bool caminarLento = false;       // Variable para rastrear si el jugador camina lentamente

    private void Start()
    {
        // Obtener el componente CharacterController adjunto a este GameObject
        characterController = GetComponent<CharacterController>();
        if (characterController == null)
        {
            Debug.LogError("Falta CharacterController en " + name + ". Agrega Component > Physics > Character Controller.", this);
            enabled = false;
            return;
        }
        camara = GetComponentInChildren<Camera>()?.transform;
        if (camara == null)
        {
            Debug.LogError("Falta una Camera hija en " + name + " para la mirada vertical.", this);
            enabled = false;
            return;
        }
        // Bloquear el cursor para evitar que salga de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        // Obtener las entradas de teclado para el movimiento
        float movimientoHorizontal = Input.GetAxis("Horizontal");
        float movimientoVertical = Input.GetAxis("Vertical");

        // Obtener la entrada del mouse para la rotación horizontal y vertical
        float mouseX = Input.GetAxis("Mouse X") * sensibilidadMouse;
        float mouseY = Input.GetAxis("Mouse Y") * sensibilidadMouse;

        // Aplicar la rotación horizontal al objeto del jugador
        transform.Rotate(Vector3.up * mouseX);

        // Aplicar la rotación vertical a la cámara hija, con límite para no voltearse
        rotacionVertical = Mathf.Clamp(rotacionVertical - mouseY, -90f, 90f);
        camara.localRotation = Quaternion.Euler(rotacionVertical, 0f, 0f);

        // Aplicar la gravedad
        if (characterController.isGrounded)
        {
            velocidadVertical = -0.5f; // Restablecer la velocidad vertical cuando el jugador toca el suelo
            saltosRestantes = 2;  // Reiniciar los saltos disponibles al tocar el suelo
        }
        else
        {
            // Aplicar la gravedad constante cuando el jugador está en el aire
            velocidadVertical += Physics.gravity.y * Time.deltaTime;
        }

        // Saltar
        if (Input.GetButtonDown("Jump") && saltosRestantes > 0)
        {
            // Calcular la velocidad vertical necesaria para realizar un salto (ajustar el valor 3.0f según sea necesario)
            velocidadVertical = Mathf.Sqrt(2f * 3.0f * Mathf.Abs(Physics.gravity.y));
            saltosRestantes--;  // Reducir el número de saltos restantes
        }

        // Caminar lentamente (Ctrl)
        if (Input.GetKey(KeyCode.LeftControl) && characterController.isGrounded)
        {
            caminarLento = true;
        }
        else
        {
            caminarLento = false;
        }

        // Verificar si el jugador está corriendo (Shift) o caminando lentamente (Ctrl)
        float currentSpeed = caminarLento ? velocidadCaminarLento : (Input.GetKey(KeyCode.LeftShift) ? velocidadCarrera : velocidadMovimiento);

        // Crear un vector de movimiento basado en la dirección de la cámara
        Vector3 movimiento = (transform.forward * movimientoVertical + transform.right * movimientoHorizontal) * currentSpeed;

        // Aplicar la velocidad vertical al movimiento
        movimiento.y = velocidadVertical;

        // Mover al jugador utilizando el CharacterController
        characterController.Move(movimiento * Time.deltaTime);
    }
}
