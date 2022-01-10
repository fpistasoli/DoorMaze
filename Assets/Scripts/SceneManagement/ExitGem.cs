using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Maze.Manager;

namespace Maze.SceneManagement
{

	public class ExitGem : MonoBehaviour
	{
        //[SerializeField] int sceneToLoad;
        //public Transform spawnPoint;
        //public int gemLevel; //level at which the gem is

        void Update(){
			//make it shine and darken over time
		}

		void OnTriggerEnter2D(Collider2D otherCollider){

			if ((otherCollider.tag == "Player")) {
				//print("current level: " + GameManager.sharedInstance.GetCurrentLevel());
				GameManager.sharedInstance.LevelComplete(); // ganamos el nivel!
			}
			
		}

	}

}

