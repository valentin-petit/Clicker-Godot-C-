using Godot;
using System;

public partial class TxtbtnSignatureQuitter : TextureButton
{
	private nodeRootPrincipal _root ;	
	
	
	public override void _Ready()
	{
		
		_root = GetTree().CurrentScene as nodeRootPrincipal;
		//prend la root node de prinipal
		this.Pressed+=FermerTout;
	}
	public override void _Process(double delta)
	{
		
	}
	
	private void FermerTout()
	{
		
		GD.Print("Entrée dans la méthode fermertout réussi");
		if (_root == null)
		{
			GD.PrintErr("ERREUR : La racine _root n'a pas été trouvée.");
			return;
		}

		//cache le panneau parent audit du bonton cliqué (Audit Sécurité si on affiche celui ci, etc ..)
		CanvasItem panneauAudit = GetParent().GetParent() as CanvasItem; //GetParent().GetParent() --> txtbtnSignatureQuitter --> sprSecuF1 --> AuditSecurtite
		if (panneauAudit != null)
		{
			panneauAudit.Hide();
			GD.Print($"{panneauAudit.Name} a été caché avec succès");
		}
		else
		{
			GD.PrintErr("ERREUR : {panneauAudit.Name} n'a pas pu être converti et caché");
		}

		//on déclare sprMenuTel en tant qu'enfant de la scène amélio
		Sprite2D sprMenuTel = _root._sceneAmelioration.GetNodeOrNull<Sprite2D>("sprMenuTel");
		
		if (sprMenuTel != null)
		{
			sprMenuTel.Hide();
			GD.Print("sprMenuTel a été caché avec succès");
		}
		else
		{
			GD.PrintErr("ERREUR : sprMenuTel introuvable sous la scène _root._sceneAmelioration.");
		}
	}
}
