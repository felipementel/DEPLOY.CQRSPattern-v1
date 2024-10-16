using DEPLOY.CQRSPattern.Domain.Commands;
using DEPLOY.CQRSPattern.Domain.Queries;

namespace DEPLOY.CQRSPattern.Domain.Commands
{
    public interface ICommand
    {
    }
}

namespace DEPLOY.CQRSPattern.Domain.Queries
{
    public interface IQuery<TResult>
    {
    }
}

namespace DEPLOY.CQRSPattern.Application.Handlers
{
    public interface ICommandHandler<TCommand> where TCommand : ICommand
    {
        Task Handle(TCommand command);
    }
}

namespace DEPLOY.CQRSPattern.Application.Handlers
{
    public interface IQueryHandler<TQuery, TResult> where TQuery : IQuery<TResult>
    {
        Task<TResult> Handle(TQuery query);
    }
}