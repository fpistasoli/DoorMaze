using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Maze.Manager;
using UnityEngine.SceneManagement;
using Maze.Control;

namespace Maze.UI
{
   
    public class ViewMainMenu : MonoBehaviour
    {

        public static ViewMainMenu sharedInstance;

        public Button[] levels;

        void Awake()
        {
            sharedInstance = this;
            //levels = new Button[GameManager.sharedInstance.numberOfLevels];
        }


        public void EnableNextLevel()
        {
            if (GameManager.sharedInstance.currentGameState == GameState.menu)
            {
                levels[GameManager.sharedInstance.currentLevel-1].enabled = true; //habilito boton del prox nivel
                //cambio el color del boton a habilitado
                levels[GameManager.sharedInstance.currentLevel-1].GetComponent<Image>().color = new Color32(197,53,165,255);
        
            }
        }


        // Start is called before the first frame update
        void Start()
        {
            
        }

        // Update is called once per frame
        void Update()
        {
            
        }

      /*  public void LoadScene()
        {
            int currentLevel = GameManager.sharedInstance.GetCurrentLevel();
            //DontDestroyOnLoad(GameObject.FindWithTag("Player")); //para preservar el objeto Player entre escenas
            SceneManager.LoadSceneAsync(GameManager.sharedInstance.GetSceneToLoad());
            GameManager.sharedInstance.ChangeGameState(GameState.inTheGame);
            GameManager.sharedInstance.StartGame(); //cargo el nivel actual
        }
      */


    }

}
