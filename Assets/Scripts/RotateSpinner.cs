using UnityEngine;

public class RotateSpinner : MonoBehaviour
{
    [SerializeField] private float speed;

    // Update is called once per frame
    void Update()
    {
        transform.eulerAngles += new Vector3(0, 0, speed * Time.deltaTime);
    }
}
