using System;

namespace StudyConnect.Data.Repositories;

public class BaseRepository
{
    protected readonly StudyConnectDbContext _context;

    public BaseRepository(StudyConnectDbContext context)
    {
        _context = context ?? throw new ArgumentNullException(nameof(context));
    }
}
