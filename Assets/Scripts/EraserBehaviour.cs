using UnityEngine;

public class EraserBehaviour : MonoBehaviour
{
    private bool collisionChecked;

    private void Update()
    {
        if (!collisionChecked)
            collisionChecked = true;
        else
            Destroy(gameObject);
    }
}