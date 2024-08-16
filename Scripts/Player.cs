using Godot;
using System;

public partial class Player : CharacterBody3D
{
	// Called when the node enters the scene tree for the first time.

	private Node3D camPivot;
	private Camera3D cam;

	[Export] private float mouseSpeed = 1f;
	
	private Vector3 vel = Vector3.Zero;

	private float steeringValue = 0;

	private Node3D parrentNode;
	
	
	
	public override void _Ready()
	{
		camPivot = GetNode<Node3D>("CamPivot");
		cam = GetNode<Camera3D>("CamPivot/Camera3D");
		parrentNode = GetNode<Node3D>("..");
		
		Input.MouseMode = Input.MouseModeEnum.Confined;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		// Position = parrentNode.Position;
		// parrentNode.Position = Vector3.Zero;
		
		float dTime = (float) delta;
		Vector2 input_dir = Input.GetVector("Right","Left" , "Forward", "Backward");
		
		

		//RotateZ(input_dir.X * dTime);
		//GlobalRotate();

		steeringValue = Mathf.LerpAngle(steeringValue,  Mathf.DegToRad(input_dir.X), dTime);
		
		//Rotate(cam.GlobalTransform.Basis.Z, steeringValue);
		//Rotate(cam.GlobalTransform.Basis.Z, input_dir.X * dTime);
		
		
		//Vector3 direction = camPivot.Transform.Basis * new Vector3(input_dir.X, 0f, input_dir.Y);
		Vector3 direction = cam.GlobalTransform.Basis.Z * input_dir.Y;
		//Input.GetLastMouseScreenVelocity()
		


		vel = vel.Lerp(direction * 20, dTime); 

		//vel = direction * 20f * dTime;
				
		Velocity = vel;
		
		MoveAndSlide();

		// parrentNode.Position = Position;
		// Position = Vector3.Zero;

	}

	public override void _Input(InputEvent @event)
	{ 
		base._Input(@event);
		
		if (@event is InputEventMouseMotion)
		{
			moveCamera((InputEventMouseMotion)@event);
		}
		
		if (@event.IsActionPressed("Escape"))
		{
			
			if (Input.MouseMode == Input.MouseModeEnum.Visible)
			{
				Input.MouseMode = Input.MouseModeEnum.Confined;
			}
			else
			{
				Input.MouseMode = Input.MouseModeEnum.Visible;
			}
			
		}
		
	}
	
	private void moveCamera(InputEventMouseMotion @event)
	{ 
		GD.Print(@event.Position);
		
		//camPivot.RotateY(-@event.Relative.X * 0.001f * mouseSpeed);
		camPivot.Rotate(cam.GlobalTransform.Basis.Y, -@event.Relative.X * 0.001f * mouseSpeed);
		//GlobalRotate(camPivot.GlobalTransform.Basis.Y, -@event.Relative.X * 0.001f * mouseSpeed);
		cam.RotateX(-@event.Relative.Y * 0.001f * mouseSpeed);

		// Vector3 tempCamRot = cam.RotationDegrees;
		// tempCamRot.X = Mathf.Clamp(tempCamRot.X, -90f, 90f);
		// cam.RotationDegrees = tempCamRot;
	}
	
}
