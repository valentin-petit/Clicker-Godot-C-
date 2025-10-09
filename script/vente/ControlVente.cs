using Godot;
using System;

public partial class ControlVente : Control
{
	public int _prixVente;
	public int _coefficientAmeliorationPrixVente;
	public int _coefficientAmeliorationReputation;
	
	private nodeRootPrincipal _root;
	
	private Timer _tmrVente;
	
	public override void _Ready()
	{
		// initialisation prix de vente
		_prixVente=5;
		
		//recuperation de la scene principale
		_root = GetTree().CurrentScene as nodeRootPrincipal;
		if (_root == null)
			GD.Print("Erreur : impossible de récupérer nodeRootPrincipal !");
		
		// recuperation du timer pour la vente, et fonction vente a chaque fin de timer
		_tmrVente = GetNode<Timer>("tmrVente");
		_tmrVente.Timeout += OnTmrVenteTimeOut;
	}

	// focntion fin de timer vente, une vente
	public void OnTmrVenteTimeOut()
	{
		int nVenteParSeconde;
		// plus le prix de vente est grand, moins on vend d'unité par seconde
		// plus la reputationest mauvaise, moins on vend
		nVenteParSeconde = (1/_prixVente) * (_root.getReputation()/100);
		
		//vente
		_root.addArgent(nVenteParSeconde*_prixVente);
	}
	

}
