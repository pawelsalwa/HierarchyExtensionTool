using System;

namespace HierarchyExtensionTool
{
	/// <summary>
	/// Add this attribute to a method in MonoBehaviour and button for it will appear in Hierarchy window.
	/// </summary>
	[AttributeUsage(AttributeTargets.Method)]
	public class HierarchyButtonAttribute : Attribute
	{
		
	}
}