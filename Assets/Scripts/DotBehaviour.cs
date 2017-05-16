using UnityEngine;

public class DotBehaviour : MonoBehaviour
{
    private bool collisionChecked;
    private Renderer rend;

    private void destroyDot()
    {
        Destroy(gameObject);
        GameObject.Find("UI Handler").GetComponent<Draw>().addInk(1);
    }

    private void Start()
    {
        var pos = Camera.main.WorldToScreenPoint(transform.position);
        if (pos.y > Screen.height - Screen.height / 7 && pos.x > Screen.width - Screen.width / 3)
            // dont draw over buttons
            destroyDot();
        else rend = GetComponent<Renderer>();
    }

    private void Update()
    {
        if (!collisionChecked)
        {
            collisionChecked = true;
        }
        else
        {
            rend.enabled = true;
            GetComponent<DotBehaviour>().enabled = false; //disable Update()
        }
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        //if try to draw over anything, delete dot
        destroyDot();
    }
}