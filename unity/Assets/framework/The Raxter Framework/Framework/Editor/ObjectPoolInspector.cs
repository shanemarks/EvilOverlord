using UnityEngine;
using UnityEditor;
using System.Collections;
using System.Collections.Generic;

[CustomEditor(typeof(ObjectPool))]
public class ObjectPoolInspector : Editor
{
	class ObjectHistory
	{
		public GameObject go;
		public int id;
		public List<ObjectPool.ObjectHistoryEvent> history;
	}
	
	List<ObjectHistory> histories = new List<ObjectHistory>();
	
	Dictionary<GameObject, bool> open = new Dictionary<GameObject, bool>();
	
	public override void OnInspectorGUI()
	{
		if (Application.isPlaying)
		{
//			EditorGUILayout.TextArea("this\nis\nsome\ntext");
			
			histories.Clear();
			foreach(GameObject gameObject in ObjectPool.instance.objectHistory.Keys)
			{
				ObjectHistory toAdd = new ObjectHistory()
				{
					go = gameObject,
					id = ObjectPool.instance.objectIds[gameObject],
					history = ObjectPool.instance.objectHistory[gameObject],
				};
				histories.Add(toAdd);
				
			}
			
			histories.Sort( (x, y) => x.id - y.id );
			
//			for(int i = 0 ; i < histories.Count ; i++)
			foreach(ObjectHistory history in histories)
			{
				if (history.go == null) continue;
				
				if (!open.ContainsKey(history.go))
				{
					open[history.go] = false;
				}
				
				open[history.go] = EditorGUILayout.Foldout(open[history.go], history.go.name);
				
				if (open[history.go])
				{
					EditorGUI.indentLevel += 1;
					foreach (ObjectPool.ObjectHistoryEvent historyEvent in history.history)
					{
						EditorGUILayout.BeginHorizontal();
						EditorGUILayout.ObjectField(history.go,typeof(GameObject), true, GUILayout.MaxWidth(200));
						EditorGUILayout.LabelField(historyEvent.poolEvent+"\t@ frame"+historyEvent.frameCount);
						EditorGUILayout.EndHorizontal();
						EditorGUILayout.TextArea(historyEvent.stacktrace);
					}
					EditorGUI.indentLevel -= 1;
				}
				
				
			}
		}
	}
}
