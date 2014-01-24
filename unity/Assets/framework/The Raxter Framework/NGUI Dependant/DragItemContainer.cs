using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class DragItemContainer : MonoBehaviour 
{
	[SerializeField]
	Transform [] containerTargets;

	[SerializeField]
	Vector3 separation = Vector3.zero;
	
	[SerializeField]
	int overflowAfter = 0;
	
	[SerializeField]
	bool reorderOnRemove = false;
	
	Dictionary<Transform, List<DragItem>> items = new Dictionary<Transform, List<DragItem>>();
	Transform containerLastAddedTo = null;
	
	void OnTriggerStay(Collider collider)
	{
		Debug.Log ("DragItemContainer::OnTriggerStay "+collider+" "+Time.frameCount);
	}
	
	void OnDrag (Vector2 delta)
	{
		Debug.Log ("DragItemContainer::OnDrag "+delta+" "+Time.frameCount);
	}
	
	void OnDrop(GameObject drag)
	{
		Debug.Log ("DragItemContainer::OnDrop "+drag+" "+Time.frameCount);
		
		DragItem dragItem = drag.GetComponent<DragItem>();
		
		if (dragItem != null)
		{
			if (containerTargets.Length == 0)
			{
				containerTargets = new Transform[] {transform};
			}
			
			if (containerLastAddedTo == null)
			{
				containerLastAddedTo = containerTargets[containerTargets.Length-1];
			}
			bool addOnNext = false;
			for (int t = 0 ; t < containerTargets.Length+1 ; t++)
			{
				if (addOnNext)
				{
					Transform toAddTo = containerTargets[t % containerTargets.Length];
					if (!items.ContainsKey(toAddTo))
					{
						items[toAddTo] = new List<DragItem>();
					}
					dragItem.transform.parent = toAddTo;
//					dragItem.transform.position = toAddTo.position + separation*items[toAddTo].Count;
					TweenPosition.Begin(dragItem.gameObject, 1, separation*items[toAddTo].Count);
					items[toAddTo].Add(dragItem);
					dragItem.SendMessage("OnContained", this, SendMessageOptions.DontRequireReceiver);
					break;
				}
				if (containerLastAddedTo == containerTargets[t])
				{
					addOnNext = true;
				}
			}
		}
	}

	public void ReleaseItem (DragItem dragItem)
	{
		foreach(Transform t in containerTargets)
		{
			if(items[t].Contains(dragItem))
			{
				items[t].Remove(dragItem);
					dragItem.SendMessage("OnReleased", this, SendMessageOptions.DontRequireReceiver);
			}
		}
	}
}
