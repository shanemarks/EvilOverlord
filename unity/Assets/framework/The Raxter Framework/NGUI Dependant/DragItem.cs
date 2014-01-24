using UnityEngine;
using System.Collections;

[RequireComponent(typeof(UIDragObject))]
public class DragItem : MonoBehaviour 
{
	[SerializeField]
	bool _snapToContainer;
	
	DragItemContainer _container;
	
	void OnCollision(Collision collision)
	{
		Debug.Log ("DragItem::OnCollision "+collision);
	}
	
	void OnPress (bool pressed)
	{
		
		Debug.Log ("DragItem::OnPress "+pressed+" "+Time.frameCount);
		if (pressed)
		{
		}
		else
		{
		}
	}
	
	void OnDrop (GameObject drag)
	{
		Debug.Log ("DragItem::OnDrop "+drag+" "+Time.frameCount);
	}
	
	void OnDrag (Vector2 delta)
	{
		Debug.Log ("DragItem::OnDrag "+delta+" "+Time.frameCount);
		if (_container != null)
		{
			_container.ReleaseItem(this);
		}
	}
	
	void OnContained(DragItemContainer newContainer)
	{
		Debug.Log ("DragItem::OnContained "+newContainer+" "+Time.frameCount);
		_container = newContainer;
	}
	
	void OnReleased(DragItemContainer oldContainer)
	{
		Debug.Log ("DragItem::OnReleased "+oldContainer+" "+Time.frameCount);
		_container = null;
		TweenToContainer();
	}
	
	void TweenToContainer()
	{
		if (_container != null)
		{
			TweenPosition.Begin(this.gameObject, 3, _container.transform.position);
		}
	}
	
//	public void AddToContainer(DragItemContainer container)
//	{
//		
//	}
}
