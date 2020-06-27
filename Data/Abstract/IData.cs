using Data.Core;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;

namespace Data.Abstract
{
    public interface IData<T>
    {
        DataResult<T> Insert(T t);
        DataResult<T> Update(T t);
        DataResult<bool> Update(T data, Expression<Func<T, bool>> predicate, bool IsUpSert = false);
        DataResult<bool> Delete(T t);
        DataResult<bool> DeleteByKey(long id);

        T GetByKey(long id);
        T GetByKey(string id);
        List<T> GetAll();
        List<T> GetAll(string orderBy, bool isDesc = false);
        List<T> GetBy(Expression<Func<T, bool>> predicate);
        List<T> GetRandom(int limit);
        List<T> GetRandom(Expression<Func<T, bool>> predicate, int limit);
        List<T> GetByPage(Expression<Func<T, bool>> predicate, int pageNumber, int pageCount, string orderBy = "Id", bool isDesc = false);

        int GetCount();
        int GetCount(Expression<Func<T, bool>> predicate);


    }
}
