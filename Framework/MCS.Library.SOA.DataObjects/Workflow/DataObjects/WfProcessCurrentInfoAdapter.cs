﻿using MCS.Library.Core;
using MCS.Library.Data.Adapters;
using MCS.Library.Data.Builder;
using MCS.Library.Data.Mapping;
using MCS.Library.OGUPermission;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Reflection;
using System.Text;

namespace MCS.Library.SOA.DataObjects.Workflow
{
    /// <summary>
    /// 流程当前信息的实现
    /// </summary>
    public class WfProcessCurrentInfoAdapter
    {
        public static readonly WfProcessCurrentInfoAdapter Instance = new WfProcessCurrentInfoAdapter();

        private WfProcessCurrentInfoAdapter()
        {
        }

        public string ConnectionName
        {
            get
            {
                return GetConnectionName();
            }
        }

        /// <summary>
        /// 加载流程运行时的信息
        /// </summary>
        /// <param name="processIDs"></param>
        /// <returns></returns>
        public WfProcessCurrentInfoCollection Load(bool fillAssignees, params string[] processIDs)
        {
            processIDs.NullCheck("processIDs");

            WfProcessCurrentInfoCollection result = new WfProcessCurrentInfoCollection();

            InSqlClauseBuilder builder = new InSqlClauseBuilder();
            builder.AppendItem(processIDs);

            if (builder.Count > 0)
            {
                string fieldNames = ORMapping.GetSelectFieldsNameSql<WfProcessInstanceData>("Data");

                string sql = string.Format("SELECT {0} FROM WF.PROCESS_INSTANCES WHERE INSTANCE_ID {1}",
                    fieldNames,
                    builder.ToSqlStringWithInOperator(TSqlBuilder.Instance));

                DataTable table = DbHelper.RunSqlReturnDS(sql, GetConnectionName()).Tables[0];

                result.LoadFromDataView(table.DefaultView);

                if (fillAssignees)
                    FillAssignees(result);
            }

            return result;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="fillAssignees"></param>
        /// <param name="action"></param>
        /// <returns></returns>
        public WfProcessCurrentInfoCollection Load(bool fillAssignees, Action<WhereSqlClauseBuilder> action)
        {
            action.NullCheck("action");

            WfProcessCurrentInfoCollection result = new WfProcessCurrentInfoCollection();

            WhereSqlClauseBuilder whereBuilder = new WhereSqlClauseBuilder();

            action(whereBuilder);

            string fieldNames = ORMapping.GetSelectFieldsNameSql<WfProcessInstanceData>("Data");

            string sql = string.Format("SELECT {0} FROM WF.PROCESS_INSTANCES WHERE {1}",
                    fieldNames,
                    whereBuilder.ToSqlString(TSqlBuilder.Instance));

            DataTable table = DbHelper.RunSqlReturnDS(sql, GetConnectionName()).Tables[0];

            result.LoadFromDataView(table.DefaultView);

            if (fillAssignees)
                FillAssignees(result);

            return result;
        }

        /// <summary>
        /// 加载根流程的流程实例
        /// </summary>
        /// <param name="resourceIDs">资源ID</param>
        /// <returns></returns>
        public WfProcessCurrentInfoCollection Load(params string[] resourceIDs)
        {
            resourceIDs.NullCheck("resourceIDs");

            WfProcessCurrentInfoCollection result = new WfProcessCurrentInfoCollection();

            InSqlClauseBuilder builder = new InSqlClauseBuilder();
            builder.AppendItem(resourceIDs);

            if (builder.Count > 0)
            {
                string fieldNames = ORMapping.GetSelectFieldsNameSql<WfProcessInstanceData>("Data");

                string sql = string.Format("SELECT {0} FROM WF.PROCESS_INSTANCES WHERE RESOURCE_ID {1} AND OWNER_ACTIVITY_ID IS NULL",
                    fieldNames,
                    builder.ToSqlStringWithInOperator(TSqlBuilder.Instance));

                DataTable table = DbHelper.RunSqlReturnDS(sql, GetConnectionName()).Tables[0];

                result.LoadFromDataView(table.DefaultView);

                FillAssignees(result);
            }

            return result;
        }

        public WfProcessCurrentInfoCollection LoadByProcessID(params string[] processIDs)
        {
            processIDs.NullCheck("processIDs");

            WfProcessCurrentInfoCollection result = new WfProcessCurrentInfoCollection();

            InSqlClauseBuilder builder = new InSqlClauseBuilder();
            builder.AppendItem(processIDs);

            if (builder.Count > 0)
            {
                string fieldNames = ORMapping.GetSelectFieldsNameSql<WfProcessInstanceData>("Data");

                string sql = string.Format("SELECT {0} FROM WF.PROCESS_INSTANCES WHERE INSTANCE_ID {1}",
                    fieldNames,
                    builder.ToSqlStringWithInOperator(TSqlBuilder.Instance));

                DataTable table = DbHelper.RunSqlReturnDS(sql, GetConnectionName()).Tables[0];

                result.LoadFromDataView(table.DefaultView);

                FillAssignees(result);
            }

            return result;
        }

        /// <summary>
        /// 加载分支流程信息
        /// </summary>
        /// <param name="fillAssignees"></param>
        /// <param name="activityID"></param>
        /// <param name="templateKey"></param>
        /// <param name="includeAborted">是否包含已经作废的流程</param>
        /// <returns></returns>
        public WfProcessCurrentInfoCollection LoadByOwnerActivityID(bool fillAssignees, string activityID, string templateKey, bool includeAborted)
        {
            activityID.NullCheck("activityID");

            WfProcessCurrentInfoCollection result = new WfProcessCurrentInfoCollection();

            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem("OWNER_ACTIVITY_ID", activityID);

            if (templateKey.IsNotEmpty())
                builder.AppendItem("OWNER_TEMPLATE_KEY", templateKey);

            if (includeAborted == false)
                builder.AppendItem("Status", WfProcessStatus.Aborted.ToString(), "<>");

            string fieldNames = ORMapping.GetSelectFieldsNameSql<WfProcessCurrentInfo>();

            string sql = string.Format("SELECT {0} FROM WF.PROCESS_INSTANCES WHERE {1}",
                fieldNames,
                builder.ToSqlString(TSqlBuilder.Instance));

            DataTable table = DbHelper.RunSqlReturnDS(sql, GetConnectionName()).Tables[0];

            result.LoadFromDataView(table.DefaultView);

            if (fillAssignees)
                FillAssignees(result);

            return result;
        }

        /// <summary>
        /// 得到某个活动下的分支流程统计信息
        /// </summary>
        /// <param name="activityID">不可以为空</param>
        /// <param name="templateKey">可以为空，如果有值，则只统计某个模板的流程</param>
        /// <returns></returns>
        public Dictionary<WfProcessStatus, int> LoadStatisticsDataByOwnerActivityID(string activityID, string templateKey)
        {
            activityID.NullCheck("activityID");

            Dictionary<WfProcessStatus, int> result = new Dictionary<WfProcessStatus, int>();

            WhereSqlClauseBuilder builder = new WhereSqlClauseBuilder();
            builder.AppendItem("OWNER_ACTIVITY_ID", activityID);

            if (templateKey.IsNotEmpty())
                builder.AppendItem("OWNER_TEMPLATE_KEY", templateKey);

            string fieldNames = ORMapping.GetSelectFieldsNameSql<WfProcessCurrentInfo>();

            string sql = string.Format("SELECT STATUS, COUNT(*) AS COUNT FROM WF.PROCESS_INSTANCES WHERE {0} GROUP BY STATUS",
                builder.ToSqlString(TSqlBuilder.Instance));

            DataTable table = DbHelper.RunSqlReturnDS(sql, GetConnectionName()).Tables[0];

            foreach (DataRow row in table.Rows)
            {
                string statusText = row["STATUS"].ToString();

                WfProcessStatus status;

                if (Enum.TryParse(statusText, true, out status))
                    result[status] = (int)row["COUNT"];
            }

            return result;
        }

        /// <summary>
        /// 更新流程运行时活动节点的Assignees
        /// </summary>
        /// <param name="processCurrentInfo"></param>
        public void UpdateProcessRelatedUsers(IWfProcess process)
        {
            WfProcessCurrentAssigneeCollection pcas = new WfProcessCurrentAssigneeCollection();

            foreach (var activity in process.Activities)
            {
                foreach (var assignee in activity.Assignees)
                {
                    if (assignee.User != null)
                    {
                        WfProcessCurrentAssignee pca = new WfProcessCurrentAssignee(assignee);

                        pca.ProcessID = process.ID;
                        pca.ActivityID = activity.ID;

                        pcas.Add(pca);
                    }
                }

                foreach (var candidate in activity.Candidates)
                {
                    if (candidate.User != null && activity.Assignees.Contains(candidate.User.ID) == false)
                    {
                        WfProcessCurrentAssignee pca = new WfProcessCurrentAssignee(candidate);

                        pca.ProcessID = process.ID;
                        pca.ActivityID = activity.ID;

                        pcas.Add(pca);
                    }
                }
            }

            WfProcessCurrentAssigneeAdapter.Instance.Update(process.ID, pcas);
        }

        /// <summary>
        /// 删除流程运行时活动节点的Assignees
        /// </summary>
        /// <param name="activityID"></param>
        public void DeleteProcessCurrentAssignees(WfProcessCurrentInfoCollection processesInfo)
        {
            InSqlClauseBuilder builder = new InSqlClauseBuilder("ACTIVITY_ID");

            processesInfo.ForEach(p => builder.AppendItem(p.CurrentActivityID));

            if (builder.Count > 0)
            {
                string sql = string.Format("DELETE WF.PROCESS_CURRENT_ASSIGNEES WHERE {0}",
                    builder.ToSqlStringWithInOperator(TSqlBuilder.Instance));

                DbHelper.RunSql(sql, GetConnectionName());
            }
        }

        /// <summary>
        /// 查询用户相关的运行中（包含维护中和未运行的）的流程信息，包括该流程最新的待办信息
        /// </summary>
        /// <param name="userID"></param>
        /// <returns>返回是一个DataTable</returns>
        public DataTable QueryUserRelativeRunningProcesses(string userID)
        {
            userID.CheckStringIsNullOrEmpty("userID");

            return DbHelper.RunSqlReturnDS(string.Format("WF.QueryUserRelativeRunningProcesses {0}",
                TSqlBuilder.Instance.CheckUnicodeQuotationMark(userID)), GetConnectionName()).Tables[0];
        }

        private static void FillAssignees(WfProcessCurrentInfoCollection result)
        {
            InSqlClauseBuilder inBuilder = new InSqlClauseBuilder();

            result.ForEach(currentInfo => inBuilder.AppendItem(currentInfo.CurrentActivityID));

            if (inBuilder.Count > 0)
            {
                string sql = string.Format("SELECT * FROM WF.PROCESS_CURRENT_ASSIGNEES WHERE ACTIVITY_ID {0} ORDER BY ID",
                inBuilder.ToSqlStringWithInOperator(TSqlBuilder.Instance));

                DataTable table = DbHelper.RunSqlReturnDS(sql, GetConnectionName()).Tables[0];

                DataView assigneeView = new DataView(table);

                assigneeView.Sort = "ACTIVITY_ID";

                foreach (var currentInfo in result)
                {
                    DataRowView[] rows = assigneeView.FindRows(currentInfo.CurrentActivityID);

                    Array.ForEach(rows, drv =>
                    {
                        currentInfo.Assignees.Add(DataRowToAssignee(drv.Row));
                    });
                }
            }
        }

        private static WfAssignee DataRowToAssignee(DataRow row)
        {
            WfAssignee result = new WfAssignee();

            result.AssigneeType = (WfAssigneeType)Enum.Parse(typeof(WfAssigneeType), row["ASSIGNEE_TYPE"].ToString(), true);

            OguUser user = new OguUser();
            user.ID = row["USER_ID"].ToString();
            user.Name = row["USER_NAME"].ToString();
            user.FullPath = row["USER_PATH"].ToString();

            result.User = user;

            return result;
        }

        private static string GetConnectionName()
        {
            return WfRuntime.ProcessContext.SimulationContext.GetConnectionName(WorkflowSettings.GetConfig().ConnectionName);
        }
    }
}
