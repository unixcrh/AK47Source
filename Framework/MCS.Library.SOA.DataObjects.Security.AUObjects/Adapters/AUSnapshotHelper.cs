﻿using MCS.Library.Data;
using MCS.Library.Data.Adapters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace MCS.Library.SOA.DataObjects.Security.AUObjects.Adapters
{
	public static class AUSnapshotHelper
	{
		public static void ClearUp()
		{
			AUCommon.DoDbAction(() =>
			{
				using (DbContext context = DbContext.GetContext(AUCommon.DBConnectionName))
				{
					DbHelper.RunSql("EXEC SC.ClearAllData", AUCommon.DBConnectionName);
				}
			});
		}
	}
}
