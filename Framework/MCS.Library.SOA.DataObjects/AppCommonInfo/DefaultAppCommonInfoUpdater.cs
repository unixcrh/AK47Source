﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Transactions;
using MCS.Library.SOA.DataObjects.Workflow;
using MCS.Library.Data.Mapping;
using MCS.Library.Data.Builder;
using MCS.Library.Core;
using MCS.Library.Data;
using MCS.Library.Data.Adapters;

namespace MCS.Library.SOA.DataObjects
{
    internal class DefaultAppCommonInfoUpdater : UpdatableAdapterBase<AppCommonInfo>, IAppCommonInfoUpdater
    {
        public static readonly DefaultAppCommonInfoUpdater Instance = new DefaultAppCommonInfoUpdater();

        private DefaultAppCommonInfoUpdater()
        {
        }

        public void Update(AppCommonInfo aci, params string[] ignoredUpdateProperties)
        {
            aci.NullCheck("aci");
            ExceptionHelper.FalseThrow<ArgumentNullException>(ignoredUpdateProperties != null, "ignoredUpdateProperties");

            Dictionary<string, object> context = new Dictionary<string, object>();

            using (TransactionScope scope = TransactionScopeFactory.Create(TransactionScopeOption.Required))
            {
                if (InnerUpdate(aci, ignoredUpdateProperties) == 0)
                    InnerInsert(aci, context);

                scope.Complete();
            }
        }

        public void UpdateProcessStatus(IEnumerable<IWfProcess> processes)
        {
            processes.NullCheck("processes");

            StringBuilder strB = new StringBuilder();

            foreach (IWfProcess process in processes)
            {
                if (process.SearchID.IsNotEmpty())
                {
                    if (strB.Length > 0)
                        strB.Append(TSqlBuilder.Instance.DBStatementSeperator);

                    WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();

                    builder.AppendItem("RESOURCE_ID", process.SearchID);
                    builder.AppendTenantCode(typeof(AppCommonInfo));

                    strB.AppendFormat("UPDATE WF.APPLICATIONS_COMMON_INFO SET COMPLETED_FLAG = {0} WHERE {1}",
                        (int)AppCommonInfo.ConvertProcessStatusToCompletedFlag(process.Status),
                        builder.ToSqlString(TSqlBuilder.Instance));
                }
            }

            if (strB.Length > 0)
            {
                DbHelper.RunSqlWithTransaction(strB.ToString(), this.GetConnectionName());
            }
        }

        private int InnerUpdate(AppCommonInfo aci, string[] ignoredUpdateProperties)
        {
            List<string> ignoredProperties = new List<string>();

            for (int i = 0; i < ignoredUpdateProperties.Length; i++)
            {
                if (ignoredUpdateProperties[i] != "ResourceID")
                    ignoredProperties.Add(ignoredUpdateProperties[i]);
            }

            ignoredProperties.Add("ResourceID");

            string sql = ORMapping.GetUpdateSql(aci, TSqlBuilder.Instance, ignoredProperties.ToArray());

            return DbHelper.RunSql(sql, this.GetConnectionName());
        }

        protected override void InnerInsert(AppCommonInfo aci, Dictionary<string, object> context)
        {
            string sql = ORMapping.GetInsertSql(aci, TSqlBuilder.Instance);

            DbHelper.RunSql(sql, this.GetConnectionName());
        }

        protected override string GetConnectionName()
        {
            return WfRuntime.ProcessContext.SimulationContext.GetConnectionName(WorkflowSettings.GetConfig().ConnectionName);
        }
    }
}
