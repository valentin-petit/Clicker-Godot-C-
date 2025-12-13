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

		// Récupération du Timer global (0.34s)
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
		// 1. Si panne, on arrête tout
		if (_estEnPanne) return;

		// 2. Vérification du stock (Machine 3 consomme 2 chaussettes)
		if (_root.getStockChaussette() < 2)
		{
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine3frame0.png");
			return;
		}

		// --- NOUVEAU PLACEMENT ACCIDENT ---
		// On vérifie l'accident à chaque tic (0.34s) tant que la machine tourne,
		// exactement comme pour la Panne, pour respecter la probabilité temporelle.
		Checkaccident();

		// --- LOGIQUE DE RYTHME (TICS) ---
		_compteurTics++;

		if (_compteurTics < _ticsPourChangerFrame)
		{
			return;
		}

		_compteurTics = 0;
		// --------------------------------

		_compteur++;
		
		// Animation 8 frames (0 à 7)
		int index = _compteur % 8; 

		if (index == 0) {
			// Note: Checkaccident a été déplacé plus haut pour être cohérent avec le temps
			AjouterStock();  // Production
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine3frame0.png");
		}
		else if (index >= 1 && index <= 7) {
			_sprite.Texture = GD.Load<Texture2D>($"res://image/machine3frame{index}.png");
		}
	}

	public void augmenterVitesse()
	{
		if (_vitesse < 5)
		{
			_vitesse += 1;
			_lblVitesse.Text = "Vitesse N°" + _vitesse;
			_lblVitesse.Text = "Vitesse N°" + _vitesse;
			UpdateVitesseProduction();
			UpdateStats();
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
			CalculerDelaiFrame();
		}
	}

	private void CalculerDelaiFrame()
	{
		switch (_vitesse)
		{
			case 1: _ticsPourChangerFrame = 8; break; 
			case 2: _ticsPourChangerFrame = 6; break;
			case 3: _ticsPourChangerFrame = 4; break;
			case 4: _ticsPourChangerFrame = 2; break;
			case 5: _ticsPourChangerFrame = 1; break; 
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
		// On récupère les stats (en pourcentage par minute)
		double chanceAccident = GetPourcentageAccident();
		double chancePanneMin = GetPourcentagePanne();

		_lblAccident.Text = "Accident = " + chanceAccident.ToString("0.0") + "% / min";
		_lblPanne.Text = "Panne = " + chancePanneMin.ToString("0.0") + "% / min";
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
	
	// --- LOGIQUE ACCIDENT (CALCULÉ COMME PANNE) ---
	public void Checkaccident() 
	{
		if(_estEnPanne) return;

		// 1. Récupérer le % de chance d'accident sur 1 minute
		double targetProbOneMinute = GetTargetAccidentProbability();

		// Si la probabilité est 0, on ne fait rien
		if (targetProbOneMinute <= 0.0) return;

		// 2. Calculer combien de fois le timer (0.34s) s'exécute en 1 minute (60s)
		double ticksPerMinute = 60.0 / 0.34;

		// 3. Convertir la probabilité "par minute" en probabilité "par tic"
		// Formule : P_tick = 1 - (1 - P_minute)^(1 / N_ticks)
		double probaParTick = 1.0 - Math.Pow(1.0 - targetProbOneMinute, 1.0 / ticksPerMinute);

		// 4. Tirage au sort
		double tirage = _rng.NextDouble();

		if (tirage < probaParTick) 
		{
			// Conséquence de l'accident
			_root.subArgent(5000);
			_root.afficher_overlay_accident();
			// Note: On ne met pas forcément en panne ici, sauf si c'est souhaité. 
			// Le code original ne mettait pas _estEnPanne = true dans CheckAccident.
		}
	}

	// --- LOGIQUE PANNE ---
	public void CheckEnPanne()
	{
		if (_estEnPanne) return;

		double targetProbOneMinute = GetTargetPanneProbability();
		double ticksPerMinute = 60.0 / 0.34;
		double probaParTick = 1.0 - Math.Pow(1.0 - targetProbOneMinute, 1.0 / ticksPerMinute);

		double tirage = _rng.NextDouble();

		if (tirage < probaParTick)
		{
			_estEnPanne = true;
			_sprite.Texture = GD.Load<Texture2D>("res://image/Machine1EnPanne.png");
			GD.Print("La machine 3 est tombée en panne !");
			UpdateStats(); 
		}
	} 

	// --- FONCTIONS D'AFFICHAGE STATISTIQUES ET VALEURS CIBLES ---

	public double GetPourcentageAccident()
	{
		// Pour l'affichage, on renvoie le % par minute
		return GetTargetAccidentProbability() * 100.0;
	}

	public double GetPourcentagePanne()
	{
		return GetTargetPanneProbability() * 100.0;
	}

	// Nouvelle méthode pour définir les probabilités d'accident demandées
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
			case 1: return 0.10; // 10% / min
			case 2: return 0.14; // 14% / min
			case 3: return 0.17; // 17% / min
			case 4: return 0.22; // 22% / min
			case 5: return 0.35; // 35% / min
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
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine3frame0.png");
		}
	}
}
