using UnityEngine;

public class EyeFollowPlayer : MonoBehaviour
{
    private Quaternion defaultRot;

    private void Start()
    {
        defaultRot = transform.rotation;
    }

    // Update is called once per frame
    private void Update()
    {
        if (GameObject.FindGameObjectWithTag("Player"))
        {
            var playerPos = GameObject.FindGameObjectWithTag("Player").transform;

            //found this code online, no idea how it works
            var diff = playerPos.position - transform.position;
            diff.Normalize();

            var rot_z = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            transform.rotation = Quaternion.Euler(0f, 0f, rot_z - 90);
        }
        else
        {
            transform.rotation = defaultRot;
        }
    }
}