using Godot;
using System;

public partial class nodeRootPrincipal : Node2D
{
	private float _argent=5000.00f;
	
	private float _reputation=50;
	
	private int _stockLaine=150;
	private int _stockTissue=0;
	private int _stockChaussette=0;//le produit n'est pas fini ilrest l'embalage
	private int _stockProduitFini=0;
	// float:F2 pour garder 2 decimal, valeur = (float)Math.Round(valeur, 2);
	
	private PackedScene _machine1Scene = GD.Load<PackedScene>("res://scenes/machine1.tscn");
	private PackedScene _machine2Scene = GD.Load<PackedScene>("res://scenes/machine2.tscn");
	private PackedScene _machine3Scene = GD.Load<PackedScene>("res://scenes/machine3.tscn");
	
	public HBoxContainer machinesContainer; 
	
	public VBoxContainer colonne1;
	public VBoxContainer colonne2;
	public VBoxContainer colonne3;
	
	public int _machineCountCol1 = 0;
	public int _machineCountCol2 = 0;
	public int _machineCountCol3 = 0;
	
	private Label _lblReputation;
	private Label _lblArgent;
	
	private Label _lblStockLaine;
	private Label _lblStockTissue;
	private Label _lblStockChaussette;
	private Label _lblStockProduitFini;
	
	private Button _btnAchatLaine;
	
	public Node2D _sceneAmelioration;
	
	

	public override void _Ready()
	{
		// instanciation des labels
		_lblReputation = GetNode<Label>("Sprite2DFond/lblReputation");
		_lblArgent = GetNode<Label>("Sprite2DFond/lblArgent");
		
		_lblStockLaine = GetNode<Label>("Sprite2DFond/lblStokLaine");
		_lblStockTissue = GetNode<Label>("Sprite2DFond/lblStockTissue");
		_lblStockChaussette = GetNode<Label>("Sprite2DFond/lblStockChaussette");
		_lblStockProduitFini = GetNode<Label>("Sprite2DFond/lblStockProduitFini");
		
		_btnAchatLaine = GetNode<Button>("Sprite2DFond/btnAchatLaine");
		_btnAchatLaine.Pressed+=achatLaine;
		
		
		
		_lblArgent.Text += " " + _argent.ToString("F2");
		_lblReputation.Text += " " + _reputation;

		_lblStockLaine.Text += " " + _stockLaine;
		_lblStockTissue.Text += " " + _stockTissue;
		_lblStockChaussette.Text += " " + _stockChaussette;
		_lblStockProduitFini.Text += " " + _stockProduitFini;
		
		
		//creation de la scene amelioration
		PackedScene ps = GD.Load<PackedScene>("res://scenes/amelioration.tscn");
		_sceneAmelioration = (Node2D)ps.Instantiate();
		AddChild(_sceneAmelioration);
		_sceneAmelioration.Hide(); 
		


		// instanciation des machines
		machinesContainer = GetNode<HBoxContainer>("Sprite2DFond/machinesContainer");
		
		colonne1 = GetNode<VBoxContainer>("Sprite2DFond/machinesContainer/colonne1");
		colonne2 = GetNode<VBoxContainer>("Sprite2DFond/machinesContainer/colonne2");
		colonne3 = GetNode<VBoxContainer>("Sprite2DFond/machinesContainer/colonne3");
				
		// Colonne 1
		_machineCountCol1++;
		AddNewMachine(colonne1, _machineCountCol1);
	
		// Colonne 2
		_machineCountCol2++;
		AddNewMachine(colonne2, _machineCountCol2);

		// Colonne 3
		_machineCountCol3++;
		AddNewMachine(colonne3, _machineCountCol3);
		
		//creation de l'acceuille
		PackedScene ac = GD.Load<PackedScene>("res://scenes/acceuille.tscn");
		Control _sceneAcceuille = (Control)ac.Instantiate();
		_sceneAcceuille.Size = GetViewport().GetVisibleRect().Size;

		AddChild(_sceneAcceuille);
		
	}
	
	public void AddNewMachine(VBoxContainer colonne, int machineNumber)
	{
		
		Control machineContainer;
		if (colonne==colonne1)
		{
			machineContainer = (Control)_machine1Scene.Instantiate();
		}
		else if (colonne==colonne2)
		{
			machineContainer = (Control)_machine2Scene.Instantiate();
		}
		else
		{
			machineContainer = (Control)_machine3Scene.Instantiate();
		}
		
		Label machineLabel = machineContainer.GetNode<Label>("lblNumMachine"); 

		if (machineLabel != null)
		{
			machineLabel.Text = $"Machine N° {machineNumber}";
		}
	
		Area2D machineArea = machineContainer.GetNode<Area2D>("Area2DMachine"); 

		if (machineArea != null)
		{
			machineArea.InputPickable = false;
		}
		colonne.AddChild(machineContainer);
		
	}
	
	
	public override void _Process(double delta)
	{
		
	}
	public void achatLaine()
	{
		if(_argent>150)
		{
			subArgent(150);
			addStockLaine(10);
			
		}
	}
	
	// ARGENT
	public void addArgent(float ajout)
	{
		_argent += ajout;
		_lblArgent.Text = "Argent : " + _argent.ToString("F2");
	}

	public void subArgent(float retrait)
	{
		_argent -= retrait;
		_lblArgent.Text = "Argent : " + _argent.ToString("F2");
	}

	public float getArgent()
	{
		return (float)Math.Round(_argent, 2);
	}

	// RÉPUTATION
	public void addReputation(float ajout)
	{
		_reputation += ajout;
		_lblReputation.Text = "Reputation : " + _reputation;
	}

	public void subReputation(float retrait)
	{
		_reputation -= retrait;
		_lblReputation.Text = "Reputation : " + _reputation;
	}

	public float getReputation()
	{
		return _reputation;
	}

	// STOCK PRODUIT FINI
	public void addStockProduitFini(int ajout)
	{
		_stockProduitFini += ajout;
		_lblStockProduitFini.Text = "Stock de Produit Finit: " + _stockProduitFini;
	}

	public void subStockProduitFini(int retrait)
	{
		_stockProduitFini -= retrait;
		_lblStockProduitFini.Text = "Stock de Produit Finit: " + _stockProduitFini;
	}

	public int getStockProduitFini()
	{
		return _stockProduitFini;
	}

	// STOCK LAINE
	public void addStockLaine(int ajout)
	{
		_stockLaine += ajout;
		_lblStockLaine.Text = "Stock de Laine: " + _stockLaine;
		
	}

	public void subStockLaine(int retrait)
	{
		_stockLaine -= retrait;
		_lblStockLaine.Text = "Stock de Laine: " + _stockLaine;
		
	}

	public int getStockLaine()
	{
		return _stockLaine;
	}

	// STOCK TISSUE
	public void addStockTissue(int ajout)
	{
		_stockTissue += ajout;
		_lblStockTissue.Text = "Stock de Tissue: " + _stockTissue;
	}

	public void subStockTissue(int retrait)
	{
		_stockTissue -= retrait;
		_lblStockTissue.Text = "Stock de Tissue: " + _stockTissue;
	}

	public int getStockTissue()
	{
		return _stockTissue;
	}

	// STOCK CHAUSSETTE
	public void addStockChaussette(int ajout)
	{
		_stockChaussette += ajout;
		_lblStockChaussette.Text = "Stock de Chaussette: " + _stockChaussette;
	}

	public void subStockChaussette(int retrait)
	{
		_stockChaussette -= retrait;
		_lblStockChaussette.Text = "Stock de Chaussette: " + _stockChaussette;
	}

	public int getStockChaussette()
	{
		return _stockChaussette;
	}


}
