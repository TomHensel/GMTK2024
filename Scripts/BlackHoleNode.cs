using Godot;
using System;

public partial class BlackHoleNode : Node3D
{
	private MeshInstance3D blackHoleMesh;

	public float blackHoleScale = 1.0f;

	private Material blackHoleMat;

	public MeshInstance3D blackHoleFeeder;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		blackHoleFeeder = GetNode<MeshInstance3D>("BlackHoleFeeder");
		blackHoleMesh = GetNode<MeshInstance3D>("BlackHoleMesh");
		blackHoleScale = blackHoleMesh.Scale.X;
		blackHoleMat =  blackHoleMesh.MaterialOverride;
		blackHoleMat.Set("shader_parameter/objectScale", blackHoleMesh.Scale.X);
		//blackHoleMat.Set("shader_parameter/spehereRadius",0.5);

	}
	

	public void feed(float amount)
	{
		float addedAmount = amount;
		blackHoleScale += addedAmount;
		blackHoleMesh.Position -= new Vector3(0,0,addedAmount / 2f);
		//blackHoleFeeder.Position += new Vector3(0, 0, addedAmount/2f);
		// GD.Print(blackHoleFeeder.Position);
		// GD.Print(blackHoleScale);
		blackHoleMesh.Scale = new Vector3(blackHoleScale, blackHoleScale, blackHoleScale);
		blackHoleMat.Set("shader_parameter/spehereRadius",blackHoleScale * 0.25);
        blackHoleMat.Set("shader_parameter/objectScale", blackHoleMesh.Scale.X);
	}

	public float take(float takenAmount, float dTime)
	{
		float scaleMinTaken = blackHoleScale - takenAmount;
		if (scaleMinTaken > 1f)
		{
			blackHoleScale = scaleMinTaken;
			blackHoleMesh.Scale = new Vector3(blackHoleScale, blackHoleScale, blackHoleScale);
			blackHoleMesh.Position += new Vector3(0,0,takenAmount / 2f);
			blackHoleMat.Set("shader_parameter/spehereRadius",blackHoleScale * 0.25);
			blackHoleMat.Set("shader_parameter/objectScale", blackHoleMesh.Scale.X);
			
			return takenAmount - dTime*0.25f;
		}

		//scaleMinTaken = blackHoleScale - (blackHoleScale-1.0f);
		
		// blackHoleScale = scaleMinTaken;
		// blackHoleMesh.Scale = new Vector3(blackHoleScale, blackHoleScale, blackHoleScale);
		// blackHoleMesh.Position += new Vector3(0,0,takenAmount / 2f);
		// blackHoleMat.Set("shader_parameter/spehereRadius",blackHoleScale * 0.25);
		// blackHoleMat.Set("shader_parameter/objectScale", blackHoleMesh.Scale.X);
		
		return 0f;
		

	}
}
