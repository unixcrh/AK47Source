﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Text;
using System.Transactions;
using MCS.Library.Core;
using MCS.Library.Data.Builder;
using MCS.Library.Data.Mapping;

namespace MCS.Library.Data.Adapters
{
    /// <summary>
    /// 带数据更新功能的DataAdapter的基类
    /// </summary>
    /// <typeparam name="T"></typeparam>
    public abstract class UpdatableAdapterBase<T>
    {
        private static readonly Dictionary<string, object> _DefaultContext = new Dictionary<string, object>();

        /// <summary>
        /// 得到连接串的名称
        /// </summary>
        public string ConnectionName
        {
            get
            {
                return GetConnectionName();
            }
        }

        /// <summary>
        /// 更新对象
        /// </summary>
        /// <param name="data"></param>
        public void Update(T data)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(data != null, "data");

            PerformanceMonitorHelper.GetDefaultMonitor().WriteExecutionDuration(string.Format("Update({0})", this.GetType().FullName), () =>
            {
                Dictionary<string, object> context = new Dictionary<string, object>();

                BeforeInnerUpdate(data, context);

                using (TransactionScope scope = TransactionScopeFactory.Create())
                {
                    if (InnerUpdate(data, context) == 0)
                        InnerInsert(data, context);

                    AfterInnerUpdate(data, context);

                    scope.Complete();
                }
            });
        }

        /// <summary>
        /// 删除对象
        /// </summary>
        /// <param name="data"></param>
        public virtual void Delete(T data)
        {
            ExceptionHelper.FalseThrow<ArgumentNullException>(data != null, "data");

            PerformanceMonitorHelper.GetDefaultMonitor().WriteExecutionDuration(string.Format("Delete({0})", this.GetType().FullName), () =>
            {
                Dictionary<string, object> context = new Dictionary<string, object>();

                BeforeInnerDelete(data, context);

                using (TransactionScope scope = TransactionScopeFactory.Create())
                {
                    InnerDelete(data, context);

                    AfterInnerDelete(data, context);

                    scope.Complete();
                }
            });
        }

        /// <summary>
        /// 按照条件删除
        /// </summary>
        /// <param name="whereAction"></param>
        public virtual void Delete(Action<WhereSqlClauseBuilder> whereAction)
        {
            whereAction.NullCheck("whereAction");

            PerformanceMonitorHelper.GetDefaultMonitor().WriteExecutionDuration(string.Format("Delete(whereAction-{0})", this.GetType().FullName), () =>
            {
                WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();

                whereAction(builder);

                if (builder.Count > 0)
                {
                    Dictionary<string, object> context = new Dictionary<string, object>();

                    string sql = string.Format("DELETE {0} WHERE {1}",
                        this.GetTableName(),
                        builder.ToSqlString(TSqlBuilder.Instance));

                    BeforeInnerDelete(builder, context);

                    using (TransactionScope scope = TransactionScopeFactory.Create())
                    {
                        DbHelper.RunSql(db => db.ExecuteNonQuery(CommandType.Text, sql), this.GetConnectionName());

                        AfterInnerDelete(builder, context);

                        scope.Complete();
                    }
                }
            });
        }

        /// <summary>
        /// 删除所有数据，用于测试
        /// </summary>
        [Conditional("DEBUG")]
        public virtual void ClearAll()
        {
            string sql = "DELETE FROM " + this.GetTableName();
            DbHelper.RunSql(sql, GetConnectionName());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="context"></param>
        protected virtual void BeforeInnerDelete(T data, Dictionary<string, object> context)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="context"></param>
        protected virtual void BeforeInnerDelete(WhereSqlClauseBuilder builder, Dictionary<string, object> context)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="context"></param>
        protected virtual void AfterInnerDelete(T data, Dictionary<string, object> context)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="builder"></param>
        /// <param name="context"></param>
        protected virtual void AfterInnerDelete(WhereSqlClauseBuilder builder, Dictionary<string, object> context)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        protected abstract string GetConnectionName();

        //protected virtual string GetConnectionName()
        //{
        //    return ConnectionDefine.DBConnectionName;
        //}

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="context"></param>
        protected virtual void BeforeInnerUpdate(T data, Dictionary<string, object> context)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="context"></param>
        protected virtual void AfterInnerUpdate(T data, Dictionary<string, object> context)
        {
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual int InnerDelete(T data, Dictionary<string, object> context)
        {
            ORMappingItemCollection mappings = GetMappingInfo(context);

            WhereSqlClauseBuilder builder = ORMapping.GetWhereSqlClauseBuilderByPrimaryKey(data, mappings);

            ExceptionHelper.FalseThrow(builder.Count > 0, "必须为对象{0}指定关键字", typeof(T));
            string sql = string.Format("DELETE FROM {0} WHERE {1}", this.GetTableName(), builder.ToSqlString(TSqlBuilder.Instance));

            int result = 0;

            DbHelper.RunSql(db => result = db.ExecuteNonQuery(CommandType.Text, sql), this.GetConnectionName());

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual ORMappingItemCollection GetMappingInfo(Dictionary<string, object> context)
        {
            return ORMapping.GetMappingInfo<T>();
        }

        /// <summary>
        /// 得到表名
        /// </summary>
        /// <returns></returns>
        protected virtual string GetTableName()
        {
            return this.GetMappingInfo(_DefaultContext).TableName;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual int InnerUpdate(T data, Dictionary<string, object> context)
        {
            ORMappingItemCollection mappings = GetMappingInfo(context);

            string sql = this.GetUpdateSql(data, mappings, context);

            int result = 0;

            DbHelper.RunSql(db => result = db.ExecuteNonQuery(CommandType.Text, sql), this.GetConnectionName());

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="mappings"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual string GetUpdateSql(T data, ORMappingItemCollection mappings, Dictionary<string, object> context)
        {
            return ORMapping.GetUpdateSql(data, mappings, TSqlBuilder.Instance);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="context"></param>
        protected virtual void InnerInsert(T data, Dictionary<string, object> context)
        {
            ORMappingItemCollection mappings = GetMappingInfo(context);

            string sql = this.GetInsertSql(data, mappings, context);

            DbHelper.RunSql(db => db.ExecuteNonQuery(CommandType.Text, sql), this.GetConnectionName());
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="data"></param>
        /// <param name="mappings"></param>
        /// <param name="context"></param>
        /// <returns></returns>
        protected virtual string GetInsertSql(T data, ORMappingItemCollection mappings, Dictionary<string, object> context)
        {
            return ORMapping.GetInsertSql(data, mappings, TSqlBuilder.Instance);
        }
    }
}
