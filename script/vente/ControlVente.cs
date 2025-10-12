using Godot;
using System;

public partial class ControlVente : Control
{
	public float _prixVente;
	public int _coefficientAmeliorationPrixVente = 1;
	public int _coefficientAmeliorationReputation = 1;
	
	// à voir si il faut pas plutot l'intégrer dans un coeff deja existant mais en attendant de savoir il est la
	public float _coefficientAmeliorationPub = 1.0f; 
	
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
		float nVenteParSeconde;
		// plus le prix de vente est grand, moins on vend d'unité par seconde 
		// plus la reputationest mauvaise, moins on vend
		nVenteParSeconde = (5/_prixVente)* 1.3f * _coefficientAmeliorationPrixVente *
		(_root.getReputation()/100) * _coefficientAmeliorationReputation * _coefficientAmeliorationPub;
		// J'ai' (Valentin) ajouté mon coeff de pub au dessus mais faut qu'on en discute ptet;
		
		float argentGagner = (float)Math.Round(nVenteParSeconde, 2) * _prixVente;
		
		// test si stock suffisant
		if ((float) _root.getStock() < nVenteParSeconde)
		{
			// afficher un message
			return;
		}
		
		//test si nVenteParSeconde n'est pas plus petit que 1
		if (nVenteParSeconde < 1)
		{
			// afficher un message
			return;
		}
		
		//vente
		_root.addArgent(argentGagner);
		_root.addStock((int)-(nVenteParSeconde));
		
		
		// message pour joueur pres de argent, informe sur la vente
		var labelVente = new Label();
		labelVente.Text = (float)Math.Round(nVenteParSeconde) + " paires de chaussettes vendues pour " + 
		(float)Math.Round(nVenteParSeconde*_prixVente, 2) + "$";
		labelVente.Modulate = new Color(1, 1, 1, 1);
		labelVente.Position = new Vector2(-800, 55);
		AddChild(labelVente);

		var tweenVente = GetTree().CreateTween();
		tweenVente.TweenProperty(labelVente, "modulate:a", 1.0f, 2.5f);
		tweenVente.Finished += () => labelVente.QueueFree();
	
		
		// texte pour test
	var label = new Label();
	label.Text = "Une vente!  nVenteParSeconde*_prixVente = " + argentGagner.ToString("F2") +
	"\n(5/_prixVente) * (_root.getReputation()/100) : "+ (5/_prixVente) +" * " +(_root.getReputation()/100) +
	"\n= " + argentGagner.ToString("F2") +
	"\n(5/_prixVente) : " + (5/_prixVente) ;
	label.Modulate = new Color(1, 1, 1, 1);
	label.Position = new Vector2(0, 75);
	AddChild(label);

	var tween = GetTree().CreateTween();
	tween.TweenProperty(label, "modulate:a", 1.0f, 2.0f);
	tween.Finished += () => label.QueueFree();
	
	}
	

}
