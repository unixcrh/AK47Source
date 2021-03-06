﻿CREATE TABLE [WF].[WORKFLOW_TASK_TEMPLATE] (
    [TEMPLATE_KEY]  NVARCHAR (36)  NOT NULL,
    [TEMPLATE_NAME] NVARCHAR (128) NOT NULL,
    [CATEGORY_ID]   INT            NOT NULL,
    [CREATETIME]    DATETIME       NULL,
    [CREATOR_ID]    NVARCHAR (36)  NULL,
    [CREATOR_NAME]  NVARCHAR (36)  NULL,
    CONSTRAINT [PK_WORKFLOW_TASK_TEMPLATE] PRIMARY KEY CLUSTERED ([TEMPLATE_KEY] ASC)
);

