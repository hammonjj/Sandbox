using System;
using System.Collections.Generic;

//Might rename to Conversation
public class DialogItem
{
	public struct Dialog
	{
		public Constants.Enums.Characters Character;
		public string DialogLine;
	}

	private SortedDictionary<Constants.Enums.Characters, string> _dialog;

    public DialogItem()
	{
	}

	public Dialog GetNextLine()
	{
		var lineOfDialog = new Dialog();

		return lineOfDialog;
	}
}

