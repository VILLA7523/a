using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
public class Change : MonoBehaviour
{
    // Start is called before the first frame update
    public GameObject ObjectMenuPause;
    public GameObject menuExit;
    void Start()
    {
        ObjectMenuPause.SetActive(false);
    }


    // Update is called once per frame
    void Update()
    {
        
    }

    public void pause() {
        ObjectMenuPause.SetActive(true);
        Time.timeScale = 0;
        Cursor.lockState = CursorLockMode.None;
    }
    public void resume()
    {
        ObjectMenuPause.SetActive(false);
        menuExit.SetActive(false);

        Time.timeScale = 1;
        Cursor.lockState = CursorLockMode.None;
    }

    public void goMenu(string nameMenu) {
        SceneManager.LoadScene(nameMenu);
    }
    
    public void Quit() {
        Application.Quit();
    }
}
