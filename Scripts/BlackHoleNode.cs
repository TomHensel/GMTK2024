using Godot;
using System;

public partial class BlackHoleNode : Node3D
{
	private MeshInstance3D blackHoleMesh;

	public float blackHoleScale = 1.0f;

	private Material blackHoleMat;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		blackHoleMesh = GetNode<MeshInstance3D>("BlackHoleMesh");
		blackHoleScale = blackHoleMesh.Scale.X;
		blackHoleMat =  blackHoleMesh.MaterialOverride;
		//blackHoleMat.Set("shader_parameter/spehereRadius",0.5);

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		
	}

	public void feed(float amount)
	{
		blackHoleScale += amount;
		GD.Print(blackHoleScale);
		blackHoleMesh.Scale = new Vector3(blackHoleScale + blackHoleScale/2, blackHoleScale + blackHoleScale/2, blackHoleScale + blackHoleScale/2);
		blackHoleMat.Set("shader_parameter/spehereRadius",blackHoleScale * 0.5);
	}
}
