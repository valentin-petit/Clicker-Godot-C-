using Godot;
using System;

public partial class Machine3Container : Control
{
	private int _vitesse = 1;
	private nodeRootPrincipal _root;
	private double _timer = 0;
	private Label _lblVitesse;
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
		
		_btnPlus.Pressed += augmenterVitesse;
		_btnMoins.Pressed += baisserVitesse;
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
		}
	}
	public void baisserVitesse()
	{
		if (_vitesse>1)
		{
			_vitesse-=1;
			_lblVitesse.Text="Vitesse N°"+_vitesse;
		}
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
