using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Maze.Manager;

namespace Maze.UI
{

	public class ViewLevelComplete : MonoBehaviour
	{
		public static ViewLevelComplete sharedInstance;
		
		// public Text newSkillOrBoost; //al ganar el area se te otorga un nuevo skill o boost (mejora de un skill)
		
		public Text areaLabel;

		public Text levelCompleteLabel;
		
		public Button backToMainMenu;
		


		void Awake() {
			sharedInstance = this;
		}

		// Use this for initialization
		void Start () {
		
		}
		
		// Update is called once per frame
		void Update () {
			
		}

		public void UpdateUI(){
			if (GameManager.sharedInstance.currentGameState == GameState.levelComplete) {
				areaLabel.text = "Area " + GameManager.sharedInstance.currentArea.ToString ();
				levelCompleteLabel.text = "Level " + GameManager.sharedInstance.currentLevel.ToString ("f0") + " complete!";
			}
		}
		
		
		
		
	}

}
