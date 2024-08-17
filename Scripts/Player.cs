using Godot;
using System;

public partial class Player : CharacterBody3D
{
	// Called when the node enters the scene tree for the first time.

	private Node3D camPivot;
	private Camera3D cam;

	[Export] private BlackHoleNode blackHole;

	[Export] private float mouseSpeed = 1f;
	
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
		// Position = parrentNode.Position;
		// parrentNode.Position = Vector3.Zero;

		//camRotation = camPivot.Rotation;
		
		float dTime = (float) delta;
		Vector2 input_dir = Input.GetVector("Right","Left" , "Forward", "Backward");
		
		

		//RotateZ(input_dir.X * dTime);
		//GlobalRotate();

		steeringValue = Mathf.LerpAngle(steeringValue,  Mathf.DegToRad(input_dir.X), dTime);
		
		//Rotate(cam.GlobalTransform.Basis.Z, steeringValue);
		//Rotate(cam.GlobalTransform.Basis.Z, input_dir.X * dTime);

		camRotation.X += -normMousePos.Y * dTime * 5f;
		camRotation.X =Mathf.Clamp(camRotation.X, -Mathf.Pi/2f, Mathf.Pi/2f);
		camRotation.Y += -normMousePos.X * dTime * 5f;
		// camRotation.Z += steeringValue;
		camPivot.GlobalRotation = camRotation;
		
		// camPivot.RotateX(-normMousePos.Y*dTime * 5f);
		// camPivot.RotateY(-normMousePos.X*dTime * 5f);
		//cam.Rotate(camPivot.GlobalBasis.X,-normMousePos.Y*dTime * 5f);
		//cam.GlobalRotate(camPivot.Basis.Y,-normMousePos.X*dTime * 5f);
		//cam.Rotate(camPivot.Basis.Y, -normMousePos.X*dTime * 5f);
		
		//Vector3 direction = camPivot.Transform.Basis * new Vector3(input_dir.X, 0f, input_dir.Y);
		Vector3 direction = cam.GlobalTransform.Basis.Z * input_dir.Y;
		//Input.GetLastMouseScreenVelocity()
		


		vel = vel.Lerp(direction * 20, dTime); 

		//vel = direction * 20f * dTime;
				
		Velocity = vel;
		
		MoveAndSlide();
		

		// parrentNode.Position = Position;
		// Position = Vector3.Zero;



		
		// Vector3 tempCamRot = camPivot.RotationDegrees;
		// tempCamRot.X = Mathf.Clamp(tempCamRot.X, -89f, 89f);
		// camPivot.RotationDegrees = tempCamRot;
		
		//GD.Print(camPivot.RotationDegrees);
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
		
		//GD.Print(normMousePos);
		
		


		// camPivot.RotateY(-@event.Relative.X * 0.001f * mouseSpeed);
		// cam.RotateX(-@event.Relative.Y * 0.001f * mouseSpeed);


		//camPivot.Rotate(cam.GlobalTransform.Basis.Y, -@event.Relative.X * 0.001f * mouseSpeed);
		//GlobalRotate(camPivot.GlobalTransform.Basis.Y, -@event.Relative.X * 0.001f * mouseSpeed);
		// Vector3 tempCamRot = cam.RotationDegrees;
		// tempCamRot.X = Mathf.Clamp(tempCamRot.X, -90f, 90f);
		// cam.RotationDegrees = tempCamRot;
	}
	
}
