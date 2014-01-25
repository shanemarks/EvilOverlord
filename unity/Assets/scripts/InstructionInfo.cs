using UnityEngine;
using System.Collections;


public class InstructionInfo 
{
	
	public const string RevealPrevTruth = "Last person wanted me to give you this piece of information, it is true that ...";
	public const string RevealPrevLied = "Last person wanted me to lie to you about this piece of information, it is not true that ...";
	
	public const string Is = " is";
	public const string IsNot = "is not";
	
	public const string Fake = "fake and will not gas the room";
	public const string Poisoned = "going to release poison gas";

	public const string Pass = "Pass the phone on";
	
	public const string What = "what";
	public const string Where = "where";
	public const string If = "if";
	public const string FakeOrPoisoned = "will release a poison or if it is fake";

	public string WillTellNext(bool willLie)
	{
		return "I will "+ (willLie ? "lie to them about" : "tell them");
	}
	
	public string GetPrepositionedObject(RoomObject roomObject)
	{
		switch (roomObject)
		{
		case RoomObject.RedBed:
		case RoomObject.GreenBed:
		case RoomObject.Shelf:
			return "under";
		case RoomObject.Crate:
		case RoomObject.Sink:
		case RoomObject.Toilet:
		case RoomObject.WallLamp:
			return "behind";
		}

		return "";
	}
	
	public string GetRoomObjectName(RoomObject roomObject)
	{
		switch (roomObject)
		{
		case RoomObject.RedBed:
			return "the red bed";
		case RoomObject.GreenBed:
			return "the green bed";
		case RoomObject.Shelf:
			return "the shelf";
		case RoomObject.Crate:
			return "the crate";
		case RoomObject.Sink:
			return "the sink";
		case RoomObject.Toilet:
			return "the toilet";
		case RoomObject.WallLamp:
			return "the wall lamp";
		}
		return "";
	}

	public string GetItemName(ItemType roomObject)
	{
		switch (roomObject)
		{
		case ItemType.SomethingUseful:
			return "something useful";
		case ItemType.SomeKnife:
			return "a knife";
		case ItemType.BoobyTrap:
			return "a booby trap";
		case ItemType.FakeKnife:
			return "the fake knife";
		case ItemType.Knife:
			return "the real knife";
		case ItemType.GasMask:
			return "the gas mask";
		}
		return "";
	}

	public string GetVentName(VentObject ventObject)
	{
		switch (ventObject)
		{
		case VentObject.Vent:
			return "the clean vent";
		case VentObject.RustyVent:
			return "the rusty vent";
		}
		return "";
	}




	public class RevealInfo
	{

		public bool revealPreviousIntent = false;

		// need to read previous intent
	}
	
	public class MainInfo
	{
		public enum InfoType {ItemLocation, VentInfo}
		
		public InfoType infoType = InfoType.ItemLocation;
		
		// for InfoType == ItemLocation
		public ItemType itemType; // None == "Something Useful"
		public RoomObject roomObject;
		public bool negateTruth = false;
		
		
		// for InfoType == VentInfo
		public VentObject vent;
		public bool isVentFake = false;
	}

	public class PassOnInfo
	{
		public bool willTellNextPlayerSomething = false;

		public bool willLie = false;

		// if true it means the overlord will say I will tell the next player what is [not] at Y, but won't say what to this player
		// if false it means the overlord will say I will tell them where X is [not]
		public bool willRevealWhatIsAtLocation = false; // only applicable if info to tell is the location of a specific item

		public MainInfo infoToTell = null;
	}

	public InstructionInfo previousInstructionInfo;

	public RevealInfo revealInfo;
	public MainInfo mainInfo;
	
	public PassOnInfo passOnInfo;

	bool FirstRound { get { return previousInstructionInfo == null; } }

	public string CreateString()
	{
		string mainString = "";
		if (revealInfo.revealPreviousIntent && previousInstructionInfo.passOnInfo.willTellNextPlayerSomething)
		{
			if (!FirstRound)
			{
				if (previousInstructionInfo.passOnInfo.willLie)
					mainString += RevealPrevLied;
				else
					mainString += RevealPrevTruth;
				mainString += "\n\n";
			}


		}



		if (mainInfo.infoType == MainInfo.InfoType.ItemLocation)
		{
			mainString += GetItemName(mainInfo.itemType) + " " + (mainInfo.negateTruth ? IsNot : Is) + " " + GetPrepositionedObject(mainInfo.roomObject) + " " +GetRoomObjectName(mainInfo.roomObject);
		}
		if (mainInfo.infoType == MainInfo.InfoType.VentInfo)
		{
			mainString += GetVentName(mainInfo.vent) + " " + Is + " " + (mainInfo.isVentFake ? Fake : Poisoned);
		}
		
		mainString += "\n\n";

		mainString += Pass;
		
		if (passOnInfo.willTellNextPlayerSomething)
		{
			// TODO if it is going to be a vent story , tell them that that will be told instead

			mainString += ", ";
			mainString += WillTellNext(passOnInfo.willLie) + " ";
			if (passOnInfo.willRevealWhatIsAtLocation)
			{
				if (passOnInfo.infoToTell.infoType == MainInfo.InfoType.ItemLocation)
					mainString += What + " " + (passOnInfo.infoToTell.negateTruth ? IsNot : Is) +" " +GetPrepositionedObject (passOnInfo.infoToTell.roomObject) + " "+ GetRoomObjectName(passOnInfo.infoToTell.roomObject);
				else if (passOnInfo.infoToTell.infoType == MainInfo.InfoType.VentInfo)
					mainString += If + " " + GetVentName(passOnInfo.infoToTell.vent) + " " +FakeOrPoisoned;
			}
			else
			{
				mainString += Where + " " + GetItemName(passOnInfo.infoToTell.itemType) + (passOnInfo.infoToTell.negateTruth ? IsNot : Is);
			}
		}
	
		return mainString;
	}






}













