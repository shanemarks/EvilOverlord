using UnityEngine;
using System.Collections.Generic;





public enum ItemType {None, SomethingUseful, SomeKnife, BoobyTrap, Knife, FakeKnife, GasMask}

public enum RoomObject {Bed, BunkBed, Sink, Toilet, Rug, Crate, WallLamp}

public enum VentObject {VentA, VentB}


public class GameController : SingletonBehaviour<GameController> 
{
	
	Dictionary<RoomObject, ItemType> roomItemLocations;



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

//		state.ChangeState(
	}


	public ItemType TakeObjectFrom(RoomObject roomObject)
	{

		return roomItemLocations[roomObject];
	}










	public void PlayerActivatedLocation(Player player, RoomObject roomObject)
	{

		Debug.Log ("GameController::PlayerActivatedLocation "+roomObject);
	}













}





































