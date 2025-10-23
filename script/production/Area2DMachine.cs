using Godot;
using System;

public partial class Area2DMachine : Area2D
{
	//nouvelles logique de prod 
	private const int PRODUCTION_PAR_CYCLE = 1;   
	private const float TEMPS_PRODUCTION = 3.0f;
	private float _tempsRestant = TEMPS_PRODUCTION;
	
	// variables propres à la machine
	private int _prodMan = 1;
	private int _prodAuto = 0;
	private nodeRootPrincipal _root;

	public override void _Ready()
	{
		_root = GetTree().Root.GetNode<nodeRootPrincipal>("nodeRootPrincipal");
		if (_root == null){
			GD.Print("Erreur : impossible de récupérer nodeRootPrincipal !");
		}			
			_tempsRestant = TEMPS_PRODUCTION;
	}
	
	public override void _Process(double delta)
	{
		// Décrémenter le temps
		_tempsRestant -= (float)delta;
		
		// Produire lorsque le temps est écoulé
		if (_tempsRestant <= 0)
		{			
			// Réinitialiser le timer (ajoute le reste du temps pour plus de précision)
			_tempsRestant = TEMPS_PRODUCTION + _tempsRestant; 
		}
	}

	public void SetProdAuto(int ajout)
	{
		_prodAuto += ajout;
	}

}
