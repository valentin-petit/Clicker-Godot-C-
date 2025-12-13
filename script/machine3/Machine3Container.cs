using Godot;
using System;

public partial class Machine3Container : Control
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
	// --------------------------------------------------

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

		// Utilisation du Timer partagé (tmrMachine) - WaitTime ~0.34s
		_timer = _root.GetNode<Timer>("tmrMachine");
		
		// Initialisation
		CalculerDelaiFrame(); 
		UpdateVitesseProduction();
		UpdateStats();

		// Connexion des signaux
		_timer.Timeout += OnTmrMachineFinished;
		_timer.Timeout += CheckEnPanne;
	}

	public override void _Process(double delta)
	{

	}

	public void OnTmrMachineFinished()
	{
		// 1. Si panne, on arrête tout
		if (_estEnPanne) return;

		// 2. Vérification du stock (Machine 3 consomme des chaussettes)
		if (_root.getStockChaussette() < 2)
		{
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine3frame0.png");
			return;
		}

		// --- LOGIQUE DE RYTHME (TICS) ---
		_compteurTics++;

		// Tant qu'on n'a pas atteint le nombre de tics requis, on attend
		if (_compteurTics < _ticsPourChangerFrame)
		{
			return;
		}

		// On reset le compteur pour la prochaine frame
		_compteurTics = 0;
		// --------------------------------

		_compteur++;
		// Machine 3 a 8 images (0 à 7), donc modulo 8
		int index = _compteur % 8; 

		if (index == 0) {
			Checkaccident();
			AjouterStock();
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine3frame0.png");
		}
		else if (index == 1) {
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine3frame1.png");
		}
		else if (index == 2) {
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine3frame2.png");
		}
		else if (index == 3) {
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine3frame3.png");
		}
		else if (index == 4) {
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine3frame4.png");
		}
		else if (index == 5) {
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine3frame5.png");
		}
		else if (index == 6) {
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine3frame6.png");
		}
		else if (index == 7) {
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine3frame7.png");
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
			
			// Recalculer le délai d'animation
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

			// Recalculer le délai d'animation
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
		_compteurTics = 0; // Reset immédiat
	}

	private void UpdateVitesseProduction()
	{        
		double vitesseProd = _vitesse * 2;   
		_lblVitProdMachine.Text = "Vitesse de Production : \n" + vitesseProd.ToString("0.00") + " pièces/s";
	}
	
	private void UpdateStats()
	{
		double pourcentageAccident = _vitesse * 2.5; 
		
		// On récupère le vrai % défini pour la minute
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
				if (_root.getStockChaussette() >= 2)
				{
					_root.addStockProduitFini(1);
					_root.subStockChaussette(2);
				}
			}
		}
	}
	
	public void Checkaccident()
	{
		// Logique d'accident conservée telle quelle
		double proba = (Math.Sqrt(_vitesse) / 100.0) / 1.7;
		double tirage = _rng.NextDouble(); 

		if (tirage < proba)
		{
			GD.Print("Accident Machine 3");
			int tirage2 = _rng.Next(100); 

			if (tirage2 < 60)                
			{
				GD.Print("il s'est coupé\nvous payer 1000$ de compensation");
				_root.subArgent(1000);
			}
			else if (tirage2 < 95)               
			{
				GD.Print("il a le bras cassé\nvous payer 7000$ de compensation");
				_root.subArgent(7000);
			}
			else                              
			{
				GD.Print("un employer est tomber dans une machine\nil est mort\nvous payer 55000$ de compensation");
				_root.subArgent(55000);
			}
		}
	}

	// --- NOUVELLE LOGIQUE DE PANNE PROBABILISTE (Copie conforme Machine 2) ---
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
			// Charge l'image de panne (vérifie le chemin de l'image si tu veux une spécifique pour M3)
			_sprite.Texture = GD.Load<Texture2D>("res://image/Machine1EnPanne.png");
			GD.Print("La machine 3 est tombée en panne !");
		}
	}

	// Helper pour définir les % par minute selon la vitesse (Identique Machine 2)
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
