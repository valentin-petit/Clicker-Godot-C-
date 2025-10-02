using Godot;
using System;

public partial class ButtonMoinsVente : Button
{
	private ControlVente _rootVente;
	private Label _labelVente;
	
	
	public override void _Ready()
	{
		_rootVente = GetParent<ControlVente>();
		_labelVente = _rootVente.GetNode<Label>("LabelPrixVente");


		this.Pressed+=BaisserPrix;
	}
	
	
	private void BaisserPrix()
	{
		if(_rootVente._prixVente>1)
		{
			_rootVente._prixVente-=1;
			_labelVente.Text="Prix de vente:"+ _rootVente._prixVente+" $";
		}
	}
}
