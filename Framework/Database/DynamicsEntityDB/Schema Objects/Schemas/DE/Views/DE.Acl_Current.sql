﻿CREATE VIEW [DE].[Acl_Current]
WITH SCHEMABINDING 
	AS 
	SELECT 
	      ContainerID, 
		  ContainerPermission, 
		  MemberID, 
		  VersionStartTime, 
		  VersionEndTime, 
	      [Status], 
		  SortID, 
		  Data, 
		  ContainerSchemaType, 
		  MemberSchemaType
	FROM [DE].[Acl]
	WHERE (VersionEndTime = CONVERT(DATETIME, '99990909 00:00:00', 112)) AND 
	      (Status = 1)
		  GO

		  /****** Object:  Index [Conditions_Current_ClusteredIndex]    Script Date: 2013/5/17 16:16:13 ******/
CREATE UNIQUE CLUSTERED INDEX [Acl_Current_ClusteredIndex] ON [DE].[Acl_Current]
(
	[ContainerID] ASC,
	[ContainerPermission] ASC,
	[MemberID] ASC,
	[VersionStartTime] ASC
)WITH (PAD_INDEX = OFF, STATISTICS_NORECOMPUTE = OFF, SORT_IN_TEMPDB = OFF, IGNORE_DUP_KEY = OFF, DROP_EXISTING = OFF, ONLINE = OFF, ALLOW_ROW_LOCKS = ON, ALLOW_PAGE_LOCKS = ON) ON [PRIMARY]
GO

CREATE INDEX [IX_Acl_Current_MemberID] ON [DE].[Acl_Current] ([MemberID])