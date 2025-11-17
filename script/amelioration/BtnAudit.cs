using Godot;
using System;

public partial class BtnAudit : Button
{
	private nodeRootPrincipal _nodeRootPrincipal ;
	private Node2D _sceneAmelioration ;
	
	public override void _Ready()
	{
		_nodeRootPrincipal = GetTree().CurrentScene as nodeRootPrincipal;
		_sceneAmelioration = GetOwner<Node2D>();
		
		this.Pressed+=OuvrirAudit;
	}
	public override void _Process(double delta)
	{
		
	}
	private void OuvrirAudit()
	{
		GD.Print("Le bouton a bien été cliqué et le signal a été reçu.");
		

		// Si on arrive ici, on essaie l'affichage
		//_nodeRootPrincipal.GetNode<Sprite2D>("Sprite2DFond").Hide();
		//_nodeRootPrincipal._sceneAudit.Show();	
		//_nodeRootPrincipal._sceneAmelioration.Hide();
		
		var sprite = _sceneAmelioration.GetNodeOrNull<Sprite2D>("sprFondAudit");
		if (sprite != null)
		{
			sprite.Show();			
		}
		else
		{
			GD.PrintErr("Erreur : sprFondAudit introuvable dans _sceneAmelioration !");
		}	
		
	}
}
