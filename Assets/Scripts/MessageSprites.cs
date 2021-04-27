using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MessageSprites
{
	public string[] PathToSprite;
	public bool[] Enter;
	public bool[] Exit;
	public int[] Side; // -1 = left, +1 = right
}