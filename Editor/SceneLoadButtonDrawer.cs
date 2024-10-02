using UnityEditor;
using UnityEngine;

namespace HierarchyExtensionTool
{
	internal class SceneLoadButtonDrawer
	{
		private static void DrawSceneNameArea(int instanceID, Rect rect)
		{
			var gameObj = (GameObject) EditorUtility.InstanceIDToObject(instanceID);
			if (gameObj != null) return;
			DrawSceneLoadButtonOnSceneRect(rect);
		}

		private static void DrawSceneLoadButtonOnSceneRect(Rect sceneNameRect)
		{
			var loadButtonRect = GetLoadButtonRect(sceneNameRect);
			var quickLoadRect = GetLoadScenePopupRect(loadButtonRect);
			var guiEnabledCache = GUI.enabled;
			GUI.enabled = !Application.isPlaying;
			if (GUI.Button(loadButtonRect, "Load...")) 
				DrawSceneLoadButton(quickLoadRect);
			GUI.enabled = guiEnabledCache;
		}

		private static void DrawSceneLoadButton(Rect sceneLoadPopupRect) => 
			PopupWindow.Show(sceneLoadPopupRect, new SceneChoosePopupContent());

		private static Rect GetLoadButtonRect(Rect sceneNameRect) => 
			new() {xMin = sceneNameRect.xMax - 64f, xMax = sceneNameRect.xMax - 11f, yMin = sceneNameRect.yMin, yMax = sceneNameRect.yMax};
		
		private static Rect GetLoadScenePopupRect(Rect sceneNameRect) => 
			new() {xMin = sceneNameRect.x - 45f, xMax = sceneNameRect.x - 3f, yMin = sceneNameRect.yMin, yMax = sceneNameRect.yMax};


		[InitializeOnLoadMethod]
		private static void Init()
		{
			EditorApplication.hierarchyWindowItemOnGUI -= DrawSceneNameArea;
			EditorApplication.hierarchyWindowItemOnGUI += DrawSceneNameArea;
		}
	}
}