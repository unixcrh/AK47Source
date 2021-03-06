﻿using MCS.Library.Data.Adapters;
using MCS.Library.SOA.DataObjects.Security;
using MCS.Library.SOA.DataObjects.Security.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCS.Library.SOA.DataObjects.Security.DataSources
{
	public class ADSyncLogDataSource : DataViewDataSourceQueryAdapterBase
	{
		public ADSyncLogDataSource()
			: base("SC.ADSynchronizeLog")
		{
		}

		protected override string GetConnectionName()
		{
			return SCConnectionDefine.DBConnectionName;
		}
	}
}
