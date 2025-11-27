using Godot;
using System;

public partial class lblArgent : Label
{
	private nodeRootPrincipal _root;
	
	public override void _Ready()
	{
		_root = GetTree().Root.GetNode<nodeRootPrincipal>("nodeRootPrincipal");

		if (GetTree().CurrentScene is nodeRootPrincipal rootPrincipalNode)
		{
			_root = rootPrincipalNode;
		}

		if (_root == null)
		{
			GD.PushError("Erreur fatale : nodeRootPrincipal non trouvé ou mauvais type. La logique du jeu est inaccessible.");
			Text = "Erreur de chargement."; 
			SetProcess(false); 
			return;
		}

		Text = "Argent : " + _root.getArgent().ToString("F2"); 
	}
	
	public override void _Process(double delta)
	{
		if (_root != null)
		{
			Text = "Argent : " + _root.getArgent().ToString("F2");
		}
	}
}
