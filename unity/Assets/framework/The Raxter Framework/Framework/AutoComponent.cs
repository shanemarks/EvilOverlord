using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class AutoComponent<T> where T : MonoBehaviour
{
	public AutoComponent(GameObject go)
	{
		_holder = go;
	}
	
	public void Refresh()
	{
		_component = null;
	}
	
	GameObject _holder;
	public GameObject Holder { get { return _holder; } }
	
	T _component;
	public T Get
	{
		get 
		{
			if (_holder != null && _component == null)
				_component = _holder.GetComponent<T>();
			
			return _component;
		}
	}
	
	public bool Has
	{
		get 
		{
			return Get != null;
		}
	}
}

public class AutoComponents<T> where T : MonoBehaviour
{
	public AutoComponents(GameObject go)
	{
		_holder = go;
	}
	
	public void Refresh()
	{
		_components = null;
	}
	
	GameObject _holder;
	public GameObject Holder { get { return _holder; } }
	
	public IEnumerable<T> GetAll
	{
		get 
		{
			if (!Has) yield break;
			
			foreach(T t in Get)
			{
				yield return t;
			}
		}
	}
	
	T [] _components;
	public T [] Get
	{
		get 
		{
			if (_holder != null && _components == null)
				_components = _holder.GetComponents<T>();
			
			return _components;
		}
	}
	
	public bool Has
	{
		get 
		{
			return Get != null || Get.Length != 0;
		}
	}
}
