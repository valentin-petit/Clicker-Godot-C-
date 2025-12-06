using Godot;
using System;

public partial class Machine2Container : Control
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

	// --- NOUVELLES VARIABLES (Comme Machine 1) ---
	private int _compteurTics = 0;       
	private int _ticsPourChangerFrame = 8; 
	// ---------------------------------------------

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

		// On récupère le timer global
		// Vous pouvez utiliser "tmrMachine" ou "tmrMachine2", cela n'a plus d'importance
		// pour la vitesse car on gère le délai nous-mêmes via les "tics".
		_timer = _root.GetNode<Timer>("tmrMachine"); 

		// Initialisation du délai
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


		// 2. Si pas assez de stock
		if (_root.getStockTissue() < 2)
		{
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine2frame0.png");
			return;
		}

		// --- NOUVELLE LOGIQUE DE RYTHME (Comme Machine 1) ---
		_compteurTics++; 

		// Si on n'a pas encore atteint le nombre de tics requis, on attend
		if (_compteurTics < _ticsPourChangerFrame)
		{
			return; 
		}

		// On a atteint le seuil : on reset le compteur et on lance l'animation
		_compteurTics = 0;
		// ----------------------------------------------------

		_compteur++;
		// Machine 2 a 5 frames, donc modulo 5
		int index = _compteur % 5; 

		if (index == 0) {
			Checkaccident();
			AjouterStock(); 
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine2frame0.png");
		}
		else if (index == 1) {
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine2frame1.png");
		}
		else if (index == 2) {
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine2frame2.png");
		}
		else if (index == 3) {
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine2frame3.png");
		}
		else if (index == 4) {
			_sprite.Texture = GD.Load<Texture2D>("res://image/machine2frame4.png");
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
			
			// Recalcul du rythme
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

			// Recalcul du rythme
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
		double pourcentagePanne = Math.Min(100, _vitesse * 0.1); 

		_lblAccident.Text = "Accident = " + pourcentageAccident.ToString("0.0") + "%";
		_lblPanne.Text = "Panne = " + pourcentagePanne.ToString("0.0") + "% (Est)";
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


	public void Checkaccident()
	{
		double proba = (Math.Sqrt(_vitesse) / 100.0) / 99.7;
		double tirage = _rng.NextDouble(); 

		if (tirage < proba)
		{
			GD.Print("Accident Machine 2");
			int tirage2 = _rng.Next(100); 

			if (tirage2 < 60)                
			{
				_root.subArgent(1000);
			}
			else if (tirage2 < 95)              
			{
				_root.subArgent(7000);
			}
			else                              
			{
				_root.subArgent(55000);
			}
		}
	}

	public void CheckEnPanne()
	{
		if (_estEnPanne) return;

		double proba = Math.Sqrt(_vitesse) / 5000.0; 
		double tirage = _rng.NextDouble();

		if (tirage < proba)
		{
			_estEnPanne = true;
			// Assurez-vous d'avoir une image de panne cohérente (Machine1EnPanne ou Machine2EnPanne)
			_sprite.Texture = GD.Load<Texture2D>("res://image/Machine1EnPanne.png");
			GD.Print("La machine 2 est tombée en panne !");
		}
	}
}
