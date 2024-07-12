using System.ComponentModel.DataAnnotations;
using Kanban.Shared.Models.Models;

namespace Kanban.API.Request;
public record BoardsRequest([Required] string Name, [Required] DateTime CreatedAt);
