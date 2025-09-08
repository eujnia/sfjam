using System;
using EasyTextEffects;
using UnityEngine;

public class Menu : MonoBehaviour
{
    int state = -1;
    Transform buttonStart;
    Vector3 initialPosition;
    TMPro.TMP_Text buttonText;
    TextEffect textEffects;

    TextInput inputTuNombre;
    TextInput inputOtroNombre;
    Camera cam;

    BlackTransitionEffect blackTransition;
    Transform wheel;
    float wheelSpeed = 60f;

    void Start()
    {
        buttonStart = transform.Find("StartButton");
        buttonStart.GetComponent<UnityEngine.UI.Button>().onClick.AddListener(Forward);
        buttonText = buttonStart.transform.Find("Text").GetComponent<TMPro.TMP_Text>();
        textEffects = buttonText.GetComponent<TextEffect>();
        initialPosition = buttonStart.position;

        inputTuNombre = buttonStart.Find("TuNombre").GetComponent<TextInput>();
        inputOtroNombre = buttonStart.Find("OtroNombre").GetComponent<TextInput>();

        blackTransition = GameObject.Find("BlackTransition").GetComponent<BlackTransitionEffect>();
        cam = GameObject.Find("Camera").GetComponent<Camera>();

        wheel = GameObject.Find("LlantaTrasera").transform;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            Backward();
        }
        if (Input.GetKeyDown(KeyCode.Return))
        {
            Forward();
        }

        ManageStates();

        ManageColors();
        
        BicycleWheel();
    }

    private void BicycleWheel()
    {
        wheel.Rotate(Vector3.right * wheelSpeed * Time.deltaTime);
        wheelSpeed -= Time.deltaTime * 4f;
        if (wheelSpeed < 0) wheelSpeed = 0;
    }

    private void ManageColors()
    {

        bool leftClick = Input.GetMouseButtonDown(0);
        bool rightClick = Input.GetMouseButtonDown(1);

        if (leftClick || rightClick)
        {
            Ray ray = cam.ScreenPointToRay(Input.mousePosition);
            RaycastHit hit;
            if (Physics.Raycast(ray, out hit, 200f)) // 200 = distancia máxima del raycast
            {
                // Intentamos obtener el componente ColorExpansion del objeto golpeado
                ColorExpansion ce = hit.collider.GetComponent<ColorExpansion>();
                if (ce != null)
                {
                    if(state == -1) state = 0;
                    ce.StartEffect(hit.point, ce.debugMenu);
                }
                else
                {
                    Debug.Log("El objeto golpeado no tiene un componente ColorExpansion.");
                }
            }
        }
    }

    private void Backward()
    {
        state--;
        if (state < 1)
        {
            state = 0;
            ChangeButtonText("Jugar");
        }
    }

    private void Forward()
    {
        if (state == 1 && Config.Instance.data.tuNombre == "") return;
        if (state == 2 && Config.Instance.data.otroNombre == "") return;
        state++;
        ChangeButtonText("Continuar");
    }

    private void ManageStates()
    {
        switch (state)
        {
            case -1:
                inputTuNombre.active = false;
                inputOtroNombre.active = false;
                buttonStart.position = Vector3.Lerp(
                    buttonStart.position,
                    initialPosition,
                    Time.deltaTime * 10
                );
                break;
            case 0:
                inputTuNombre.active = false;
                inputOtroNombre.active = false;
                buttonStart.position = Vector3.Lerp(
                    buttonStart.position,
                    initialPosition + new Vector3(0, -260, 0),
                    Time.deltaTime * 10
                );
                break;
            case 1:
                inputTuNombre.active = true;
                inputOtroNombre.active = false;
                buttonStart.position = Vector3.Lerp(
                    buttonStart.position,
                        initialPosition + new Vector3(0, -390, 0),
                    Time.deltaTime * 10
                );
                break;
            case 2:
                inputTuNombre.active = false;
                inputOtroNombre.active = true;
                buttonStart.position = Vector3.Lerp(
                        buttonStart.position,
                        initialPosition + new Vector3(0, -520, 0),
                        Time.deltaTime * 10
                    );
                break;
            case 3:
                inputTuNombre.active = false;
                inputOtroNombre.active = false;
                blackTransition.GoBlack(
                    () =>
                    {
                        Music.Instance.PlaySong("karma");
                        UnityEngine.SceneManagement.SceneManager.LoadScene("Game1");
                    }
                );
                break;
        }
    }

    void ChangeButtonText(string newText)
    {
        buttonText.text = newText;
        textEffects.Refresh();
    }

}
