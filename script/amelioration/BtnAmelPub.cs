using Godot;
using System;

public partial class BtnAmelPub : Button
{
	private nodeRootPrincipal _root;
	private ControlVente _sceneVente;
	
	private const float COUT_AMELIO_PUB = 1000f;
	private const float AUGMENTE_PUB = 0.1f;
	
	public override void _Ready()
	{
		// récupération de code venant de ControlVente.cs pour trouver root
		_root = GetTree().CurrentScene as nodeRootPrincipal;
		if (_root == null)
			GD.Print("Erreur : impossible de récupérer nodeRootPrincipal !");
		
		// ptite différence avec FindChild car le control Vente n'est pas enfant direct de nodeRootPrincipal
		_sceneVente = _root.FindChild("ControlVente") as ControlVente; 
		if (_sceneVente == null)
			GD.PrintErr("Erreur : impossible de récupérer ControlVente ! Vérifiez le chemin.");
		
		this.Pressed+=achatPub;
	}
	public override void _Process(double delta)
	{
		
	}
	private void achatPub()
	{
		// si l'argent qu'on a est plus grand que le cout d'amelioration alors transaction possible
		if(_root.getArgent() >= COUT_AMELIO_PUB){
			_root.subArgent(COUT_AMELIO_PUB);		
		}
		// ajout du bonus de pub qui vient jouer dans la formule du calcul de "nVenteParSeconde" qui augmente un ptit peu
		_sceneVente._coefficientAmeliorationPub += AUGMENTE_PUB;
		
	}
}
