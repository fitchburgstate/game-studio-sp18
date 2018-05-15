using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using Fabric;
using System.Linq;

public class AudioTestingInputv2 : MonoBehaviour {

	[SerializeField]
	private GameObject AudioManager;
	[SerializeField]
	private Canvas canvas;
	[SerializeField]
	private Button buttonPrefab;

	private List<string> MyEventList;

	void Awake () {
		// Insantiate a shitload of buttons in a canvas to create a board
		CreateSoundboard();
	}

	void CreateSoundboard(){
		MyEventList = AudioManager.GetComponent<EventManager>()._eventList;
		foreach (string eventString in MyEventList.Skip(1)) {

			Button choice = Instantiate (buttonPrefab) as Button;
			choice.transform.SetParent (canvas.transform);
            choice.name = eventString + " | Soundboard Button";
			Text choiceText = choice.GetComponentInChildren<Text> ();
			choiceText.text = eventString;

			choice.onClick.AddListener(delegate {
				Fabric.EventManager.Instance?.PostEvent(eventString, gameObject);
			});
		}
	}
} 
