using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class VoiceActList : SingletonBehaviour<VoiceActList>
{
	
	public AudioClip positive1LocationSink;
	public AudioClip positive1LocationRedBed;
	public AudioClip positive1LocationGreenBed;
	public AudioClip positive1LocationToilet;
	public AudioClip positive1LocationShelf;
	public AudioClip positive1LocationGrate;
	public AudioClip positive1LocationWallLight;
	public AudioClip positive1LocationRustVent;
	public AudioClip positive1LocationCleanVent;
	
	public AudioClip positive1NextPlayerMessage;

	public static IEnumerable<AudioClip> Positive1First(LocationType location)
	{
		switch(location)
		{
		case LocationType.RedBed:
			yield return instance.positive1LocationRedBed;
        	break;
		case LocationType.GreenBed:
			yield return instance.positive1LocationGreenBed;
			break;
		case LocationType.Shelf:
			yield return instance.positive1LocationShelf;
			break;
		case LocationType.Grate:
			yield return instance.positive1LocationGrate;
			break;
		case LocationType.Sink:
			yield return instance.positive1LocationSink;
			break;
		case LocationType.Toilet:
			yield return instance.positive1LocationToilet;
			break;
		case LocationType.WallLamp:
			yield return instance.positive1LocationWallLight;
			break;
		case LocationType.RustyVent:
			yield return instance.positive1LocationRustVent;
			break;
		case LocationType.CleanVent:
			yield return instance.positive1LocationCleanVent;
			break;
		}
		yield return instance.positive1NextPlayerMessage;

	}
	
	
	public AudioClip positive1ItemKnife;
	public AudioClip positive1ItemFakeKnife;
	public AudioClip positive1ItemGasTrap;
	public AudioClip positive1ItemGasMask;
	public AudioClip positive1ItemBoobyTrap;
	
	public AudioClip positive1PlayerGivenInfo0Yellow;
	public AudioClip positive1PlayerGivenInfo1Blue;
	public AudioClip positive1PlayerGivenInfo2Green;
	public AudioClip positive1PlayerGivenInfo3Red;
	
	public AudioClip positive1PassOn;

	public static IEnumerable<AudioClip> Positive1Second(PickupType item, int playerWhoPassed)
	{
		switch (item)
		{
		case PickupType.BoobyTrap1:
		case PickupType.BoobyTrap2:
		case PickupType.BoobyTrap3:
		case PickupType.BoobyTrap4:
			yield return instance.positive1ItemBoobyTrap;
			break;
		case PickupType.FakeKnife:
			yield return instance.positive1ItemFakeKnife;
			break;
		case PickupType.RealKnife1:
		case PickupType.RealKnife2:
			yield return instance.positive1ItemKnife;
			break;
		case PickupType.GasMask:
			yield return instance.positive1ItemGasMask;
			break;
		case PickupType.GasTrap:
			yield return instance.positive1ItemGasTrap;
			break;
		}

		
		switch (playerWhoPassed)
		{
		case 0:
			yield return instance.positive1PlayerGivenInfo0Yellow;
			break;
		case 1:
			yield return instance.positive1PlayerGivenInfo1Blue;
			break;
		case 2:
			yield return instance.positive1PlayerGivenInfo2Green;
			break;
		case 3:
			yield return instance.positive1PlayerGivenInfo3Red;
			break;
		}
		
		yield return instance.positive1PassOn;
	}

	
	public AudioClip negative2LocationSink;
	public AudioClip negative2LocationRedBed;
	public AudioClip negative2LocationGreenBed;
	public AudioClip negative2LocationToilet;
	public AudioClip negative2LocationShelf;
	public AudioClip negative2LocationGrate;
	public AudioClip negative2LocationWallLight;
	public AudioClip negative2LocationRustVent;
	public AudioClip negative2LocationCleanVent;
	
	public AudioClip negative2ItemKnife;
	public AudioClip negative2ItemFakeKnife;
	public AudioClip negative2ItemGasTrap;
	public AudioClip negative2ItemGasMask;
	public AudioClip negative2ItemBoobyTrap;


	static IEnumerable<AudioClip> Negative2General(LocationType location, PickupType item)
	{
		switch(location)
		{
		case LocationType.RedBed:
			yield return instance.negative2LocationRedBed;
			break;
		case LocationType.GreenBed:
			yield return instance.negative2LocationGreenBed;
			break;
		case LocationType.Shelf:
			yield return instance.negative2LocationShelf;
			break;
		case LocationType.Grate:
			yield return instance.negative2LocationGrate;
			break;
		case LocationType.Sink:
			yield return instance.negative2LocationSink;
			break;
		case LocationType.Toilet:
			yield return instance.negative2LocationToilet;
			break;
		case LocationType.WallLamp:
			yield return instance.negative2LocationWallLight;
			break;
		case LocationType.RustyVent:
			yield return instance.negative2LocationRustVent;
			break;
		case LocationType.CleanVent:
			yield return instance.negative2LocationCleanVent;
			break;
		}

		switch (item)
		{
		case PickupType.BoobyTrap1:
		case PickupType.BoobyTrap2:
		case PickupType.BoobyTrap3:
		case PickupType.BoobyTrap4:
			yield return instance.negative2ItemBoobyTrap;
			break;
		case PickupType.FakeKnife:
			yield return instance.negative2ItemFakeKnife;
			break;
		case PickupType.RealKnife1:
		case PickupType.RealKnife2:
			yield return instance.negative2ItemKnife;
			break;
		case PickupType.GasMask:
			yield return instance.negative2ItemGasMask;
			break;
		case PickupType.GasTrap:
			yield return instance.negative2ItemGasTrap;
			break;
		}
		
	}

	
	public AudioClip negative2WillRepeatMessage;

	public static IEnumerable<AudioClip> Negative2First(LocationType location, PickupType item)
	{
		foreach(AudioClip clip in Negative2General(location, item))
			yield return clip;


		yield return instance.negative2WillRepeatMessage;
	}
	
	
	public AudioClip negative2RepeatedFromMessage0Yellow;
	public AudioClip negative2RepeatedFromMessage1Blue;
	public AudioClip negative2RepeatedFromMessage2Green;
	public AudioClip negative2RepeatedFromMessage3Red;
	
	public AudioClip negative2PassOn;

	public static IEnumerable<AudioClip> Negative2Second(LocationType location, PickupType item, int playerWhoPassed)
	{
		foreach(AudioClip clip in Negative2General(location, item))
			yield return clip;
		
		switch (playerWhoPassed)
		{
		case 0:
			yield return instance.negative2RepeatedFromMessage0Yellow;
			break;
		case 1:
			yield return instance.negative2RepeatedFromMessage1Blue;
			break;
		case 2:
			yield return instance.negative2RepeatedFromMessage2Green;
			break;
		case 3:
			yield return instance.negative2RepeatedFromMessage3Red;
			break;
		}
		yield return instance.negative2PassOn;
	}

	public AudioClip noInfo3PassOn;

	public static IEnumerable<AudioClip> NoInfo3()
	{
		yield return instance.noInfo3PassOn;
	}
	
	public AudioClip critical4InTwoTurns;
	public AudioClip critical4InThreeTurns;
	
	public AudioClip critical4RememberThisAndPassOn;

	
	public static IEnumerable<AudioClip> Critical4Warning(bool threeTurns)
	{
		yield return threeTurns ? instance.critical4InThreeTurns : instance.critical4InTwoTurns;

		yield return instance.critical4RememberThisAndPassOn;
	}
	
	public AudioClip critical4LocationSink;
	public AudioClip critical4LocationRedBed;
	public AudioClip critical4LocationGreenBed;
	public AudioClip critical4LocationToilet;
	public AudioClip critical4LocationShelf;
	public AudioClip critical4LocationGrate;
	public AudioClip critical4LocationWallLight;
	public AudioClip critical4LocationRustVent;
	public AudioClip critical4LocationCleanVent;
	
	public AudioClip critical4AnotherPlayerKnowsPassOn;

	public static IEnumerable<AudioClip> Critical4Reveal(LocationType location)
	{

		switch(location)
		{
		case LocationType.RedBed:
			yield return instance.critical4LocationRedBed;
			break;
		case LocationType.GreenBed:
			yield return instance.critical4LocationGreenBed;
			break;
		case LocationType.Shelf:
			yield return instance.critical4LocationShelf;
			break;
		case LocationType.Grate:
			yield return instance.critical4LocationGrate;
			break;
		case LocationType.Sink:
			yield return instance.critical4LocationSink;
			break;
		case LocationType.Toilet:
			yield return instance.critical4LocationToilet;
			break;
		case LocationType.WallLamp:
			yield return instance.critical4LocationWallLight;
			break;
		case LocationType.RustyVent:
			yield return instance.critical4LocationRustVent;
			break;
		case LocationType.CleanVent:
			yield return instance.critical4LocationCleanVent;
			break;
		}
		
		yield return instance.critical4AnotherPlayerKnowsPassOn;
	}
}















