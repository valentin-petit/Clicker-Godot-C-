using Godot;
using System;

public partial class MenuTel : Sprite2D
{
	private AuditSceneFactory _factory;
	
	private Button _btnSecurite; 
	private Button _btnQualite; 
	private Button _btnFiabilite;
	
	[Export] public SceneController MainController; // permet de définir dans l'inspecteur de sprMenuTel le controller associé
		
	public override void _Ready()
	{
		_factory = GetNodeOrNull<AuditSceneFactory>("../AuditFactoryNode");
		
		//debug
		if (_factory == null)
		{
			Console.Error.WriteLine("Erreur : Le nœud AuditFactoryNode est introuvable. Vérifiez le chemin : ../AuditFactoryNode");			
		}
		else
		{
			Console.WriteLine("Succès : AuditSceneFactory trouvé et lié !");
		}
		
		//ca récup les différents boutons		
		_btnSecurite = GetNode<Button>("btnSecurite");
		_btnQualite = GetNode<Button>("btnQualite");
		_btnFiabilite = GetNode<Button>("btnFiabilite");
		
		//chaque boutons est caractérisé par un ID ("S", "F", "Q") et cet ID est mis en paramètre de la méthode commune OnAuditButtonPressed
		_btnSecurite.Pressed += () => { 
			if (MainController != null) {
				MainController.SelectAuditPanel("S"); 
			}
		};
		
		_btnQualite.Pressed += () => { 
			if (MainController != null) {
				MainController.SelectAuditPanel("Q"); 
			}
		};
		
		_btnFiabilite.Pressed += () => { 
			if (MainController != null) {
				MainController.SelectAuditPanel("F"); 
			}
		};
	}
	
}	
