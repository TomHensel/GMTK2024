using Godot;

public partial class UiIndicator : Node3D
{

	private Camera3D cam;
	
	private TextureRect onScreenIndicator;
	private TextureRect offScreenIndicator;

	private Vector2 viewPortCenter;
	private Vector2 maxIndicatorPos;

	private Vector2 offSetVec = new Vector2(16, 16);
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		cam = GetViewport().GetCamera3D();
		onScreenIndicator = GetNode<TextureRect>("TextureRect2");
		offScreenIndicator = GetNode<TextureRect>("TextureRect");

		viewPortCenter = GetViewport().GetVisibleRect().GetCenter();
		maxIndicatorPos = viewPortCenter - offSetVec;
		// GD.Print(viewPortCenter);
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		if (cam.IsPositionInFrustum(GlobalPosition))
		{
			offScreenIndicator.Hide();
			onScreenIndicator.Show();
			Vector2 pos2d = cam.UnprojectPosition(GlobalPosition);
			onScreenIndicator.Position = pos2d - offSetVec;
		}
		else
		{
			onScreenIndicator.Hide();
			offScreenIndicator.Show();

			Vector3 localToCam = cam.ToLocal(GlobalPosition);
			Vector2 pos2d = new Vector2(localToCam.X, -localToCam.Y);
			//GD.Print(pos2d);
			if (pos2d.Abs().Aspect() > maxIndicatorPos.Aspect())
			{
				pos2d *= maxIndicatorPos.X / Mathf.Abs(pos2d.X);
			}
			else
			{
				pos2d *= maxIndicatorPos.Y / Mathf.Abs(pos2d.Y);
			}

			offScreenIndicator.Position = viewPortCenter + pos2d - offSetVec;
			float angle = Vector2.Up.AngleTo(pos2d);
			offScreenIndicator.Rotation = angle;
			
		}
		
		//GD.Print(onScreenIndicator.GlobalPosition);
	}
}
