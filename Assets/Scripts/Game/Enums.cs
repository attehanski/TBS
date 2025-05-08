public class Enums
{
    public enum Direction
    {
        None = 0,
        Right = -1,
        Left = 1,
        Up,
        Down
    }

    public enum InputState
    {
        Down,
        Up,
        Held,
        Default
    }

    public enum InputType
    {
        Left,
        Right,
        Up,
        Down,
        Jump,
        Dash,
        Interact,
        Tool,
        Cancel,
        Attack,
        Option1,
        Option2,
        Option3
    }

    public enum InteractionType
    {
        Jump,
        Activate,
        Collision
    }

    public enum Animation
    {
        Default,
        Idle,
        Run,
        Jump,
        Fall
    }

    public enum TileState
    {
        Default,
        Highlighted,
        Unavailable
    }
}