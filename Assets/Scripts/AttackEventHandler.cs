using UnityEngine;

public class AttackEventHandler : MonoBehaviour
{
    public GameObject attackPointUp;
    public GameObject attackPointDown;
    public GameObject attackPointSide;

    public void AttackCheckUp()
    {
        attackPointUp.SetActive(true);
    }

    public void AttackCheckDown()
    {
        attackPointDown.SetActive(true);
    }

    public void AttackCheckSide()
    {
        attackPointSide.SetActive(true);
    }


    public void AttackEnd()
    {
        attackPointUp.SetActive(false);
        attackPointDown.SetActive(false);
        attackPointSide.SetActive(false);
    }
}
