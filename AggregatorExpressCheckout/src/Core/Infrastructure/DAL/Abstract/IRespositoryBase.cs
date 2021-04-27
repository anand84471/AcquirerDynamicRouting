using Core.Infrastructure.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace Core.Infrastructure.DAL.Abstract
{
    /*
     * T is a generic model enitity
     *
     */

    public interface IEntityRespositoryBase<T> where T : class, IEntity
    {
        Task<bool> Save(T enity);

        Task<bool> Update<ID>(T enitity, ID pk);

        Task<T> GetByID<ID>(ID pk);

        Task<List<T>> GetAll();

        Task<List<T>> GetListByID<ID>(ID pk);

        Task<bool> Delete<ID>(ID pk);
    }
}