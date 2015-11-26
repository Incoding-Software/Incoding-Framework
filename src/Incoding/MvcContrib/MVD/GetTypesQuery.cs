namespace Incoding.MvcContrib.MVD
{
    #region << Using >>

    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Reflection;
    using Incoding.CQRS;
    using Incoding.Data;
    using Incoding.Extensions;

    #endregion

    internal class GetDuplicatesQuery : QueryBase<List<Type>>
    {
        #region Static Fields

        internal static readonly List<Type> duplicates = new List<Type>();

        static readonly object lockObject = new object();

        #endregion

        #region Properties

        public List<Assembly> Assemblies { get; set; }

        
        #endregion

        protected override List<Type> ExecuteResult()
        {
            ////ncrunch: no coverage start
            if (duplicates.Any())
                return duplicates;

            ////ncrunch: no coverage end
            lock (lockObject)
            {
                ////ncrunch: no coverage start
                if (duplicates.Any())
                    return duplicates;

                ////ncrunch: no coverage end
                var temp = Assemblies
                        .Select(s => s.GetLoadableTypes())
                        .SelectMany(r => r);
                

                duplicates.AddRange(temp.Where(r => temp.Count(s => s.Name == r.Name) > 1));
                return duplicates;
            }
        }
    }
}