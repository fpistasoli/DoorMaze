// ------------------------------------------------------------------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------------
// ------------------------------------- GAME MANAGER PARA EL JUEGO "DOOR MAZE" -------------------------------------
// ------------------------------------------------------------------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------------
// ------------------------------------------------------------------------------------------------------------------

namespace Maze.Manager
{

using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Maze.UI;
using Maze.Control;
using Maze.Pickups;
using Maze.SceneManagement;
using UnityEngine.SceneManagement;
using System.IO;
using System;


public enum GameState {
		menu, // seleccion de niveles
		inTheGame, // pantalla del nivel
		levelComplete // aparece cuando ganas el nivel junto con un boton que te dirige al menu, donde debe estar desbloqueado el siguiente nivel
}
	

public class GameManager : MonoBehaviour
{
	 
	public Camera levelCamera; // camara actual mostrando el nivel actual
	
	public Camera originalCamera; // primera camara (1er nivel)
	
	public int numberOfAreas = 6; 
	
	public int numberOfLevels = 18;
	
	public static GameManager sharedInstance;
	
	public GameState currentGameState = GameState.menu;

	public Canvas menuCanvas;
	public Canvas gameCanvas;
	public Canvas levelCompleteCanvas;

	public int hpMAX = 3; //max vidas 
	public int hp = 3; // vidas
	
	public int collectedKeys = 0; // llaves para abrir puertas rojas con cerradura
	
	//boosts para las habilidades en el currentLevel
	public int dashBoost;
	public int rewindBoost;
	public int teleportBoost;
	
	//indican si cada skill está activado o no
	public bool dashOn = false;
	public bool rewindOn = false;
	public bool teleportOn = false;
	
	public int currentArea;
	public int currentLevel;
	public int sceneToLoad;
	
	//disponible de boosts por nivel (la iesima posicion del array corresponde a la cantidad de boost de ese skill en el nivel i+1)
	public int[] dashBoostPerLevel = new int[18];
	public int[] rewindBoostPerLevel = new int[18];
	public int[] teleportBoostPerLevel = new int[18];
	
	//upgrades de los skills (duracion de los mismos)
	public int[] dashFuel = new int[2]; //[3,5]; al llegar a determinada area, pasamos de tener 3 segundos a 5 para el dash 
	public int[] rewindFuel = new int[2]; //[3,5]; al llegar a determinada area, pasamos de tener 3 segundos a 5 para el rewind 
	public float[] teleportRadius = new float[2]; //[32,64]; al llegar a determinada area, pasamos de un radio de 32 pixeles a 64 para el teleport
	public int currentDashFuel;
	public int currentRewindFuel;
	public float currentTeleportRadius;


	void Awake(){

        sharedInstance = this;
		currentArea = 1;
		currentLevel = 1;
		hp = hpMAX;
		originalCamera = levelCamera;
		currentDashFuel = dashFuel[0]; //cuando haya un upgrade, pasa a dashFuel[1]
		currentRewindFuel = rewindFuel[0]; //cuando haya un upgrade, pasa a rewindFuel[1]
		currentTeleportRadius = teleportRadius[0]; //cuando haya un upgrade, pasa a teleportRadius[1]
		
	}


    // Start is called before the first frame update
    void Start()
    {
        currentGameState = GameState.menu;
		menuCanvas.enabled = true;
		gameCanvas.enabled = false;
		levelCompleteCanvas.enabled = false;
		
		//Debug.Log("Dash desactivado, velocidad: " + PlayerController.sharedInstance.runSpeed);
		
    }

 
    // Update is called once per frame
    void Update()
	{
	

	}
	
	
	// se llama para iniciar la partida (se llama cliqueando en el boton de PLAY)
	public void StartGame () {
        Time.timeScale = 1; //reanudamos el juego
        ChangeGameState (GameState.inTheGame);
		PlayerController.sharedInstance.StartGame();
    	
		if (GetCurrentLevel()!=1) {ChangeCameraPosition(); } //el player queda en el lugar de la gema del
		                                                     //nivel anterior
		
        //RestoreSwitchesState();
        //RestoreDoorsState();
        //RestoreEnemiesPosition(); //volver a poner a los enemigos donde arrancaron moviendose (pos inicial)
    }
	

	public int GetCurrentLevel()
	{
		return currentLevel;
	}

	public int GetSceneToLoad()
	{
        return sceneToLoad;
	}

	public GameState GetGameState()
	{
		return currentGameState;
    }
	
	public void CollectHP() {

		if(hp<hpMAX){
			hp++;
		}
		
		ViewInGame.sharedInstance.UpdateHpLabel();
	}

	
	
	public void CollectKey(){ // llave roja
		collectedKeys++;
	}
	
	

	
	public void Attacked(string enemyTag){ // se llama cuando el jugador es atacado por un enemigo / objeto 
		
		if (enemyTag == "Enemy"){
			
			hp--;
			
			ViewInGame.sharedInstance.UpdateHpLabel(); //actualizo el HUD de HP
	
			//Debug.Log("Te quedan " + hp + " vidas");
			
		} else { // si lo ataca otra cosa (definir despues), puede que aca le saque mas vidas
			
			
			
		}
		
		
		if (hp == 0){
			
			GameOver(); // el jugador perdio el nivel
			
		}
		
	}
	
	
	
	
	
	public void ChangeGameState(GameState newGameState) {

		if (newGameState == GameState.menu) {
			//La escena de Unity deberá msotrar el menú principal
			menuCanvas.enabled = true;
			gameCanvas.enabled = false;
			levelCompleteCanvas.enabled = false;

		} else if (newGameState == GameState.inTheGame) {
			//La escena de Unity debe configurarse para mostrar el juego en si
			menuCanvas.enabled = false;
			gameCanvas.enabled = true;
			levelCompleteCanvas.enabled = false;

		} else if (newGameState == GameState.levelComplete) {
			//La escena debe mostrar la pantalla de fin de la partida.
			menuCanvas.enabled = false;
			gameCanvas.enabled = false;
			levelCompleteCanvas.enabled = true;
		}


		currentGameState = newGameState;

	}
	
	
	
		//metodo que se llama cuando se completa un nivel
	public void LevelComplete(){
		//sceneToLoad = sceneToLoadParam;
		ChangeGameState(GameState.levelComplete);
        ViewLevelComplete.sharedInstance.UpdateUI();
		UpdateAreaAndLevel();
        Time.timeScale = 0; //pausamos el juego
	}

	
	

	public void GameOver(){
		
		//StopCoroutine(SwitchesController.sharedInstance.KeepBlueDoorsOpen(3.0f));
		RestoreCollectableState(); // volver a poner los collectables donde estaban antes
		// reestablecer el estado de las puertas y switches
		RestoreSwitchesState(); 
		RestoreDoorsState();
		RestoreEnemiesPosition(); //volver a poner a los enemigos donde arrancaron moviendose (pos inicial)
		StartGame();
		
	}
	
	
	
	void UpdateAreaAndLevel(){
		//solo modifico area y nivel si no estoy en el ultimo nivel del juego
		if(currentLevel < numberOfLevels){
			currentLevel++;
			if (currentLevel % 3 == 0){ //nivel 3, 6, 9, 12, 15
				currentArea++;
			}
		}	
	}
	
	
	
	//se llama cuando se hace click en el boton de MAIN MENU en el levelCompleteCanvas
	public void LoadNextLevel(){

		//solo cargo el prox nivel si no estoy en el ultimo
		if(currentLevel < numberOfLevels){
			//aca ya muestra que estoy en nivel 3 cuando tendria q estar en el 2
			ChangeGameState(GameState.menu);
			ViewMainMenu.sharedInstance.EnableNextLevel();
		} else {
            GameFinished();
		}
	}		
	
	
	//se carga cuando ganamos todo el juego
	public void GameFinished(){
	
		
	}
	
	
	// mostramos el nivel siguiente, que estara debajo del nivel anterior
	public void ChangeCameraPosition(){
		
		float height = 2f * originalCamera.orthographicSize; // alto de la camara
		float originalXValue = originalCamera.transform.position.x;
		float originalYValue = originalCamera.transform.position.y;
		float originalZValue = originalCamera.transform.position.z;
		
		// bajo al nivel siguiente
		levelCamera.transform.position = new Vector3(originalXValue, (currentLevel-1) * (originalYValue - height), originalZValue); 
		
	}
	
	
	
	//volver a poner los collectables (llaves, HP, etc) donde estaban al principio del nivel
	private void RestoreCollectableState(){
		
		GameObject[] allCollectable = GameObject.FindGameObjectsWithTag("Key"); // incluye las vidas y los boosts de los skills, porque estan bajo el tag "Key" tambien

		foreach (GameObject col in allCollectable){
			
			Collectable colGO = col.GetComponent<Collectable>();
			
			colGO.ShowCollectable();
			
		}
		
	}
	
	
	
	//volver a poner los switches como estaban
	private void RestoreSwitchesState(){
		
		GameObject[] allSwitches = GameObject.FindGameObjectsWithTag("Switch");
		
		foreach (GameObject switches in allSwitches){

			SwitchesController switchGO = switches.GetComponent<SwitchesController>();
		
			switchGO.RestoreInitialProperties();
		
		}
		
	}
	
	
	
	
	//volver a poner las puertas como estaban (incluyendo la puerta final)
	private void RestoreDoorsState(){
		
		GameObject[] allDoors = GameObject.FindGameObjectsWithTag("Door");
		
		foreach (GameObject door in allDoors){

			Door doorGO = door.GetComponent<Door>();
		
			doorGO.RestoreInitialProperties();
		
			// Debug.Log("Esta abierta la puerta " + door.GetComponent<Door>() + "?: " + doorGO.isOpen + " BoxCollider activado?: " + door.GetComponent<BoxCollider2D>().enabled);
		
		}
		
//		ExitDoor.sharedInstance.isOpen = false; // cierro la ExitDoor si estaba abierta
		
	}
	
	
	
	//volver a poner a los enemigos donde estaban al principio del nivel
	private void RestoreEnemiesPosition(){
		
		GameObject[] allEnemies = GameObject.FindGameObjectsWithTag("Enemy");
		
		foreach (GameObject enemy in allEnemies){

			Enemy enemyGO = enemy.GetComponent<Enemy>();
		
			enemyGO.RestoreStartPosition();
		
		}
		
	}
	
	
	
	public void ActivateDash(){
		
		
		if(dashBoost>0){
		
			dashOn = true;
			dashBoost--;
			ViewInGame.sharedInstance.skillFuelRunOut = false; //arranca el contador del fuel
		
			ViewInGame.sharedInstance.SetCountdownTimer(currentDashFuel,"dash");
				
			//cambio la velocidad
			float newSpeed = PlayerController.sharedInstance.initialRunSpeed * 1.75f;// aumento la velocidad del jugador en un 75%
				
			PlayerController.sharedInstance.runSpeed = newSpeed;
			
			Debug.Log("Dash activado, velocidad: " + PlayerController.sharedInstance.runSpeed);
		
		}
	
	}


}

}
	
	
