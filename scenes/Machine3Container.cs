using Godot;
using System;

public partial class Machine3Container : Control
{
	private float _vitesse = 1.0f;
	private nodeRootPrincipal _root;
	private double _timer = 0;
	
	private Label _lblVitesse;
	private Label _lblVitProdMachine;	
	private Label _lblAccident;
	private Label _lblPanne;
	
	private Button _btnPlus;
	private Button _btnMoins;

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
		
		UpdateVitesseProduction();
		UpdateStats();
	}
	public override void _Process(double delta)
	{
		_timer += delta;

		if (_timer >= 2.1)
		{
			
			_timer = 0; // Réinitialise le timer
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
}
