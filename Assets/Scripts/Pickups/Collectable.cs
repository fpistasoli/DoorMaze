using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maze.Control;
using Maze.Manager;

namespace Maze.Pickups
{

	public class Collectable : MonoBehaviour // expandir luego esta clase con las vidas y los items que incrementan la cant disponible de habilidades
	{
		
		public enum collectableType {Key, HP} // llave que abre puertas rojas / llave maestra que abre la puerta de salida / vida

		public collectableType type;

		bool isCollected = false;
		
		public Sprite[] spriteArray; // [KEY, HP]
		
		public Vector3 startPosition;

		void Awake(){
			
			//mostrar la llave roja o la maestra segun type
			if (type == collectableType.Key){ 
				GetComponent<SpriteRenderer>().sprite = spriteArray[0];    
			} else if (type == collectableType.HP) {
				GetComponent<SpriteRenderer>().sprite = spriteArray[1]; 
			}
			startPosition = GetComponent<Transform>().position;
		}

		public void ShowCollectable() {
			
			this.GetComponent<SpriteRenderer> ().enabled = true;
		
			this.GetComponent<BoxCollider2D> ().enabled = true;
			
			isCollected = false;
		}

		void HideCollectable() {
			this.GetComponent<SpriteRenderer> ().enabled = false;	
		}

		void Collect() {
			
			isCollected = true;
		
			HideCollectable();

			//Debug.Log("Cantidad de llaves: " + GameManager.sharedInstance.collectedKeys);
				
			
			if(type == collectableType.Key){
				//Notificar al manager del juego que hemos recogido una llave roja...
				GameManager.sharedInstance.CollectKey();
				
				//Debug.Log("Cantidad de llaves: " + GameManager.sharedInstance.collectedKeys);
		
			} else if(type == collectableType.HP) { // es una vida
				
				GameManager.sharedInstance.CollectHP();
				
			}
			
		}

		void OnTriggerEnter2D(Collider2D otherCollider) {

			if ((otherCollider.tag == "Player") && this.GetComponent<SpriteRenderer>().enabled) {
				Collect();
			}
			
		}
		
	}
}
