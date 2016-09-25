using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public class CacheableResult<T, U>
    {
        bool first = true;
        U cachedResult = default(U);
        T lastCacheDependency = default(T);

        Func<T> GetCacheDependency;
        Func<U> GetNewResult;

        public CacheableResult(Func<T> getCacheDependency, Func<U> getNewResult)
        {
            GetCacheDependency = getCacheDependency;
            GetNewResult = getNewResult;
        }

        public U Get()
        {
            T cache = GetCacheDependency();
            if(first || !EqualityComparer<T>.Default.Equals(lastCacheDependency, cache))
            {
                first = false;
                lastCacheDependency = cache;
                cachedResult = GetNewResult();
            }
            return cachedResult;
        }
    }
}
