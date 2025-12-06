using Godot;
using System;

public partial class Machine1Container : Control
{
	private float _vitesse = 1.0f;
	private nodeRootPrincipal _root;
	private Label _lblVitesse;
	private Label _lblVitProdMachine;
	private Label _lblAccident;
	private Label _lblPanne;
	private bool _estEnPanne;
	
	private Button _btnPlus;
	private Button _btnMoins;
	
	private Timer _timer;
	
	public int _compteur; // Compteur pour les frames (0, 1, 2...)
	public Sprite2D _sprite;

	// --- NOUVELLES VARIABLES ---
	private int _compteurTics = 0;       // Compte combien de fois le timer a sonné
	private int _ticsPourChangerFrame = 8; // Combien de tics il faut attendre pour bouger
	// ---------------------------

	private Random _rng = new Random(); 

	public override void _Ready()
	{
		_root = GetTree().Root.GetNode<nodeRootPrincipal>("nodeRootPrincipal");
		if (_root == null)
			GD.Print("Erreur : impossible de récupérer nodeRootPrincipal !");
			
		_lblVitesse = GetNode<Label>("lblVitesse");
		_btnPlus = GetNode<Button>("btnPlus");
		_btnMoins = GetNode<Button>("btnMoins");
		
		_lblVitProdMachine = GetNode<Label>("lblVitProdMachine");
		_lblAccident = GetNode<Label>("lblAccident");
		_lblPanne = GetNode<Label>("lblPanne");
		
		_btnPlus.Pressed += augmenterVitesse;
		_btnMoins.Pressed += baisserVitesse;
		
		_estEnPanne = false;
		_compteur = 0;
		_sprite = GetNode<Sprite2D>("Area2DMachine/Sprite2DMachine");

		// Récupération du Timer global
		_timer = _root.GetNode<Timer>("tmrMachine");
		
		// Initialisation
		CalculerDelaiFrame(); // Définit le nombre de tics initiaux
		UpdateVitesseProduction();
		UpdateStats();
		
		_timer.Timeout += OnTmrMachineFinished;
		_timer.Timeout += CheckEnPanne;
	}

	public override void _Process(double delta)
	{
		
	}
	
	public void OnTmrMachineFinished()
	{
		// 1. Si panne, on ne fait rien
		if (_estEnPanne) return;

		// 2. Si pas assez de stock, on force l'image 0 et on reset le rythme
		if (_root.getStockLaine() < 2) 
		{
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine1frame0.png");
			return; 
		}

		// --- NOUVELLE LOGIQUE DE RYTHME ---
		_compteurTics++; // On ajoute un tic

		// Si on n'a pas encore atteint le nombre de tics requis, on attend
		if (_compteurTics < _ticsPourChangerFrame)
		{
			return; 
		}

		// Si on a atteint le seuil, on remet le compteur à 0 et on fait l'animation
		_compteurTics = 0;
		// ----------------------------------

		// Le reste de votre logique d'animation reste identique
		_compteur++;
		int index = _compteur % 6; 

		if (index == 0) {
			Checkaccident();
			AjouterStock(); 
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine1frame0.png");
		}
		else if (index == 1) {
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine1frame1.png");
		}
		else if (index == 2) {
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine1frame2.png");
		}
		else if (index == 3) {
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine1frame3.png");
		}
		else if (index == 4) {
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine1frame4.png");
		}
		else if (index == 5) {
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine1frame5.png");
		}
	}
	
	public void augmenterVitesse()
	{
		if (_vitesse < 5)
		{
			_vitesse += 1;
			_lblVitesse.Text = "Vitesse N°" + _vitesse;
			UpdateVitesseProduction();
			UpdateStats();
			
			// On recalcule le délai (nombre de tics à attendre)
			CalculerDelaiFrame();
		}
	}

	public void baisserVitesse()
	{
		if (_vitesse > 1)
		{
			_vitesse -= 1;
			_lblVitesse.Text = "Vitesse N°" + _vitesse;
			UpdateVitesseProduction();
			UpdateStats();

			// On recalcule le délai
			CalculerDelaiFrame();
		}
	}

	// --- C'est ici qu'on définit votre règle de 8 à 1 ---
	private void CalculerDelaiFrame()
	{
		// On ne touche PAS au Timer.WaitTime.
		// On change seulement combien de fois on ignore le Timer avant de bouger.
		switch (_vitesse)
		{
			case 1:
				_ticsPourChangerFrame = 8; // Très lent : attend 8 coups
				break;
			case 2:
				_ticsPourChangerFrame = 6; // Un peu plus rapide
				break;
			case 3:
				_ticsPourChangerFrame = 4; // Moyen
				break;
			case 4:
				_ticsPourChangerFrame = 2; // Rapide
				break;
			case 5:
				_ticsPourChangerFrame = 1; // Très rapide : bouge à chaque coup
				break;
			default:
				_ticsPourChangerFrame = 8;
				break;
		}
		// Reset le compteur pour appliquer le changement immédiatement
		_compteurTics = 0; 
	}
	
	private void UpdateVitesseProduction()
	{        
		double vitesseProd = _vitesse * 2;  
		_lblVitProdMachine.Text = "Vitesse de Production : \n" + vitesseProd.ToString("0.00") + " pièces/s";
	}
	
	private void UpdateStats()
	{
		double pourcentageAccident = _vitesse * 2.5; 
		double pourcentagePanne = Math.Min(100, _vitesse * 0.1); 

		_lblAccident.Text = "Accident = " + pourcentageAccident.ToString("0.0") + "%";
		_lblPanne.Text = "Panne = " + pourcentagePanne.ToString("0.0") + "% (Est)";
	}

	public void AjouterStock()
	{
		if (!_estEnPanne)
		{
			for (int i = 0; i < _vitesse; i++)
			{
				if (_root.getStockLaine() >= 2)
				{
					_root.addStockTissue(1); 
					_root.subStockLaine(2);
				}
			}
		}
	}

	public void Checkaccident()
	{
		double proba = (Math.Sqrt(_vitesse) / 100.0) / 99.7;
		double tirage = _rng.NextDouble(); 

		if (tirage < proba)
		{
			GD.Print("Accident");
			int tirage2 = _rng.Next(100); 

			if (tirage2 < 60)                
			{
				GD.Print("Coupure légère (-1000$)");
				_root.subArgent(1000);
			}
			else if (tirage2 < 95)              
			{
				GD.Print("Bras cassé (-7000$)");
				_root.subArgent(7000);
			}
			else                            
			{
				GD.Print("Accident mortel (-55000$)");
				_root.subArgent(55000);
			}
		}
	}

	public void CheckEnPanne()
	{
		// La vérification de panne se fait à chaque tick du timer (indépendamment de l'animation)
		// C'est mieux ainsi, sinon à vitesse 1 on aurait 8x moins de risques de panne par seconde.
		if (_estEnPanne) return;

		double proba = Math.Sqrt(_vitesse) / 5000.0; 
		double tirage = _rng.NextDouble();

		if (tirage < proba)
		{
			_estEnPanne = true;
			_sprite.Texture = GD.Load<Texture2D>("res://image/Machine1EnPanne.png");
			GD.Print("La machine est tombée en panne !");
		}
	}
}
