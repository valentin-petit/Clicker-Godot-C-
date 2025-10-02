using Godot;
using System;

public partial class ControlVente : Control
{
	public int _prixVente;
	private nodeRootPrincipal _root;
	public override void _Ready()
	{
		_prixVente=5;
		_root = GetTree().CurrentScene as nodeRootPrincipal;
		if (_root == null)
			GD.Print("Erreur : impossible de récupérer nodeRootPrincipal !");
	
		
	}


	

}
