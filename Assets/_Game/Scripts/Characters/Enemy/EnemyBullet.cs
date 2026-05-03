using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EnemyBullet : BaseProjectile
{
   protected override void Move()
   {
      transform.Translate(direction * moveSpeed * Time.deltaTime, Space.World);
   }

   private void OnTriggerEnter2D(Collider2D collision)
   {
      if (collision.gameObject.tag == "Player")
      {
         Debug.Log("Hit");
      }
   }
}
