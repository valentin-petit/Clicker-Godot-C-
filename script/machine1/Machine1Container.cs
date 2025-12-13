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
	
	public int _compteur; 
	public Sprite2D _sprite;

	// --- VARIABLES D'ANIMATION (Indépendance Vitesse) ---
	private int _compteurTics = 0;        
	private int _ticsPourChangerFrame = 8; 
	// ----------------------------------------------------

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

		// Récupération du Timer global (0.34s)
		_timer = _root.GetNode<Timer>("tmrMachine");
		
		// Initialisation
		CalculerDelaiFrame(); 
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

		// 2. Si pas assez de stock (Machine 1 a besoin de Laine)
		if (_root.getStockLaine() < 2) 
		{
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine1frame0.png");
			return; 
		}

		// --- LOGIQUE DE RYTHME (TICS) ---
		_compteurTics++; 

		// Si on n'a pas encore atteint le nombre de tics requis, on attend
		if (_compteurTics < _ticsPourChangerFrame)
		{
			return; 
		}

		// Si on a atteint le seuil, on reset le compteur et on avance l'animation
		_compteurTics = 0;
		// ----------------------------------

		_compteur++;
		// Machine 1 a 6 frames (0 à 5), donc modulo 6
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
			
			// On recalcule le délai d'animation
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

			// On recalcule le délai d'animation
			CalculerDelaiFrame();
		}
	}

	// --- FONCTION DE GESTION DU DELAI ---
	private void CalculerDelaiFrame()
	{
		switch (_vitesse)
		{
			case 1: _ticsPourChangerFrame = 8; break; // Lent
			case 2: _ticsPourChangerFrame = 6; break;
			case 3: _ticsPourChangerFrame = 4; break;
			case 4: _ticsPourChangerFrame = 2; break;
			case 5: _ticsPourChangerFrame = 1; break; // Rapide
			default: _ticsPourChangerFrame = 8; break;
		}
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
		
		// On récupère le vrai % défini pour la minute pour l'affichage
		double pourcentagePanneMinute = GetTargetPanneProbability() * 100.0;

		_lblAccident.Text = "Accident = " + pourcentageAccident.ToString("0.0") + "%";
		_lblPanne.Text = "Panne = " + pourcentagePanneMinute.ToString("0.0") + "% / min";
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

	// Fonction de vérification d'accident
	public void Checkaccident() 
	{
		if(_estEnPanne)
		{
			return;
		}
		// 1. Vérifier si la vitesse est suffisante (> 2)
		if (_vitesse > 2) 
		{
			// CORRECTION 1 : On définit la variable 'pourcentage' localement
			// (Basé sur votre logique dans UpdateStats : vitesse * 2.5)
			double pourcentage = _vitesse * 2.5;

			// CORRECTION 2 : On remplace Math.random() par GD.RandRange(0f, 100f)
			// GD.RandRange est la méthode native de Godot pour l'aléatoire
			if (GD.RandRange(0f, 100f) < (pourcentage - 10)) 
			{
				// 3. Si l'accident arrive
				_root.subArgent(5000);
				_root.afficher_overlay_accident();
			}
		}
	}

	// --- NOUVELLE LOGIQUE DE PANNE PROBABILISTE (Standardisée) ---
	public void CheckEnPanne()
	{
		if (_estEnPanne) return;

		// 1. Récupérer le % de chance de tomber en panne sur 1 minute
		double targetProbOneMinute = GetTargetPanneProbability();

		// 2. Calculer combien de fois le timer (0.34s) s'exécute en 1 minute (60s)
		double ticksPerMinute = 60.0 / 0.34;

		// 3. Convertir la probabilité "par minute" en probabilité "par tic"
		// Formule : P_tick = 1 - (1 - P_minute)^(1 / N_ticks)
		double probaParTick = 1.0 - Math.Pow(1.0 - targetProbOneMinute, 1.0 / ticksPerMinute);

		// 4. Tirage au sort
		double tirage = _rng.NextDouble();

		if (tirage < probaParTick)
		{
			_estEnPanne = true;
			_sprite.Texture = GD.Load<Texture2D>("res://image/Machine1EnPanne.png");
			GD.Print("La machine 1 est tombée en panne !");
		}
	}

	// Helper pour définir les % par minute selon la vitesse
	private double GetTargetPanneProbability()
	{
		switch ((int)_vitesse)
		{
			case 1: return 0.10; // 10%
			case 2: return 0.14; // 14%
			case 3: return 0.17; // 17%
			case 4: return 0.22; // 22%
			case 5: return 0.35; // 35%
			default: return 0.10;
		}
	}
}
