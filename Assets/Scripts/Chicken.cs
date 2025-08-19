using UnityEngine;

public class Chicken : MonoBehaviour
{
    public float speed = 2;
    public float stopDistance = 1;
    [SerializeField] Transform player;
    Vector2 playerPos, chickenPos, newPos;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        chickenPos = transform.position;
        playerPos = player.position;

        newPos = Vector2.MoveTowards(chickenPos,playerPos,speed * Time.deltaTime);

        transform.position = new Vector3(newPos.x, newPos.y, transform.position.z);
    }
}
