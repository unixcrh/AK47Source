﻿ALTER TABLE [dbo].[SYS_USER_LOGON]
    ADD CONSTRAINT [PK_SYS_USER_LOGON] PRIMARY KEY CLUSTERED ([ID] DESC) WITH (FILLFACTOR = 90, ALLOW_PAGE_LOCKS = ON, ALLOW_ROW_LOCKS = ON, PAD_INDEX = OFF, IGNORE_DUP_KEY = OFF, STATISTICS_NORECOMPUTE = OFF);

