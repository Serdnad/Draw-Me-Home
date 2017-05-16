using UnityEngine;

public class SpinSaw : MonoBehaviour
{
    // Update is called once per frame
    private float rotation;

    // Use this for initialization
    private void Start()
    {
    }

    private void FixedUpdate()
    {
        transform.eulerAngles = new Vector3(0, 0, (rotation -= 10f) % 360);
    }
}