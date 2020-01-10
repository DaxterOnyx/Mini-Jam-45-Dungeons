
using System;
using UnityEditor;
using UnityEngine;

public class Node
{
	public Rect rect;
	public string title;
	public bool isDragged;
	public bool isSelected;

	// Rect for the title of the node 
	public Rect rectID;

	// Two Rect for the unlock field (1 for the label and other for the checkbox)
	public Rect rectUnlockLabel;
	public Rect rectUnlocked;

	// Two Rect for the cost field (1 for the label and other for the text field)
	public Rect rectNameLabel;
	public Rect rectName;

	public ConnectionPoint inPoint;
	public ConnectionPoint outPoint;

	public GUIStyle style;
	public GUIStyle defaultNodeStyle;
	public GUIStyle selectedNodeStyle;

	// GUI Style for the title
	public GUIStyle styleID;

	// GUI Style for the fields
	public GUIStyle styleField;

	public System.Action<Node> OnRemoveNode;

	// Skill linked with the node
	public Skill skill;

	// Bool for checking if the node is whether unlocked or not
	private bool unlocked = false;

	// StringBuilder to create the node's title
	private System.Text.StringBuilder nodeTitle;
	private static int index;

	public Node(Vector2 position, float width, float height, GUIStyle nodeStyle,
		GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle,
		System.Action<ConnectionPoint> OnClickInPoint, System.Action<ConnectionPoint> OnClickOutPoint,
		System.Action<Node> OnClickRemoveNode, int id, bool unlocked, string name, int[] dependencies)
	{
		rect = new Rect(position.x, position.y, width, height);
		skill.position = rect.position;

		style = nodeStyle;

		inPoint = new ConnectionPoint(this, ConnectionPointType.In,
			inPointStyle, OnClickInPoint);

		outPoint = new ConnectionPoint(this, ConnectionPointType.Out,
			outPointStyle, OnClickOutPoint);

		defaultNodeStyle = nodeStyle;
		selectedNodeStyle = selectedStyle;
		OnRemoveNode = OnClickRemoveNode;

		// Create new Rect and GUIStyle for our title and custom fields
		float rowHeight = height / 7;

		rectID = new Rect(position.x, position.y + rowHeight, width, rowHeight);
		styleID = new GUIStyle();
		styleID.alignment = TextAnchor.UpperCenter;

		rectUnlocked = new Rect(position.x + width / 2,
			position.y + 3 * rowHeight, width / 2, rowHeight);

		rectUnlockLabel = new Rect(position.x,
			position.y + 3 * rowHeight, width / 2, rowHeight);

		styleField = new GUIStyle();
		styleField.alignment = TextAnchor.UpperRight;

		rectNameLabel = new Rect(position.x, position.y + 4 * rowHeight, width / 2, rowHeight);

		rectName = new Rect(position.x + width / 2,	position.y + 4 * rowHeight, width / 2 - 10, rowHeight);

		this.unlocked = unlocked;

		// We create the skill with current node info
		skill = new Skill
		{
			id = id,
			unlocked = unlocked,
			name = name,
			dependencies = dependencies
		};

		// Create string with ID info
		nodeTitle = new System.Text.StringBuilder();
		nodeTitle.Append("ID: ");
		nodeTitle.Append(id);

	}

	public Node(Vector2 position, float width, float height, GUIStyle nodeStyle,
		GUIStyle selectedStyle, GUIStyle inPointStyle, GUIStyle outPointStyle,
		System.Action<ConnectionPoint> OnClickInPoint, System.Action<ConnectionPoint> OnClickOutPoint,
		System.Action<Node> OnClickRemoveNode) : this(position, width, height, nodeStyle, selectedStyle, inPointStyle, outPointStyle, OnClickInPoint, OnClickOutPoint, OnClickRemoveNode, index++, false, "node", null) { }

	public void Drag(Vector2 delta)
	{
		rect.position += delta;
		skill.position = rect.position;
		rectID.position += delta;
		rectUnlocked.position += delta;
		rectUnlockLabel.position += delta;
		rectName.position += delta;
		rectNameLabel.position += delta;
	}

	public void MoveTo(Vector2 pos)
	{
		rect.position = pos;
		skill.position = rect.position;
		rectID.position = pos;
		rectUnlocked.position = pos;
		rectUnlockLabel.position = pos;
		rectName.position = pos;
		rectNameLabel.position = pos;
	}

	public void Draw()
	{
		inPoint.Draw();
		outPoint.Draw();
		GUI.Box(rect, title, style);

		// Print the title
		GUI.Label(rectID, nodeTitle.ToString(), styleID);

		// Print the unlock field
		GUI.Label(rectUnlockLabel, "Unlocked: ", styleField);
		if (GUI.Toggle(rectUnlocked, unlocked, ""))
			unlocked = true;
		else
			unlocked = false;

		skill.unlocked = unlocked;

		// Print the cost field
		GUI.Label(rectNameLabel, "Name: ", styleField);
		skill.name = GUI.TextField(rectName, skill.name);
	}

	public bool ProcessEvents(Event e)
	{
		switch (e.type) {
			case EventType.MouseDown:
				if (e.button == 0) {
					if (rect.Contains(e.mousePosition)) {
						isDragged = true;
						GUI.changed = true;
						isSelected = true;
						style = selectedNodeStyle;
					} else {
						GUI.changed = true;
						isSelected = false;
						style = defaultNodeStyle;
					}
				}

				if (e.button == 1 && isSelected && rect.Contains(e.mousePosition)) {
					ProcessContextMenu();
					e.Use();
				}
				break;

			case EventType.MouseUp:
				isDragged = false;
				break;

			case EventType.MouseDrag:
				if (e.button == 0 && isDragged) {
					Drag(e.delta);
					e.Use();
					return true;
				}
				break;
		}

		return false;
	}

	private void ProcessContextMenu()
	{
		GenericMenu genericMenu = new GenericMenu();
		genericMenu.AddItem(new GUIContent("Remove node"), false, OnClickRemoveNode);
		genericMenu.ShowAsContext();
	}

	private void OnClickRemoveNode()
	{
		OnRemoveNode?.Invoke(this);
	}
}

public enum ConnectionPointType { In, Out }

public class ConnectionPoint
{
	public Rect rect;

	public ConnectionPointType type;

	public Node node;

	public GUIStyle style;

	public System.Action<ConnectionPoint> OnClickConnectionPoint;

	public ConnectionPoint(Node node, ConnectionPointType type, GUIStyle style, System.Action<ConnectionPoint> OnClickConnectionPoint)
	{
		this.node = node;
		this.type = type;
		this.style = style;
		this.OnClickConnectionPoint = OnClickConnectionPoint;
		rect = new Rect(0, 0, 10f, 20f);
	}

	public void Draw()
	{
		rect.y = node.rect.y + (node.rect.height * 0.5f) - rect.height * 0.5f;

		switch (type) {
			case ConnectionPointType.In:
				rect.x = node.rect.x - rect.width + 8f;
				break;

			case ConnectionPointType.Out:
				rect.x = node.rect.x + node.rect.width - 8f;
				break;
		}

		if (GUI.Button(rect, "", style)) {
			OnClickConnectionPoint?.Invoke(this);
		}
	}
}

public class Connection
{
	public ConnectionPoint inPoint;
	public ConnectionPoint outPoint;
	public System.Action<Connection> OnClickRemoveConnection;

	public Connection(ConnectionPoint inPoint, ConnectionPoint outPoint, System.Action<Connection> OnClickRemoveConnection)
	{
		this.inPoint = inPoint;
		this.outPoint = outPoint;
		this.OnClickRemoveConnection = OnClickRemoveConnection;
	}

	public void Draw()
	{
		Handles.DrawBezier(
			inPoint.rect.center,
			outPoint.rect.center,
			inPoint.rect.center + Vector2.left * 50f,
			outPoint.rect.center - Vector2.left * 50f,
			Color.white,
			null,
			2f
		);

		if (Handles.Button((inPoint.rect.center + outPoint.rect.center) * 0.5f, Quaternion.identity, 4, 8, Handles.RectangleCap)) {
			OnClickRemoveConnection?.Invoke(this);
		}
	}
}