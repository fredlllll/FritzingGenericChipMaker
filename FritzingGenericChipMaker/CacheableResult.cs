using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace FritzingGenericChipMaker
{
    public class CacheableResult<DEPENDENCYTYPE, RESULTTYPE>
    {
        bool first = true;
        RESULTTYPE cachedResult = default(RESULTTYPE);
        DEPENDENCYTYPE lastCacheDependency = default(DEPENDENCYTYPE);

        Func<DEPENDENCYTYPE> GetCacheDependency;
        Func<RESULTTYPE> GetNewResult;

        public CacheableResult(Func<DEPENDENCYTYPE> getCacheDependency, Func<RESULTTYPE> getNewResult)
        {
            GetCacheDependency = getCacheDependency;
            GetNewResult = getNewResult;
        }

        public RESULTTYPE Get()
        {
            DEPENDENCYTYPE cache = GetCacheDependency();
            if(first || !EqualityComparer<DEPENDENCYTYPE>.Default.Equals(lastCacheDependency, cache))
            {
                first = false;
                lastCacheDependency = cache;
                cachedResult = GetNewResult();
            }
            return cachedResult;
        }

        public static implicit operator RESULTTYPE(CacheableResult<DEPENDENCYTYPE,RESULTTYPE> val) {
            return val.Get();
        }
    }
}
