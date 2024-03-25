using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class enemyShips : MonoBehaviour
{
    private Transform player; // Reference to the player's transform
    public float speed = 5f; // Movement speed of the enemy spaceship
    public float stoppingDistance = 10f; // Distancia a la que la nave se detendrá del jugador
    public bool canShoot = true; // Boolean to determine if the spaceship can shoot

    public GameObject bulletPrefab; // Bullet prefab
    public Transform firePoint; // Point of origin for shooting
    public float bulletSpeed = 100f; // Speed of the bullets

    public bool war = false;    //Determines if the ships faction is at war with the player

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.B))
        {
            starWar();
        }

        if (war)
        {
            // Calculate the distance between the spaceship and the player
            float distanceToPlayer = Vector3.Distance(transform.position, player.position);

            if (distanceToPlayer >= stoppingDistance)
            {
                // Calculate the direction vector to the player
                Vector3 directionToPlayer = player.position - transform.position;

                // Normalize the vector to get the direction
                directionToPlayer.Normalize();

                // Move the spaceship towards the player with speed
                transform.Translate(directionToPlayer * speed * Time.deltaTime);

            }

            if (canShoot)
            {
                Shoot();
            }
        }
    }

    void Shoot()
    {
        // Create a bullet at the firePoint
        GameObject bullet = Instantiate(bulletPrefab, firePoint.position, firePoint.rotation);
        Rigidbody bulletRb = bullet.GetComponent<Rigidbody>();
        bulletRb.velocity = firePoint.forward * bulletSpeed;

        // Set canShoot to false to prevent continuous shooting
        canShoot = false;

        // Set a delay before the spaceship can shoot again
        Invoke("ResetShoot", 1.5f);
    }

    void ResetShoot()
    {
        // Set canShoot to true after the delay
        canShoot = true;
    }

    public void starWar()
    {
        war = true;
        transform.parent = null;
    }

    public void setPlayer(GameObject p) { player = p.transform; }
}
