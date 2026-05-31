using MediatR;
using SMT.Application.Dtos;

namespace SMT.Application.Years.CreateTable;

public record CreateTableCommand(
    Guid UserId,
    string NameTable
) : IRequest<YearDTO>;