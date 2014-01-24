
using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using SimpleJSON;

public static class Serializer
{
	
	public static JSONNode SerializeToCompressedJSON<T> (T toSerialize)
	{
		return SerializeObjectToJSON(toSerialize, typeof(T)).SaveToCompressedBase64();
	}
	
	public static JSONNode SerializeToJSON<T> (T toSerialize)
	{
		return SerializeObjectToJSON(toSerialize, typeof(T));
	}
	
	static JSONNode SerializeObjectToJSON (System.Object toSerialize, System.Type type)
	{
//		Debug.Log("Serialising "+toSerialize+" -> "+type.ToString());
		if (type.IsEnum)
		{
			return toSerialize.ToString();
		}
		else if (type.IsPrimitive)
		{
			if (type == typeof(int)   ||
				type == typeof(float) ||
				type == typeof(bool))
			{
				return toSerialize.ToString();
			}
			else
			{
				Debug.LogWarning("Could not parse "+toSerialize+" ("+type+")");
			}
		}
		else if (type.IsClass || type.IsValueType)
		{
			if (toSerialize == null)
			{
				return "null";
			}
			if (type == typeof(string))
			{
				return (string)toSerialize;
			}
			else // it's just a class of some sort
			{
				System.Type iListType = GetIListType(type);
				System.Type keyType;
				System.Type valueType;
				GetIDictionaryType(type, out keyType, out valueType);
				if(iListType != null)
				{
//					Debug.Log ("Found generic IList "+iListType);
					
					JSONArray jsonList = new JSONArray();
					
					IList list = (IList)toSerialize;
					foreach(System.Object subObj in list)
					{
						jsonList[-1] = SerializeObjectToJSON(subObj, iListType);
					}
					return jsonList;
				}
				if(keyType != null)
				{
//					Debug.Log ("Found generic IDictionary "+keyType);
					
					JSONArray jsonList = new JSONArray();
					
					IDictionary dictionary = (IDictionary)toSerialize;
					foreach(DictionaryEntry keyValue in dictionary)
					{
						System.Object keyObj = keyValue.Key;
						System.Object valueObj = keyValue.Value;
						JSONArray keyValNode = new JSONArray();
						keyValNode[0] = SerializeObjectToJSON(keyObj, keyType);
						keyValNode[1] = SerializeObjectToJSON(valueObj, valueType);
						jsonList[-1] = keyValNode;
					}
					return jsonList;
				}
				else if(type.IsArray)
				{
//					Debug.Log ("Found array");
					
					JSONArray jsonArray = new JSONArray();
					
					System.Type elementType = type.GetElementType();
					System.Array array = (System.Array)toSerialize;
					
					foreach(System.Object subObj in array)
					{
						jsonArray[-1] = SerializeObjectToJSON(subObj, elementType);
					}
					return jsonArray;
				}
				else
				{
					
					FieldInfo [] fieldInfo = toSerialize.GetType().GetFields(BindingFlags.Public | BindingFlags.Instance);
					
					
					JSONClass json = new JSONClass();
					
					foreach(FieldInfo info in fieldInfo)
					{
						System.Object fieldVal = info.GetValue(toSerialize);
						System.Type fieldType = info.FieldType;
						string fieldName = info.Name;
						json[fieldName] = SerializeObjectToJSON(fieldVal, fieldType);
					}
					return json;
					
					
				}
			}
		}
		
		return null;
		
	}
	
	public static T DeserializeFromCompressedJSON <T>(string compressedJSON)
	{
		return DeserializeFromJSON<T>(JSONLazyCreator.LoadFromCompressedBase64(compressedJSON));
	}
	
	public static T DeserializeFromJSON <T>(JSONNode json)
	{
		System.Object deserialised = DeserializeObjectFromJSON(json, typeof(T));;
		
		if (deserialised == null)
		{
			Debug.LogWarning("Deserialised object of type "+typeof(T)+" is null");
		}
		return (T) deserialised;
	}
	
	static System.Object DeserializeObjectFromJSON (JSONNode node, System.Type type)
	{
		
		if (type.IsEnum)
		{
			return node.AsEnum(type);
		}
		else if (type.IsPrimitive)
		{
			if (type == typeof(int))
			{
				return node.AsInt;
			}
			else if (type == typeof(float))
			{
				return node.AsFloat;
			}
			else if (type == typeof(bool))
			{
				return node.AsBool;
			}
		}
		else if (type.IsClass || type.IsValueType) // is class 
		{
			if (node.Value == "null")
			{
				return null;
			}
			if (type == typeof(string))
			{
				return node.Value;
			}
			else // it's just a thing
			{
				
				System.Type iListType = GetIListType(type);
				System.Type keyType;
				System.Type valueType;
				GetIDictionaryType(type, out keyType, out valueType);
//				Debug.Log ("genericTypes " + string.Join(", ", genericTypes.ConvertAll((input) => input.ToString()).ToArray()));
				if(iListType != null)
				{
//					Debug.Log ("Found generic IList");
				
					IList list = (IList)System.Activator.CreateInstance(type);
					foreach(JSONNode subNode in node.AsArray)
					{
						list.Add(DeserializeObjectFromJSON(subNode, iListType));
					}
					return list;
				}
				if(keyType != null)
				{
//					Debug.Log ("Found generic IDictionary");
				
					IDictionary dictionary = (IDictionary)System.Activator.CreateInstance(type);
					foreach(JSONNode subNode in node.AsArray)
					{
						dictionary.Add(
							DeserializeObjectFromJSON(subNode[0], keyType),
							DeserializeObjectFromJSON(subNode[1], valueType));
					}
					return dictionary;
				}
				else if(type.IsArray)
				{
//					Debug.Log ("Found Array");
					System.Type elementType = type.GetElementType();
					System.Array array = System.Array.CreateInstance(elementType, node.AsArray.Count);
					for(int i = 0 ; i < node.AsArray.Count ; i++)
					{
						array.SetValue(DeserializeObjectFromJSON(node.AsArray[i], elementType), i);
					}
					return array;
				}
				else
				{
					System.Object targetObject;
					
					if (type.IsClass)
					{
						targetObject = System.Activator.CreateInstance(type);
					}
					else if (type.IsValueType)
					{
						targetObject = System.Runtime.Serialization.FormatterServices.GetUninitializedObject(type);
					}
					else
					{
						return null;
					}
					
					FieldInfo [] fieldInfo = type.GetFields(BindingFlags.Public | BindingFlags.Instance);
					
					foreach(FieldInfo info in fieldInfo)
					{
						System.Type fieldType = info.FieldType;
						string fieldName = info.Name;
//						Debug.Log("Field "+fieldName);
						
						info.SetValue(targetObject, DeserializeObjectFromJSON(node.AsObject[fieldName], fieldType));
					}
					
					return targetObject;
				}
			}
		}
		
		return null;
		
	}
	
	static System.Type GetIListType(System.Type type)
	{
		List<System.Type> interfaces = new List<System.Type>(type.GetInterfaces());
//		Debug.Log ("interfaces " + string.Join("\n", interfaces.ConvertAll((input) => input.ToString()).ToArray()));
				
		if (interfaces.Contains(typeof(IList)))
		{
//			Debug.Log("IsGenericType "+type.IsGenericType);
//			Debug.Log("IsArray "+type.IsArray);
			
			if (type.IsGenericType)
			{
				List<System.Type> genericTypes = new List<System.Type>(type.GetGenericArguments());
//				Debug.Log ("genericTypes " + string.Join("\n", genericTypes.ConvertAll((input) => input.ToString()).ToArray()));
				
				if (interfaces.Contains(typeof(IList<>).MakeGenericType(genericTypes[0])))
				{
					return genericTypes[0];
				}
			}
			
		}
		return null;
	}
	
	static void GetIDictionaryType(System.Type type, out System.Type keyType, out System.Type valueType)
	{
		List<System.Type> interfaces = new List<System.Type>(type.GetInterfaces());
//		Debug.Log ("interfaces " + string.Join("\n", interfaces.ConvertAll((input) => input.ToString()).ToArray()));
		
		if (interfaces.Contains(typeof(IDictionary)))
		{
//			Debug.Log("IsGenericType "+type.IsGenericType);
//			Debug.Log("IsArray "+type.IsArray);
			
			if (type.IsGenericType)
			{
				List<System.Type> genericTypes = new List<System.Type>(type.GetGenericArguments());
//				Debug.Log ("genericTypes " + string.Join("\n", genericTypes.ConvertAll((input) => input.ToString()).ToArray()));
				
				if (interfaces.Contains(typeof(IDictionary<,>).MakeGenericType(genericTypes[0], genericTypes[1])))
				{
					keyType = genericTypes[0];
					valueType = genericTypes[1];
					return;
				}
			}
			
		}
		keyType = null;
		valueType = null;
		return;
	}
	
}
