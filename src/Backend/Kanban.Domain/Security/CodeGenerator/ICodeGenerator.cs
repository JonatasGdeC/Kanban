namespace Kanban.Domain.Security.CodeGenerator;

public interface ICodeGenerator
{
    string Generate(int length = 6);
}