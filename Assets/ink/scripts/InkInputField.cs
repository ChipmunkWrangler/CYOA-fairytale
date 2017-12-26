using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using System;

public class InkInputField : MonoBehaviour {			
	[SerializeField] InputField inputFieldPrefab;

	InputField inputField;

	public void SetText(string text) {
		inputField.text = text; 
	}

	public void SetParent (Transform parentTransform) {
		inputField.transform.SetParent (parentTransform, false);
	}

	public void AddListener(Action<string> callback) {
		inputField.onEndEdit.AddListener (delegate { callback(inputField.text); });
	}

	void Awake () {
		inputField = Instantiate (inputFieldPrefab) as InputField;
	}
}
