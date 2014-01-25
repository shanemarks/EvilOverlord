using UnityEngine;
using System.Collections.Generic;





public enum ItemType {None, SomethingUseful, SomeKnife, BoobyTrap, Knife, FakeKnife, GasMask}

public enum RoomObject {Bed, BunkBed, Sink, Toilet, Rug, Crate, WallLamp}

public enum VentObject {Vent, RustyVent}


public class GameController : SingletonBehaviour<GameController> 
{
	
	Dictionary<RoomObject, ItemType> roomItemLocations;

	List<RoomObject> unexploredLocations;

	List<InstructionInfo> instructionList = new List<InstructionInfo>();

	VentObject ventWithPoison;


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

	void SetupRoomObjects()
	{
		roomItemLocations = new Dictionary<RoomObject, ItemType>();

		int numRoomObjects = System.Enum.GetValues(typeof(RoomObject)).Length;

		List<ItemType> roomItems = new List<ItemType>();

		foreach (ItemType itemType in System.Enum.GetValues(typeof(ItemType)))
		{
			if (IsGeneralItem(itemType))
				continue;

			roomItems.Add (itemType);
		}

		while (roomItems.Count < numRoomObjects)
			roomItems.Add(ItemType.BoobyTrap);

		// add random itoms to objects
		foreach (RoomObject roomObject in System.Enum.GetValues(typeof(RoomObject)))
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

		// do anything else that is needed

		unexploredLocations = new List<RoomObject>(roomItemLocations.Keys);

		instructionList.Clear();
		instructionList.Add (CreateRandomInstructionInfo(null));

//		foreach (RoomObject roomObject in roomItemLocations.Keys)
//			unexploredLocations.Add(roomObject);

		ventWithPoison = Random.Range(0,2) == 0 ? VentObject.Vent : VentObject.RustyVent; 
		state.ChangeState(GameState.GiveHeadphone);
	}


	RoomObject GetUnexploredLocation()
	{
		RoomObject unexploredLocation = unexploredLocations[Random.Range(0, unexploredLocations.Count)];
		return unexploredLocation;
	}

	
	public float chanceToRevealPreviousIntent = 0.3f;
	public float chanceToGiveVentInfo = 0.15f;
	public float chanceToNegateInfo = 0.3f;


	public InstructionInfo.MainInfo CreateRandomMainInfo(bool willLie)
	{
		//		List<InstructionInfo.MainInfo.InfoType> infoTypes = new List<InstructionInfo.MainInfo.InfoType>();
		//		foreach (InstructionInfo.MainInfo.InfoType infoType in System.Enum.GetValues(typeof(InstructionInfo.MainInfo.InfoType)))
		//			infoTypes.Add (infoType);
		
		RoomObject unexploredLocation = GetUnexploredLocation();
		
		VentObject ventToMention = Random.Range(0,2) == 0 ? VentObject.Vent : VentObject.RustyVent; 
		
		
		return new InstructionInfo.MainInfo()
		{
			infoType = Random.value < chanceToGiveVentInfo ? InstructionInfo.MainInfo.InfoType.VentInfo : InstructionInfo.MainInfo.InfoType.ItemLocation,
			itemType = roomItemLocations[unexploredLocation],
			roomObject = unexploredLocation,
			negateTruth = Random.value < chanceToNegateInfo,
			vent = ventToMention,
			isVentFake = willLie ? ventToMention == ventWithPoison : ventToMention != ventWithPoison
			
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



		iInfo.mainInfo = previousInfo != null ? previousInfo.passOnInfo.infoToTell : CreateRandomMainInfo(false);

		bool nextWillLie = Random.Range(0,2) == 0;

		iInfo.passOnInfo = new InstructionInfo.PassOnInfo()
		{
			willTellNextPlayerSomething = Random.Range(0,2) == 0,
			willLie = nextWillLie,
			willRevealWhatIsAtLocation = Random.Range(0,2) == 0,
			infoToTell =  CreateRandomMainInfo(nextWillLie),
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
		
		instructionList.Add (CreateRandomInstructionInfo(instructionList[instructionList.Count - 1]));
	}

	void Instructing(GameState oldState, GameState newState)
	{
		Debug.Log ("GameController::Instructing");
		Debug.Log (instructionList[instructionList.Count-1].CreateString());
		state.ChangeState(GameState.GiveHeadphone);
	}









	public void PlayerActivatedLocation(Player player, RoomObject roomObject)
	{

		Debug.Log ("GameController::PlayerActivatedLocation "+roomObject);

		if (!unexploredLocations.Contains(roomObject))
		{
			Debug.Log (roomObject + " has already been explored");
			return;
		}

		unexploredLocations.Remove(roomObject);

		player.ItemsOwned.Add(roomItemLocations[roomObject]);
	}













}





































