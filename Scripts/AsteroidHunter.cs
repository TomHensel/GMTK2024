using Godot;
using System;

public partial class AsteroidHunter : Node3D
{
	private AsteroidSpawner asteroids;
	private Vector3 targetPosition = Vector3.Zero;
	[Export] private float speed = 60f;

	private MeshInstance3D mesh;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		asteroids = GetNode<AsteroidSpawner>("../Asteroids");
		//GD.Print(asteroids);
		mesh = GetNode<MeshInstance3D>("MeshInstance3D");
	}

	public void pickRandomAsteroid()
	{
		int randomIndex = GD.RandRange(0, asteroids.allAsteroids.Count - 1);
		targetPosition = asteroids.allAsteroids[randomIndex].Position;
		// GD.Print();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (Position.DistanceTo(targetPosition) > 5f)
		{
			float dTime = (float)delta;
			Position += Position.DirectionTo(targetPosition) * dTime * speed;
			mesh.LookAt(targetPosition);
		}
		else
		{
			
		}

		
		if (Input.IsActionPressed("Take"))
		{
			pickRandomAsteroid();
		}
	}
}
