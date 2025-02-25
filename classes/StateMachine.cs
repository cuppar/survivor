using System;
using Godot;

namespace Survivor.Classes;

public partial class StateMachine<TState> : Node
    where TState : struct, Enum
{
    private TState _currentState;

    public double StateTime { get; set; }

    public TState CurrentState
    {
        get => _currentState;
        private set
        {
            if (Owner is not IStateMachine<TState> owner)
                throw new OwnerIsNotIStateMachineException();

            owner.TransitionState(CurrentState, value);
            _currentState = value;
            StateTime = 0;
        }
    }

    public static StateMachine<TState> Create(Node owner)
    {
        var machine = new StateMachine<TState>();
        machine.Name = "StateMachine";
        owner.AddChild(machine);
        machine.Owner = owner;
        return machine;
    }

    public override async void _Ready()
    {
        await ToSignal(Owner, SignalName.Ready);
        CurrentState = Enum.GetValues<TState>()[0];
    }

    public override void _PhysicsProcess(double delta)
    {
        if (Owner is not IStateMachine<TState> owner)
            throw new OwnerIsNotIStateMachineException();

        while (true)
        {
            var next = owner.GetNextState(CurrentState, out var keepCurrent);
            if (keepCurrent)
                break;
            CurrentState = next;
        }

        owner.TickPhysics(CurrentState, delta);
        StateTime += delta;
    }

    #region Nested type: OwnerIsNotIStateMachine

    private class OwnerIsNotIStateMachineException() : Exception("Owner is not IStateMachine who has a state machine");

    #endregion
}