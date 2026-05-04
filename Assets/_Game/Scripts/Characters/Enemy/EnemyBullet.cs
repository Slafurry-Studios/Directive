using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : BaseProjectile
{
   [SerializeField] private int damageAmount = 10;
   
   protected override void Move()
   {
      transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.gameObject.tag == "Player")
      {
      // Gets the PlayerHealth component from the player object
         PlayerHealth player = collision.GetComponent<PlayerHealth>();

         if (player != null)
         {
            // Calls a function to reduce the player's life[cite: 7]
            player.TakeDamage(damageAmount);
            
            // Provides logs to ensure incoming damage
            Debug.Log("Enemy Bullet hit Player for " + damageAmount + " damage.");
            
            // Destroys enemy bullets after hitting the player[cite: 4]
            Destroy(gameObject);
         }
      }
   }
}
