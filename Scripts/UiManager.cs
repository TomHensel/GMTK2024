using Godot;

public partial class UiManager : Control
{
	public Label pressToActionLabel;
	public TextureRect crossHair;
	public TextureRect rayUi;
	public Label blackHoleFoodLabel;
	
	public Label planetDestroyerLabel;
	public TextureRect planetDestroyerIcon;

	public Label asteroidLabel;
	public TextureRect asteroidIcon;

	public Control pauseMenu;
	
	// Called when the node enters the scene tree for the first time.
	public override void _Ready()
	{
		pauseMenu = GetNode<Control>("PauseMenu");
		pauseMenu.Visible = false;
		
		pressToActionLabel = GetNode<Label>("PressToActionLabel");
		crossHair = GetNode<TextureRect>("TextureRect");
		rayUi = GetNode<TextureRect>("RayUi");
		blackHoleFoodLabel = GetNode<Label>("BlackHoleFoodLabel");

		planetDestroyerLabel = GetNode<Label>("PlanetDestroyerLabel");
		planetDestroyerIcon = GetNode<TextureRect>("PlanetDestroyerIcon");
		
		asteroidLabel = GetNode<Label>("AsteroidLabel");
		asteroidIcon = GetNode<TextureRect>("AsteroidIcon");
	}


}
