using System.Collections.Generic;

[System.Serializable]
public class DialogObject
{
	public string ID;
	public string[] Names;
	public string[] Messages;
    public List<DialogChoice> Choices;
}
/*
"Juno_JunoSprites/Juno-Happy_enterLeft" -> Split('_')

[
"Juno",
"JunoSprites/Juno-Happy",
"enterLeft"
]

*/