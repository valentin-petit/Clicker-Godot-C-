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
		nVenteParSeconde = (5/_prixVente) * (_root.getReputation()/100);
		
		//vente
		_root.addArgent(nVenteParSeconde*_prixVente);
		_root.addStock(-(nVenteParSeconde));
		
		// texte pour test
	var label = new Label();
	label.Text = "Une vente!  nVenteParSeconde*_prixVente = " + nVenteParSeconde*_prixVente +
	"\n(5/_prixVente) * (_root.getReputation()/100) : "+ (5/_prixVente) +" + " +(_root.getReputation()/100);
	label.Modulate = new Color(1, 1, 1, 1);
	label.Position = new Vector2(0, 75);
	AddChild(label);

	var tween = GetTree().CreateTween();
	tween.TweenProperty(label, "modulate:a", 0.0f, 2.0f);
	tween.Finished += () => label.QueueFree();
	
	}
	

}
