namespace BulletinBoard.Application.Repositories;

public interface ITenant
{
    IBulletinRepository Bulletins { get; }
    IUserRepository Users { get; }

    Task CommitAsync(CancellationToken cancellationToken);
}