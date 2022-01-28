using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class MainMenuButtons2 : MonoBehaviour
{
    public bool isStart;
    public bool isQuit;

	// Start is called before the first frame update
	void OnMouseUp()
	{
		if (isStart)
		{
			//Application.LoadLevel(4);

			// or use
			SceneManager.LoadScene("Scene2");
		}
		if (isQuit)
		{
			Application.Quit();

			// or use
			// Debug.Log("QUIT");
			// SceneManager.LoadScene("Quit")
		}
	}
}
