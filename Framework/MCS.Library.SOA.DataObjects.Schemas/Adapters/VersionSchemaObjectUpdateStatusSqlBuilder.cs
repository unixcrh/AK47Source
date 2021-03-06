﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using MCS.Library.Core;
using MCS.Library.Data.Builder;
using MCS.Library.Data.Mapping;
using MCS.Library.SOA.DataObjects;
using MCS.Library.SOA.DataObjects.Schemas.SchemaProperties;

namespace MCS.Library.SOA.DataObjects.Schemas.Adapters
{
	public class VersionSchemaObjectUpdateStatusSqlBuilder : VersionStrategyUpdateSqlBuilder<VersionedSchemaObjectBase>
	{
		public static readonly VersionSchemaObjectUpdateStatusSqlBuilder Instance = new VersionSchemaObjectUpdateStatusSqlBuilder();

		private VersionSchemaObjectUpdateStatusSqlBuilder()
		{
		}

		protected override string PrepareInsertSql(VersionedSchemaObjectBase obj, ORMappingItemCollection mapping)
		{
			List<string> selectFieldNames = new List<string>(ORMapping.GetSelectFieldsName(mapping, "VersionStartTime", "VersionEndTime", "Status"));

			selectFieldNames.Add("Data");

			List<string> insertFieldNames = new List<string>(selectFieldNames);

			insertFieldNames.Add("VersionStartTime");
			insertFieldNames.Add("VersionEndTime");
			insertFieldNames.Add("Status");

			StringBuilder strB = new StringBuilder();

			strB.Append(TSqlBuilder.Instance.DBStatementSeperator);

			strB.AppendFormat("INSERT INTO {0}({1})", GetTableName(obj, mapping), string.Join(",", insertFieldNames));
			strB.Append(TSqlBuilder.Instance.DBStatementSeperator);
			strB.AppendFormat("SELECT {0},{1},{2},{3} FROM {4} WHERE {5}",
				string.Join(",", selectFieldNames),
				"@currentTime",
				TSqlBuilder.Instance.FormatDateTime(ConnectionDefine.MaxVersionEndTime),
				(int)obj.Status,
				GetTableName(obj, mapping),
				this.PrepareWhereSqlBuilder(obj, mapping).ToSqlString(TSqlBuilder.Instance));

			return strB.ToString();
		}

		protected override string GetTableName(VersionedSchemaObjectBase obj, ORMappingItemCollection mapping)
		{
			return mapping.TableName;
		}
	}
}
