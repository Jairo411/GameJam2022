using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MainMenuButtons : MonoBehaviour
{
    public bool isStart;
    public bool isQuit;

	// Start is called before the first frame update
	void OnMouseUp()
	{
		if (isStart)
		{
			Application.LoadLevel(1);
		}
		if (isQuit)
		{
			Application.Quit();
		}
	}
}
