using UnityEngine;
using System.Collections.Generic;


public enum ItemType {BoobyTrap, Knife, FakeKnife, GasMask}

public enum RoomObject {Bed, BunkBed, GasVent, FakeVent, Sink, Toilet, Rug, Crate, WallLamp}


public class GameController : MonoBehaviour 
{
	
	Dictionary<RoomObject, ItemType> roomItemLocations;

	void SetupRoomObjects()
	{
		roomItemLocations = new Dictionary<RoomObject, ItemType>();

		int numRoomObjects = System.Enum.GetValues(typeof(RoomObject)).Length;

		List<ItemType> roomItems = new List<ItemType>();

		foreach (ItemType itemType in System.Enum.GetValues(typeof(ItemType)))
		{
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


	public enum GameState {Intro, GiveHeadphone, Instructing, Finish}


	FSM<GameState> state = new FSM<GameState>();


	void Start ()
	{
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
	}


	public ItemType TakeObjectFrom(RoomObject roomObject)
	{

		return roomItemLocations[roomObject];
	}
























}





































