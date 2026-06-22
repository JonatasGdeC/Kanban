namespace Kanban.App.UseState;

public abstract class UseStateOnChange
{
    public event Action? OnChange;
    protected void Notify() => OnChange?.Invoke();
}