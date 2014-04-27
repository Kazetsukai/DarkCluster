using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class GuiController : MonoBehaviour {

	List<string> _enemies = new List<string>();

	// Use this for initialization
	void Start () 
	{
		_enemies.Add("ship 1");
		_enemies.Add("ship 2");
		_enemies.Add("ship 3");
	}
	
	// Update is called once per frame
	void Update () 
	{
	}

	void OnGUI ()
	{
		DrawEnemySelectors(_enemies);
	}

	void DrawEnemySelectors(List<string> enemies)
	{
		var location = 0;

		foreach (var enemy in enemies)
		{
			if (GUI.Button (new Rect (10, 10 + (location * 40), 150, 30), enemy))
			{
				Debug.Log("Fired at " + enemy);
			}

			location++;
		}
	}
}
