using UnityEngine;
using System.Collections.Generic;





//public enum ItemType {None, SomethingUseful, SomeKnife, BoobyTrap, RealKnife, FakeKnife, GasMask}
//
public enum LocationType {RedBed, GreenBed, Sink, Toilet, Shelf, Grate, WallLamp, CleanVent, RustyVent}

public enum PickupType {None = -1, BoobyTrap1, BoobyTrap2, BoobyTrap3, RealKnife1, RealKnife2, FakeKnife, GasMask1, GasMask2, GasTrap}

//public enum VentObject {}


public class GameController : SingletonBehaviour<GameController> 
{
	
	Dictionary<LocationType, PickupType> roomItemLocations;

	List<LocationType> unexploredLocations;

	List<InstructionInfo> instructionList = new List<InstructionInfo>();

	LocationType ventWithPoison;

	public LocationType VentWithPoison { get { return ventWithPoison; } }

	public float knifeRange = 1f;

	int criticalInfoCounter = 0;

	public void TriggerGasTrap ()
	{
		poisonVentOpen = true;
	}
	
	bool poisonVentOpen = false;
	public bool GasTrapTriggered { get { return poisonVentOpen; } }

//	static bool IsGeneralItem(ItemType itemType)
//	{
//		if (itemType == ItemType.None)
//			return true;
//		if (itemType == ItemType.SomethingUseful)
//			return true;
//		if (itemType == ItemType.SomeKnife)
//			return true;
//
//		return false;
//	}
//
//	static bool CanHideInRoomObject(LocationType roomObject)
//	{
//		if (roomObject == LocationType.CleanVent)
//			return false;
//		if (roomObject == LocationType.RustyVent)
//			return false;
//
//		return true;
//	}

	void SetupRoomObjects()
	{
		roomItemLocations = new Dictionary<LocationType, PickupType>();

		List<LocationType> roomLocations = new List<LocationType>();
		foreach(LocationType roomLocation in  System.Enum.GetValues(typeof(LocationType)))
		{
			roomLocations.Add(roomLocation);
		}


		List<PickupType> pickupItems = new List<PickupType>();

		foreach (PickupType itemLocation in System.Enum.GetValues(typeof(PickupType)))
		{
			if (itemLocation == PickupType.None)
				continue;

			pickupItems.Add (itemLocation);
		}

//		while (roomItems.Count < roomObjects.Count)
//			roomItems.Add(ItemType.BoobyTrap);

		// add random itoms to objects
//		foreach (LocationType roomObject in roomLocations)
		while(roomLocations.Count > 0)
		{
			int randomRoomIndex = Random.Range(0, roomLocations.Count);
			int randomItemIndex = Random.Range(0, pickupItems.Count);
			roomItemLocations[roomLocations[randomRoomIndex]] = pickupItems[randomItemIndex];
			
			pickupItems.RemoveAt(randomItemIndex);
			roomLocations.RemoveAt(randomRoomIndex);

			bool debugBreak = false;
			if (debugBreak)
				break;
		}

	}



	public enum GameState {Unintialised, Intro, GiveHeadphone, Instructing, Finish}


	FSM<GameState> state = new FSM<GameState>();


	void Start ()
	{
		Debug.Log("GameController::Start");
		state[GameState.Intro].changeToStateFunction = RestartGame;
		state[GameState.GiveHeadphone].changeToStateFunction = GiveHeadphones;
		state[GameState.Instructing].changeToStateFunction = Instructing;

		state.ChangeState(GameState.Intro);
	}

	void Update()
	{
		if (Input.GetKeyDown(KeyCode.F4))
		{
			criticalInfoCounter = 0;
		}

		state.UpdateAll();
	}

	void RestartGame(GameState oldState, GameState newState)
	{
		Debug.Log ("Restarting Game");
		SetupRoomObjects();

		
		poisonVentOpen = false;
		ResetCriticalCounter();

		// do anything else that is needed

		unexploredLocations = new List<LocationType>(roomItemLocations.Keys);

		instructionList.Clear();
//		instructionList.Add (CreateRandomInstructionInfo(null));

//		foreach (RoomObject roomObject in roomItemLocations.Keys)
//			unexploredLocations.Add(roomObject);

//		ventWithPoison = Random.Range(0,2) == 0 ? LocationType.CleanVent : LocationType.RustyVent; 


		// add 1 or 2 pass on instructions
		
//		AddPassOnInstruction(); // adding an instruction to skip

		AddPassOnInstruction();
		if (Random.value < 0.5)
			AddPassOnInstruction();

		state.ChangeState(GameState.GiveHeadphone);
	}


	LocationType GetUnexploredLocation()
	{
		LocationType unexploredLocation = unexploredLocations[Random.Range(0, unexploredLocations.Count)];
		return unexploredLocation;
	}

	
	public float chanceToRevealPreviousIntent = 0.3f;
	//	public float chanceToGiveVentInfo = 0.15f;
	public float chanceToNegateInfo = 0.3f;


	public float chanceToGenerliseToSomethingUseful = 0.2f;

	public float chanceToGenerliseToSomeKnife = 0.5f;
	
	public static PickupType PickRandomItem(params PickupType [] objs)
	{
		// range is from 1 snce we do not want to pick None
		return objs[Random.Range(1, objs.Length)];
	}
	
	public static PickupType PickRandomInverseItem(params PickupType [] notObjs)
	{
		List<PickupType> availableObjects = new List<PickupType>();
		foreach (PickupType obj in System.Enum.GetValues(typeof(PickupType)))
		{
			if (System.Array.FindIndex(notObjs, (x) => x == obj) == -1)
				availableObjects.Add(obj);
		}
		
		return PickRandomItem(availableObjects.ToArray());
	}

	public static LocationType PickRandomRoom()
	{
		List<LocationType> roomLocations = new List<LocationType>();
		foreach(LocationType roomLocation in  System.Enum.GetValues(typeof(LocationType)))
		{
			roomLocations.Add(roomLocation);
		}

		return PickRandomRoomFromList(roomLocations.ToArray());
	}

	public static LocationType PickRandomRoomFromList(params LocationType [] objs)
	{
		Debug.Log ("PickRandomRoom.Length "+objs.Length);
		// range is from 1 snce we do not want to pick None
		return objs[Random.Range(0, objs.Length)];
	}
	
	public static LocationType PickRandomInverseRoom(params LocationType [] notObjs)
	{
		Debug.Log ("PickRandomInverseRoom.Length "+notObjs.Length);
		List<LocationType> availableObjects = new List<LocationType>();
		foreach (LocationType obj in System.Enum.GetValues(typeof(LocationType)))
		{
			if (System.Array.FindIndex(notObjs, (x) => x == obj) == -1) // can't find the Room in the notList
				availableObjects.Add(obj);
		}
		
		return PickRandomRoomFromList(availableObjects.ToArray());
	}


//	public ItemType FindRandomOpposite(ItemType itemType)
//	{
//		switch(itemType)
//		{
//			case ItemType.BoobyTrap:
//			return ItemType.SomethingUseful;
//			break;
//			case ItemType.FakeKnife:
//			return PickRandomInverseItem(ItemType.FakeKnife, ItemType.SomeKnife, ItemType.SomethingUseful);
//			break;
//			case ItemType.RealKnife:
//			return  PickRandomInverseItem(ItemType.RealKnife, ItemType.SomeKnife, ItemType.SomethingUseful);
//			break;
//			case ItemType.GasMask:
//			return  PickRandomInverseItem(ItemType.GasMask, ItemType.SomethingUseful);
//			break;
//			case ItemType.SomeKnife:
//			return  PickRandomInverseItem(ItemType.SomeKnife, ItemType.FakeKnife, ItemType.RealKnife);
//			break;
//			case ItemType.SomethingUseful:
//			return  ItemType.BoobyTrap;
//			break;
//			default:
//			Debug.LogError("Could not find reandom inverse/opposite item of "+itemType);
//			return  ItemType.None;
//			break;
//		}
//	}

	/*public InstructionInfo.MainInfo CreateRandomMainInfo(bool nextWillLie, bool nextWillRevealWhatIsAtLocation)
	{
		//		List<InstructionInfo.MainInfo.InfoType> infoTypes = new List<InstructionInfo.MainInfo.InfoType>();
		//		foreach (InstructionInfo.MainInfo.InfoType infoType in System.Enum.GetValues(typeof(InstructionInfo.MainInfo.InfoType)))
		//			infoTypes.Add (infoType);

		bool willLie = nextWillLie;//previousInfo != null ? previousInfo.passOnInfo.willLie : false;

		LocationType unexploredLocation = GetUnexploredLocation();
		
//		RoomObject ventToMention = Random.Range(0,2) == 0 ? RoomObject.CleanVent : RoomObject.RustyVent; 

		ItemType itemTypeToTell = roomItemLocations[unexploredLocation];

		if (!IsGeneralItem(itemTypeToTell) && itemTypeToTell != ItemType.BoobyTrap && Random.value < chanceToGenerliseToSomethingUseful)
		{
			itemTypeToTell = ItemType.SomethingUseful;
		}
		if ((itemTypeToTell == ItemType.RealKnife || itemTypeToTell == ItemType.FakeKnife) && Random.value < chanceToGenerliseToSomeKnife)
		{
			itemTypeToTell = ItemType.SomeKnife;
		}

		if (willLie)
		{
			if (nextWillRevealWhatIsAtLocation)//previousInfo.passOnInfo.willRevealWhatIsAtLocation)
			{
				// will reveal what is at a location, we will lie about that item
				// make the itemType a lie / opposite version

				itemTypeToTell = FindRandomOpposite(itemTypeToTell);
			}
			else
			{
				// we will reveal where an item is, wee will lie about its location


				// need to change the location to somewhere where itemTypeToTell is NOT

				if (itemTypeToTell == ItemType.BoobyTrap)
				{
					ItemType oppositeItem = FindRandomOpposite(itemTypeToTell);

					// find a room that contains an opposite item
					
					Debug.Log ("unexploredLocation"+ unexploredLocation);
					Debug.Log ("oppositeItem"+ oppositeItem);

					if (oppositeItem == ItemType.SomethingUseful)
					{
						oppositeItem = PickRandomInverseItem(ItemType.BoobyTrap, ItemType.SomethingUseful, ItemType.SomeKnife);
					}
					if (oppositeItem == ItemType.SomeKnife)
					{
						oppositeItem = PickRandomItem(ItemType.RealKnife, ItemType.FakeKnife);
					}
					
					Debug.Log ("oppositeItem specified"+ oppositeItem);

					unexploredLocation = roomItemLocations.CreateReverseLookup()[oppositeItem];
				}


			}
		}
		
		return new InstructionInfo.MainInfo()
		{
//			infoType = Random.value < chanceToGiveVentInfo ? InstructionInfo.MainInfo.InfoType.VentInfo : InstructionInfo.MainInfo.InfoType.ItemLocation,
			itemType = itemTypeToTell,
			roomObject = unexploredLocation,
			negateTruth = Random.value < chanceToNegateInfo,
//			vent = ventToMention,
//			isVentFake = willLie ? ventToMention == ventWithPoison : ventToMention != ventWithPoison
			
		};
	}
*/

//	public InstructionInfo CreateRandomInstructionInfo(InstructionInfo previousInfo)
//	{
//		InstructionInfo iInfo = new InstructionInfo();
//
//		iInfo.previousInstructionInfo = previousInfo;
//
//		iInfo.revealInfo = new InstructionInfo.RevealInfo()
//		{
//			revealPreviousIntent = previousInfo != null && Random.value < chanceToRevealPreviousIntent,
//		};
//
//		Debug.Log ("PREVIOUS INFO "+previousInfo);
//
//		iInfo.mainInfo = previousInfo != null ? previousInfo.passOnInfo.infoToTell : CreateRandomMainInfo(false, false);
//		
//		bool nextWillLie = Random.Range(0,2) == 0;
//		bool nextWillRevealWhatIsAtLocation = Random.Range(0,2) == 0;
//
//		iInfo.passOnInfo = new InstructionInfo.PassOnInfo()
//		{
//			willTellNextPlayerSomething = Random.Range(0,2) == 0,
//			willLie = nextWillLie,
//			willRevealWhatIsAtLocation = nextWillRevealWhatIsAtLocation,
//			infoToTell =  CreateRandomMainInfo(nextWillLie, nextWillRevealWhatIsAtLocation),
//		};
//
//		return iInfo;
//	}

	
	void AddPassOnInstruction()
	{
		Debug.Log("Add PassOnInstruction");
		instructionList.Add(new InstructionInfo() { instructionType = InstructionType.PassOn, });
	}
	
	void AddPositiveInstruction()
	{
		Debug.Log("Add PositiveInstruction");

		PickupType pickupItem = PickRandomInverseItem(PickupType.BoobyTrap1, PickupType.BoobyTrap2, PickupType.BoobyTrap3);

		
		Dictionary<PickupType, LocationType> whereItemsAre = roomItemLocations.CreateReverseLookup();

		LocationType roomLocation = whereItemsAre[pickupItem];
//		PickupType pickupItem = roomItemLocations[roomLocation];

		InstructionInfo.InfoPacket iPacket = new InstructionInfo.InfoPacket() {item = pickupItem, location = roomLocation};
		
		instructionList.Add(new InstructionInfo() { instructionType = InstructionType.Positive1Location, infoPacket = iPacket, });
		instructionList.Add(new InstructionInfo() { instructionType = InstructionType.Positive2Item, infoPacket = iPacket, });
	}

	IEnumerable<PickupType> GetSimilarItems(PickupType pickupItem)
	{
		switch (pickupItem)
		{
		case PickupType.BoobyTrap1:
		case PickupType.BoobyTrap2:
		case PickupType.BoobyTrap3:
			yield return PickupType.BoobyTrap1;
			yield return PickupType.BoobyTrap2;
			yield return PickupType.BoobyTrap3;
			break;
		case PickupType.FakeKnife:
			yield return PickupType.FakeKnife;
			break;
		case PickupType.RealKnife1:
		case PickupType.RealKnife2:
			yield return PickupType.RealKnife1;
			yield return PickupType.RealKnife2;
			break;
		case PickupType.GasMask1:
		case PickupType.GasMask2:
			yield return PickupType.GasMask1;
			yield return PickupType.GasMask2;
			break;
		case PickupType.GasTrap:
			yield return PickupType.GasTrap;
			break;
		}
	}

	void AddNegativeInstruction()
	{
		Debug.Log("Add NegativeInstruction");
		LocationType roomLocation = PickRandomRoom();
		PickupType pickupItem = roomItemLocations[roomLocation];

		List<PickupType> similarItems = new List<PickupType>(GetSimilarItems(pickupItem));

//		Dictionary<PickupType, RoomLocation> reverseItemLookup = roomItemLocations.CreateReverseLookup(

		Dictionary<PickupType, LocationType> whereItemsAre = roomItemLocations.CreateReverseLookup();
		
		LocationType notLocation = PickRandomInverseRoom(similarItems.ConvertAll((i) => whereItemsAre[i]).ToArray());

//		LocationType notLocation = PickRandomInverseRoom(roomLocation);

		InstructionInfo.InfoPacket iPacket = new InstructionInfo.InfoPacket() {item = pickupItem, location = notLocation};
		
		instructionList.Add(new InstructionInfo() { instructionType = InstructionType.Negative1Both, infoPacket = iPacket, });
		instructionList.Add(new InstructionInfo() { instructionType = InstructionType.Negative2Both, infoPacket = iPacket, });
//		instructionList.Add(new InstructionInfo() { instructionType = InstructionType.PassOn, });
	}

	void AddCriticalInstruction()
	{

		InstructionInfo criticalWarning = new InstructionInfo() { instructionType = InstructionType.CriticalWarning, };
		instructionList.Add(criticalWarning);

		int sizeBeforeAdd = instructionList.Count;
		
		AddNonCriticalInstruction();

		int instructionsBeforeReveal = instructionList.Count - sizeBeforeAdd;

		criticalWarning.foreWarning = instructionsBeforeReveal + 1;

		LocationType roomLocation = PickRandomRoom();
		PickupType pickupItem = roomItemLocations[roomLocation];
		InstructionInfo.InfoPacket iPacket = new InstructionInfo.InfoPacket() {item = pickupItem, location = roomLocation};

		instructionList.Add(new InstructionInfo() { instructionType = InstructionType.CriticalReveal, infoPacket = iPacket, });
		criticalWarning.infoPacket = iPacket;

	}

	public PickupType TakeObjectFrom(LocationType location)
	{
		unexploredLocations.Remove(location);
		return roomItemLocations[location];
	}

	bool readyForInstruction = false;

	public void NextInstructions()
	{
		if (state.CurrentState == GameState.GiveHeadphone)
		{
			IncrementInstruction();
			state.ChangeState(GameState.Instructing);
		}
	}
	
	void GiveHeadphones(GameState oldState, GameState newState)
	{
		Debug.Log ("GameController::GiveHeadphones");

//		instructionList.Add (CreateRandomInstructionInfo(instructionList.Count == 0 ? null : instructionList[instructionList.Count - 1]));



	}

	void ResetCriticalCounter()
	{
		criticalInfoCounter = Random.Range(10,16);
	}

	void AddNonCriticalInstruction()
	{
		float randomValue = Random.value;
		if (randomValue < 0.1f)
		{
			// 10% passon
			
			AddPassOnInstruction();
		}
		else if (randomValue < 0.35f)
		{
			// 25% positive
			
			AddPositiveInstruction();
		}
		else
		{
			// 65% negative info
			
			AddNegativeInstruction();
		}
	}

	void IncrementInstruction()
	{
		Debug.Log("IncrementInstruction");
		instructionList.RemoveAt(0);

		// append new instructions

		if (instructionList.Count < 2)
		{
			// add new instruction(s)


			if (criticalInfoCounter == 0)
			{
				ResetCriticalCounter();

				AddCriticalInstruction();
			}
			else
			{
				criticalInfoCounter -= 1;
				AddNonCriticalInstruction();
			}
		}
	}

	void Instructing(GameState oldState, GameState newState)
	{
		Debug.Log ("GameController::Instructing");

		SayCurrentInstruction();
		state.ChangeState(GameState.GiveHeadphone);
	}


	public void SayCurrentInstruction()
	{
		InstructionInfo ii = instructionList[0];
		Debug.Log ("info type "+ii.instructionType);
		string s = ii.CreateString();
		Debug.Log ("SAYING "+s);
		Debug.Log ("---");
		UIManager.instance.TextInfo.text = s;
		VoiceSpeaker.instance.Talk (s);
	}

	public bool showDebugOutput = false;

	void OnGUI()
	{
		if (showDebugOutput)
		{
			GUILayout.Label("");
			foreach (LocationType roomObject in roomItemLocations.Keys)
			{
				GUILayout.Label(roomObject +"\t <- "+roomItemLocations[roomObject]);
			}

			foreach (InstructionInfo ii in instructionList)
			{
				GUILayout.Label(ii.instructionType.ToString());
			}
			GUILayout.Label(criticalInfoCounter.ToString());
		}
		
		UIManager.instance.TextInfo.gameObject.SetActive(showDebugOutput);
	}


	
	public static string GetGenericItemName(PickupType pickupType)
	{
		switch (pickupType)
		{
		case PickupType.BoobyTrap1:
		case PickupType.BoobyTrap2:
		case PickupType.BoobyTrap3:
			return "booby trap";
		case PickupType.FakeKnife:
		case PickupType.RealKnife1:
		case PickupType.RealKnife2:
			return "knife";
		case PickupType.GasMask1:
		case PickupType.GasMask2:
			return "gas mask";
		case PickupType.GasTrap:
			return "gas trap";
		}
		return "a non item";
	}
	
	public void PlayerActivatedItem(Player player)
	{
		Debug.Log ("GameController::PlayerActivatedItem "+player.name);
	
		if (
			player.ItemsOwned == PickupType.RealKnife1 || 
			player.ItemsOwned == PickupType.RealKnife2 || 
			player.ItemsOwned == PickupType.FakeKnife)
		{

			// find nearest player
			Debug.Log ("Looking for closest player to "+player.name);
			
			float minDist = float.PositiveInfinity;
			Player closestPlayer = null;
			foreach (Player otherPlayer in PlayerController.instance.Players)
			{
				if (otherPlayer == player)
					continue;

				float dist = Vector2.Distance(otherPlayer.transform.position, player.transform.position);
				Debug.Log ("Distance "+ dist);
				if (dist < minDist)
				{
					Debug.Log ("Setting closest player to "+ otherPlayer.name+" ("+ dist+")");
					closestPlayer = otherPlayer;
					minDist = dist;
				}
			}

			if (minDist <= GameController.instance.knifeRange)
			{
				if (player.ItemsOwned == PickupType.RealKnife1 || 
					player.ItemsOwned == PickupType.RealKnife2)
				{
					UIManager.instance.CreateObjectPickupAnimation (player.transform.position, "Stab!");
					UIManager.instance.CreateObjectPickupAnimation (closestPlayer.transform.position, "Death");
					closestPlayer.KillPlayer();

					player.DropItem();

				}
				if (player.ItemsOwned == PickupType.FakeKnife)
				{
					UIManager.instance.CreateObjectPickupAnimation (player.transform.position, "Fake knife breaks!");
					UIManager.instance.CreateObjectPickupAnimation (closestPlayer.transform.position, "Survives!");
					player.DropItem();
				}
			}

		}
	
	
	}

	public void PlayerActivatedLocation(Player player, LocationType roomLocation)
	{

		Debug.Log ("GameController::PlayerActivatedLocation "+player.name + " at " + roomLocation);


		 
		if (roomItemLocations[roomLocation] == PickupType.GasTrap)
		{

			GameController.instance.TriggerGasTrap();
			UIManager.instance.CreateObjectPickupAnimation(player.transform.position, "Gas trap!");

			
			return;
		}
		if (roomItemLocations[roomLocation] == PickupType.BoobyTrap1 || 
		    roomItemLocations[roomLocation] == PickupType.BoobyTrap2 || 
		    roomItemLocations[roomLocation] == PickupType.BoobyTrap3)
		{
			UIManager.instance.CreateObjectPickupAnimation(player.transform.position, "Booby trap!");
			player.KillPlayer();
			
			return;
		}

		if  (unexploredLocations.Contains(roomLocation))
		{
			// TODO display item picked up
			if (roomItemLocations[roomLocation] == PickupType.FakeKnife  || 
			    roomItemLocations[roomLocation] == PickupType.RealKnife1 || 
			    roomItemLocations[roomLocation] == PickupType.RealKnife2)
			{
				if (player.ItemsOwned == PickupType.None)
				{
					player.ItemsOwned = TakeObjectFrom(roomLocation);
					UIManager.instance.CreateObjectPickupAnimation(player.transform.position, GetGenericItemName(player.ItemsOwned)+"!");
				}
				else
				{
					UIManager.instance.CreateObjectPickupAnimation(player.transform.position, "Already have a "+GetGenericItemName(player.ItemsOwned)+"!");
				}
			}
			else // ... gas mask
			{
				if (player.hasGasMask == false)
				{
					TakeObjectFrom(roomLocation);
					player.hasGasMask = true;
					UIManager.instance.CreateObjectPickupAnimation(player.transform.position, GetGenericItemName(PickupType.GasMask1)+"!");
				}
				else
				{
					UIManager.instance.CreateObjectPickupAnimation(player.transform.position, "Already have a "+GetGenericItemName(player.ItemsOwned)+"!");
				}
			}

//				UIManager.instance.CreateObjectPickupAnimation(player.transform.position, GetGenericItemName(player.ItemsOwned)+"!");
		}
		else
		{
			UIManager.instance.CreateObjectPickupAnimation(player.transform.position, "Nothing here");
		}


//		else if (roomObject == RoomObject.CleanVent || roomObject == RoomObject.RustyVent )
//		{
//			UIManager.instance.CreateObjectPickupAnimation (player.transform.position, "vent did nothing");
//			return;
//		}
//		
//		if (!unexploredLocations.Contains(roomObject))
//		{
//			Debug.Log (roomObject + " has already been explored");
//			return;
//		}
//
//		unexploredLocations.Remove(roomObject);
//
//		if (roomItemLocations[roomObject] != ItemType.BoobyTrap)
//		{
//			UIManager.instance.CreateObjectPickupAnimation (player.transform.position,"Object Picked Up");
//		}
//		else
//		{
//			// TODO display item NOT picked up
//			UIManager.instance.CreateObjectPickupAnimation(player.transform.position, "nothing");
//		}
//
//		if (roomObject == ventWithPoison)
//		{
//			UIManager.instance.CreateObjectPickupAnimation (player.transform.position+Vector3.up*1f, "POISON GAS RELEASED!");
//			poisonVentOpen = true;
//
//			return;
//		}
//		else if (roomObject == LocationType.CleanVent || roomObject == LocationType.RustyVent)
//		{
//			UIManager.instance.CreateObjectPickupAnimation (player.transform.position, "vent did nothing");
//			return;
//		}
//		
//		if (!unexploredLocations.Contains(roomObject))
//		{
//			Debug.Log (roomObject + " has already been explored");
//			return;
//		}
//
//		unexploredLocations.Remove(roomObject);
//
//		if (roomItemLocations[roomObject] != ItemType.BoobyTrap)
//		{
//			UIManager.instance.CreateObjectPickupAnimation (player.transform.position,"Object Picked Up");
//		}
//		else
//		{
//			UIManager.instance.CreateObjectPickupAnimation (player.transform.position,"BOOBY TRAP!");
//		}
//		player.ItemsOwned = roomItemLocations[roomObject];
	}













}





































