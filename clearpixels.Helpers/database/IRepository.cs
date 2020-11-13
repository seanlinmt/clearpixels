
/*
 * LICENSE NOTE:
 *
 * Copyright  2012-2013 Clear Pixels Limited, All Rights Reserved.
 *
 * Unless explicitly acquired and licensed from Licensor under another license, the
 * contents of this file are subject to the Reciprocal Public License ("RPL")
 * Version 1.5, or subsequent versions as allowed by the RPL, and You may not copy
 * or use this file in either source code or executable form, except in compliance
 * with the terms and conditions of the RPL. 
 *
 * All software distributed under the RPL is provided strictly on an "AS IS" basis,
 * WITHOUT WARRANTY OF ANY KIND, EITHER EXPRESS OR IMPLIED, AND LICENSOR HEREBY
 * DISCLAIMS ALL SUCH WARRANTIES, INCLUDING WITHOUT LIMITATION, ANY WARRANTIES OF
 * MERCHANTABILITY, FITNESS FOR A PARTICULAR PURPOSE, QUIET ENJOYMENT, OR
 * NON-INFRINGEMENT. See the RPL for specific language governing rights and
 * limitations under the RPL.
 *
 * @author         Sean Lin Meng Teck <seanlinmt@clearpixels.co.nz>
 * @copyright      2012-2013 Clear Pixels Limited
 */
using System;
using System.Collections.Generic;
using System.Linq;

namespace clearpixels.Helpers.database
{
    public interface IRepository<T> where T : class
    {
        bool Any(Func<T, bool> exp);
        T Single(Func<T, bool> exp);
        T SingleOrDefault(Func<T, bool> exp);
        T FirstOrDefault(Func<T, bool> exp);
        IQueryable<T> GetAll();
        void SaveOrUpdate(T entity);
        void MarkForDeletion(T entity);
        void MarkAllForDeletion(IEnumerable<T> entity);
        T CreateInstance();
    }

    public interface IRepository
    {
        object Single(int id);
        IQueryable GetAll();
        void SaveOrUpdate();
        void MarkForDeletion(object entity);
    }

    public interface IRepositoryFactory<T>
        where T : class
    {
        IRepository<T> Resolve();
        void Release(IRepository<T> repository);
    }
}