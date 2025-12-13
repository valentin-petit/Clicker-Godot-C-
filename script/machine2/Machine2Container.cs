using Godot;
using System;

public partial class Machine2Container : Control
{
	private float _vitesse = 1.0f;
	private nodeRootPrincipal _root;
	
	// UI Elements
	private Label _lblVitesse;
	private Label _lblVitProdMachine;
	private Label _lblAccident;
	private Label _lblPanne;
	
	private Button _btnPlus;
	private Button _btnMoins;
	
	// Logic
	private bool _estEnPanne;
	private Timer _timer;
	public int _compteur;
	public Sprite2D _sprite;

	// --- VARIABLES D'ANIMATION ---
	private int _compteurTics = 0;        
	private int _ticsPourChangerFrame = 8; 
	// -----------------------------

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

		// Récupération du timer global (WaitTime = 0.34f)
		_timer = _root.GetNode<Timer>("tmrMachine"); 

		// Initialisation
		CalculerDelaiFrame();
		UpdateVitesseProduction();
		UpdateStats();
		
		// Connexion des signaux
		if (!_timer.IsConnected("timeout", new Callable(this, MethodName.OnTmrMachineFinished)))
			_timer.Timeout += OnTmrMachineFinished;
			
		if (!_timer.IsConnected("timeout", new Callable(this, MethodName.CheckEnPanne)))
			_timer.Timeout += CheckEnPanne;
	}

	public override void _Process(double delta)
	{
		
	}
	
	public void OnTmrMachineFinished()
	{
		// 1. Si panne, on ne fait rien
		if (_estEnPanne) return;

		// 2. Si pas assez de stock au départ (Tissu < 2)
		if (_root.getStockTissue() < 2)
		{
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine2frame0.png");
			return;
		}

		// --- PLACEMENT ACCIDENT ---
		Checkaccident();

		// --- LOGIQUE DE RYTHME (Animation) ---
		_compteurTics++; 

		if (_compteurTics < _ticsPourChangerFrame)
		{
			return; 
		}

		_compteurTics = 0;
		// -------------------------------------

		_compteur++;
		// Machine 2 a 5 frames (0 à 4), donc modulo 5
		int index = _compteur % 5; 

		if (index == 0) {
			// Fin du cycle, on produit
			AjouterStock(); 
			
			
			// 1. On saute la frame 0 et on affiche direct la frame 1
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine2frame1.png");
			
			// 2. On décale le compteur pour que le prochain tic affiche la frame 2
			_compteur++; 
			
		}
		else if (index >= 1 && index <= 4) {
			 _sprite.Texture = GD.Load<Texture2D>($"res://image/machine2frame{index}.png");
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
			
			// Recalcul du rythme d'animation
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

			// Recalcul du rythme d'animation
			CalculerDelaiFrame();
		}
	}

	// --- FONCTION DE GESTION DU DELAI D'ANIMATION ---
	private void CalculerDelaiFrame()
	{
		switch (_vitesse)
		{
			case 1: _ticsPourChangerFrame = 8; break; // Lent
			case 2: _ticsPourChangerFrame = 6; break;
			case 3: _ticsPourChangerFrame = 4; break;
			case 4: _ticsPourChangerFrame = 3; break;
			case 5: _ticsPourChangerFrame = 2; break; // Rapide
			default: _ticsPourChangerFrame = 8; break;
		}
		_compteurTics = 0; 
	}
	
	private void UpdateVitesseProduction()
	{        
		double vitesseProd = _vitesse * 2;  
		_lblVitProdMachine.Text = "Vitesse de Production : \n" + vitesseProd.ToString("0.00") + " pièces/s";
	}
	
	// --- GESTION DE L'AFFICHAGE DES STATS ---
	private void UpdateStats()
	{
		// 1. Calcul Panne (Pourcentage par Minute)
		double pourcentagePanneMinute = GetTargetPanneProbability() * 100.0;
		
		// 2. Calcul Accident (Pourcentage par Minute)
		double pourcentageAccidentMinute = GetTargetAccidentProbability() * 100.0;

		_lblAccident.Text = "Accident = " + pourcentageAccidentMinute.ToString("0.0") + "% / min";
		_lblPanne.Text = "Panne = " + pourcentagePanneMinute.ToString("0.0") + "% / min";
	}

	public void AjouterStock()
	{
		for (int i=0; i<_vitesse;i++)
		{
			if(_root.getStockTissue()>=2)
			{
				_root.addStockChaussette(5);
				_root.subStockTissue(2);
			}
		}
	}

	// --- LOGIQUE D'ACCIDENT (METHODE TEMPORELLE) ---
	public void Checkaccident() 
	{
		if(_estEnPanne) return;

		// 1. Récupérer le % de chance d'accident sur 1 minute
		double targetProbOneMinute = GetTargetAccidentProbability();

		// Si 0%, on sort tout de suite
		if (targetProbOneMinute <= 0.0) return;

		// 2. Calculer ticks par minute (60s / 0.34s)
		double ticksPerMinute = 60.0 / 0.34;

		// 3. Convertir probabilité minute -> probabilité par tic
		double probaParTick = 1.0 - Math.Pow(1.0 - targetProbOneMinute, 1.0 / ticksPerMinute);

		// 4. Tirage
		double tirage = _rng.NextDouble();

		if (tirage < probaParTick) 
		{
			_root.subArgent(5000);
			_root.afficher_overlay_accident();
		}
	}

	// --- LOGIQUE DE PANNE PROBABILISTE ---
	public void CheckEnPanne()
	{
		if (_estEnPanne) return;

		double targetProbOneMinute = GetTargetPanneProbability();
		double ticksPerMinute = 60.0 / 0.34;
		double probaParTick = 1.0 - Math.Pow(1.0 - targetProbOneMinute, 1.0 / ticksPerMinute);

		double tirage = _rng.NextDouble();

		if (tirage < probaParTick)
		{
			setEstEnPanne(true);
			_sprite.Texture = GD.Load<Texture2D>("res://image/Machine1EnPanne.png");
			GD.Print("La machine 2 est tombée en panne !");
		}
	}

	// --- VALEURS CIBLES (PROBABILITÉS) ---

	private double GetTargetAccidentProbability()
	{
		switch ((int)_vitesse)
		{
			case 1: return 0.00;
			case 2: return 0.00;
			case 3: return 0.17; // 17% / min
			case 4: return 0.22; // 22% / min
			case 5: return 0.35; // 35% / min
			default: return 0.10;
		}
	}

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

	public void setEstEnPanne(bool res)
	{
		_estEnPanne = res;
	}
	
	public bool getEstEnPanne()
	{
		return _estEnPanne;
	}
	
	public void Reparer()
	{
		if (_root.getArgent() > 499)
		{
			_estEnPanne = false;
			_root.subArgent(500);
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine2frame0.png");
		}
	}
}
