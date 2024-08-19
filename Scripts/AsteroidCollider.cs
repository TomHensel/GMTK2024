using Godot;
using System;

public partial class AsteroidCollider : StaticBody3D
{
	private MeshInstance3D parentNode;
	
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		parentNode = GetNode<MeshInstance3D>("..");
	}

	public void Destroy(float dTime)
	{
		parentNode.Scale = parentNode.Scale - new Vector3(dTime,dTime,dTime);

		if (parentNode.Scale.X < 0.1f)
		{
			parentNode.QueueFree();
		}
		
	}
}
