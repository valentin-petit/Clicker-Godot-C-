using Godot;
using System;

public partial class ButtonPlusVente : Button
{
	private ControlVente _rootVente;
	private Label _labelVente;
	
	
	public override void _Ready()
	{
		_rootVente = GetParent<ControlVente>();
		_labelVente = _rootVente.GetNode<Label>("LabelPrixVente");



		this.Pressed+=AugmenterPrix;
	}
	
	
	private void AugmenterPrix()
	{
		
		_rootVente._prixVente+=1;
		GD.Print(_rootVente._prixVente);
		_labelVente.Text="Prix de vente:"+ _rootVente._prixVente+" $";
		
	}
}
