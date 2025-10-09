using Godot;
using System;

public partial class nodeRootPrincipal : Node2D
{
	private int _argent=5000;
	private int _stock=0;
	private int _reputation=50;
	
	 Label _lblReputation;
	private Label _lblArgent;
	private Label _lblStock;
	
	public Node2D _sceneAmelioration;

	public override void _Ready()
	{
		// instanciation des labels
		_lblReputation = GetNode<Label>("Sprite2DFond/lblReputation");
		_lblArgent = GetNode<Label>("Sprite2DFond/lblArgent");
		_lblStock = GetNode<Label>("Sprite2DFond/lblStock");
		
		this.addReputation(0);//cela permet d'ecrir la valeur de reputation dans le label
		this.addArgent(0);
		this.addStock(0);
		
		//creation de la scene amelioration
		PackedScene ps = GD.Load<PackedScene>("res://scenes/amelioration.tscn");
		_sceneAmelioration = (Node2D)ps.Instantiate();
		AddChild(_sceneAmelioration);
		_sceneAmelioration.Hide(); 
		
		
		
		
	}
	public override void _Process(double delta)
	{
		
	}
	
	public void addArgent(int ajout)
	{
		
		this._argent+= ajout;
		_lblArgent.Text="Argent : "+this._argent;
	}
	public void addStock(int ajout)
	{
		this._stock+= ajout;
		_lblStock.Text="Stock : "+this._stock;
	}
	public void addReputation(int ajout)
	{
		
		this._reputation+= ajout;
		_lblReputation.Text="Reputation : "+this._reputation;
	}
	
	public int getArgent()
	{
		return this._argent;
	}
	public int getStock()
	{
		return this._stock;
	}
	public int getReputation()
	{
		return this._reputation;
	}
}
