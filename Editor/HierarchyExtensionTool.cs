using System;
using System.Reflection;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HierarchyExtensionTool
{
	internal static class HierarchyExtensionTool
	{
		[InitializeOnLoadMethod]
		private static void Init()
		{
			EditorApplication.hierarchyWindowItemOnGUI -= OnHierarchyItemDraw;
			EditorApplication.hierarchyWindowItemOnGUI += OnHierarchyItemDraw;
		}

		private static void OnHierarchyItemDraw(int instanceid, Rect selectionrect)
		{
			var go = (GameObject) EditorUtility.InstanceIDToObject(instanceid);
			if (go == null) return;
			DrawToggle(selectionrect, go);
			DrawHierarchyButton(selectionrect, go);
		}

		private static void DrawHierarchyButton(Rect rect, GameObject go)
		{
			var btnRect = new Rect {xMin = rect.xMax - 105, xMax = rect.xMax - 20, yMin = rect.yMin, yMax = rect.yMax - 1};
			(string btnName, Action btnAction) = GetHierarchyButtonMethod(go);
			if (btnAction == null) return;
			if (GUI.Button(btnRect, btnName)) btnAction();
		}

		private static (string, Action) GetHierarchyButtonMethod(GameObject go)
		{
			MonoBehaviour[] mbs = go.GetComponents<MonoBehaviour>();
			foreach (MonoBehaviour mb in mbs)
			{
				if (mb == null) continue;
				Type type = mb.GetType();
				foreach (MethodInfo mi in type.GetMethods(BindingFlags.Default | BindingFlags.Instance | BindingFlags.Public | BindingFlags.Static | BindingFlags.NonPublic))
				{
					var attribute = mi.GetCustomAttribute<HierarchyButtonAttribute>();
					if (attribute == null) continue;
					return (mi.Name, () => mi.Invoke(mb, null));
				}
			}

			return default;
		}

		private static void DrawToggle(Rect selectionrect, GameObject gameObj)
		{
			var toggleRect = new Rect {x = selectionrect.xMax - 15, y = selectionrect.yMin, width = 15, height = 15f};
			var activated = GUI.Toggle(toggleRect, gameObj.activeSelf, string.Empty);

			if (activated == gameObj.activeSelf) return;
			gameObj.SetActive(!gameObj.activeSelf);
			MarkSceneDirty(gameObj.scene);
		}

		private static void MarkSceneDirty(Scene scene)
		{
			if (Application.isPlaying) return;
			EditorSceneManager.MarkSceneDirty(scene);
			var prefabStage = PrefabStageUtility.GetCurrentPrefabStage();
			if (prefabStage != null)
				EditorSceneManager.MarkSceneDirty(prefabStage.scene);
		}
	}
}