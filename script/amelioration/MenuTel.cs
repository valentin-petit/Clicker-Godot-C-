using Godot;
using System;

public partial class MenuTel : Sprite2D
{
	private AuditSceneFactory _factory;
	
	private Button _btnSecurite; 
	private Button _btnQualite; 
	private Button _btnFiabilite;
	
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
		
		//chaque boutons est caractérisé par un ID et c'est ID est mis en paramètre de la méthode commune OnAuditButtonPressed
		_btnSecurite.Pressed += () => OnAuditButtonPressed("S");
		_btnQualite.Pressed += () => OnAuditButtonPressed("Q");
		_btnFiabilite.Pressed += () => OnAuditButtonPressed("F");
	}

	public void OnAuditButtonPressed(string auditKey)
	{			
		// si la scene existe, on s'y deplace (en prenant le paramètre du genre "S" --> AuditSecurite
		if (_factory != null)
		{
			string scenePath = _factory.GetScenePath(auditKey);
			
			if (scenePath != null)
			{
				// Enleve la scene actuelle
				GetTree().ChangeSceneToFile(scenePath); 
			}
			else
			{
				Console.Error.WriteLine($"Erreur : Chemin de scène non trouvé pour la clé : {auditKey}");
			}
		}
	}
}
