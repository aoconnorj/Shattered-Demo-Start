using System.Collections.Generic;

[System.Serializable]
public class DialogObject
{
	public string ID;
	public string[] Names;
	public string[] Messages;
    //public string[] JunoSprites;
    public List<DialogChoice> Choices;
}