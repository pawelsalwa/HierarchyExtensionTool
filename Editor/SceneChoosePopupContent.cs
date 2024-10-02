using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using UnityEngine.SceneManagement;

namespace HierarchyExtensionTool
{
	public class SceneChoosePopupContent : PopupWindowContent
	{
		private const string loadAsSinglePropertyName = "HierarchyExtensionTool.LoadAsSingle";
		
		private bool loadAsSingle = false;
		private List<SceneAsset> sceneAssets;
		private Vector2 scrollPosition;

		public override void OnGUI(Rect rect)
		{
			loadAsSingle = GUILayout.Toggle(loadAsSingle, "load as single");
			EditorPrefs.SetBool(loadAsSinglePropertyName, loadAsSingle);
			DrawSceneButton();
		}

		private void DrawSceneButton()
		{
			scrollPosition = GUILayout.BeginScrollView(scrollPosition, false, true);
			foreach (SceneAsset sceneAsset in sceneAssets)
			{
				if (!GUILayout.Button(sceneAsset.name)) continue;
				var scenes = new List<Scene>();
				for (var i = 0; i < SceneManager.sceneCount; i++) 
					scenes.Add(SceneManager.GetSceneAt(i));

				var proceed = true;
				if (loadAsSingle) proceed = EditorSceneManager.SaveCurrentModifiedScenesIfUserWantsTo();
				if (!proceed) return;
				string path = AssetDatabase.GetAssetPath(sceneAsset);
				EditorSceneManager.OpenScene(path, loadAsSingle ? OpenSceneMode.Single : OpenSceneMode.Additive);
				editorWindow.Close();
			}

			GUILayout.EndScrollView();
		}

		public override void OnOpen()
		{
			loadAsSingle = EditorPrefs.GetBool(loadAsSinglePropertyName, loadAsSingle);
			
			string[] guids = AssetDatabase.FindAssets("t:scene", new[] {"Assets"});
			sceneAssets = guids.Select(GetAssetFromGuid).ToList();
			return;

			SceneAsset GetAssetFromGuid(string guid) => AssetDatabase.LoadAssetAtPath<SceneAsset>(AssetDatabase.GUIDToAssetPath(guid));
		}

		public override Vector2 GetWindowSize()
		{
			float height = Mathf.Max(200f, Mathf.Min(sceneAssets.Count * EditorGUIUtility.singleLineHeight, 400f));
			return new Vector2(base.GetWindowSize().x, height);
		}
	}
}