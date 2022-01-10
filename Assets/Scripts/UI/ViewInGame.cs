using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System; //para el tiempo
using Maze.Control;
using Maze.Manager;

namespace Maze.UI
{

	public class ViewInGame : MonoBehaviour
	{
	
		public static ViewInGame sharedInstance;

		//HUD (heads-up display) ------------------
		
		//parte superior:
		public Text hpLabel;
		public Text collectedKeysLabel;
		public Text stageLabel; //area y level
		
		//parte inferior:
		public Button dashButton;
		public Button rewindButton;
		public Button teleportButton;
		public Text dashBoost;
		public Text rewindBoost;
		public Text teleportBoost;
		public Button giveUpButton;
		public Button mainMenuButton;
		
		
		//contador (skillFuel)
		public Text skillFuel; //contador para el skill
		private float CountdownTimerDuration; //tiempo que debe durar el contador
		private float CountdownTimerStartTime; 
		public bool skillFuelRunOut; //true significa que el fuel llegó a 0, mientras que false es que todavia tengo fuel
		
		
		void Awake() {
			sharedInstance = this;
		}

		
		void FixedUpdate () {
			UpdateHpLabel();
			UpdateCollectedKeysLabel();
			InitializeSkillsBoosts();
			UpdateSkillFuelLabel("dash");
			//UpdateSkillFuelLabel("rewind");
		}

		void Start(){
			skillFuelRunOut = true; //arranco con 0 ya que inicialmente ningun skill esta activado
			
			
		}




		public void UpdateHpLabel(){
			if (GameManager.sharedInstance.currentGameState == GameState.inTheGame) {
				hpLabel.text = GameManager.sharedInstance.hp.ToString();
			}
		}
		
		
		public void UpdateCollectedKeysLabel(){
			if (GameManager.sharedInstance.currentGameState == GameState.inTheGame) {
				collectedKeysLabel.text = GameManager.sharedInstance.collectedKeys.ToString ();
			}
		}


		public void UpdateStageLabel(){
			stageLabel.text = "Area " + GameManager.sharedInstance.currentArea.ToString() + " - Level " + GameManager.sharedInstance.currentLevel.ToString();	
		}


		public void UpdateSkillFuelLabel(string activatedSkill) {
			
			string TimerMessage = "0"; //inicialmente los skills no estan activados
			
			if (GameManager.sharedInstance.currentGameState == GameState.inTheGame) {
				
				int TimeLeft = (int)CountdownTimeRemaining();
				
				if (TimeLeft>0){
					skillFuelRunOut = false;
					TimerMessage = TimeLeft.ToString();
				
					if (activatedSkill == "dash"){
						ChangeSprite("dash");
					//change sprite to Dash Sprite
					}
					
					
				} else { //se acabo el tiempo, por lo tanto se acabo el fuel
					skillFuelRunOut = true;
					PlayerController.sharedInstance.runSpeed = PlayerController.sharedInstance.initialRunSpeed;
					GameManager.sharedInstance.dashOn = false;
					//Debug.Log("Dash desactivado, velocidad: " + PlayerController.sharedInstance.runSpeed);
					if (activatedSkill == "dash"){
						ChangeSprite("normal"); //vuelvo a renderizar con el sprite del player normal
					}
			
			
				}
					
					//Time.timeScale = 0; //el tiempo en el videojuego deja de correr, por ejemplo al pausar el juego
				
				}
				
				skillFuel.text = TimerMessage;
				
			}
		


		public void InitializeSkillsBoosts() {
					
			if (GameManager.sharedInstance.currentGameState == GameState.inTheGame) {
				
				dashBoost.text = "x " + GameManager.sharedInstance.dashBoost.ToString();
				rewindBoost.text = "x " + GameManager.sharedInstance.rewindBoost.ToString();
				teleportBoost.text = "x " + GameManager.sharedInstance.teleportBoost.ToString();
			
			}
		}



		public void SetCountdownTimer(float delayInSeconds, string skillType){
			
			if (GameManager.sharedInstance.currentGameState == GameState.inTheGame) {
				
				if (skillType == "dash"){
					
					delayInSeconds = GameManager.sharedInstance.currentDashFuel;
					
				} else if (skillType == "rewind"){
					
					delayInSeconds = GameManager.sharedInstance.currentRewindFuel;
				}
					
			CountdownTimerDuration = delayInSeconds;
			
			CountdownTimerStartTime = Time.time; //tiempo en segundos desde que empezó el juego
				
				
				
			}
			
			
		}
		
		
		public float CountdownTimeRemaining(){
			float ElapsedSeconds = Time.time - CountdownTimerStartTime;
			float TimeLeft = CountdownTimerDuration - ElapsedSeconds;
			return TimeLeft;
		}
		
		
		
		
		
		private void ChangeSprite(string activatedSkill){ //
			
			Sprite newSprite;
				
			// reemplazar el sprite actual por el verde ON u OFF y el azul ON (ya que si es azul, ChangeSprite solo se llama cuando isOn es false)
		
			int positionInPlayerStates = 0;
			
			switch(activatedSkill){
				
				case "normal":
					positionInPlayerStates = 0;
					break;
					
				case "dash":
					positionInPlayerStates = 1;
					break;
					
				case "rewind":
					positionInPlayerStates = 2;
					break;
			}
		
			newSprite = PlayerController.sharedInstance.playerStates[positionInPlayerStates];
				
			GameObject ParentGameObject = GameObject.FindGameObjectWithTag("Player"); 
			
//			GameObject ChildGameObject0 = ParentGameObject.transform.GetChild(0).gameObject; //le cambio el sprite al hijo de Player ("sprite")
				
//  		ChildGameObject0.GetComponent<SpriteRenderer>().sprite = newSprite;
				
		}
			

	}

}