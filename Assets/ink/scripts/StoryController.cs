using UnityEngine;
using UnityEngine.UI;
using System.Collections;
using Ink.Runtime;
using System.Collections.Generic;
using UnityEngine.Assertions;
using System;

public class StoryController : MonoBehaviour {
	[SerializeField] TextAsset inkJSONAsset;
	[SerializeField] Canvas canvas;

	// UI Prefabs
	[SerializeField] Text textPrefab;
	[SerializeField] Button buttonPrefab;
	[SerializeField] InkInputField inputFieldPrefab;

	Story story;
	Dictionary<string, Action> commands;
	bool isPaused;

	void Awake () {
		commands = new Dictionary<string, Action> ();
		commands.Add ("RequestInput", this.RequestInput);
		InitStory ();
		StartStory();
	}

	void InitStory () {
		story = new Story (inkJSONAsset.text);
	}

	void RequestInput() {
		InkInputField inputField = Instantiate (inputFieldPrefab) as InkInputField;
		Assert.AreEqual (story.currentTags.Count, 4);
		string varToSet = story.currentTags [2];
		string defaultText = story.currentTags [3];
		inputField.SetText( defaultText );
		inputField.SetParent (canvas.transform);
		inputField.AddListener (input => this.ProcessInput(input, varToSet));
		isPaused = true;
	}

	void ProcessInput(string input, string varToSet) {	
		story.variablesState [varToSet] = input;
		isPaused = false;
		RefreshView ();
	}

	void StartStory () {
		RefreshView();
	}

	void RefreshView () {
		RemoveChildren ();

		while (story.canContinue && !isPaused) {
			string text = story.Continue ().Trim();
			CreateContentView(text);
			if (IsCommand ()) {
				ExecuteCommand ();
			} 
		}

		if (isPaused) {
			return;
		}

		if(story.currentChoices.Count > 0) {
			for (int i = 0; i < story.currentChoices.Count; i++) {
				Choice choice = story.currentChoices [i];
				Button button = CreateChoiceView (choice.text.Trim ());
				button.onClick.AddListener (delegate {
					OnClickChoiceButton (choice);
				});
			}
		} else {
			Button choice = CreateChoiceView("End of story.\nRestart?");
			choice.onClick.AddListener(delegate{
				StartStory();
			});
		}
	}

	bool IsCommand() {
		return story.currentTags.Count > 0 && story.currentTags [0] == "COMMAND";
	}
		
	void ExecuteCommand() {
		UnityEngine.Assertions.Assert.IsTrue (IsCommand ());
		if (story.currentTags.Count <= 1) {
			Debug.LogError ("Command with no name");
			return;
		}
		string commandName = story.currentTags [1];
		if (commands.ContainsKey(commandName)) {
			commands [commandName] ();
		} else {
			Debug.LogError ("Unknown command " + commandName);
		}
	}

	void OnClickChoiceButton (Choice choice) {
		story.ChooseChoiceIndex (choice.index);
		RefreshView();
	}

	void CreateContentView (string text) {
		Text storyText = Instantiate (textPrefab) as Text;
		storyText.text = text;
		storyText.transform.SetParent (canvas.transform, false);
	}

	Button CreateChoiceView (string text) {
		Button choice = Instantiate (buttonPrefab) as Button;
		choice.transform.SetParent (canvas.transform, false);

		Text choiceText = choice.GetComponentInChildren<Text> ();
		choiceText.text = text;

		HorizontalLayoutGroup layoutGroup = choice.GetComponent <HorizontalLayoutGroup> ();
		layoutGroup.childForceExpandHeight = false;

		return choice;
	}

	void RemoveChildren () {
		int childCount = canvas.transform.childCount;
		for (int i = childCount - 1; i >= 0; --i) {
			GameObject.Destroy (canvas.transform.GetChild (i).gameObject);
		}
	}
}
