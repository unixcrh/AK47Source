﻿using System;
using System.Collections.Generic;
using System.Data;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Transactions;
using System.Xml.Linq;
using MCS.Library.Core;
using MCS.Library.Data;
using MCS.Library.Data.Builder;
using MCS.Library.Data.Mapping;
using MCS.Library.SOA.DataObjects.Workflow;
using MCS.Library.Data.Adapters;

namespace MCS.Library.SOA.DataObjects
{
    public class JobScheduleAdapter : UpdatableAndLoadableAdapterBase<JobSchedule, JobScheduleCollection>
    {
        private const string DELETE_JOB_SCHEDULE_DEF_SQL_CLAUSE = "DELETE WF.JOB_SCHEDULE_DEF WHERE ";
        private const string QUERY_SCHEDULE_DEF_BY_JOB_ID_SQL_CLAUSE = "SELECT S.JOB_ID, D.* FROM WF.JOB_SCHEDULE_DEF D INNER JOIN WF.JOB_SCHEDULES S ON S.SCHEDULE_ID = D.SCHEDULE_ID WHERE {0} ORDER BY START_TIME";

        public static readonly JobScheduleAdapter Instance = new JobScheduleAdapter();

        private JobScheduleAdapter() { }

        public void FillJobsSchdules(IEnumerable<JobBase> jobs)
        {
            List<string> jobIDs = new List<string>();

            jobs.ForEach(job => jobIDs.Add(job.JobID));

            JobScheduleWithJobIDCollection allSchedules = LoadByJobID(jobIDs.ToArray());

            foreach (JobBase job in jobs)
            {
                job.Schedules.Clear();
                job.Schedules.CopyFrom(allSchedules.FindAll(s => s.JobID == job.JobID));
            }
        }

        public void FillJobSchdules(JobBase job)
        {
            JobScheduleWithJobIDCollection allSchedules = LoadByJobID(job.JobID);

            job.Schedules.Clear();
            job.Schedules.CopyFrom(allSchedules.FindAll(s => s.JobID == job.JobID));
        }

        /// <summary>
        /// 按照JobID查找对应Schedule关系数据。不使用TenantCode筛选
        /// </summary>
        /// <param name="jobIDs"></param>
        /// <returns></returns>
        public JobScheduleWithJobIDCollection LoadByJobID(params string[] jobIDs)
        {
            jobIDs.NullCheck("jobIDs");

            InSqlClauseBuilder inBuilder = new InSqlClauseBuilder("JOB_ID");

            inBuilder.AppendItem(jobIDs);

            JobScheduleWithJobIDCollection result = null;

            if (inBuilder.Count > 0)
            {
                //ConnectiveSqlClauseCollection builder = new ConnectiveSqlClauseCollection(inBuilder,
                //    new WhereSqlClauseBuilder().AppendTenantCode("D.TENANT_CODE"),
                //    new WhereSqlClauseBuilder().AppendTenantCode("S.TENANT_CODE"));

                //string sql = string.Format(QUERY_SCHEDULE_DEF_BY_JOB_ID_SQL_CLAUSE,
                //    builder.ToSqlString(TSqlBuilder.Instance));

                string sql = string.Format(QUERY_SCHEDULE_DEF_BY_JOB_ID_SQL_CLAUSE,
                    inBuilder.ToSqlString(TSqlBuilder.Instance));

                result = QueryData<JobScheduleWithJobID, JobScheduleWithJobIDCollection>(
                    ORMapping.GetMappingInfo<JobScheduleWithJobID>(), sql);
            }
            else
            {
                result = new JobScheduleWithJobIDCollection();
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="ids"></param>
        /// <returns>无法删除的ID列表</returns>
        public string[] Delete(string[] ids)
        {
            StringBuilder strBuilder = new StringBuilder();
            List<string> result = new List<string>();

            foreach (string id in ids)
            {
                if (CheckScheduleInJob(id))
                {
                    result.Add(id);
                    continue;
                }

                WhereSqlClauseBuilder whereBuilder = new WhereSqlClauseBuilder();

                whereBuilder.AppendItem("SCHEDULE_ID", id);
                whereBuilder.AppendTenantCode(typeof(JobSchedule));

                strBuilder.Append(DELETE_JOB_SCHEDULE_DEF_SQL_CLAUSE);
                strBuilder.Append(whereBuilder.ToSqlString(TSqlBuilder.Instance));
                strBuilder.Append(TSqlBuilder.Instance.DBStatementSeperator);
            }
            if (!string.IsNullOrEmpty(strBuilder.ToString()))
            {
                using (TransactionScope tran = TransactionScopeFactory.Create())
                {
                    DbHelper.RunSqlWithTransaction(strBuilder.ToString(), GetConnectionName());
                    tran.Complete();
                }
            }
            return result.ToArray();
        }

        public override void ClearAll()
        {
            base.ClearAll();

            DbHelper.RunSql("DELETE FROM WF.JOB_SCHEDULES", GetConnectionName());
        }

        protected override string GetConnectionName()
        {
            return WorkflowSettings.GetConfig().ConnectionName;
        }

        private bool CheckScheduleInJob(string schID)
        {
            WhereSqlClauseBuilder whereBuilder = new WhereSqlClauseBuilder();
            whereBuilder.AppendItem("SCHEDULE_ID", schID);
            whereBuilder.AppendTenantCode(typeof(JobSchedule));

            DataSet ds = DbHelper.RunSqlReturnDS(
                JobBaseAdapter.SELECT_JOB_SCHEDULES_ID_CLAUSE + whereBuilder.ToSqlString(TSqlBuilder.Instance),
                GetConnectionName());

            return ds.Tables[0].Rows.Count != 0 ? true : false;
        }
    }
}
