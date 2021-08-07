using System.Collections.Generic;
using System;
using System.IO;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Hash.Products.Domain.Repository;

namespace Hash.Products.Repository
{
    public class Repository<TEntity> : IRepository<TEntity>
        where TEntity : class
    {
        private readonly FileInfo _fileInfo;
        public Repository(FileInfo fileinfo)
        {
            _fileInfo = fileinfo ?? throw new ArgumentNullException(nameof(fileinfo));
            if(!_fileInfo.Exists) throw new DirectoryNotFoundException(nameof(fileinfo));
        }
        
        public async Task<IEnumerable<TEntity>> GetAllAsync()
        {
            using(var stream = _fileInfo.OpenRead())
            {
                using(var streamReader = new StreamReader(stream))
                {
                    var content = await streamReader.ReadToEndAsync();
                    return JsonConvert.DeserializeObject<IEnumerable<TEntity>>(content);
                }
            }
        }
    }
}