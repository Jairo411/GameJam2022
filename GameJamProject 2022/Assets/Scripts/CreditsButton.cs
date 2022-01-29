using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CreditsButton : MonoBehaviour
{
	public bool isCreditsClicked;

	// Start is called before the first frame update
	public void OnMouseUp()
	{
		if (isCreditsClicked)
		{
			SceneManager.LoadScene("Credits"); // Go to credits scene
		}
	}
}
