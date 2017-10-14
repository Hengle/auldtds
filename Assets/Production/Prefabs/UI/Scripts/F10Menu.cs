using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class F10Menu : MonoBehaviour
{

    [SerializeField]
    private GameObject f10Menu;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        SceneManager.sceneLoaded += InitializeThis;
    }
    void InitializeThis(Scene scene, LoadSceneMode mode)
    {
        f10Menu.SetActive(false);
    }
    // Use this for initialization
    void Start ()
    {
		
	}
	
	// Update is called once per frame
	void Update ()
    {
        ToggleGameMenu();
    }

    private void ToggleGameMenu()
    {
        if (Input.GetKeyDown(KeyCode.F10))
        {
            if (f10Menu.activeInHierarchy)
            {
                f10Menu.SetActive(false);
            }
            else
            {
                f10Menu.SetActive(true);
            }
        }
        if (Input.GetKeyDown(KeyCode.Escape))
        {
            f10Menu.SetActive(false);
        }
    }
}
