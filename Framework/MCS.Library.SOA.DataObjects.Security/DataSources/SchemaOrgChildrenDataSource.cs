﻿using MCS.Library.Core;
using MCS.Library.Data.Adapters;
using MCS.Library.Data.Builder;
using MCS.Library.Data.DataObjects;
using MCS.Library.SOA.DataObjects.Schemas.Configuration;
using MCS.Library.SOA.DataObjects.Schemas.SchemaProperties;
using MCS.Library.SOA.DataObjects.Security.Adapters;
using MCS.Library.SOA.DataObjects.Security.Configuration;
using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;

namespace MCS.Library.SOA.DataObjects.Security.DataSources
{
	public class SchemaOrgChildrenDataSource : DataViewDataSourceQueryAdapterBase
	{
		private string _SearchOuID = string.Empty;
		private string[] schemaTypes = null;

		public DataView Query(int startRowIndex, int maximumRows, string ou, string where, string orderBy, ref int totalCount)
		{
			this._SearchOuID = ou;
			this.schemaTypes = null;
			return base.Query(startRowIndex, maximumRows, where, orderBy, ref totalCount);
		}

		public int GetQueryCount(string ou, string where, ref int totalCount)
		{
			this._SearchOuID = ou;
			this.schemaTypes = null;
			return base.GetQueryCount(where, ref totalCount);
		}

		public DataView Query(int startRowIndex, int maximumRows, string ou, string[] schemaTypes, string where, string orderBy, ref int totalCount)
		{
			this._SearchOuID = ou;
			this.schemaTypes = schemaTypes;
			return base.Query(startRowIndex, maximumRows, where, orderBy, ref totalCount);
		}

		public int GetQueryCount(string ou, string[] schemaTypes, string where, ref int totalCount)
		{
			this._SearchOuID = ou;
			this.schemaTypes = schemaTypes;
			return base.GetQueryCount(where, ref totalCount);
		}

		protected override string GetConnectionName()
		{
			return SCConnectionDefine.DBConnectionName;
		}

		protected override void OnBuildQueryCondition(QueryCondition qc)
		{
			qc.FromClause = TimePointContext.Current.UseCurrentTime ? "SC.SchemaObjectSnapshot_Current O INNER JOIN SC.SchemaRelationObjectsSnapshot_Current R ON O.ID = R.ObjectID" : "SC.SchemaObjectSnapshot O INNER JOIN SC.SchemaRelationObjectsSnapshot R ON O.ID = R.ObjectID";
			qc.SelectFields = "O.*";
			if (string.IsNullOrEmpty(qc.OrderByClause))
				qc.OrderByClause = "R.InnerSort ASC";

			qc.WhereClause.IsNotEmpty((s) => qc.WhereClause += " AND ");
			InSqlClauseBuilder inSql = new InSqlClauseBuilder("O.SchemaType");

			if (this.schemaTypes != null && this.schemaTypes.Length > 0)
			{
				inSql.AppendItem(this.schemaTypes);
			}
			else
			{
				var config = ObjectSchemaSettings.GetConfig();
				inSql.AppendItem(SchemaInfo.FilterByCategory("Users", "Groups", "Organizations").ToSchemaNames());
				if (inSql.IsEmpty)
					throw new ApplicationException("配置中不存在任何可用的Schema");
			}

			var timeCondition1 = VersionStrategyQuerySqlBuilder.Instance.TimePointToBuilder("R.");
			var where = new WhereSqlClauseBuilder();
			where.AppendItem("R.Status", (int)SchemaObjectStatus.Normal);
			where.AppendItem("R.ParentSchemaType", "Organizations");
			where.AppendItem("R.ParentID", this._SearchOuID);
			var timeCondition2 = VersionStrategyQuerySqlBuilder.Instance.TimePointToBuilder("O.");
			where.AppendItem("O.Status", (int)SchemaObjectStatus.Normal);

			qc.WhereClause += new ConnectiveSqlClauseCollection(timeCondition1, timeCondition2, where, inSql).ToSqlString(TSqlBuilder.Instance);
		}
	}
}
