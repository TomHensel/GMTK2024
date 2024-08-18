using Godot;
using System;

public partial class Player : CharacterBody3D
{
	// Called when the node enters the scene tree for the first time.

	private Node3D camPivot;
	private Camera3D cam;

	[Export] private BlackHoleNode blackHole;

	[Export] private float mouseSpeed = 1f;

	[Export] private float rotationSpeed = 5f;
	[Export] private float steeringSpeed = 0.1f;
	[Export] private float steeringAccelaration = 5f;
	
	private Vector3 vel = Vector3.Zero;

	private float steeringValue = 0;

	private Node3D parrentNode;

	private Vector2 normMousePos = Vector2.Zero;

	private Vector3 camRotation = Vector3.Zero;

	private UiManager uiManager;
	
	public override void _Ready()
	{
		camPivot = GetNode<Node3D>("CamPivot");
		cam = GetNode<Camera3D>("CamPivot/Camera3D");
		parrentNode = GetNode<Node3D>("..");
		uiManager = GetNode<UiManager>("UIManager");
		
		Input.MouseMode = Input.MouseModeEnum.Confined;

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		float dTime = (float) delta;
		Vector2 input_dir = Input.GetVector("Right","Left" , "Forward", "Backward");

		steeringValue = Mathf.LerpAngle(steeringValue,  Mathf.DegToRad(input_dir.X*steeringSpeed), dTime * steeringAccelaration);
		
		
		camPivot.Rotate(cam.GlobalBasis.X, -normMousePos.Y*dTime * rotationSpeed);
		camPivot.Rotate(cam.GlobalTransform.Basis.Y, -normMousePos.X*dTime * rotationSpeed);
		camPivot.Rotate(cam.GlobalTransform.Basis.Z, steeringValue);
		

		Vector3 direction = cam.GlobalTransform.Basis.Z * input_dir.Y;
		


		vel = vel.Lerp(direction * 20, dTime); 

				
		Velocity = vel;
		
		MoveAndSlide();
		
		
		handleBlackHole(dTime);

	}

	public void handleBlackHole(float dTime)
	{
		if (Position.DistanceTo(blackHole.Position) < 5f)
		{
			uiManager.pressToActionLabel.Text = "E - Feed the black hole";
			//GD.Print("Feed The Black Hole");

			if (Input.IsActionPressed("Interact"))
			{
				blackHole.feed(dTime);
			}
		}
		else
		{
			uiManager.pressToActionLabel.Text = "";
		}
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

		normMousePos = (@event.Position / new Vector2(1920f, 1080f)) -new Vector2(0.5f,0.5f);
		
	}
	
}
