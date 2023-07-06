using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PauseQuitMenu : MonoBehaviour
{
    [SerializeField] GameObject pausemenu;


    public void Pause(){
        pausemenu.SetActive(true);   
        Time.timeScale = 0f;    // pauses game
    }

    public void Resume(){
        pausemenu.SetActive(false);   
        Time.timeScale = 1f;    // pauses game
    }
    public void Quit()
    {
        Application.Quit();   // Quits when you're in a build
        Debug.Log("I'm out, I quit!");
    }

    public void ChangeScene(int scene_build_index)
    {
        SceneManager.LoadScene(scene_build_index);
    }




    private void Update() {   // Escape key to pause
        if (Input.GetKeyDown(KeyCode.Escape) )
        {
            if ( pausemenu.activeSelf == false )  // if pause menu if pause
            {
                Pause();
            }
            else
            {
                Resume();
            }
            
        }
    }

}
