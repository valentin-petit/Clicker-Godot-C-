using Godot;
using System;

public partial class Sprite2dOrdinateur : Sprite2D
{
	public override void _Ready()
	{
		
		// Taille de la fenêtre de jeu
		Vector2 screenSize = GetViewport().GetVisibleRect().Size;

		// Taille de l'image (texture)
		Vector2 textureSize = Texture.GetSize();

		// Facteur d'échelle pour remplir l'écran
		Vector2 scaleFactor = new Vector2(
			screenSize.X / textureSize.X,
			screenSize.Y / textureSize.Y
		);

		// Applique l'échelle
		Scale = scaleFactor;

		// Ajuste la position selon si "Centered" est activé ou non
		if (Centered)
		{
			// Si le Sprite est centré, on le place au milieu de l'écran
			Position = screenSize / 2;
		}
		else
		{
			// Si le Sprite n'est pas centré, on le colle en haut-gauche
			Position = Vector2.Zero;
		}
	}
	
	public override void _Process(double delta)
	{
		
	}
}
