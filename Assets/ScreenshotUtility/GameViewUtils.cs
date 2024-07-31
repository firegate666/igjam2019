#if UNITY_EDITOR
using System.Reflection;
using UnityEditor;
using UnityEngine;

public static class GameViewUtils
{
	static object gameViewSizesInstance;
	static MethodInfo getGroup;

	static GameViewUtils()
	{
		// gameViewSizesInstance  = ScriptableSingleton<GameViewSizes>.instance;
		var sizesType = typeof(Editor).Assembly.GetType("UnityEditor.GameViewSizes");
		var singleType = typeof(ScriptableSingleton<>).MakeGenericType(sizesType);
		var instanceProp = singleType.GetProperty("instance");
		getGroup = sizesType.GetMethod("GetGroup");
		gameViewSizesInstance = instanceProp.GetValue(null, null);
	}

	public static bool FindAndSetSize(int width, int height)
	{
		var sizeGroupType = GameViewSizeGroupType.Standalone;
#if UNITY_ANDROID
            sizeGroupType = GameViewSizeGroupType.Android;
#elif UNITY_IOS
		sizeGroupType = GameViewSizeGroupType.iOS;
#else
            // unexpected
#endif

		int idx = FindSize(sizeGroupType, width, height);
		if (idx != -1)
		{
			SetSize(idx);
			return true;
		}

		return false;
	}

	private static void SetSize(int index)
	{
		var gvWndType = typeof(Editor).Assembly.GetType("UnityEditor.GameView");
		var selectedSizeIndexProp = gvWndType.GetProperty("selectedSizeIndex",
			BindingFlags.Instance | BindingFlags.Public | BindingFlags.NonPublic);
		var gvWnd = EditorWindow.GetWindow(gvWndType);
		selectedSizeIndexProp.SetValue(gvWnd, index, null);
	}

	private static int FindSize(GameViewSizeGroupType sizeGroupType, int width, int height)
	{
		// goal:
		// GameViewSizes group = gameViewSizesInstance.GetGroup(sizeGroupType);
		// int sizesCount = group.GetBuiltinCount() + group.GetCustomCount();
		// iterate through the sizes via group.GetGameViewSize(int index)

		var group = GetGroup(sizeGroupType);
		var groupType = group.GetType();
		var getBuiltinCount = groupType.GetMethod("GetBuiltinCount");
		var getCustomCount = groupType.GetMethod("GetCustomCount");
		int sizesCount = (int) getBuiltinCount.Invoke(group, null) + (int) getCustomCount.Invoke(group, null);
		var getGameViewSize = groupType.GetMethod("GetGameViewSize");
		var gvsType = getGameViewSize.ReturnType;
		var widthProp = gvsType.GetProperty("width");
		var heightProp = gvsType.GetProperty("height");
		var indexValue = new object[1];
		for (int i = 0; i < sizesCount; i++)
		{
			indexValue[0] = i;
			var size = getGameViewSize.Invoke(group, indexValue);
			int sizeWidth = (int) widthProp.GetValue(size, null);
			int sizeHeight = (int) heightProp.GetValue(size, null);
			if (sizeWidth == width && sizeHeight == height)
				return i;
		}

		return -1;
	}

	static object GetGroup(GameViewSizeGroupType type)
	{
		return getGroup.Invoke(gameViewSizesInstance, new object[] {(int) type});
	}
}
#else
public static class GameViewUtils {
	public static bool FindAndSetSize(int width, int height) {
		return false;
	}
}
#endif