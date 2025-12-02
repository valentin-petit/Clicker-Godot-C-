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

	public override void _Ready()
	{
		_root = GetTree().Root.GetNode<nodeRootPrincipal>("nodeRootPrincipal");
		if (_root == null)
			GD.Print("Erreur : impossible de récupérer nodeRootPrincipal !");
		_lblVitesse=GetNode<Label>("lblVitesse");
		_btnPlus=GetNode<Button>("btnPlus");
		_btnMoins=GetNode<Button>("btnMoins");
		
		_lblVitProdMachine = GetNode<Label>("lblVitProdMachine");
		_lblAccident = GetNode<Label>("lblAccident");
		_lblPanne = GetNode<Label>("lblPanne");
		
		_btnPlus.Pressed += augmenterVitesse;
		_btnMoins.Pressed += baisserVitesse;
		
		_timer = _root.GetNode<Timer>("tmrMachine");
		_timer.Timeout += OnTmrMachineFinished;
		_timer.Timeout += CheckEnPanne;
		
		UpdateVitesseProduction();
		UpdateStats();
	}
	public override void _Process(double delta)
	{

	}

	public void OnTmrMachineFinished()
	{
		if(_estEnPanne!=true)
		{
			Checkaccident();
			AjouterStock();
		}
	}

	public void augmenterVitesse()
	{
		if (_vitesse<5)
		{
			_vitesse+=1;
			_lblVitesse.Text="Vitesse N°"+_vitesse;
			UpdateVitesseProduction();
			UpdateStats();
		}
	}
	public void baisserVitesse()
	{
		if (_vitesse>1)
		{
			_vitesse-=1;
			_lblVitesse.Text="Vitesse N°"+_vitesse;
			UpdateVitesseProduction();
			UpdateStats();
		}
	}

	private void UpdateVitesseProduction()
	{    	
		// si j'ai bien compris quand _vitesse vaut 1 on transforme 2 stocks
		double vitesseProd = _vitesse * 2;  
		
		_lblVitProdMachine.Text = "Vitesse de Production : \n" + vitesseProd.ToString("0.00") + " pièces/s";
	}
	
	// pour les accidents et panne
	private void UpdateStats()
	{
		double pourcentageAccident = _vitesse * 2.5; 
		double pourcentagePanne = Math.Min(100, _vitesse * 1.5); //100 c'est la limite mais on la bougera pcq 100% de panne voila

		_lblAccident.Text = "Accident = " + pourcentageAccident.ToString("0.0") + "%";
		_lblPanne.Text = "Panne = " + pourcentagePanne.ToString("0.0") + "%";
	}

	public void AjouterStock()
	{
		for (int i=0; i<_vitesse;i++)
		{
			if(_root.getStockChaussette()>=2)
			{
				_root.addStockProduitFini(1);
				_root.subStockChaussette(2);
			}
		}
		
	}
	
	public void Checkaccident()
	{
		Random rng = new Random();

		double proba = (Math.Sqrt(_vitesse) / 100.0)/1.7;
		double tirage = rng.NextDouble(); // renvoie un double entre 0.0 et 1.0

		if (tirage < proba)
		{
			GD.Print("Accident");
			int tirage2 = rng.Next(100); // nombre entre 0 et 99

			if (tirage2 < 60)                  // 0 à 59 → 60%
			{
				GD.Print("il s'est coupé\nvous payer 1000$ de compenssation");
				_root.subArgent(1000);
			}
			else if (tirage2 < 95)             // 60 à 89 → 30%
			{
				GD.Print("il a le brat cassé\nvous payer 7000$ de compenssation");
				_root.subArgent(7000);
			}
			else                              // 90 à 99 → 10%
			{
				GD.Print("un employer est tomber dans une machine\nil est mort\nvous payer 55000$ de compenssation");
				_root.subArgent(55000);
			}

		}
	}
	public void CheckEnPanne()
	{
		
		Random rng = new Random();

		double proba = Math.Sqrt(_vitesse) / 17.0;
		double tirage = rng.NextDouble(); // renvoie un double entre 0.0 et 1.0

		if (tirage< proba)
		{
			_estEnPanne=true;
			Sprite2D sprite = GetNode<Sprite2D>("Area2DMachine/Sprite2DMachine");
			sprite.Texture = GD.Load<Texture2D>("res://image/Machine1EnPanne.png");

		}
	}
}
