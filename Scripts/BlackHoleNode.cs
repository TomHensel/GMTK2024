using Godot;
using System;

public partial class BlackHoleNode : Node3D
{
	private MeshInstance3D blackHoleMesh;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		blackHoleMesh = GetNode<MeshInstance3D>("BlackHoleMesh");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void feed(float amount)
	{
		blackHoleMesh.Scale += new Vector3(amount, amount, amount);
	}
}
