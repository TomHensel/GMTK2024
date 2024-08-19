using Godot;
using Godot.NativeInterop;

[Tool]
public partial class Line3d : Node3D
{
	[Export] private float lineRadius = 0.1f;
	[Export] private int lineResolution = 90;
	private CsgPolygon3D polygon;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		polygon = GetNode<CsgPolygon3D>("CSGPolygon3D");
	}

	// Called every frame. 'delta' is the elapsed time since the previous frame.
	public override void _Process(double delta)
	{
		 Vector2[] circle = new Vector2[lineResolution];
		 for (int i = 0; i < lineResolution; ++i)
		 {
			 float x = lineRadius * Mathf.Sin(Mathf.Pi * 2 * i / lineResolution);
			 float y = lineRadius * Mathf.Cos(Mathf.Pi * 2 * i / lineResolution);
			 Vector2 coords = new Vector2(x,y);
			 circle[i] = coords;
		 }

		 polygon.Polygon = circle;
	}
}
