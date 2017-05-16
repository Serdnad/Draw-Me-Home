using UnityEngine;

public class LoopBackground : MonoBehaviour
{
    private Vector2 offset = new Vector2(0, 0);

    public Renderer rend;

    // Use this for initialization
    private void Start()
    {
        rend = GetComponent<Renderer>();
    }

    // Update is called once per frame
    private void Update()
    {
        offset = new Vector2((offset.x += Time.deltaTime * 0.03f) % 1, 0);
        rend.material.mainTextureOffset = offset;
    }
}