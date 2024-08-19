using Godot;
using Godot.Collections;

public partial class AsteroidSpawner : Node3D
{
	private Godot.Collections.Array<MeshInstance3D> asteroids = new Array<MeshInstance3D>();

	private Node3D mainNode;

	private bool firstFrame = true;

	[Export] private int amount = 100;
	[Export] private float asteroidDistance = 100;
    
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		
		
		mainNode = GetNode<Node3D>("..");
		
		foreach (Node child in GetChildren())
		{
			if (child is MeshInstance3D)
			{
				asteroids.Add((MeshInstance3D) child);
			}
		}



	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{

		if (firstFrame)
		{
			firstFrame = false;
			for (int i = 0; i < amount / 3; ++i)
			{
				foreach (MeshInstance3D asteroid in asteroids)
				{
					MeshInstance3D newAsteroid = (MeshInstance3D) asteroid.Duplicate();


					float randomScale = GD.Randf()*4f +1.5f;
					newAsteroid.Scale = new Vector3(randomScale,randomScale,randomScale);
				
					newAsteroid.Position = (new Vector3(GD.Randf() * 2f -1f,GD.Randf()* 2f -1f,GD.Randf()* 2f -1f)).Normalized() * asteroidDistance;

					mainNode.AddChild(newAsteroid);
				}
			}

		}
	}
}
