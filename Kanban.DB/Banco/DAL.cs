namespace Kanban.DB.Banco;

public class DAL<T> where T : class
{
    private readonly KanbanContext context;
    
    public DAL(KanbanContext context)
    {
        this.context = context;
    }

    public IEnumerable<T> Listar()
    {
        return context.Set<T>().ToList();
    }
}