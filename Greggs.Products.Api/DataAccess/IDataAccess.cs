using System.Collections.Generic;

namespace Greggs.Products.Api.DataAccess;

public interface IDataAccess<out T>
{
    IEnumerable<T> GetMenu(int? pageStart, int? pageSize);
}