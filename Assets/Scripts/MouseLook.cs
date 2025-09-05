using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 200f;

    float xRotation = 0f;
    float yRotation = 0f;

    void Start()
    {
        SetCursorLocked(true);
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;

        // Acumulamos rotación
        xRotation -= mouseY; // arriba/abajo
        yRotation += mouseX; // izquierda/derecha

        // Clamp solo al eje X (vertical)
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Aplicamos rotación completa
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);
    }

    public void SetCursorLocked(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

}
