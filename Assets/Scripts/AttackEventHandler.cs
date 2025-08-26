using UnityEngine;

public class AttackEventHandler : MonoBehaviour
{
    public GameObject attackPoint;
    public void AttackCheck()
    {
        attackPoint.SetActive(true);
    }
    public void AttackEnd()
    {
        attackPoint.SetActive(false);
    }
}