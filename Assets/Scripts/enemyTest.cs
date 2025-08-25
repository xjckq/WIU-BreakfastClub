using UnityEngine;

public class enemyTest : MonoBehaviour
{

    Animator animator;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {

        // attack
        if (Input.GetKeyDown(KeyCode.U))
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttacking", true);
        }
        // chase
        if (Input.GetKeyDown(KeyCode.I))
        {
            animator.SetBool("isMoving", true);
            animator.SetBool("isAttacking", false);
        }
        
        if (Input.GetKeyDown(KeyCode.O))
        {
            animator.SetBool("isMoving", false);
            animator.SetBool("isAttacking", false);
        }
    }
}
