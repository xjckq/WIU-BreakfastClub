using UnityEditor.Timeline.Actions;
using UnityEngine;

public class AttackEventHandler : MonoBehaviour
{
    public GameObject attackPointUp;
    public GameObject attackPointDown;
    public GameObject attackPointSide;
    [SerializeField] AudioClip attackSound;
    [SerializeField] AudioSource audioSource;

    public void AttackCheckUp()
    {
        attackPointUp.SetActive(true);
        audioSource.PlayOneShot(attackSound);
    }

    public void AttackCheckDown()
    {
        attackPointDown.SetActive(true);
        audioSource.PlayOneShot(attackSound);
    }

    public void AttackCheckSide()
    {
        attackPointSide.SetActive(true);
        audioSource.PlayOneShot(attackSound);
    }


    public void AttackEnd()
    {
        attackPointUp.SetActive(false);
        attackPointDown.SetActive(false);
        attackPointSide.SetActive(false);
    }
}
