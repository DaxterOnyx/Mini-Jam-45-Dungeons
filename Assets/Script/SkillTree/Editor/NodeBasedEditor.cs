using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

public class NodeBasedEditor : EditorWindow
{
	private List<Node> nodes;
	private List<Connection> connections;

	private GUIStyle nodeStyle;
	private GUIStyle selectedNodeStyle;
	private GUIStyle inPointStyle;
	private GUIStyle outPointStyle;

	private ConnectionPoint selectedInPoint;
	private ConnectionPoint selectedOutPoint;

	private Vector2 offset;
	private Vector2 drag;
	public SkillTreeData MySkillTree;
	private Rect rectButtonClear = new Rect(new Vector2(0, 25), new Vector2(50, 20));
	private Rect rectButtonSave = new Rect(new Vector2(50, 25), new Vector2(50, 20));

	[MenuItem("Tools/Skill Tree Editor")]
	static NodeBasedEditor OpenWindow()
	{
		NodeBasedEditor window = GetWindow<NodeBasedEditor>();
		window.titleContent = new GUIContent("Node Based Editor");
		window.MySkillTree = ScriptableObject.CreateInstance<SkillTreeData>();

		return window;
	}

	private void OnEnable()
	{
		nodeStyle = new GUIStyle();
		nodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1.png") as Texture2D;
		nodeStyle.border = new RectOffset(12, 12, 12, 12);

		selectedNodeStyle = new GUIStyle();
		selectedNodeStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/node1 on.png") as Texture2D;
		selectedNodeStyle.border = new RectOffset(12, 12, 12, 12);

		inPointStyle = new GUIStyle();
		inPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left.png") as Texture2D;
		inPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn left on.png") as Texture2D;
		inPointStyle.border = new RectOffset(4, 4, 12, 12);

		outPointStyle = new GUIStyle();
		outPointStyle.normal.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right.png") as Texture2D;
		outPointStyle.active.background = EditorGUIUtility.Load("builtin skins/darkskin/images/btn right on.png") as Texture2D;
		outPointStyle.border = new RectOffset(4, 4, 12, 12);
	}

	private void OnGUI()
	{
		DrawGrid(20, 0.2f, Color.gray);
		DrawGrid(100, 0.4f, Color.gray);

		DrawNodes();
		DrawConnections();

		DrawConnectionLine(Event.current);
		var temp = (SkillTreeData)EditorGUILayout.ObjectField(MySkillTree, typeof(SkillTreeData), allowSceneObjects: false);
		if (temp != MySkillTree) {
			MySkillTree = temp;
			LoadNodes(temp);
		}
		DrawButtons();

		ProcessNodeEvents(Event.current);
		ProcessEvents(Event.current);


		if (GUI.changed) Repaint();
	}

	private void DrawGrid(float gridSpacing, float gridOpacity, Color gridColor)
	{
		int widthDivs = Mathf.CeilToInt(position.width / gridSpacing);
		int heightDivs = Mathf.CeilToInt(position.height / gridSpacing);

		Handles.BeginGUI();
		Handles.color = new Color(gridColor.r, gridColor.g, gridColor.b, gridOpacity);

		offset += drag * 0.5f;
		Vector3 newOffset = new Vector3(offset.x % gridSpacing, offset.y % gridSpacing, 0);

		for (int i = 0; i < widthDivs; i++) {
			Handles.DrawLine(new Vector3(gridSpacing * i, -gridSpacing, 0) + newOffset, new Vector3(gridSpacing * i, position.height, 0f) + newOffset);
		}

		for (int j = 0; j < heightDivs; j++) {
			Handles.DrawLine(new Vector3(-gridSpacing, gridSpacing * j, 0) + newOffset, new Vector3(position.width, gridSpacing * j, 0f) + newOffset);
		}

		Handles.color = Color.white;
		Handles.EndGUI();
	}

	private void DrawNodes()
	{
		if (nodes != null) {
			for (int i = 0; i < nodes.Count; i++) {
				nodes[i].Draw();
			}
		}
	}

	private void DrawConnections()
	{
		if (connections != null) {
			for (int i = 0; i < connections.Count; i++) {
				connections[i].Draw();
			}
		}
	}

	private void ProcessEvents(Event e)
	{
		drag = Vector2.zero;

		switch (e.type) {
			case EventType.MouseDown:
				if (e.button == 0) {
					ClearConnectionSelection();
				}

				if (e.button == 1) {
					ProcessContextMenu(e.mousePosition);
				}
				break;

			case EventType.MouseDrag:
				if (e.button == 0) {
					OnDrag(e.delta);
				}
				break;
		}
	}

	private void ProcessNodeEvents(Event e)
	{
		if (nodes != null) {
			for (int i = nodes.Count - 1; i >= 0; i--) {
				bool guiChanged = nodes[i].ProcessEvents(e);

				if (guiChanged) {
					GUI.changed = true;
				}
			}
		}
	}

	private void DrawConnectionLine(Event e)
	{
		if (selectedInPoint != null && selectedOutPoint == null) {
			Handles.DrawBezier(
				selectedInPoint.rect.center,
				e.mousePosition,
				selectedInPoint.rect.center + Vector2.left * 50f,
				e.mousePosition - Vector2.left * 50f,
				Color.white,
				null,
				2f
			);

			GUI.changed = true;
		}

		if (selectedOutPoint != null && selectedInPoint == null) {
			Handles.DrawBezier(
				selectedOutPoint.rect.center,
				e.mousePosition,
				selectedOutPoint.rect.center - Vector2.left * 50f,
				e.mousePosition + Vector2.left * 50f,
				Color.white,
				null,
				2f
			);

			GUI.changed = true;
		}
	}

	private void ProcessContextMenu(Vector2 mousePosition)
	{
		GenericMenu genericMenu = new GenericMenu();
		genericMenu.AddItem(new GUIContent("Add node"), false, () => OnClickAddNode(mousePosition));
		genericMenu.ShowAsContext();
	}

	private void OnDrag(Vector2 delta)
	{
		drag = delta;

		if (nodes != null) {
			for (int i = 0; i < nodes.Count; i++) {
				nodes[i].Drag(delta);
			}
		}

		GUI.changed = true;
	}

	private void OnClickAddNode(Vector2 mousePosition)
	{
		if (nodes == null) {
			nodes = new List<Node>();
		}

		// We create the node with the default info for the node.
		nodes.Add(new Node(mousePosition, 200, 100, nodeStyle, selectedNodeStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode));
	}

	private void OnClickInPoint(ConnectionPoint inPoint)
	{
		selectedInPoint = inPoint;

		if (selectedOutPoint != null) {
			if (selectedOutPoint.node != selectedInPoint.node) {
				CreateConnection();
				ClearConnectionSelection();
			} else {
				ClearConnectionSelection();
			}
		}
	}

	private void OnClickOutPoint(ConnectionPoint outPoint)
	{
		selectedOutPoint = outPoint;

		if (selectedInPoint != null) {
			if (selectedOutPoint.node != selectedInPoint.node) {
				CreateConnection();
				ClearConnectionSelection();
			} else {
				ClearConnectionSelection();
			}
		}
	}

	private void OnClickRemoveNode(Node node)
	{
		if (connections != null) {
			List<Connection> connectionsToRemove = new List<Connection>();

			for (int i = 0; i < connections.Count; i++) {
				if (connections[i].inPoint == node.inPoint || connections[i].outPoint == node.outPoint) {
					connectionsToRemove.Add(connections[i]);
				}
			}

			for (int i = 0; i < connectionsToRemove.Count; i++) {
				connections.Remove(connectionsToRemove[i]);
			}

			connectionsToRemove = null;
		}

		nodes.Remove(node);
	}

	private void OnClickRemoveConnection(Connection connection)
	{
		connections.Remove(connection);
	}

	private void CreateConnection()
	{
		if (connections == null) {
			connections = new List<Connection>();
		}

		connections.Add(new Connection(selectedInPoint, selectedOutPoint, OnClickRemoveConnection));
	}

	private void ClearConnectionSelection()
	{
		selectedInPoint = null;
		selectedOutPoint = null;
	}

	#region Buttons

	private void DrawButtons()
	{
		if (GUI.Button(rectButtonClear, "Clear"))
			ClearNodes();
		if (GUI.Button(rectButtonSave, "Save"))
			SaveSkillTree();
	}

	// Save data from the window to the skill tree
	private void SaveSkillTree()
	{
		if (nodes != null && nodes.Count > 0) {
			// We fill with as many skills as nodes we have

			//for (int i = 1; i <= nodes.Count; i++) {
			//	nodes[i].skill.id = i; 
			//}

			MySkillTree.skills = new Skill[nodes.Count];
			int[] dependencies;
			List<int> dependenciesList = new List<int>();
			List<Connection> connectionsToRemove = new List<Connection>();

			// Iterate over all of the nodes. Populating the skills with the node info
			for (int nodeIndex = 0; nodeIndex < nodes.Count; ++nodeIndex) {
				if (connections != null) {
					for (int connexionIndex = 0; connexionIndex < connections.Count; connexionIndex++) {
						if (connections[connexionIndex].inPoint == nodes[nodeIndex].inPoint) {
							bool added = false;
							for (int dependantIndex = 0; dependantIndex < nodes.Count; ++dependantIndex) {
								if (connections[connexionIndex].outPoint == nodes[dependantIndex].outPoint) {
									dependenciesList.Add(nodes[dependantIndex].skill.id);
									added = true;
									break;
								}
							}
							if (!added)
								connectionsToRemove.Add(connections[connexionIndex]);
						}
					}
				}
				dependencies = dependenciesList.ToArray();
				dependenciesList.Clear();
				MySkillTree.skills[nodeIndex] = nodes[nodeIndex].skill;
				MySkillTree.skills[nodeIndex].dependencies = dependencies;
				//MySkillTree.skills[nodeIndex].editor_position = nodes[nodeIndex].rect.position; // est-ce utile de le rajouter ici ?? #labna
			}
			System.Array.Sort(MySkillTree.skills,MySkillTree.Compare); 
			Debug.LogWarning("connection Weird" + connectionsToRemove.Count);
			foreach (var item in connectionsToRemove) {
				connections.Remove(item);
			}

			if (!UnityEditor.AssetDatabase.Contains(MySkillTree)) {
				string path = EditorUtility.SaveFilePanelInProject("Save Skill Tree", "skill tree", "asset", "Please enter a file name to save the skill tree to", "Data/Skill Trees/");
				UnityEditor.AssetDatabase.CreateAsset(MySkillTree, path);
			}

			UnityEditor.AssetDatabase.Refresh();
		}
	}

	// Function for clearing data from the editor window
	private void ClearNodes()
	{
		MySkillTree = ScriptableObject.CreateInstance<SkillTreeData>();

		if (nodes != null && nodes.Count > 0) {
			Node node;
			while (nodes.Count > 0) {
				node = nodes[0];
				OnClickRemoveNode(node);
			}
		}
	}

	private void LoadNodes(SkillTreeData skillData)
	{
		ClearNodes();
		MySkillTree = skillData;

		// Store the SkillTree as an array of Skill
		Skill[] _skillTree = skillData.skills;

		// Create nodes
		for (int i = 0; i < _skillTree.Length; ++i) {
			if (nodes == null) {
				nodes = new List<Node>();
			}

			nodes.Add(new Node(_skillTree[i].editor_position, 200, 150, nodeStyle, selectedNodeStyle, inPointStyle,
			   outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode,
			   _skillTree[i].id, _skillTree[i].unlocked, _skillTree[i].name, _skillTree[i].cost, _skillTree[i].description,
			   _skillTree[i].dependencies));
		}

		Node outNode;
		// Create connections
		for (int i = 0; i < nodes.Count; ++i) {
			for (int j = 0; j < nodes[i].skill.dependencies.Length; ++j) {
				for (int k = 0; k < nodes.Count; k++) {

					if (nodes[i].skill.dependencies[j] == nodes[k].skill.id) {
						outNode = nodes[k];
						OnClickOutPoint(outNode.outPoint);
						break;
					}
				}
				OnClickInPoint(nodes[i].inPoint);
			}
		}
	}
}
#endregion
