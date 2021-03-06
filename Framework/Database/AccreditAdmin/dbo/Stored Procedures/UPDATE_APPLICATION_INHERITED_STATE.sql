﻿CREATE PROCEDURE [dbo].[UPDATE_APPLICATION_INHERITED_STATE]
@APP_ID NVARCHAR(36),
@OLD_INHERITED_STATE INT,
@NEW_INHERITED_STATE INT
AS
--继承状态没有变化，则退出
IF (@OLD_INHERITED_STATE ^ @NEW_INHERITED_STATE = 0)
	RETURN
DECLARE
@SCOPE_STATE INT,	--服务范围继承状态
@ROLE_STATE INT,	--角色继承状态
@FUNC_STATE INT,	--功能继承状态
@RTF_STATE INT,		--角色功能关系继承状态
@OBJ_STATE INT		--被授权对象继承状态
--获得分项继承状态
SET @SCOPE_STATE= -1;
SET @ROLE_STATE = -1;
SET @FUNC_STATE = -1;
SET @RTF_STATE 	= -1;
SET @OBJ_STATE	= -1;
IF (@OLD_INHERITED_STATE ^ @NEW_INHERITED_STATE & 1 = 1)
	SET @SCOPE_STATE= @NEW_INHERITED_STATE & 1;
IF (@OLD_INHERITED_STATE ^ @NEW_INHERITED_STATE & 2 = 2)
	SET @ROLE_STATE = @NEW_INHERITED_STATE & 2;
IF (@OLD_INHERITED_STATE ^ @NEW_INHERITED_STATE & 4 = 4)
	SET @FUNC_STATE = @NEW_INHERITED_STATE & 4;
IF (@OLD_INHERITED_STATE ^ @NEW_INHERITED_STATE & 8 = 8)
	SET @RTF_STATE 	= @NEW_INHERITED_STATE & 8;
IF (@OLD_INHERITED_STATE ^ @NEW_INHERITED_STATE & 16 = 16)
	SET @OBJ_STATE	= @NEW_INHERITED_STATE & 16;
DECLARE
@INHERITED NCHAR(1),
@LEVEL NVARCHAR(32),
@PARENT_APP_ID NVARCHAR(36)
SELECT @LEVEL = RESOURCE_LEVEL
FROM APPLICATIONS
WHERE ID = @APP_ID
--获得父应用ID
SELECT @PARENT_APP_ID = ID
FROM APPLICATIONS
WHERE RESOURCE_LEVEL = LEFT(@LEVEL, LEN(@LEVEL) - 3)
--修改服务范围继承状态
IF (@SCOPE_STATE = 0)
BEGIN
	IF (@SCOPE_STATE=0) 
		SET @INHERITED = 'n'
	ELSE
		SET @INHERITED = 'y'
/*	
	UPDATE SCOPES SET INHERITED = @INHERITED
	WHERE ID IN
		(SELECT T1.ID
		FROM 	(SELECT * FROM SCOPES WHERE APP_ID = @APP_ID ) AS T1,
			(SELECT * FROM SCOPES WHERE APP_ID = @PARENT_APP_ID) AS T2
		WHERE T1.CODE_NAME = T2.CODE_NAME)
*/
	UPDATE SCOPES SET INHERITED = @INHERITED
	WHERE APP_ID = @APP_ID
	
END
--修改角色继承状态
IF (@ROLE_STATE = 0)
BEGIN
	IF (@ROLE_STATE=0) 
		SET @INHERITED = 'n'
	ELSE
		SET @INHERITED = 'y'
/*	
	UPDATE ROLES SET INHERITED = @INHERITED
	WHERE ID IN 
		(SELECT T1.ID
		FROM 	(SELECT * FROM ROLES WHERE APP_ID = @APP_ID AND CLASSIFY = 'n') AS T1,
			(SELECT * FROM ROLES WHERE APP_ID = @PARENT_APP_ID AND CLASSIFY = 'n') AS T2
		WHERE T1.CODE_NAME = T2.CODE_NAME)
*/
	UPDATE ROLES SET INHERITED = @INHERITED
	WHERE APP_ID = @APP_ID
	AND CLASSIFY = 'n'
END
--功能继承状态
IF (@FUNC_STATE = 0)
BEGIN
	IF (@FUNC_STATE=0) 
		SET @INHERITED = 'n'
	ELSE
		SET @INHERITED = 'y'
/*
	--功能表
	UPDATE FUNCTIONS SET INHERITED = @INHERITED
	WHERE ID IN 
		(SELECT T1.ID
		FROM 	(SELECT * FROM FUNCTIONS WHERE APP_ID = @APP_ID	AND CLASSIFY = 'n') AS T1,
			(SELECT * FROM FUNCTIONS WHERE APP_ID = @PARENT_APP_ID AND CLASSIFY = 'n') AS T2
		WHERE T1.CODE_NAME = T2.CODE_NAME)
	--功能集合表
	UPDATE FUNCTION_SETS SET INHERITED = @INHERITED
	WHERE ID IN 
		(SELECT T1.ID
		FROM 	(SELECT * FROM FUNCTION_SETS WHERE APP_ID = @APP_ID AND CLASSIFY = 'n') AS T1,
			(SELECT * FROM FUNCTION_SETS WHERE APP_ID = @PARENT_APP_ID AND CLASSIFY = 'n') AS T2
		WHERE T1.CODE_NAME = T2.CODE_NAME)
*/
	UPDATE FUNCTIONS SET INHERITED = @INHERITED
	WHERE APP_ID = @APP_ID
	AND CLASSIFY = 'n'
	UPDATE FUNCTION_SETS SET INHERITED = @INHERITED
	WHERE APP_ID = @APP_ID
	AND CLASSIFY = 'n'
END
--角色功能关系继承状态
IF (@RTF_STATE = 0)
BEGIN
	IF (@RTF_STATE=0) 
		SET @INHERITED = 'n'
	ELSE
		SET @INHERITED = 'y'
/*	
	UPDATE ROLE_TO_FUNCTIONS SET INHERITED = @INHERITED
	WHERE ROLE_ID IN 
		(SELECT T1.ID
		FROM 	(SELECT * FROM ROLES WHERE APP_ID = @APP_ID AND CLASSIFY = 'n') AS T1,
			(SELECT * FROM ROLES WHERE APP_ID = @PARENT_APP_ID AND CLASSIFY = 'n') AS T2
		WHERE T1.CODE_NAME = T2.CODE_NAME)
	AND FUNC_ID IN
		(SELECT T1.ID
		FROM 	(SELECT * FROM FUNCTIONS WHERE APP_ID = @APP_ID	AND CLASSIFY = 'n') AS T1,
			(SELECT * FROM FUNCTIONS WHERE APP_ID = @PARENT_APP_ID AND CLASSIFY = 'n') AS T2
		WHERE T1.CODE_NAME = T2.CODE_NAME)
*/
	UPDATE ROLE_TO_FUNCTIONS SET INHERITED = @INHERITED
	WHERE ROLE_ID IN 
		(SELECT ID FROM ROLES WHERE APP_ID = @APP_ID AND CLASSIFY = 'n')
	AND FUNC_ID IN
		(SELECT ID FROM FUNCTIONS WHERE APP_ID = @APP_ID	AND CLASSIFY = 'n')
END
--被授权对象继承状态
IF (@OBJ_STATE = 0)
BEGIN
	IF (@OBJ_STATE=0) 
		SET @INHERITED = 'n'
	ELSE
		SET @INHERITED = 'y'
/*
	--角色对应关系临时表
	UPDATE EXPRESSIONS SET INHERITED = @INHERITED
	WHERE ID IN 
		(SELECT T1.ID
		FROM 	(SELECT * FROM EXPRESSIONS WHERE ROLE_ID IN (SELECT ID FROM ROLES WHERE APP_ID = @APP_ID AND CLASSIFY = 'n')) AS T1,
			(SELECT * FROM EXPRESSIONS WHERE ROLE_ID IN (SELECT ID FROM ROLES WHERE APP_ID = @PARENT_APP_ID AND CLASSIFY = 'n')) AS T2,
			(SELECT T1.ID SON_ID, T2.ID FATHER_ID
				FROM 	(SELECT * FROM ROLES WHERE APP_ID = @APP_ID AND CLASSIFY = 'n') AS T1,
					(SELECT * FROM ROLES WHERE APP_ID = @PARENT_APP_ID AND CLASSIFY = 'n') AS T2
				WHERE T1.CODE_NAME = T2.CODE_NAME) AS T3
		WHERE T1.EXPRESSION = T2.EXPRESSION
		AND T1.ROLE_ID = T3.SON_ID
		AND T2.ROLE_ID = T3.FATHER_ID)
*/
	UPDATE EXPRESSIONS SET INHERITED = @INHERITED
	WHERE ROLE_ID IN 
		(SELECT ID FROM ROLES WHERE APP_ID = @APP_ID AND CLASSIFY = 'n')
END
