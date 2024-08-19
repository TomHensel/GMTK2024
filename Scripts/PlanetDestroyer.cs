using Godot;
using System;

public partial class PlanetDestroyer : Node3D
{
	public MeshInstance3D planet;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Visible&& planet.Scale.X > 0)
		{
			planet.Scale -= new Vector3(1, 1, 1) * (float) delta;
		}
	}
}
