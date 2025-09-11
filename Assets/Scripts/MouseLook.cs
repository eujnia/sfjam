using UnityEngine;

public class MouseLook : MonoBehaviour
{
    public float sensitivity = 200f;
    public Vector3 initialPosition;

    float xRotation = 0f;
    float yRotation = 0f;
    public float var1 = 0f;
    public float var2 = 0f;

    void Start()
    {
        SetCursorLocked(true);
        initialPosition = transform.localPosition;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * sensitivity * Config.Instance.data.sensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * sensitivity * Config.Instance.data.sensitivity * Time.deltaTime;

        // Acumulamos rotación
        xRotation -= mouseY; // arriba/abajo
        yRotation += mouseX; // izquierda/derecha

        // Clamp solo al eje X (vertical)
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Aplicamos rotación completa
        transform.localRotation = Quaternion.Euler(xRotation, yRotation, 0f);

        float sideMovementFactor = Mathf.Clamp01((-30 + xRotation) / 10);
        transform.localPosition = Vector3.Lerp(transform.localPosition, initialPosition + new Vector3(0.3f * sideMovementFactor, 0f, 0f), Time.deltaTime * 5f);
    }

    public void SetCursorLocked(bool locked)
    {
        Cursor.lockState = locked ? CursorLockMode.Locked : CursorLockMode.None;
        Cursor.visible = !locked;
    }

}
