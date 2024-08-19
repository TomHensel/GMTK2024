using Godot;
using System;

public partial class PlanetDestroyer : Node3D
{
	public MeshInstance3D planet;
	public BlackHoleNode blackHole;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Visible&& planet.Scale.X > 0)
		{
			float dTime = (float) delta;
			planet.Scale -= new Vector3(1, 1, 1) * dTime * 0.001f;
			// player.BlackHoleFoodAmount += dTime;
			// player.changeFoodUi();
			blackHole.feed(dTime * 0.1f);
		}
	}
}
