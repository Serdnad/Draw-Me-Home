using UnityEngine;

public class EraserBehaviour : MonoBehaviour
{
    private bool collisionChecked = false;

    private void Update()
    {
        if (!collisionChecked)
            collisionChecked = true;
        else
            Destroy(gameObject);
    }
}