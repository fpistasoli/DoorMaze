using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maze.Manager;

namespace Maze.Control
{
    // Este tipo de enemigo nunca muere; se mueve en direccion horizontal/vertical y cambia el sentido del movimiento cuando se choca 
    // contra algun Collideable (pared o puerta)
    public class Enemy : MonoBehaviour
    {
        public enum movementDirection { Horizontal, Vertical };
        public movementDirection direction;
        private Rigidbody2D body;
        private Vector3 startPosition;
        public float runSpeed; //velocidad del movimiento en píxeles por segundo (aprox)
        public Animator animator;

        void Awake()
        {
            body = GetComponent<Rigidbody2D>();
            animator = GetComponent<Animator>();
            startPosition = this.transform.position;
        }

        // Start is called before the first frame update
        void Start()
        {

        }

        // Update is called once per frame
        void Update()
        {
            SetMovementDirection();
            AnimateEnemy();
        }

        private void AnimateEnemy()
        {

            if (direction == movementDirection.Horizontal)
            {
                if (runSpeed > 0)
                    animator.SetFloat("Horizontal", 1f);
                else
                    animator.SetFloat("Horizontal", -1f);
            }

            if (direction == movementDirection.Vertical)
            {
                if (runSpeed > 0)
                    animator.SetFloat("Vertical", 1f);
                else
                    animator.SetFloat("Vertical", -1f);
            }    
        }

        private void SetMovementDirection()
        {
            if (direction == movementDirection.Horizontal)
            {
                body.velocity = new Vector3(runSpeed/2, 0, 0);
            }
            else
            {
                body.velocity = new Vector3(0, runSpeed/2, 0); 
            }
        }

        void OnTriggerEnter2D(Collider2D otherCollider)
        {

            if (otherCollider.tag == "Player")
            {

                GameManager.sharedInstance.Attacked("Enemy");

            }
            else if (otherCollider.tag == "Wall" || otherCollider.tag == "Door")
            {

                runSpeed = -runSpeed; //cambio la orientacion	

            }

        }

        public void RestoreStartPosition()
        {
            this.transform.position = startPosition;
        }

    }
}





