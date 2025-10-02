using Godot;
using System;

public partial class btnQuitter : Button
{
	private nodeRootPrincipal _nodeRootPrincipal ;
	public override void _Ready()
	{
		
		_nodeRootPrincipal = GetTree().CurrentScene as nodeRootPrincipal;
		//prend la root node de prinipal
		this.Pressed+=FermerBureau;
	}
	public override void _Process(double delta)
	{
		
	}
	private void FermerBureau()
	{
		_nodeRootPrincipal._sceneAmelioration.Hide();
		//j'apelle l'atribut  _sceneAmelioration de nodeRootPrincipal
		//et on le cache
		//JE n'ai pas reussit a faire une methode plus propre que 
		//celle ci		
	}
}
