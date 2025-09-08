using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Splines;

public class MoveAlongSpline : MonoBehaviour
{
    [SerializeField]
    float predictionTime = 0.005f;
    public SplineContainer spline;
    public float speed = 1f;
    public Vector2 percentageRange = new Vector2(0.2f, .96f);
    Transform bicicleta;
    Transform manubrio;
    Transform ruedaDelantera;
    Transform ruedaTrasera;
    Transform pedales;
    float distancePercentage = 0f;


    float splineLength;

    private void Start()
    {
        splineLength = spline.CalculateLength();
        bicicleta = transform.Find("Bici").Find("Bicycle1").transform;
        manubrio = bicicleta.Find("Manubrio").transform;
        ruedaDelantera = manubrio.Find("LlantaDelantera").transform;
        ruedaTrasera = bicicleta.Find("LlantaTrasera").transform;
        pedales = bicicleta.Find("Pedales").transform;
        distancePercentage = percentageRange.x;
    }


    void Update()
    {
        distancePercentage += speed * Time.deltaTime / splineLength;
        distancePercentage = Mathf.Clamp(distancePercentage, percentageRange.x, percentageRange.y);



        Vector3 currentPosition = spline.EvaluatePosition(distancePercentage);
        transform.position = currentPosition;

        if (distancePercentage > 1f)
        {
            distancePercentage = 0f;
        }

        Vector3 nextPosition = spline.EvaluatePosition(distancePercentage + predictionTime);
        Vector3 direction = nextPosition - currentPosition;
        transform.rotation = Quaternion.LookRotation(direction, Vector3.up);

        nextPosition = spline.EvaluatePosition(distancePercentage + predictionTime * 3);
        direction = nextPosition - currentPosition;
        float angle = Vector3.SignedAngle(transform.forward, direction, transform.up);
        manubrio.localRotation = Quaternion.Euler(-21.99f, 0f, angle);


        ruedaDelantera.Rotate(Vector3.right, 360f * speed * 0.62f * Time.deltaTime / (2 * Mathf.PI * 0.35f));
        ruedaTrasera.Rotate(Vector3.right, 360f * speed * 0.62f * Time.deltaTime / (2 * Mathf.PI * 0.35f));
        pedales.Rotate(Vector3.right, 90f * speed * Time.deltaTime / (2 * Mathf.PI * 0.35f));
    }
}