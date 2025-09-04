using UnityEngine;

public class EinsteinRosenBridge : MonoBehaviour
{
    public Transform whiteBridge;
    public float minGrabDistance = 3f;
    public float minTeleportDistance = 0.5f;
    public float grabStrengthFactor = 1;

    void Start()
    {
        whiteBridge = GameObject.FindGameObjectWithTag("WhiteBridge").transform;
    }

    // Update is called once per frame
    void Update()
    {
        GameObject[] props = GameObject.FindGameObjectsWithTag("Prop");
        foreach (GameObject prop in props)
        {
            Vector3 delta = transform.position - prop.transform.position;
            float distance = Vector3.Magnitude(delta);

            if (distance > minGrabDistance) continue;

            float scale = Mathf.Clamp(1 / distance, 0, 1);
            prop.GetComponent<Rigidbody>().AddForce(delta.normalized * scale * grabStrengthFactor, ForceMode.Force);

            if (distance > minTeleportDistance) continue;
            
            prop.transform.position = whiteBridge.position;
        }
    }
}
