using Godot;
using System;

public partial class AsteroidHunter : Node3D
{
	public AsteroidSpawner asteroids;
	private Vector3 targetPosition = Vector3.Zero;
	private MeshInstance3D asteroid;
	[Export] private float speed = 40f;
	[Export] public BlackHoleNode blackHole;

	public float materialAmount = 0f;

	private Node3D mesh;
	private int randomIndex;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		if (Visible)
		{
			//asteroids = GetNode<AsteroidSpawner>("../Asteroids");
			GD.Print(asteroids);
			mesh = GetNode<Node3D>("Laser2");
			pickRandomAsteroid();
		}

	}

	public void pickRandomAsteroid()
	{
		randomIndex = GD.RandRange(0, asteroids.allAsteroids.Count - 1);

		if (randomIndex > 0)
		{
			targetPosition = asteroids.allAsteroids[randomIndex].Position;
			asteroid = asteroids.allAsteroids[randomIndex];
		}

		// GD.Print();
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (Visible)
		{
			float dTime = (float)delta;

			if (materialAmount >= 25f)
			{
				if (Position.DistanceTo(blackHole.Position) > blackHole.blackHoleScale/2f + 5f)
				{
					Position += Position.DirectionTo(blackHole.Position) * dTime * speed;
					mesh.LookAt(targetPosition);
				}
				else
				{
					blackHole.feed(materialAmount);
					materialAmount = 0f;
					pickRandomAsteroid();
				}

			}
			else if(IsInstanceValid(asteroid))
			{
				if (asteroid.IsQueuedForDeletion())
				{
					pickRandomAsteroid();
				}
				
				if (Position.DistanceTo(targetPosition) > 5f)
				{
					Position += Position.DirectionTo(targetPosition) * dTime * speed;
					mesh.LookAt(targetPosition);
				}
				else
				{
					float addedAmount = dTime * 2f;
					materialAmount += addedAmount;
					asteroid.Scale = asteroid.Scale - new Vector3(addedAmount,addedAmount,addedAmount);

					if (asteroid.Scale.X < 0.1f)
					{
						asteroids.allAsteroids.Remove(asteroid);
						asteroid.QueueFree();
						pickRandomAsteroid();
					}
			
				}
			}
		}
		
		
		
		
		

		
		// if (Input.IsActionPressed("Take"))
		// {
		// 	pickRandomAsteroid();
		// }
	}
}
