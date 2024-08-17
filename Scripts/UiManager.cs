using Godot;
using System;

public partial class UiManager : Control
{
	public Label pressToActionLabel;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		pressToActionLabel = GetNode<Label>("PressToActionLabel");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
	}
}
