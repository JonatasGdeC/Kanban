using Kanban.Application.UseCase.User.Delete;
using Kanban.Application.UseCase.User.Login;
using Kanban.Application.UseCase.User.Register;
using Kanban.Application.UseCase.User.Update;
using Kanban.Application.UseCase.Board.Delete;
using Kanban.Application.UseCase.Board.GetAll;
using Kanban.Application.UseCase.Board.GetById;
using Kanban.Application.UseCase.Board.Register;
using Kanban.Application.UseCase.Board.Update;
using Kanban.Application.UseCase.Column.Delete;
using Kanban.Application.UseCase.Column.GetAll;
using Kanban.Application.UseCase.Column.GetById;
using Kanban.Application.UseCase.Column.Register;
using Kanban.Application.UseCase.Column.Update;
using Kanban.Application.UseCase.TaskEntity.Delete;
using Kanban.Application.UseCase.TaskEntity.GetAll;
using Kanban.Application.UseCase.TaskEntity.GetById;
using Kanban.Application.UseCase.TaskEntity.Register;
using Kanban.Application.UseCase.TaskEntity.Update;
using Kanban.Application.UseCase.SubTask.Delete;
using Kanban.Application.UseCase.SubTask.GetAll;
using Kanban.Application.UseCase.SubTask.Register;
using Kanban.Application.UseCase.SubTask.Update;
using Kanban.Application.UseCase.User.ForgotPassword;
using Kanban.Application.UseCase.User.Get;
using Kanban.Application.UseCase.User.ResetPassword;
using Kanban.Application.UseCase.User.UpdatePassword;
using Kanban.Application.UseCase.User.ValidateResetCode;
using Microsoft.Extensions.DependencyInjection;

namespace Kanban.Application;

public static class DependencyInjection
{
    public static void AddApplication(this IServiceCollection services)
    {
        AddAutoMapperApplication(services: services);
        AddUseCases(services: services);
    }

    private static void AddAutoMapperApplication(this IServiceCollection services)
    {
        services.AddAutoMapper(configAction: config => { }, typeof(AutoMapping.AutoMapping));
    }

    private static void AddUseCases(this IServiceCollection services)
    {
        services.AddScoped<IDeleteUserUseCase, DeleteUserUseCase>();
        services.AddScoped<ILoginUseCase, LoginUseCase>();
        services.AddScoped<IRegisterUserUseCase, RegisterUserUseCase>();
        services.AddScoped<IUpdateUserUseCase, UpdateUserUseCase>();
        services.AddScoped<IUpdatePasswordUseCase, UpdatePasswordUseCase>();
        services.AddScoped<IGetUserUseCase, GetUserUseCase>();
        services.AddScoped<IForgotPassword, ForgotPassword>();
        services.AddScoped<IValidateResetCodeUseCase, ValidateResetCodeUseCase>();
        services.AddScoped<IResetPassword, ResetPassword>();

        services.AddScoped<IDeleteBoardUseCase, DeleteBoardUseCase>();
        services.AddScoped<IGetAllBoardsUseCase, GetAllBoardsUseCase>();
        services.AddScoped<IGetBoardByIdUseCase, GetBoardByIdUseCase>();
        services.AddScoped<IRegisterBoardUseCase, RegisterBoardUseCase>();
        services.AddScoped<IUpdateBoardUseCase, UpdateBoardUseCase>();

        services.AddScoped<IDeleteColumnUseCase, DeleteColumnUseCase>();
        services.AddScoped<IGetAllColumnsUseCase, GetAllColumnsUseCase>();
        services.AddScoped<IGetColumnByIdUseCase, GetColumnByIdUseCase>();
        services.AddScoped<IRegisterColumnUseCase, RegisterColumnUseCase>();
        services.AddScoped<IUpdateColumnUseCase, UpdateColumnUseCase>();

        services.AddScoped<IDeleteTaskUseCase, DeleteTaskUseCase>();
        services.AddScoped<IGetAllTasksUseCase, GetAllTasksUseCase>();
        services.AddScoped<IGetTaskByIdUseCase, GetTaskByIdUseCase>();
        services.AddScoped<IRegisterTaskUseCase, RegisterTaskUseCase>();
        services.AddScoped<IUpdateTaskUseCase, UpdateTaskUseCase>();

        services.AddScoped<IDeleteSubTaskUseCase, DeleteSubTaskUseCase>();
        services.AddScoped<IGetAllSubTasksUseCase, GetAllSubTasksUseCase>();
        services.AddScoped<IRegisterSubTaskUseCase, RegisterSubTaskUseCase>();
        services.AddScoped<IUpdateSubTaskUseCase, UpdateSubTaskUseCase>();
    }
}