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

	//private Node3D parrentNode;

	private Vector2 normMousePos = Vector2.Zero;

	private Vector3 camRotation = Vector3.Zero;

	private UiManager uiManager;

	private TextureRect mouseUi;
	
	private Line2D line;
	
	public override void _Ready()
	{
		line = GetNode<Line2D>("UIManager/Line2D");
		
		camPivot = GetNode<Node3D>("CamPivot");
		cam = GetNode<Camera3D>("CamPivot/Camera3D");
		//parrentNode = GetNode<Node3D>("..");
		uiManager = GetNode<UiManager>("UIManager");
		
		Input.MouseMode = Input.MouseModeEnum.Confined;

		mouseUi = GetNode<TextureRect>("UIManager/MousePointer");

	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		float dTime = (float) delta;
		Vector2 input_dir = Input.GetVector("Right","Left" , "Forward", "Backward");

		steeringValue = Mathf.Lerp(steeringValue,  Mathf.DegToRad(input_dir.X*steeringSpeed), dTime * steeringAccelaration);

		//steeringValue = input_dir.X * dTime;
		
		float usedSpeed = 20;
		float usedRotationSpeed = rotationSpeed;
		if (Input.IsActionPressed("Shift") && input_dir.Y < 0f)
		{
			usedSpeed = 200;
			//usedRotationSpeed = rotationSpeed * 0.25f;
		}
		
		camPivot.Rotate(cam.GlobalBasis.X, -normMousePos.Y*dTime * usedRotationSpeed);
		camPivot.Rotate(cam.GlobalTransform.Basis.Y, -normMousePos.X*dTime * usedRotationSpeed);
		camPivot.Rotate(cam.GlobalTransform.Basis.Z, steeringValue * dTime * 240f);
		

		Vector3 direction = cam.GlobalTransform.Basis.Z * input_dir.Y;

				


		vel = vel.Lerp(direction * usedSpeed, dTime); 

				
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

	
	public override void _UnhandledInput(InputEvent @event)
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
	
	// public void handInput(InputEvent @event)
	// { 
	// 	base._Input(@event);
	// 	
	// 	if (@event is InputEventMouseMotion)
	// 	{
	// 		moveCamera((InputEventMouseMotion)@event);
	// 	}
	// 	
	// 	if (@event.IsActionPressed("Escape"))
	// 	{
	// 		
	// 		if (Input.MouseMode == Input.MouseModeEnum.Visible)
	// 		{
	// 			Input.MouseMode = Input.MouseModeEnum.ConfinedHidden;
	// 		}
	// 		else
	// 		{
	// 			Input.MouseMode = Input.MouseModeEnum.Visible;
	// 		}
	// 		
	// 	}
	// 	
	// }
	
	private void moveCamera(InputEventMouseMotion @event)
	{

		Vector2 evPos = @event.Position;

		Rect2 visRect = GetViewport().GetVisibleRect();

		float distanceToEvPos = visRect.GetCenter().DistanceTo(evPos);
		Vector2 direectionToEvPos = visRect.GetCenter().DirectionTo(evPos);
		
		line.ClearPoints();

		
		if (distanceToEvPos > 500)
		{
			evPos = visRect.GetCenter() + direectionToEvPos*500;
			// if (Input.MouseMode == Input.MouseModeEnum.ConfinedHidden)
			// {
			// 	Input.WarpMouse(evPos);
			// }

			line.AddPoint(visRect.GetCenter() +direectionToEvPos * 32f);
			line.AddPoint(evPos - direectionToEvPos*16f);
		}
		else if (distanceToEvPos < 16f)
		{
			evPos = visRect.GetCenter();
		}
		else
		{
			line.AddPoint(visRect.GetCenter() +direectionToEvPos * 32f);
			line.AddPoint(evPos -direectionToEvPos*16f);
		}
		
	
		

		float aspectRatio = visRect.Size.Y / visRect.Size.X;
		//GD.Print(aspectRatio);
		
		//Vector2 usedPos = new Vector2(evPos.X, evPos.Y * aspectRatio );
		
		Vector2 usedPos = (evPos / visRect.Size) -new Vector2(0.5f,0.5f);
		usedPos = new Vector2(usedPos.X, usedPos.Y * aspectRatio );
		normMousePos = usedPos;
		
		mouseUi.Position = evPos - new Vector2(16,16);
		
	
		
		
		
	}
	
}
