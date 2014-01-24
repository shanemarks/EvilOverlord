using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public static class DataStructureExtentions 
{
	public static Dictionary<LookupT, T> CreateLookup <T, LookupT>(this IEnumerable<T> items, System.Func<T, LookupT> getLoopup)
	{
		Dictionary<LookupT, T> lookup = new Dictionary<LookupT, T>();
		foreach(T t in items)
		{
			lookup[getLoopup(t)] = t;
		}
		return lookup;
	}
	
	public static Dictionary<T, LookupT> CreateReverseLookup <T, LookupT>(this IEnumerable<T> items, System.Func<T, LookupT> getLoopup)
	{
		Dictionary<T, LookupT> lookup = new Dictionary<T, LookupT>();
		foreach(T t in items)
		{
			lookup[t] = getLoopup(t);
		}
		return lookup;
	}
	
	public static Dictionary<T, int> CreateIndexLookup <T>(this IEnumerable<T> items)
	{
		Dictionary<T, int> lookup = new Dictionary<T, int>();
		int i = 0;
		foreach(T t in items)
		{
			lookup[t] = i;
			i++;
		}
		return lookup;
	}

	public static Dictionary<V, K> CreateReverseLookup <K, V>(this Dictionary<K, V> lookup)
	{
		Dictionary<V, K> reverseLookup = new Dictionary<V, K>();
		foreach(KeyValuePair<K, V> pair in lookup)
		{
			reverseLookup[pair.Value] = pair.Key;
		}
		return reverseLookup;
	}
	
	public static Dictionary<V, SubK> CreateReverseLookup <K, V, SubK>(this Dictionary<K, V> lookup, System.Func<K, SubK> transformKey)
	{
		Dictionary<V, SubK> reverseLookup = new Dictionary<V, SubK>();
		foreach(KeyValuePair<K, V> pair in lookup)
		{
			reverseLookup[pair.Value] = transformKey(pair.Key);
		}
		return reverseLookup;
	}
	
	public static T First <T>(this HashSet<T> hashSet) where T : class
	{
		foreach (T t in hashSet)
		{
			return t;
		}
		return null;
	}
	
//	public static bool TrueForOneShortCircuited<T>(this List<T> list, System.Predicate<T> predicate)
//	{
//		foreach (T t in list)
//		{
//			if (predicate(t))
//				return true;
//		}
//		return false;
//	}
	
	public static int MaxIndex<T>(this List<T> list, System.Func<T, float> scoreFunc)
	{
		float max = float.MinValue;
		int index = -1;
//		foreach (T t in list)
		for (int i = 0 ; i < list.Count ; i++)
		{
			float score = scoreFunc(list[i]);
			if (score > max)
			{
				max = score;
				index = i;
			}
		}
		return index;
	}
	
	public static T Max<T>(this List<T> list, System.Func<T, float> scoreFunc) where T: class
	{
		int index = list.MaxIndex(scoreFunc);
		return index == -1 ? null : list[index];
	}
//	public static T Max<T>(this List<T> list, System.Func<T, float> scoreFunc) where T: struct
//	{
//		return list[list.MaxIndex(scoreFunc)];
//	}
	
	public static int MinIndex<T>(this List<T> list, System.Func<T, float> scoreFunc)
	{
		float min = float.MaxValue;
		int index = -1;
//		foreach (T t in list)
		for (int i = 0 ; i < list.Count ; i++)
		{
			float score = scoreFunc(list[i]);
			if (score < min)
			{
				min = score;
				index = i;
			}
		}
		return index;
	}
	public static T Min<T>(this List<T> list, System.Func<T, float> scoreFunc) where T: class
	{
		int index = list.MinIndex(scoreFunc);
		return index == -1 ? null : list[index];
	}
//	public static T Min<T>(this List<T> list, System.Func<T, float> scoreFunc) where T: struct
//	{
////		return null;
//		return list[list.MinIndex(scoreFunc)];
//	}
}

public static class RaxterUtil
{
//	public static T ParseEnum <T>(string s)
//	{
//		return (T)System.Enum.Parse(typeof(T), s);
//	}
	
	public static T AsEnum<T>(this SimpleJSON.JSONNode node)
	{
		return (T)System.Enum.Parse(typeof(T), node.Value);
	}
	
	public static System.Object AsEnum(this SimpleJSON.JSONNode node, System.Type type)
	{
		return System.Enum.Parse(type, node.Value);
	}
}