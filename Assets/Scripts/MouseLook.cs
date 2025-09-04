using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 200f;
    public Transform playerBody; // referencia al objeto "Personaje"

    float xRotation = 0f;

    void Start()
    {
        // Oculta y bloquea el cursor en el centro de la pantalla
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // Rotar hacia arriba/abajo la cámara (eje X)
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);

        // Rotar al jugador en eje Y (izquierda/derecha)
        playerBody.Rotate(Vector3.up * mouseX);
    }
}
