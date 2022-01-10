using System.Collections;
using System.Collections.Generic;
using UnityEngine;


namespace Maze.Control
{

		//basarme en el script Collectable.cs de MI PRIMER VIDEOJUEGO FINISH


	public class SwitchesController : MonoBehaviour
	{
		
		public enum switchType {Green, Blue};
		
		public switchType type;
		
		public Sprite[] spriteArray; // [GREEN OFF, GREEN ON, BLUE OFF, BLUE ON]
		
		private bool isOn; // activado

		public static SwitchesController sharedInstance;
		
		
		
		
		void Awake(){ // configuracion inicial

			sharedInstance = this;
			RestoreInitialProperties();
			
		}
		


		void FixedUpdate() { 
			
			
		}


		
		void OnTriggerEnter2D(Collider2D otherCollider){ // el jugador pasa por el switch
			
			if (!(type == switchType.Blue && isOn)){ // si el switch es azul y ya está activado, no tiene efecto sobre las puertas azules
			
				if (otherCollider.tag == "Player"){
					ChangeDoorsState(); // abre/cierra puertas
				}
				
			}
				
				
				
		}
			
		
		
		
		
		void ChangeDoorsState(){
			
			if (type == switchType.Green){
				
				if (isOn){
					// mostrar sprite del switch verde OFF
					isOn = false;
				} else {
					// mostrar sprite del switch verde ON
					isOn = true;
				}
				
				ChangeSprite();
				
				OpenOrCloseDoors();
				
				
				
				
			} else { // Blue && !isOn (precondicion, dado que entra por el OnTriggerEnter2D que pide que NO este activado el switch


				if (isOn)
				{
					
					isOn = false;
				}
				else
				{
					
					isOn = true;
				}

				ChangeSprite(); //cambio sprite del switch a ON

				OpenOrCloseDoors(); //abro puertas azules

				StartCoroutine(KeepBlueDoorsOpen(3.0f)); //dejo por algunos segundos las puertas azules abiertas
				
			}
			
		}
		
		
		
		
		void ChangeSprite(){ // muestra en pantalla el switch en OFF si estaba en ON y viceversa
			
			Sprite newSprite;
				
			// reemplazar el sprite actual por el verde ON u OFF
		
			if (type == switchType.Green){
				
				if(isOn){
					newSprite = spriteArray[1]; // green ON
				} else {
					newSprite = spriteArray[0]; // green OFF
				}
					
				
			} else { 
				if(isOn){
					newSprite = spriteArray[3]; // blue ON
				} else {
					newSprite = spriteArray[2]; // blue OFF	
				}
					
			}

			//Debug.Log(newSprite);
			
			GetComponent<SpriteRenderer>().sprite = newSprite;
			
		}
		
		
		
		
		void OpenOrCloseDoors(){
			
			// modificar sprites y atributos de las puertas (Door.cs)

			GameObject[] allTheDoors = GameObject.FindGameObjectsWithTag("Door");
			
			foreach (GameObject door in allTheDoors){
				
				Door doorGO = door.GetComponent<Door>(); // tomo el script Door.cs
				
				if(doorGO.type == Door.doorType.Green && type == switchType.Green ){ // si es una puerta verde
				
					if(doorGO.isOpen){
						doorGO.isOpen = false;
					} else {
						doorGO.isOpen = true;
					}

				} else if (doorGO.type == Door.doorType.Blue && type == switchType.Blue){

					if (doorGO.isOpen){
						doorGO.isOpen = false;
					} else {
						doorGO.isOpen = true;
					}
				}
			}
		}
		
		
		
		
		public void RestoreInitialProperties(){
			
			isOn = false;
			
			if (type == switchType.Green){ 
				GetComponent<SpriteRenderer>().sprite = spriteArray[0];    // inicialmente esta OFF
			} else { // Blue && OFF
				GetComponent<SpriteRenderer>().sprite = spriteArray[2];    // inicialmente esta OFF
			}
			
		}


		//corutina
		public IEnumerator KeepBlueDoorsOpen(float seconds){

			yield return new WaitForSeconds(seconds);

			OpenOrCloseDoors(); //cierro puertas azules

			isOn = false; //pongo el switch en OFF

			ChangeSprite(); //cambio el sprite del switch a OFF



			//dejo abiertas las puertas azules por seconds segundos:
			
			/*ViewInGame.sharedInstance.SetCountdownTimer((float)seconds, "none");
			int TimeLeft = (int) ViewInGame.sharedInstance.CountdownTimeRemaining();

			if (!(TimeLeft > 0))
			{
				OpenOrCloseDoors(); //cerrar puertas azules si se acabo el tiempo
			}
			*/

		}
	
	}

}

