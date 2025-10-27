using Godot;
using System;

public partial class nodeRootPrincipal : Node2D
{
	private float _argent=5000.00f;
	private int _stock=0;
	private float _reputation=50;
	// float:F2 pour garder 2 decimal, valeur = (float)Math.Round(valeur, 2);
	
	private PackedScene _machineScene = GD.Load<PackedScene>("res://scenes/production.tscn");
	
	public HBoxContainer machinesContainer; 
	
	public VBoxContainer colonne1;
	public VBoxContainer colonne2;
	public VBoxContainer colonne3;
	
	public int _machineCountCol1 = 0;
	public int _machineCountCol2 = 0;
	public int _machineCountCol3 = 0;
	
	private Label _lblReputation;
	private Label _lblArgent;
	private Label _lblStock;
	
	public Node2D _sceneAmelioration;
	
	

	public override void _Ready()
	{
		// instanciation des labels
		_lblReputation = GetNode<Label>("Sprite2DFond/lblReputation");
		_lblArgent = GetNode<Label>("Sprite2DFond/lblArgent");
		_lblStock = GetNode<Label>("Sprite2DFond/lblStock");
		
		this.addReputation(50);//cela permet d'ecrir la valeur de reputation dans le label
		this.addArgent(0);
		this.addStock(0);
		this.subArgent(0); // amelio
		
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
		//PackedScene ac = GD.Load<PackedScene>("res://scenes/acceuille.tscn");
		//Control _sceneAcceuille = (Control)ac.Instantiate();
		//_sceneAcceuille.Size = GetViewport().GetVisibleRect().Size;

		//AddChild(_sceneAcceuille);
		
	}
	
	public void AddNewMachine(VBoxContainer colonne, int machineNumber)
	{
		Control machineContainer = (Control)_machineScene.Instantiate();
	
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
	
	public void addArgent(float ajout)
	{
		
		this._argent+= ajout;
		_lblArgent.Text="Argent : "+this._argent.ToString("F2");
	}
	
	//test pour argent perdu en améliorant 
	public void subArgent(float depense)
	{
		this._argent-= depense;
		_lblArgent.Text="Argent : "+this._argent.ToString("F2");
	}
	
	public void addStock(int ajout)
	{
		this._stock+= ajout;
		_lblStock.Text="Stock : "+this._stock;
	}
	public void addReputation(float ajout)
	{
		
		this._reputation+= ajout;
		_lblReputation.Text="Reputation : "+this._reputation;
	}
	
	public float getArgent()
	{
		return (float)Math.Round(this._argent,2);
	}
	public int getStock()
	{
		return this._stock;
	}
	public float getReputation()
	{
		return this._reputation;
	}
}
