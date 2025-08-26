using UnityEngine;
using UnityEngine.UI;
public class FloatingEnemyHealthBar : MonoBehaviour
{
    [SerializeField] private Slider slider;
    //[SerializeField] private Transform enemyTransform;
    //[SerializeField] private Camera camera;
    //[SerializeField] private Transform target;
    public void UpdateHealthBar(float currentValue, float maxValue)
    {
        slider.value = currentValue / maxValue;
    }
    // Update is called once per frame
    void Update()
    {
        //if (enemyTransform != null)
        //{
            //transform.position = enemyTransform.position;
            transform.rotation = Quaternion.Euler(0, 0, 0);
        //}
    }
}
