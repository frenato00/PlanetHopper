using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerLife : MonoBehaviour
{
    [SerializeField] static int maxHealth = 3;
    int health = maxHealth;
    bool isDead = false;

    private void OnCollisionEnter(Collision collision)
    {
        /*
        if (collision.gameObject.CompareTag("Enemy"))
        {
            StartCoroutine(TakeDamage());
        }
        */
    }

    IEnumerator TakeDamage()
    {
        health -= 1;
        if(health <= 0)
        {   
            Die();
            yield break;
        }

        yield return new WaitForSeconds(1);
    }

    void Die()
    {   
        GetComponent<MeshRenderer>().enabled = false;
        GetComponent<PlayerMovement>().enabled = false;
        GetComponent<Rigidbody>().isKinematic = true;
        isDead = true;
    }

    public void GainHealth()
    {
        if(health < maxHealth)
        {
            health += 1;
        }
    }

    public int GetCurrentHealth()
    {
        return health;
    }
}
