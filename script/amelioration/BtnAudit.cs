using Godot;
using System;

public partial class BtnAudit : Button
{
	private nodeRootPrincipal _root;
		
	public override void _Ready()
	{
		_root = GetTree().CurrentScene as nodeRootPrincipal;
		this.Pressed+=OuvrirAudit;
	}
	public override void _Process(double delta)
	{
		
	}
	private void OuvrirAudit()
	{   
	GD.Print("Le bouton a bien été cliqué et le signal a été reçu.");

		// 1. Récupérer le nœud racine de la scène 'amelioration' :
		// btnAudit -> Sprite2DOrdinateur -> _sceneAmelioration
		Node sceneAmelioration = GetParent().GetParent(); 

		// 2. Chercher sprMenuTel. On utilise GetNodeOrNull<Node>() pour la recherche par nom,
		// puis on caste vers CanvasItem (le type visuel le plus sûr)
		
		Node foundNode = sceneAmelioration.GetNodeOrNull("sprMenuTel");
		
		if (foundNode != null)
		{
			CanvasItem nodeToToggle = foundNode as CanvasItem;
			
			if (nodeToToggle != null)
			{
				nodeToToggle.Show();
				GD.Print("Le menu sprMenuTel a été affiché avec succès via CanvasItem.");
			}
			else
			{
				// Ce cas arrive si sprMenuTel n'est pas un Control/Node2D/Sprite2D (ce qui est faux ici)
				GD.PrintErr($"ERREUR : sprMenuTel trouvé, mais n'est pas un CanvasItem (Type: {foundNode.GetType().Name}).");
			}
		}
		else
		{
			GD.PrintErr("ERREUR FATALE : Le nœud sprMenuTel est introuvable par le chemin GetParent().GetParent().");
			// Débogage : Afficher le parent pour voir où nous sommes
			GD.PrintErr($"Le parent de sprMenuTel devrait être : {sceneAmelioration.Name}");
		}
	}
}
