using UnityEngine;
using System.Collections;

[CreateAssetMenu(fileName = "BuffPotionEffect", menuName = "Scriptable Objects/BuffPotionEffect")]
public class BuffPotionEffect : ItemEffect
{
    [SerializeField] private int ATK;
    [SerializeField] private int buffAmount = 5;

    public override void Use(GameObject user)
    {
        var player = user.GetComponent<PlayerController>();
        if (player != null)
        {
            ATK += buffAmount;
            Debug.Log($"player ATK increased by {buffAmount}, new ATK: {ATK}");

            // revert buff after duration
            if (duration > 0)
                user.GetComponent<MonoBehaviour>().StartCoroutine(RemoveBuffAfterTime(player));
        }
    }

    // borrow player script's Update(); cuz S0 doesnt have it
    private IEnumerator RemoveBuffAfterTime(PlayerController player)
    {
        yield return new WaitForSeconds(duration);// pause until duration is over
        ATK -= buffAmount;
        Debug.Log($"buff expired, player ATK reverted to {ATK}");
    }

}
