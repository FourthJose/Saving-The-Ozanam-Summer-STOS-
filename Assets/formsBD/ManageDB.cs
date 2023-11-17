using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ManageDB
{
	public static string sessionUser;
	public static int saveState1;
	public static int saveState2;
	public static int saveState3;

	public static bool logged { get { return sessionUser != null;} }

	public static void LogOut()
	{
		sessionUser = null;
	}
}