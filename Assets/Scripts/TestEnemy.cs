using UnityEngine;

public class TestEnemy : MonoBehaviour
{
    SpriteRenderer sr;
    bool tookDmg = false;
    float enemyTimer = 0;
    int health = 100;
    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        sr = GetComponent<SpriteRenderer>();

    }

    // Update is called once per frame
    void Update()
    {
        if(tookDmg)
        {
            enemyTimer += Time.deltaTime;
            if(enemyTimer > 1)
            {
                tookDmg = false;
                sr.color = Color.white;
                Debug.Log("Enemy health is now: " + health);
                enemyTimer = 0;
            }
        }
    }

    public void TakeDmg(int amount)
    {
        if(!tookDmg)
        health -= amount;
        Debug.Log("Enemy health is now: " + health);
        sr.color = Color.red;
        tookDmg = true;
    }
}
