using Godot;
using System;

public partial class btnReparer1 : TextureButton
{
	private Machine1Container _machineContainer; 
	
	public override void _Ready()
	{
		
		_machineContainer = GetNode<Machine1Container>("../"); 
		
		Pressed += OnCliquerReparer;
	}

	private void OnCliquerReparer()
	{
		if (_machineContainer != null)
		{
			// Appel de la fonction de réparation
			_machineContainer.Reparer(); 
		}
	}
}
