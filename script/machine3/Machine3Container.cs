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

	// --- NOUVELLES VARIABLES (Indépendance Vitesse) ---
	private int _compteurTics = 0;       
	private int _ticsPourChangerFrame = 8; 
	// --------------------------------------------------

	// Optimisation : Random déclaré une seule fois
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

		// Utilisation du Timer partagé (tmrMachine)
		_timer = _root.GetNode<Timer>("tmrMachine");
		_timer.Timeout += OnTmrMachineFinished;
		_timer.Timeout += CheckEnPanne;
		
		// Initialisation
		CalculerDelaiFrame(); // Important pour définir la vitesse initiale
		UpdateVitesseProduction();
		UpdateStats();
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

		// On reset le compteur et on change l'image
		_compteurTics = 0;
		// --------------------------------

		_compteur++;
		// Il y a 8 images (0 à 7), donc modulo 8
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
			
			// Recalculer le délai quand on change la vitesse
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

			// Recalculer le délai quand on change la vitesse
			CalculerDelaiFrame();
		}
	}

	// --- FONCTION DE GESTION DU DELAI ---
	private void CalculerDelaiFrame()
	{
		// Plus la vitesse est élevée, moins on attend de tics
		switch (_vitesse)
		{
			case 1: _ticsPourChangerFrame = 8; break; // Lent
			case 2: _ticsPourChangerFrame = 6; break;
			case 3: _ticsPourChangerFrame = 4; break;
			case 4: _ticsPourChangerFrame = 2; break;
			case 5: _ticsPourChangerFrame = 1; break; // Rapide
			default: _ticsPourChangerFrame = 8; break;
		}
		_compteurTics = 0; // Reset pour effet immédiat
	}

	private void UpdateVitesseProduction()
	{        
		double vitesseProd = _vitesse * 2;   
		_lblVitProdMachine.Text = "Vitesse de Production : \n" + vitesseProd.ToString("0.00") + " pièces/s";
	}
	
	private void UpdateStats()
	{
		double pourcentageAccident = _vitesse * 2.5; 
		double pourcentagePanne = Math.Min(100, _vitesse * 0.1); // J'ai harmonisé avec les autres machines (0.1) sinon c'est trop

		_lblAccident.Text = "Accident = " + pourcentageAccident.ToString("0.0") + "%";
		_lblPanne.Text = "Panne = " + pourcentagePanne.ToString("0.0") + "% (Est)";
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

	public void CheckEnPanne()
	{
		if (_estEnPanne) return;

		// J'ai mis la probabilité optimisée (5000.0) comme pour les autres machines
		// Si vous voulez la version très fréquente de votre code original, remettez 17.0
		double proba = Math.Sqrt(_vitesse) / 5000.0; 
		double tirage = _rng.NextDouble(); 

		if (tirage < proba)
		{
			_estEnPanne = true;
			// Assurez-vous d'avoir l'image de panne (ex: Machine1EnPanne.png ou une spécifique)
			_sprite.Texture = GD.Load<Texture2D>("res://image/Machine1EnPanne.png");
			GD.Print("La machine 3 est tombée en panne !");
		}
	}
}
