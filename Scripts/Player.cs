using Godot;

public partial class Player : CharacterBody3D
{
	// Called when the node enters the scene tree for the first time.

	public float BlackHoleFoodAmount = 0;
	public int planetDestoyerAmount = 0;
	public int asteroidHunterAmount = 0;

	private bool planetDestroyerUnlocked = false;
	private bool asteroidHunterUnlocked = false;

	[Export] private PlanetDestroyer planetDestroyer;
	[Export] private AsteroidHunter asteroidHunter;

	[Export] private Node3D sunSystem; 
	
	private Node3D camPivot;
	private Camera3D cam;

	[Export] private BlackHoleNode blackHole;

	[Export] private float mouseSpeed = 1f;

	[Export] private float rotationSpeed = 5f;
	[Export] private float steeringSpeed = 0.1f;
	[Export] private float steeringAccelaration = 5f;

	[Export] private AsteroidSpawner asteroidSpawner;

	private RayCast3D interactRay;
	
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
		interactRay = GetNode<RayCast3D>("CamPivot/RayCast3D");
		
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
		camPivot.Rotate(cam.GlobalTransform.Basis.Z, steeringValue * dTime * 60);
		

		Vector3 direction = cam.GlobalTransform.Basis.Z * input_dir.Y;

				


		vel = Velocity.Lerp(direction * usedSpeed, dTime); 

				
		Velocity = vel;
		
		MoveAndSlide();
		
		uiManager.pressToActionLabel.Text = "";
		handleBlackHole(dTime);
		
		handleInteraction(dTime);

		handleSunSystem();

		if (blackHole.blackHoleScale >= 25f)
		{
			asteroidHunterUnlocked = true;
			if (blackHole.blackHoleScale >= 50f)
			{
				planetDestroyerUnlocked = true;
			}
		}

	}

	public void handleSunSystem()
	{
		foreach (MeshInstance3D planet in sunSystem.GetChildren())
		{
			//GD.Print(planet);

			if (Position.DistanceTo(planet.GlobalPosition) < planet.Scale.X / 2f + planet.Scale.X / 4f)
			{
				if (planet.IsInGroup("Sun"))
				{
					uiManager.pressToActionLabel.Text = "";

				}
				else if(planetDestoyerAmount > 0)
				{
					uiManager.pressToActionLabel.Text = "E - Place Planet Destroyer";
					if (Input.IsActionJustPressed("Interact") && planetDestoyerAmount > 0)
					{
						PlanetDestroyer newPlanetDestroyer= (PlanetDestroyer) planetDestroyer.Duplicate();
						//newPlanetDestroyer.Position = Position;
						newPlanetDestroyer.Position = planet.Position + planet.Position.DirectionTo(Position)*(planet.Scale.X/2f + planet.Scale.X / 8f);
						newPlanetDestroyer.LookAtFromPosition( newPlanetDestroyer.Position,planet.Position, Vector3.Up);
						newPlanetDestroyer.planet = planet;
						newPlanetDestroyer.Visible = true;
						//newPlanetDestroyer.player = this;
						newPlanetDestroyer.blackHole = blackHole;
						
						planetDestoyerAmount--;
						
						//AddChild(newPlanetDestroyer);
						GetTree().Root.AddChild(newPlanetDestroyer);
					}
				}
			}
		}

		if (planetDestoyerAmount > 0)
		{
			uiManager.planetDestroyerIcon.Show();
			uiManager.planetDestroyerLabel.Show();
			uiManager.planetDestroyerLabel.Text = planetDestoyerAmount.ToString();
		}
		else
		{
			uiManager.planetDestroyerIcon.Hide();
			uiManager.planetDestroyerLabel.Hide();
		}

		if (asteroidHunterAmount > 0)
		{
			uiManager.asteroidIcon.Show();
			uiManager.asteroidLabel.Show();
			uiManager.asteroidLabel.Text = asteroidHunterAmount.ToString() + ", F To Deploy";
		}
		else
		{
			uiManager.asteroidIcon.Hide();
			uiManager.asteroidLabel.Hide();
		}

		if (Input.IsActionJustPressed("Deploy") && asteroidHunterAmount > 0)
		{
			AsteroidHunter newAsteroidHunter = (AsteroidHunter) asteroidHunter.Duplicate();
			newAsteroidHunter.Position = Position;
			newAsteroidHunter.blackHole = blackHole;
			newAsteroidHunter.Visible = true;
			newAsteroidHunter.asteroids = asteroidSpawner;
			//newAsteroidHunter.pickRandomAsteroid();
			GetTree().Root.AddChild(newAsteroidHunter);
			asteroidHunterAmount -= 1;
		}
	}

	public void changeFoodUi()
	{
		if (BlackHoleFoodAmount < 0f)
		{
			BlackHoleFoodAmount = 0f;
		}
		
		uiManager.blackHoleFoodLabel.Text = (BlackHoleFoodAmount).ToString("#.##");
	}


	public void handleInteraction(float dTime)
	{
		uiManager.rayUi.Hide();
		if (interactRay.IsColliding())
		{
			GodotObject colliderObject = interactRay.GetCollider();
			if (colliderObject is AsteroidCollider)
			{
				AsteroidCollider collider = (AsteroidCollider) colliderObject;
				//GD.Print(collider);
				//uiManager.crossHair.Scale = new Vector2(5,5);

				if (Input.IsActionPressed("MouseLeft"))
				{
					float destroyedAmount = dTime * 3f;
					collider.Destroy(destroyedAmount);
					BlackHoleFoodAmount += destroyedAmount;
					changeFoodUi();
				}
				
				uiManager.rayUi.Show();
			}
			else if (colliderObject is ShopItem)
			{
				ShopItem shopItem = (ShopItem) colliderObject;
				if (shopItem.itemType == 0)
				{
					if (planetDestroyerUnlocked)
					{
						uiManager.pressToActionLabel.Text = "E - 100 To Buy Planet Destroyer";
						if (Input.IsActionJustPressed("Interact") && BlackHoleFoodAmount >= 100)
						{
							BlackHoleFoodAmount -= 100;
							planetDestoyerAmount++;
							changeFoodUi();
						}
					}
					else
					{
						uiManager.pressToActionLabel.Text = "Give Black Hole 50 to Unlock PlanetDestroyer";
					}

					//planetDestoyerAmount++;
				}else if (shopItem.itemType == 1)
				{
					if (asteroidHunterUnlocked)
					{
						uiManager.pressToActionLabel.Text = "E - 50 To Buy AsteroidHunter";
						if (Input.IsActionJustPressed("Interact") && BlackHoleFoodAmount >= 50)
						{
							BlackHoleFoodAmount -= 50;
							asteroidHunterAmount++;
							changeFoodUi();
						}
					}
					else
					{
						uiManager.pressToActionLabel.Text = "Give Black Hole 25 to Unlock AsteroidHunter";
					}
				}
			}
		}
	}

	public void handleBlackHole(float dTime)
	{
		if (Position.DistanceTo(blackHole.blackHoleFeeder.GlobalPosition) < 5f)
		{
			if (BlackHoleFoodAmount <= 0.0001f)
			{
				uiManager.pressToActionLabel.Text = "You need Material to feed the Black Hole \n Q - Take Material";
			}
			else
			{
				uiManager.pressToActionLabel.Text = "E - Give Material \n Q - Take Material";
			}


			if (Input.IsActionPressed("Interact") && BlackHoleFoodAmount > 0)
			{
				float actualAmount = dTime * 10f;
				blackHole.feed(actualAmount);
				BlackHoleFoodAmount -= actualAmount;
				changeFoodUi();
			}

			if (Input.IsActionPressed("Take"))
			{
				BlackHoleFoodAmount += blackHole.take(dTime * 10f,dTime);
				changeFoodUi();

			}
			
			
		}
		// else
		// {
		// 	uiManager.pressToActionLabel.Text = "";
		// }
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

		if (Input.MouseMode == Input.MouseModeEnum.Confined)
		{


			Vector2 evPos = @event.Position;

			Rect2 visRect = GetViewport().GetVisibleRect();

			float distanceToEvPos = visRect.GetCenter().DistanceTo(evPos);
			Vector2 direectionToEvPos = visRect.GetCenter().DirectionTo(evPos);

			line.ClearPoints();


			if (distanceToEvPos > 500)
			{
				evPos = visRect.GetCenter() + direectionToEvPos * 500;
				// if (Input.MouseMode == Input.MouseModeEnum.ConfinedHidden)
				// {
				// 	Input.WarpMouse(evPos);
				// }

				line.AddPoint(visRect.GetCenter() + direectionToEvPos * 32f);
				line.AddPoint(evPos - direectionToEvPos * 16f);
			}
			else if (distanceToEvPos < 16f)
			{
				evPos = visRect.GetCenter();
			}
			else
			{
				line.AddPoint(visRect.GetCenter() + direectionToEvPos * 32f);
				line.AddPoint(evPos - direectionToEvPos * 16f);
			}




			float aspectRatio = visRect.Size.Y / visRect.Size.X;
			//GD.Print(aspectRatio);

			//Vector2 usedPos = new Vector2(evPos.X, evPos.Y * aspectRatio );

			Vector2 usedPos = (evPos / visRect.Size) - new Vector2(0.5f, 0.5f);
			usedPos = new Vector2(usedPos.X, usedPos.Y * aspectRatio);
			normMousePos = usedPos;

			mouseUi.Position = evPos - new Vector2(16, 16);


		}


	}
	
}
