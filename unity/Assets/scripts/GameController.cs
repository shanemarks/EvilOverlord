using UnityEngine;
using System.Collections.Generic;





public enum ItemType {None, SomethingUseful, SomeKnife, BoobyTrap, RealKnife, FakeKnife, GasMask}

public enum RoomObject {RedBed, GreenBed, Sink, Toilet, Shelf, Crate, WallLamp, CleanVent, RustyVent}

//public enum VentObject {}


public class GameController : SingletonBehaviour<GameController> 
{
	
	Dictionary<RoomObject, ItemType> roomItemLocations;

	List<RoomObject> unexploredLocations;

	List<InstructionInfo> instructionList = new List<InstructionInfo>();

	RoomObject ventWithPoison;

	public RoomObject VentWithPoison { get { return ventWithPoison; } }
	
	bool poisonVentOpen = false;
	public bool PoisonVentOpen { get { return poisonVentOpen; } }

	static bool IsGeneralItem(ItemType itemType)
	{
		if (itemType == ItemType.None)
			return true;
		if (itemType == ItemType.SomethingUseful)
			return true;
		if (itemType == ItemType.SomeKnife)
			return true;

		return false;
	}

	static bool CanHideInRoomObject(RoomObject roomObject)
	{
		if (roomObject == RoomObject.CleanVent)
			return false;
		if (roomObject == RoomObject.RustyVent)
			return false;

		return true;
	}

	void SetupRoomObjects()
	{
		roomItemLocations = new Dictionary<RoomObject, ItemType>();

		List<RoomObject> roomObjects = new List<RoomObject>();
		foreach(RoomObject roomObject in  System.Enum.GetValues(typeof(RoomObject)))
		{
			if (CanHideInRoomObject(roomObject))
			{
				roomObjects.Add(roomObject);
			}
		}


		List<ItemType> roomItems = new List<ItemType>();

		foreach (ItemType itemType in System.Enum.GetValues(typeof(ItemType)))
		{
			if (IsGeneralItem(itemType))
				continue;

			roomItems.Add (itemType);
		}

		while (roomItems.Count < roomObjects.Count)
			roomItems.Add(ItemType.BoobyTrap);

		// add random itoms to objects
		foreach (RoomObject roomObject in roomObjects)
		{
			int randomIndex = Random.Range(0, roomItems.Count);
			roomItemLocations[roomObject] = roomItems[randomIndex];

			roomItems.RemoveAt(randomIndex);
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
		state.UpdateAll();
	}

	void RestartGame(GameState oldState, GameState newState)
	{
		Debug.Log ("Restarting Game");
		SetupRoomObjects();

		
		poisonVentOpen = false;

		// do anything else that is needed

		unexploredLocations = new List<RoomObject>(roomItemLocations.Keys);

		instructionList.Clear();
//		instructionList.Add (CreateRandomInstructionInfo(null));

//		foreach (RoomObject roomObject in roomItemLocations.Keys)
//			unexploredLocations.Add(roomObject);

		ventWithPoison = Random.Range(0,2) == 0 ? RoomObject.CleanVent : RoomObject.RustyVent; 
		state.ChangeState(GameState.GiveHeadphone);
	}


	RoomObject GetUnexploredLocation()
	{
		RoomObject unexploredLocation = unexploredLocations[Random.Range(0, unexploredLocations.Count)];
		return unexploredLocation;
	}

	
	public float chanceToRevealPreviousIntent = 0.3f;
	//	public float chanceToGiveVentInfo = 0.15f;
	public float chanceToNegateInfo = 0.3f;


	public float chanceToGenerliseToSomethingUseful = 0.2f;

	public float chanceToGenerliseToSomeKnife = 0.5f;
	
	public static ItemType PickRandomItem(params ItemType [] objs)
	{
		// range is from 1 snce we do not want to pick None
		return objs[Random.Range(1, objs.Length)];
	}
	
	public static ItemType PickRandomInverseItem(params ItemType [] notObjs)
	{
		List<ItemType> availableObjects = new List<ItemType>();
		foreach (ItemType obj in System.Enum.GetValues(typeof(ItemType)))
		{
			if (System.Array.FindIndex(notObjs, (x) => x == obj) == -1)
				availableObjects.Add(obj);
		}
		
		return PickRandomItem(availableObjects.ToArray());
	}


	public ItemType FindRandomOpposite(ItemType itemType)
	{
		switch(itemType)
		{
			case ItemType.BoobyTrap:
			return ItemType.SomethingUseful;
			break;
			case ItemType.FakeKnife:
			return PickRandomInverseItem(ItemType.FakeKnife, ItemType.SomeKnife, ItemType.SomethingUseful);
			break;
			case ItemType.RealKnife:
			return  PickRandomInverseItem(ItemType.RealKnife, ItemType.SomeKnife, ItemType.SomethingUseful);
			break;
			case ItemType.GasMask:
			return  PickRandomInverseItem(ItemType.GasMask, ItemType.SomethingUseful);
			break;
			case ItemType.SomeKnife:
			return  PickRandomInverseItem(ItemType.SomeKnife, ItemType.FakeKnife, ItemType.RealKnife);
			break;
			case ItemType.SomethingUseful:
			return  ItemType.BoobyTrap;
			break;
			default:
			Debug.LogError("Could not find reandom inverse/opposite item of "+itemType);
			return  ItemType.None;
			break;
		}
	}

	public InstructionInfo.MainInfo CreateRandomMainInfo(bool nextWillLie, bool nextWillRevealWhatIsAtLocation)
	{
		//		List<InstructionInfo.MainInfo.InfoType> infoTypes = new List<InstructionInfo.MainInfo.InfoType>();
		//		foreach (InstructionInfo.MainInfo.InfoType infoType in System.Enum.GetValues(typeof(InstructionInfo.MainInfo.InfoType)))
		//			infoTypes.Add (infoType);

		bool willLie = nextWillLie;//previousInfo != null ? previousInfo.passOnInfo.willLie : false;

		RoomObject unexploredLocation = GetUnexploredLocation();
		
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

	public InstructionInfo CreateRandomInstructionInfo(InstructionInfo previousInfo)
	{
		InstructionInfo iInfo = new InstructionInfo();

		iInfo.previousInstructionInfo = previousInfo;

		iInfo.revealInfo = new InstructionInfo.RevealInfo()
		{
			revealPreviousIntent = previousInfo != null && Random.value < chanceToRevealPreviousIntent,
		};

		Debug.Log ("PREVIOUS INFO "+previousInfo);

		iInfo.mainInfo = previousInfo != null ? previousInfo.passOnInfo.infoToTell : CreateRandomMainInfo(false, false);
		
		bool nextWillLie = Random.Range(0,2) == 0;
		bool nextWillRevealWhatIsAtLocation = Random.Range(0,2) == 0;

		iInfo.passOnInfo = new InstructionInfo.PassOnInfo()
		{
			willTellNextPlayerSomething = Random.Range(0,2) == 0,
			willLie = nextWillLie,
			willRevealWhatIsAtLocation = nextWillRevealWhatIsAtLocation,
			infoToTell =  CreateRandomMainInfo(nextWillLie, nextWillRevealWhatIsAtLocation),
		};

		return iInfo;
	}


	public ItemType TakeObjectFrom(RoomObject roomObject)
	{

		return roomItemLocations[roomObject];
	}

	bool readyForInstruction = false;

	public void PlayInstructions()
	{
		if (state.CurrentState == GameState.GiveHeadphone)
			state.ChangeState(GameState.Instructing);
	}
	
	void GiveHeadphones(GameState oldState, GameState newState)
	{
		Debug.Log ("GameController::GiveHeadphones");
		instructionList.Add (CreateRandomInstructionInfo(instructionList.Count == 0 ? null : instructionList[instructionList.Count - 1]));

	}

	void Instructing(GameState oldState, GameState newState)
	{
		Debug.Log ("GameController::Instructing");


		string s = instructionList[instructionList.Count-1].CreateString();
		UIManager.instance.TextInfo.text = s;
		VoiceSpeaker.instance.Talk (s);
		state.ChangeState(GameState.GiveHeadphone);
	}


	public bool showDebugOutput = false;

	void OnGUI()
	{
		if (showDebugOutput)
		{
			GUILayout.Label("");
			foreach (RoomObject roomObject in roomItemLocations.Keys)
			{
				GUILayout.Label(roomObject +"\t <- "+roomItemLocations[roomObject]);
			}
		}
	}




	public void PlayerActivatedLocation(Player player, RoomObject roomObject)
	{

		Debug.Log ("GameController::PlayerActivatedLocation "+roomObject);

		if (!unexploredLocations.Contains(roomObject))
		{
			Debug.Log (roomObject + " has already been explored");
			return;
		}

		if (roomObject == ventWithPoison)
		{
			UIManager.instance.CreateObjectPickupAnimation (player.transform.position+Vector3.up*1f, "POISON GAS RELEASED!");
			poisonVentOpen = true;
		}
		else if (roomObject == RoomObject.CleanVent || roomObject == RoomObject.CleanVent)
		{
			UIManager.instance.CreateObjectPickupAnimation (player.transform.position, "vent did nothing");
		}

		unexploredLocations.Remove(roomObject);

		if (roomItemLocations[roomObject] != ItemType.BoobyTrap)
		{
			UIManager.instance.CreateObjectPickupAnimation (player.transform.position,"Object Picked Up");
		}
		else
		{
			UIManager.instance.CreateObjectPickupAnimation (player.transform.position,"BOOBY TRAP!");
		}
		player.ItemsOwned = roomItemLocations[roomObject];
	}













}





































