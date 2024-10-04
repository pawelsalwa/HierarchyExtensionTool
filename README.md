# HierarchyExtensionTool
Simple suite of visual tools for Unity Hierarchy Window.

Features:
- adds button for quick loading of scenes
- adds toggle to each GameObject in Hierarchy Window for easier toggling of activation
- HierarchyButtonAttribute - allows to call MonoBehaviour methods from button drawn in Hierarchy Window. 

## Installation
Add this line to your **Packages/manifest.json** file in your Unity project:
```
"com.pawelsalwa.hierarchyextensiontool": "https://github.com/pawelsalwa/HierarchyExtensionTool"
```
## Example
Example usage:
```csharp
using UnityEngine;

public class SomeBehaviour : MonoBehaviour
{
    [HierarchyButton]
    private void SomeMethodAbleToBeCalledFromHierarchyButton()
    {
        Debug.Log("hello");        
    }        
}
```