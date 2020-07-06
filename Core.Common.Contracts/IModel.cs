using System;

namespace Core.Common.Contracts
{
    /// <summary>
    /// interface for all base data model
    /// </summary>
    public interface IModel
    {
        int Id { get; set; }
        DateTime Created { get; set; }
        DateTime LastUpdated { get; set; }
    }
}
