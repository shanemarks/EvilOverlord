using UnityEngine;
using System.Collections;
using System.Collections.Generic;


/// <summary>
/// Link base class.
/// Generic class to add a linking structure to game objects.
/// 
/// usage:
/// Extend a child class with then (e.g. Slide, or Engine)
/// Connected each game object to the corresponding "Next" slot in the inspector view.
/// The last element  should have an EMPTY "Next" slot
/// 
/// Add CalculateLinkPosition () to your Start function to build the links
/// The class will have the following available info:
/// 
	/// Next         = The next element, null if none
	/// Previous     = The previous element
	/// ListPosition = The position within the list
	/// IsFirst      = Is this element the first in the chain
	/// IsLast       = Is this element last in the chain
	/// GetAdjacentLink = Returns the next element, if this is called on the LAST element , it returns the previous element

/// 
/// Static variables.  Used to call the first / last element of a given child type. 
/// Due to these being static they are not inherited by the childclass, to get around this limitation, you need to pass in a Type to get the appropriate first / last element.
	/// ChildClass.First(typeof(ChildClass))  returns  The first element of the list of that type.
	/// ChildClass.All(typeof(ChildClass)) ,  returns an array of Childclass. Derived from a BetterList
/// 
/// 
/// Iterating through all element following the one specified:
/// 	foreach (Engine e in CurrentEngine) 
/// 	{
/// 		e.....
/// 	}
/// 
/// Or to iterate through all in the list :
/// 	foreach (Engine e in Engine.All(typeof(Engine))) 
/// 	{
/// 		e.....
/// 	}
/// 
/// </summary>
public class LinkBase : MonoBehaviour, IEnumerable <LinkBase>  {


	void Awake ()
	{
		if (Next != null) 
		{
			Next.Previous = this;
		}
	}

	[SerializeField] private bool _isFirst, _isLast = false;

	/// <summary>
	/// Holds the links to the next and previous  elements.
	/// </summary>
	[SerializeField] public LinkBase Next;

	[HideInInspector] public LinkBase Previous;



	/// <summary>
	/// The calculated the position in the list. 
	/// if the value is -1 then it is unknown
	/// </summary>


	private int _listPosition = -1;


	/// <summary>
	/// Gets the items position in the list.
	/// </summary>
	/// <value>The list position.</value>
	public int ListPosition 
	{
		get
		{
			return _listPosition;
		}
	}



	/// <summary>
	/// Holds the amount of links.
	/// </summary>
	private int _linkCount;

	/// <summary>
	/// Gets the link count.
	/// </summary>
	/// <value>The link count.</value>
	public int LinkCount
	{
		get
		{
			return _linkCount;
		}
	}


	/// <summary>
	/// A reference to the first link.
	/// </summary>
	private static Dictionary <System.Type, LinkBase> _first = new Dictionary<System.Type, LinkBase> ();
	public static LinkBase First (System.Type t)
	{
		return LinkBase._first[t];

	}




	/// <summary>
	/// A reference to the last link. This has been removed, why would we ever need to?
	/// </summary>
	//private static Dictionary <System.Type, LinkBase> _last = new Dictionary<System.Type, LinkBase> ();
	//public static  LinkBase Last (System.Type t)
	//{
	//		return LinkBase._last[t];	
//	}


	public static Dictionary<System.Type, BetterList<LinkBase>> _all = new Dictionary <System.Type, BetterList<LinkBase>> ();

	public static BetterList<LinkBase> All (System.Type t)
	{
		return _all [t];
	}

	/// <summary>
	/// Calculates the positoin of each link. Must be called in Start ()
	/// </summary>
	protected void CalculateLinkPosition ()
	{
		System.Type t = this.GetType ();
		if (!_all.ContainsKey (t))
		{
			_all.Add(t, new BetterList<LinkBase> () );
		}

	
		_listPosition = -1;
		if (Debug.isDebugBuild)
		{
			Debug.Log ("Calculating Link Position");
		}
			// If there is nothing previous assume this is the first item in the list
		if (Previous == null) 
		{
			if (Debug.isDebugBuild)
			{
				Debug.Log ("First Postion Found");
			}
			_listPosition= 0;
			_isFirst = true;
			LinkBase._first [this.GetType () ] = this;
			_all [t].Insert(_listPosition,this);
			return;
		}

		else
		{

			if (Debug.isDebugBuild)
			{
				Debug.Log ("Manually Searching");
			}
			// if next is null then set this as a last element, otherwise iterate backwards till you find the last element
			_listPosition = 0;
			LinkBase e = this;
			if (Debug.isDebugBuild)
			{
				Debug.Log (e);
			}

			if (e.Next == null)
			{
//				LinkBase._last[this.GetType () ] = e;
				_isLast = true;
			}
			while (e.Previous != null)
			{
			
				e = e.Previous;
				_listPosition++;
				
			}
			
			if (_listPosition +1  > _linkCount)
			{
				_linkCount = _listPosition + 1 ;

			
			}

			_all [t].Insert(_listPosition,this);
		
		}
	
	}


	/// <summary>
	/// Gets a value indicating whether this instance is first.
	/// </summary>
	/// <value><c>true</c> if this instance is first; otherwise, <c>false</c>.</value>
	public bool IsFirst
	{
		get
		{
			return _isFirst;
		}
	}
	
	/// <summary>
	/// Gets a value indicating whether this instance is last.
	/// </summary>
	/// <value><c>true</c> if this instance is last; otherwise, <c>false</c>.</value>
	public bool IsLast
	{
		get
		{
			return _isLast;
		}
	}
	


	/// <summary>
	/// gets the next elements from given instance.
	/// </summary>
	/// <returns>The next.</returns>
	/// <param name="e">E.</param>
	private  LinkBase _getNext (LinkBase e)
	{
		return e.Next;
	}

	/// <summary>
	/// gets the previous element from a given instance.
	/// </summary>
	/// <returns>The previous.</returns>
	/// <param name="e">E.</param>
	private  LinkBase _getPrevious(LinkBase e)
	{
		return e.Previous;
	}

	public LinkBase GetAdjacentLink ()
	{
		if (Debug.isDebugBuild)
		{
			Debug.Log ("Getting Adjacent Engine");
		}
		return  ((Next == null)? Previous : Next );
	}




	#region Enumeration
	public IEnumerator <LinkBase> GetEnumerator ()
	{
		LinkBase l = this;
		LinkBase temp;
		while (l != null) 
		{
			temp= l;
			l = l.Next;
			yield return temp;
		}
	}

	IEnumerator IEnumerable.GetEnumerator()
	{
	
		return GetEnumerator();
	}
	#endregion
}
