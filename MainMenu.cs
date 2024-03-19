using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenu : MonoBehaviour
{
    public void PlayGame(){
        SceneManager.LoadSceneAsync(1);
    }

    public void Back(){
        SceneManager.LoadSceneAsync(0);
    }

    public void Easy(){
         Debug.Log("Easy Mode");
        //SceneManager.LoadSceneAsync(2);
    }

    public void Normal(){
         Debug.Log("Normal Mode");
        //SceneManager.LoadSceneAsync(3);
    }

    public void Hard(){
        Debug.Log("Hard Mode");
        //SceneManager.LoadSceneAsync(4);
    }

    public void QuitGame(){
        Application.Quit();
    }


}
