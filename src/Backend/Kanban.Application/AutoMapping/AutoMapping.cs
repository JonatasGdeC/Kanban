using AutoMapper;
using Kanban.Communication.Dtos;
using Kanban.Communication.Requests.Board;
using Kanban.Communication.Requests.Column;
using Kanban.Communication.Requests.SubTask;
using Kanban.Communication.Requests.Task;
using Kanban.Communication.Requests.User;

namespace Kanban.Application.AutoMapping;
using Domain.Entities;

public class AutoMapping : Profile
{
    public AutoMapping()
    {
        RequestToEntity();
        EntityToResponse();
    }

    private void RequestToEntity()
    {
        CreateMap<RegisterUserRequest, User>();
        CreateMap<RegisterBoardRequest, Board>();
        CreateMap<RegisterColumnRequest, Column>();
        CreateMap<UpdateColumnRequest, Column>();
        CreateMap<RegisterTaskRequest, TaskEntity>();
        CreateMap<UpdateTaskRequest, TaskEntity>();
        CreateMap<RegisterSubTaskRequest, SubTask>();
        CreateMap<UpdateSubTaskRequest, SubTask>();
    }
    
    private void EntityToResponse()
    {
        CreateMap<User, UserDto>();
        CreateMap<Board, BoardDto>();
        CreateMap<Column, ColumnDto>();
        CreateMap<TaskEntity, TaskDto>();
        CreateMap<SubTask, SubTaskDto>();
    }
}
