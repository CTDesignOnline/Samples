namespace BrambleBerry.Legacy.Services
{
    using BrambleBerry.Legacy.Models.Dto;

    using Umbraco.Core;
    using Umbraco.Core.Persistence;

    /// <summary>
    /// The legacy data service.
    /// </summary>
    public class LegacyDataService
    {
        /// <summary>
        /// The database.
        /// </summary>
        private readonly Database _database;

        /// <summary>
        /// Initializes a new instance of the <see cref="LegacyDataService"/> class.
        /// </summary>
        public LegacyDataService()
            : this(ApplicationContext.Current.DatabaseContext.Database)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="LegacyDataService"/> class.
        /// </summary>
        /// <param name="database">
        /// The database.
        /// </param>
        public LegacyDataService(Database database)
        {
            _database = database;
        }

        /// <summary>
        /// The insert.
        /// </summary>
        /// <param name="dto">
        /// The dto.
        /// </param>
        /// <returns>
        /// The <see cref="bool"/>.
        /// </returns>
        public bool AddOrUpdate(AcProductDto dto)
        {
            return (dto.Id > 0 ? ((AcProductDto)_database.Insert(dto)).Id : _database.Update(dto)) > 0; 
        }        
    }
}