using UnityEngine;
using UnityEditor;

[InitializeOnLoad]
public class EditorMousePosEditor
{
    static EditorMousePosEditor()
    {
        SceneView.duringSceneGui += SceneGUI;
    }

	static void SceneGUI(SceneView sceneView)
	{
		TerrainTreePlacer terrainTreePlacer = GameObject.FindGameObjectWithTag("TerrainTreePlacer").GetComponent<TerrainTreePlacer>();
        if(terrainTreePlacer.enabledPlacement)
        {
            Event cur = Event.current;


            if(cur.type == EventType.MouseDown && cur.button == 0)
            {
                Ray ray = HandleUtility.GUIPointToWorldRay(Event.current.mousePosition);
                RaycastHit hit;
                if(Physics.Raycast(ray, out hit))
                {
                    terrainTreePlacer.UseBrush(hit.point);
                }
            }

            if(cur.type == EventType.KeyDown && cur.keyCode == KeyCode.LeftShift)
            {
                terrainTreePlacer.shiftPressed = true;
            }
            else if(cur.type == EventType.KeyUp && cur.keyCode == KeyCode.LeftShift)
            {
                terrainTreePlacer.shiftPressed = false;
            }
        }
	}
}

