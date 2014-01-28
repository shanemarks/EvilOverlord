using UnityEngine;
using System.Collections;



public enum InstructionTypeGeneral {Positive, Negative, Critical}
public enum InstructionType {Positive1Location, Positive2Item, Negative1Both, Negative2Both, CriticalWarning, CriticalReveal, PassOn}

public class InstructionInfo 
{

	public InstructionType instructionType;

	public int foreWarning = 0;

	public InfoPacket infoPacket;

	public class InfoPacket 
	{
		public LocationType location;
		public PickupType item;
	}
	
	public const string ItemTag = "<ITEM>";
	public const string LocationTag = "<LOCATION>";
	public const string WarningTimeTag = "<WARNING_TIME>";

	public const string PassHeadphones =  "Pass the headphones to another player when you are ready.";

	string GetRawString()
	{

		switch (instructionType)
		{
		case InstructionType.Positive1Location:
			return "There is an item <LOCATION>. The next listener will be given the name of that item.";
		case InstructionType.Positive2Item:
			return "There is a <ITEM> in this room. The previous listener was given its location.";
		case InstructionType.Negative1Both:
			return "The <ITEM> is not <LOCATION>. The next listener will be given the same information.";
		case InstructionType.Negative2Both:
			return "The <ITEM> is not <LOCATION>. The previous listener was given this same information.";
		case InstructionType.CriticalWarning:
			return "In <WARNING_TIME> turns, I'll reveal the identity and location of the <ITEM> to whoever has the phone.";
		case InstructionType.CriticalReveal:
			return "The <ITEM> is <LOCATION>. Beware, someone else knows you have important information.";
		case InstructionType.PassOn:
			return "I have no information to give you at this time, but avoid ending your turn too early.";
		}

		return "There was an error in the game, please email the creators and punch them in the face.";
	}

	public string GetPrepositionedForLocation(LocationType roomLocation)
	{
		switch (roomLocation)
		{
		case LocationType.RedBed:
		case LocationType.GreenBed:
		case LocationType.Shelf:
			return "under";
		case LocationType.Grate:
		case LocationType.Sink:
		case LocationType.Toilet:
		case LocationType.WallLamp:
			return "behind";
		case LocationType.RustyVent:
		case LocationType.CleanVent:
			return "inside";
		}

		return "parallel to";
	}
	public string GetRoomLocationName(LocationType roomLocation)
	{
		switch (roomLocation)
		{
		case LocationType.RedBed:
			return "the red bed";
		case LocationType.GreenBed:
			return "the green bed";
		case LocationType.Shelf:
			return "the shelf";
		case LocationType.Grate:
			return "the grate";
		case LocationType.Sink:
			return "the sink";
		case LocationType.Toilet:
			return "the toilet";
		case LocationType.WallLamp:
			return "the wall lamp";
		case LocationType.CleanVent:
			return "the clean vent";
		case LocationType.RustyVent:
			return "the rusty vent";
		}
		return "a non location";
	}

	public string GetItemName(PickupType pickupType)
	{
		switch (pickupType)
		{
		case PickupType.BoobyTrap1:
		case PickupType.BoobyTrap2:
		case PickupType.BoobyTrap3:
			return "booby trap";
		case PickupType.FakeKnife:
			return "fake knife";
		case PickupType.RealKnife1:
		case PickupType.RealKnife2:
			return "real knife";
		case PickupType.GasMask1:
		case PickupType.GasMask2:
			return "gas mask";
		case PickupType.GasTrap:
			return "gas trap";
		}
		return "a non item";
	}

	public string CreateString()
	{

		string raw = GetRawString();

		if (infoPacket == null)
			return raw + " " + PassHeadphones;

		return raw
				.Replace("<ITEM>", GetItemName(infoPacket.item))
				.Replace("<LOCATION>", GetPrepositionedForLocation(infoPacket.location) + " " + GetRoomLocationName(infoPacket.location))
				.Replace("<WARNING_TIME>", foreWarning.ToString()) +
				" " + PassHeadphones;
	}


//	public const string RevealPrevTruth = "Last person wanted me to give you this piece of information, it is true that ";
//	public const string RevealPrevLied = "Last person wanted me to lie to you about this piece of information, it is not true that ";
//	
//	public const string Is = "is";
//	public const string IsNot = "is not";
//	
//	public const string Fake = "fake and will not gas the room";
//	public const string Poisoned = "going to release poison gas";
//
//	public const string Pass = "Pass the phone on";
//	
//	public const string What = "what";
//	public const string Where = "where";
//	public const string If = "if";
//	public const string FakeOrPoisoned = "will release a poison or if it is fake";
//
//	public string WillTellNext(bool willLie)
//	{
//		return "I will "+ (willLie ? "lie to them about" : "tell them");
//	}
//	
//	public string GetPrepositionedObject(RoomObject roomObject)
//	{
//		switch (roomObject)
//		{
//		case RoomObject.RedBed:
//		case RoomObject.GreenBed:
//		case RoomObject.Shelf:
//			return "under";
//		case RoomObject.Crate:
//		case RoomObject.Sink:
//		case RoomObject.Toilet:
//		case RoomObject.WallLamp:
//			return "behind";
//		}
//
//		return "";
//	}
//	
//	public string GetRoomObjectName(RoomObject roomObject)
//	{
//		switch (roomObject)
//		{
//		case RoomObject.RedBed:
//			return "the red bed";
//		case RoomObject.GreenBed:
//			return "the green bed";
//		case RoomObject.Shelf:
//			return "the shelf";
//		case RoomObject.Crate:
//			return "the crate";
//		case RoomObject.Sink:
//			return "the sink";
//		case RoomObject.Toilet:
//			return "the toilet";
//		case RoomObject.WallLamp:
//			return "the wall lamp";
//		case RoomObject.CleanVent:
//			return "the clean vent";
//		case RoomObject.RustyVent:
//			return "the rusty vent";
//		}
//		return "";
//	}
//
//	public string GetItemName(ItemType roomObject)
//	{
//		switch (roomObject)
//		{
//		case ItemType.SomethingUseful:
//			return "something useful";
//		case ItemType.SomeKnife:
//			return "a knife";
//		case ItemType.BoobyTrap:
//			return "a booby trap";
//		case ItemType.FakeKnife:
//			return "the fake knife";
//		case ItemType.RealKnife:
//			return "the real knife";
//		case ItemType.GasMask:
//			return "the gas mask";
//		}
//		return "";
//	}
//
//
//
//
//	public class RevealInfo
//	{
//
//		public bool revealPreviousIntent = false;
//
//		// need to read previous intent
//	}
//	
//	public class MainInfo
//	{
////		public enum InfoType {ItemLocation, VentInfo}
//		
////		public InfoType infoType = InfoType.ItemLocation;
//		
//		// for InfoType == ItemLocation
//		public ItemType itemType; // None == "Something Useful" , if is a vent, then will tell about the vent
//		public RoomObject roomObject;
//		public bool negateTruth = false;
//		
////		public RoomObject poisonedVent;
//		
//		// for InfoType == VentInfo
////		public VentObject vent;
////		public bool isVentFake = false;
//	}
//
//	public class PassOnInfo
//	{
//		public bool willTellNextPlayerSomething = false;
//
//		public bool willLie = false;
//
//		// if true it means the overlord will say I will tell the next player what is [not] at Y, but won't say what to this player
//		// if false it means the overlord will say I will tell them where X is [not]
//		public bool willRevealWhatIsAtLocation = false; // only applicable if info to tell is the location of a specific item
//
//		public MainInfo infoToTell = null;
//	}
//
//	public InstructionInfo previousInstructionInfo;
//
//	public RevealInfo revealInfo;
//	public MainInfo mainInfo;
//	
//	public PassOnInfo passOnInfo;
//
//	bool FirstRound { get { return previousInstructionInfo == null; } }
//
//	public string CreateString()
//	{
//
//		// Reveal step
//		string mainString = "";
//		if (previousInstructionInfo != null && revealInfo.revealPreviousIntent && previousInstructionInfo.passOnInfo.willTellNextPlayerSomething)
//		{
////			if (!FirstRound)
////			{
//				if (previousInstructionInfo.passOnInfo.willLie)
//					mainString += RevealPrevLied;
//				else
//					mainString += RevealPrevTruth;
//				mainString += "\n\n";
////			}
//
//
//		}
//		
//
//		// Main info step
//		if (mainInfo.roomObject == RoomObject.CleanVent || mainInfo.roomObject == RoomObject.RustyVent)
//		{
//			bool ventIsPoisonous = (mainInfo.roomObject == GameController.instance.VentWithPoison);
//
//			if (previousInstructionInfo.passOnInfo.willLie)
//				ventIsPoisonous = !ventIsPoisonous;
//			
//			if (mainInfo.negateTruth)
//				ventIsPoisonous = !ventIsPoisonous;
//
//
//			mainString += GetRoomObjectName(mainInfo.roomObject) + " " + (mainInfo.negateTruth ? IsNot : Is) + " " + (ventIsPoisonous ? Poisoned : Fake);
//		}
//		else
//		{
//			mainString += GetItemName(mainInfo.itemType) + " " + (mainInfo.negateTruth ? IsNot : Is) + " " + GetPrepositionedObject(mainInfo.roomObject) + " " +GetRoomObjectName(mainInfo.roomObject);
//		}
//		
//		mainString += ".\n\n";
//		
//		// Pass on step
//
//		mainString += Pass;
//		
//		if (passOnInfo.willTellNextPlayerSomething)
//		{
//
//			mainString += ", ";
//			mainString += WillTellNext(passOnInfo.willLie) + " ";
//			if (passOnInfo.willRevealWhatIsAtLocation)
//			{
//				if (passOnInfo.infoToTell.roomObject == RoomObject.CleanVent || passOnInfo.infoToTell.roomObject == RoomObject.RustyVent)
//				{
//					mainString += If + " " + GetRoomObjectName(passOnInfo.infoToTell.roomObject) + " " +FakeOrPoisoned;
//				}
//				else
//				{
//					mainString += What + " " + (passOnInfo.infoToTell.negateTruth ? IsNot : Is) +" " +GetPrepositionedObject (passOnInfo.infoToTell.roomObject) + " "+ GetRoomObjectName(passOnInfo.infoToTell.roomObject);
//				}
//			}
//			else
//			{
//				mainString += Where + " " + GetItemName(passOnInfo.infoToTell.itemType) + " " + (passOnInfo.infoToTell.negateTruth ? IsNot : Is);
//			}
//		}
//		mainString += ".";
//		return mainString;
//	}
//
//
//
//


}













