using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using Maze.UI;
using System;
using Maze.Manager;
using Maze.SceneManagement;

namespace Maze.Control
{


	public class PlayerController : MonoBehaviour
	{
		
		public static PlayerController sharedInstance;
		public Rigidbody2D body;
		
		private Vector3 startPosition;
		
		private Vector2 movement;
		
		private float moveLimiter = 0.7f; //para moverse igual de rápido en diagonal que horizontal y verticalmente. 0.7 representa 70% de la velocidad.
		
		public float runSpeed = 5f; //velocidad del movimiento en píxeles por segundo (aprox)
		public float initialRunSpeed;
		
		public Sprite[] playerStates; //[NORMAL,DASH,REWIND] ... se podria agregar uno para cuando es atacado, etc
		//public Sprite currentState;

		public Animator animator;

		
		void Awake()
		{
			sharedInstance = this;
			//currentState = playerStates[0];
		}


		private void Start()
		{
	
		}
			
		// Use this for initialization
		public void StartGame () {
			
			//inicializo HP, cant llaves, boosts de los skills, etc
			SetHUDInfo();
			//SetPlayerInitialPosition(); 
			initialRunSpeed = runSpeed;
			
		}
		
		
		// Update is called once per frame
		void Update()
		{
            //DontDestroyChildOnLoad(this);
			GetMovementDirection();
			AnimatePlayer();
            
        }


	/*
        public static void DontDestroyChildOnLoad(PlayerController child)
        {
            Transform parentTransform = child.transform;

            // If this object doesn't have a parent then its the root transform.
            while (parentTransform.parent != null)
            {
                // Keep going up the chain.
                parentTransform = parentTransform.parent;
            }
            GameObject.DontDestroyOnLoad(parentTransform.gameObject);
        }
	*/



        private void GetMovementDirection()
        {
            if (GameManager.sharedInstance.currentGameState == GameState.inTheGame)
            {
                movement.x = Input.GetAxisRaw("Horizontal"); //devuelve el eje virtual identificado por "Horizontal": -1(izq), 0(nada), 1(derecha)
			    movement.y = Input.GetAxisRaw("Vertical"); //devuelve el eje virtual identificado por "Vertical": -1(abajo), 0(nada), 1(arriba)
			}
        }

        private void AnimatePlayer()
        {
            animator.SetFloat("Horizontal", movement.x);
            animator.SetFloat("Vertical", movement.y);
            animator.SetFloat("Speed", movement.sqrMagnitude); //cuadrado del modulo del vector
        }

        //la física se manipula en FixedUpdate ya que este método no se llama en cada frame
        private void FixedUpdate(){ 
			
		//	if(horizontal != 0 && vertical != 0){ //chequeo movimiento diagonal
				//limito la velocidad del movimiento diagonal, o sea nos movemos a un moveLimiter * 100 % de la velocidad runSpeed
		//		horizontal *= moveLimiter;
		//		vertical *= moveLimiter;
		//	}
			

			body.MovePosition(body.position + movement * runSpeed * Time.fixedDeltaTime);
			
			//body.velocity = new Vector2(horizontal * runSpeed, vertical * runSpeed); //vector velocidad definido al moverse
		
			
		}

  
        private void SetHUDInfo(){
			
			GameManager.sharedInstance.hp = GameManager.sharedInstance.hpMAX;
			GameManager.sharedInstance.collectedKeys = 0;
			GameManager.sharedInstance.dashBoost = GameManager.sharedInstance.dashBoostPerLevel[GameManager.sharedInstance.currentLevel - 1];
			GameManager.sharedInstance.rewindBoost = GameManager.sharedInstance.rewindBoostPerLevel[GameManager.sharedInstance.currentLevel - 1];
			GameManager.sharedInstance.teleportBoost = GameManager.sharedInstance.teleportBoostPerLevel[GameManager.sharedInstance.currentLevel - 1];
			
			ViewInGame.sharedInstance.InitializeSkillsBoosts();
			ViewInGame.sharedInstance.UpdateStageLabel();
			
			
		
			
			// reiniciar HP, skills, cantllaves, etc
		
		
			
		}
		
/*
		private void SetPlayerInitialPosition(){

            //solo a partir del segundo nivel:
			if (GameManager.sharedInstance.GetCurrentLevel() != 1)
			{
                Vector3 spawnPoint = new Vector3();
                GameObject[] gemPortalsObjects = GameObject.FindGameObjectsWithTag("Portal");
                foreach (GameObject elem in gemPortalsObjects)
                {
                    ExitGem exitGem = elem.GetComponent<ExitGem>();
                    if (exitGem.gemLevel == GameManager.sharedInstance.GetCurrentLevel() - 1)
                    {
                        spawnPoint = exitGem.spawnPoint.position;
                        break;
                    }
                }

			}
		}
*/	


	}


}

