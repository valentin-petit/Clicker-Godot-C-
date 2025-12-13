using Godot;
using System;

public partial class btnReparer3 : TextureButton
{
	private Machine3Container _machineContainer; 
	
	public override void _Ready()
	{
		
		_machineContainer = GetNode<Machine3Container>("../"); 
		
		Pressed += OnCliquerReparer;
	}

	private void OnCliquerReparer()
	{
		if (_machineContainer != null)
		{
			_machineContainer.Reparer(); 
		}
	}
}
