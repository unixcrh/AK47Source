﻿CREATE TABLE [WF].[ROLE_PROPERTIES_DEFINITIONS] (
    [ROLE_ID]     NVARCHAR (36)  NOT NULL,
    [NAME]        NVARCHAR (64)  NOT NULL,
	[TENANT_CODE] NVARCHAR (36)  NULL DEFAULT 'D5561180-7617-4B67-B68B-1F0EA604B509',
    [SORT_ORDER]  INT            CONSTRAINT [DF_ROLE_PROPERTIES_DEFINITIONS_SORT_ORDER] DEFAULT ((0)) NULL,
    [DESCRIPTION] NVARCHAR (256) NULL,
    [DATA_TYPE]   INT            CONSTRAINT [DF_ROLE_PROPERTIES_DEFINITIONS_DATA_TYPE] DEFAULT ((18)) NULL,
    CONSTRAINT [PK_ROLE_PROPERTIES_DEFINITIONS] PRIMARY KEY CLUSTERED ([ROLE_ID] ASC, [NAME] ASC)
);


GO
EXEC sp_addextendedproperty @name = N'MS_Description',
    @value = N'租户编码',
    @level0type = N'SCHEMA',
    @level0name = N'WF',
    @level1type = N'TABLE',
    @level1name = N'ROLE_PROPERTIES_DEFINITIONS',
    @level2type = N'COLUMN',
    @level2name = N'TENANT_CODE'
GO

CREATE INDEX [IX_ROLE_PROPERTIES_DEFINITIONS_TENANT_CODE] ON [WF].[ROLE_PROPERTIES_DEFINITIONS] ([TENANT_CODE])
