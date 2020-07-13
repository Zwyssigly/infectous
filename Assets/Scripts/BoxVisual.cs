using UnityEngine;

public class BoxVisual : MonoBehaviour
{
    private GameObject instance;

    public float width = 10;
    public float height = 10;
    public Color color = Color.green;
    public bool isTrigger = false;

    public GameObject box;

    // Start is called before the first frame update
    void Start()
    {
        instance = Instantiate(box, transform);
        instance.transform.localScale = new Vector3(width, height, 1f);
        instance.GetComponent<SpriteRenderer>().color = color;
        instance.GetComponent<BoxCollider2D>().isTrigger = isTrigger;    
    }

    void OnDrawGizmos()
    {
        Gizmos.color = color;
        Gizmos.DrawCube(transform.position + new Vector3(0f, 0f, 0f), new Vector3(width, height, 1f));
    }
}
