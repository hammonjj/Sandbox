﻿using System;
using System.Collections.Generic;

//Might rename to Conversation
public class DialogItem
{
	public struct Dialog
	{
		public Constants.Enums.Character Character;
		public string DialogLine;
	}

	private SortedDictionary<Constants.Enums.Character, string> _dialog;

    public DialogItem()
	{
	}

	public Dialog GetNextLine()
	{

	}
}

