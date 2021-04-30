using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.SceneManagement;

public class DialogController : MonoBehaviour
{
    [SerializeField] private TextAsset dialogText;
    [SerializeField] private DialogList dialogList;
	[SerializeField] private string startingDialogID;
	
	[SerializeField] private TMP_Text messageText;
	[SerializeField] private TMP_Text nameText;
	[SerializeField] private Button choice1;
	[SerializeField] private Button choice2;

	[Header("Message Images")]
	[SerializeField] private Image LeftImage;
	[SerializeField] private Image CenterImage;
	[SerializeField] private Image RightImage;

	private DialogObject currentDialog;
	private int currentMessage = 0;
    private int currentName = 0;

    private void Start()
	{
		dialogList = JsonUtility.FromJson<DialogList>(dialogText.text);

		ShowDialog(startingDialogID);
	}

	private void Update()
	{
		if(currentDialog.Choices.Count > 0 && Input.GetKeyDown(KeyCode.Alpha1)) //Press 1 and have at least 1 choice.
		{
			ShowDialog(currentDialog.Choices[0].TargetDialog);
		}
		else if(currentDialog.Choices.Count > 1 && Input.GetKeyDown(KeyCode.Alpha2)) //Press 2 and have at least 2 choices.
		{
			ShowDialog(currentDialog.Choices[1].TargetDialog);
		}
		else if(currentMessage < currentDialog.Messages.Length-1 && Input.GetKeyDown(KeyCode.Mouse0)) //Advance message
		{
			currentMessage++;
            currentName++;
            UpdateMessage();
        }
	}

	private void ShowDialog(string dialogID)
	{
		if(dialogID.StartsWith("GotoScene"))
		{
			int targetScene = int.Parse(dialogID.Substring(9));
			SceneManager.LoadScene(targetScene);
			return;
		}

		DialogObject newDialog = dialogList.Dialogs.Find(x => x.ID == dialogID);
		if (newDialog != null)
		{
			currentMessage = 0;
            currentName = 0;
			currentDialog = newDialog;
			UpdateNameAndMessage();
			UpdateImages();
			
			
			choice1.gameObject.SetActive(false);
			choice2.gameObject.SetActive(false);
			if(currentDialog.Messages.Length == 1)
			{
				UpdateMessage();
			}
		}
		else 
		{
			Debug.LogError("Couldn't find a dialog ID: " + dialogID);
		}
	}

	private void UpdateMessage()
	{
		UpdateNameAndMessage();
		UpdateImages();		

		if (currentMessage == currentDialog.Messages.Length - 1) //Last message
		{
            if (currentDialog.Choices.Count > 0)
			{
                if (currentDialog.Choices[0].Text == "Continue")
                {
                    ShowDialog(currentDialog.Choices[0].TargetDialog);
                } else
                {
                    choice1.gameObject.SetActive(true);
                    choice1.GetComponentInChildren<TMP_Text>().text = currentDialog.Choices[0].Text;
                }
				
			}
			if (currentDialog.Choices.Count > 1)
			{
				choice2.gameObject.SetActive(true);
				choice2.GetComponentInChildren<TMP_Text>().text = currentDialog.Choices[1].Text;
			}
		}
	}

	private void UpdateNameAndMessage()
	{
		nameText.text = currentDialog.Names[currentName].Split('_')[0].Replace("#","");
		messageText.text = currentDialog.Messages[currentMessage];

		//Check names
		if (currentDialog.Names[currentName].StartsWith(" "))
		{
			messageText.text = "<i><color=green>" + messageText.text + "</color></i>";
		}
	}

	private void UpdateImages()
	{
		/*
		 Parse the string in the current 'name' for multiple characters, their names, sprite paths, positions and movement.
		 */
		if (currentDialog.Names[currentName].Contains("#")) //# denotes we should clear the sprites this update.
		{
			LeftImage.enabled = false;
			CenterImage.enabled = false;
			RightImage.enabled = false;
		}

		//Message Images
		string[] characters = new string[] { currentDialog.Names[currentName] };
		if (characters[0].Contains("&"))
		{
			characters = characters[0].Split('&');
		}
		foreach(string character in characters)
		{
			string[] parsedChar = character.Split('_');
			if (parsedChar.Length == 0)
			{
				LeftImage.enabled = false;
				CenterImage.enabled = false;
				RightImage.enabled = false;
			}
			else if (parsedChar.Length > 1) // We have a character name and a path to a sprite.
			{
				string position = parsedChar[2];

				if (position.Contains("left"))
				{
					LeftImage.enabled = true;
					LeftImage.sprite = Resources.Load<Sprite>(parsedChar[1]);

					if (position.Contains("enter"))
					{
						LeftImage.GetComponent<Animator>().Play("Enter");
					}
					else if (position.Contains("exit"))
					{
						LeftImage.GetComponent<Animator>().Play("Exit");
					}
					else
					{
						LeftImage.GetComponent<Animator>().StopPlayback();
					}
				}
				else if (position.Contains("center"))
				{
					CenterImage.enabled = true;
					CenterImage.sprite = Resources.Load<Sprite>(parsedChar[1]);
				}
				else if (position.Contains("right"))
				{
					RightImage.enabled = true;
					RightImage.sprite = Resources.Load<Sprite>(parsedChar[1]);
				}
			}
		}
	}

    public void ClickChoice(int choice)
	{
		ShowDialog(currentDialog.Choices[choice].TargetDialog);
	}
}
