using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
   private void Awake()
       {
           Cursor.lockState = CursorLockMode.None;
       }
   
       public void PlayTheLevel(string LevelName) //play the level base on given string 
       {
           SceneManager.LoadScene(LevelName);
       }
   
       public void QuitGame() //quit the game 
       {
           Application.Quit();
       }
}
