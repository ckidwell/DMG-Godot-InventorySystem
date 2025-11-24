using Godot;
using System;

public partial class UseItemEventDataLabel : Label
{
    private Timer _timer;
    public override void _Ready()
    {
        base._Ready();
        _timer = GetNode<Timer>("Timer");
        _timer.Connect("timeout", Callable.From(OnTimerTimeout));
    }

    private void OnTimerTimeout()
    {
        QueueFree();
    }
}
