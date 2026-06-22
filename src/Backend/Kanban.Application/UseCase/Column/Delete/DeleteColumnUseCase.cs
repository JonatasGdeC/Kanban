using Kanban.Domain.Repositories;
using Kanban.Domain.Repositories.Column;
using Kanban.Domain.Services.LoggedUser;
using Kanban.Exception;
using Kanban.Exception.ExceptionBase;

namespace Kanban.Application.UseCase.Column.Delete;
using Domain.Entities;

public class DeleteColumnUseCase(
    IColumWriteRepository writeRepository,
    ILoggedUser loggedUser,
    IUnitOfWork unitOfWork) : IDeleteColumnUseCase
{
    public async Task Execute(Guid id)
    {
        Column column = await GetValidatedColumn(id: id);
        writeRepository.Delete(column: column);
        await unitOfWork.Commit();
    }

    private async Task<Column> GetValidatedColumn(Guid id)
    {
        User user = await loggedUser.Get();
        Column? column = await writeRepository.GetById(id: id, userId: user.Id);

        if (column == null)
        {
            throw new NotFoundException(message: ResourceErrorMessage.COLUMN_NOT_FOUND);
        }

        return column;
    }
}
