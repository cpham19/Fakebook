IF OBJECT_ID(N'[__EFMigrationsHistory]') IS NULL
BEGIN
    CREATE TABLE [__EFMigrationsHistory] (
        [MigrationId] nvarchar(150) NOT NULL,
        [ProductVersion] nvarchar(32) NOT NULL,
        CONSTRAINT [PK___EFMigrationsHistory] PRIMARY KEY ([MigrationId])
    );
END;

GO

CREATE TABLE [Persons] (
    [PersonId] int NOT NULL IDENTITY,
    [Name] nvarchar(max) NULL,
    [Username] nvarchar(450) NOT NULL,
    [Password] nvarchar(max) NOT NULL,
    [IsAdmin] bit NOT NULL DEFAULT 0,
	[ProfileDescription] nvarchar(max) NOT NULL DEFAULT 'Profile Description',
    CONSTRAINT [PK_Persons] PRIMARY KEY ([PersonId]),
);

GO

CREATE TABLE [Forums] (
    [ForumId] int NOT NULL IDENTITY,
    [ForumName] nvarchar(max) NULL,
	[PosterId] int NOT NULL,
    CONSTRAINT [PK_Forums] PRIMARY KEY ([ForumId]),
	CONSTRAINT [FK_Forums_Persons_PosterId] FOREIGN KEY ([PosterId]) REFERENCES [Persons] ([PersonId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Topics] (
    [TopicId] int NOT NULL IDENTITY,
    [TopicName] nvarchar(max) NULL,
    [TopicContent] nvarchar(max) NULL,
    [TopicDate] datetime2 NOT NULL,
    [ForumId] int NOT NULL,
	[PosterId] int NOT NULL,
    CONSTRAINT [PK_Topics] PRIMARY KEY ([TopicId]),
	CONSTRAINT [FK_Topics_Forums_ForumId] FOREIGN KEY ([ForumId]) REFERENCES [Forums] ([ForumId]) ON DELETE NO ACTION,
	CONSTRAINT [FK_Topics_Persons_PosterId] FOREIGN KEY ([PosterId]) REFERENCES [Persons] ([PersonId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Replies] (
    [ReplyId] int NOT NULL IDENTITY,
    [ReplyContent] nvarchar(max) NULL,
	[ReplyDate] datetime2 NOT NULL,
    [TopicId] int NOT NULL,
	[PosterId] int NOT NULL,
    CONSTRAINT [PK_Replies] PRIMARY KEY ([ReplyId]),
    CONSTRAINT [FK_Replies_Topics_TopicId] FOREIGN KEY ([TopicId]) REFERENCES [Topics] ([TopicId]) ON DELETE NO ACTION,
	CONSTRAINT [FK_Replies_Persons_PosterId] FOREIGN KEY ([PosterId]) REFERENCES [Persons] ([PersonId]) ON DELETE NO ACTION
);


GO

CREATE TABLE [Friends] (
	[PersonOneId] int NOT NULL,
	[PersonTwoId] int NOT NULL,
	[StatusCode] int NOT NULL DEFAULT 0,
	CONSTRAINT [PK_Friends] PRIMARY KEY ([PersonOneId], [PersonTwoId]),
	CONSTRAINT [FK_Friends_Persons_PersonOneId] FOREIGN KEY ([PersonOneId]) REFERENCES [Persons] ([PersonId]) ON DELETE NO ACTION,
	CONSTRAINT [FK_Friends_Persons_PersonTwoId] FOREIGN KEY ([PersonTwoId]) REFERENCES [Persons] ([PersonId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Groups] (
    [GroupId] int NOT NULL IDENTITY,
    [GroupName] nvarchar(max) NULL,
    [Description] nvarchar(450) NOT NULL,
    [DateCreated] datetime2 NOT NULL,
	[GroupPictureUrl] nvarchar(max) NOT NULL,
	[GroupCreatorId] int NOT NULL,
    CONSTRAINT [PK_Groups] PRIMARY KEY ([GroupId]),
	CONSTRAINT [FK_Groups_Persons_GroupCreatorId] FOREIGN KEY ([GroupCreatorId]) REFERENCES [Persons] ([PersonId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [GroupMembers] (
	[Id] int NOT NULL IDENTITY,
    [GroupId] int NOT NULL,
    [GroupMemberId] int NOT NULL,
    CONSTRAINT [PK_GroupMembers] PRIMARY KEY ([Id]),
	CONSTRAINT [FK_GroupsMembers_Groups_GroupId] FOREIGN KEY ([GroupId]) REFERENCES [Groups] ([GroupId]) ON DELETE NO ACTION,
	CONSTRAINT [FK_GroupsMembers_Persons_GroupMemberId] FOREIGN KEY ([GroupMemberId]) REFERENCES [Persons] ([PersonId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [WallPosts] (
    [WallPostId] int NOT NULL IDENTITY,
	[GroupId] int NOT NULL DEFAULT 0,
    [Description] nvarchar(max) NULL,
	[DatePosted] datetime2 NOT NULL,
	[PosterId] int NOT NULL,
	[UserIdOfProfile] int NOT NULL DEFAULT 0,
    CONSTRAINT [PK_WallPosts] PRIMARY KEY ([WallPostId]),
    CONSTRAINT [FK_WallPosts_Persons_PosterId] FOREIGN KEY ([PosterId]) REFERENCES [Persons] ([PersonId]) ON DELETE NO ACTION,
);

GO

CREATE TABLE [ReplyPosts] (
    [ReplyPostId] int NOT NULL IDENTITY,
    [Description] nvarchar(max) NULL,
	[DatePosted] datetime2 NOT NULL,
	[WallPostId] int NOT NULL,
	[PosterId] int NOT NULL,
    CONSTRAINT [PK_ReplyPosts] PRIMARY KEY ([ReplyPostId]),
    CONSTRAINT [FK_ReplyPosts_WallPosts_WallPostId] FOREIGN KEY ([WallPostId]) REFERENCES [WallPosts] ([WallPostId]) ON DELETE NO ACTION,
	CONSTRAINT [FK_ReplyPosts_Persons_PosterId] FOREIGN KEY ([PosterId]) REFERENCES [Persons] ([PersonId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Blogs] (
    [BlogId] int NOT NULL IDENTITY,
	[Headline] nvarchar(max) NULL,
	[PictureUrl] nvarchar(max) NULL,
	[Title] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
	[DatePosted] datetime2 NOT NULL,
	[PosterId] int NOT NULL,
    CONSTRAINT [PK_Blogs] PRIMARY KEY ([BlogId]),
    CONSTRAINT [FK_Blogs_Persons_PosterId] FOREIGN KEY ([PosterId]) REFERENCES [Persons] ([PersonId]) ON DELETE NO ACTION,
);

GO

CREATE TABLE [BlogComments] (
    [BlogCommentId] int NOT NULL IDENTITY,
	[BlogId] int NOT NULL,
    [Description] nvarchar(max) NULL,
	[DatePosted] datetime2 NOT NULL,
	[PosterId] int NOT NULL,
    CONSTRAINT [PK_BlogComments] PRIMARY KEY ([BlogCommentId]),
    CONSTRAINT [FK_BlogComments_Persons_PosterId] FOREIGN KEY ([PosterId]) REFERENCES [Persons] ([PersonId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [Stores] (
    [StoreId] int NOT NULL IDENTITY,
	[StoreImageUrl] nvarchar(max) NULL,
	[StoreName] nvarchar(max) NULL,
    [StoreDescription] nvarchar(max) NULL,
	[DateCreated] datetime2 NOT NULL,
	[StoreOwnerId] int NOT NULL,
    CONSTRAINT [PK_Stores] PRIMARY KEY ([StoreId]),
    CONSTRAINT [FK_Stores_Persons_PosterId] FOREIGN KEY ([StoreOwnerId]) REFERENCES [Persons] ([PersonId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [StoreItems] (
    [StoreItemId] int NOT NULL IDENTITY,
	[StoreId] int NOT NULL,
	[ItemImageUrl] nvarchar(max) NULL,
	[ItemName] nvarchar(max) NULL,
    [ItemDescription] nvarchar(max) NULL,
	[ItemCondition] nvarchar(max) NULL,
	[DateCreated] datetime2 NOT NULL,
	[Quantity] int NOT NULL,
	[Price] float NOT NULL,
    CONSTRAINT [PK_StoreItems] PRIMARY KEY ([StoreItemId]),
    CONSTRAINT [FK_StoresItems_Stores_StoreId] FOREIGN KEY ([StoreId]) REFERENCES [Stores] ([StoreId]) ON DELETE NO ACTION
);

GO

CREATE INDEX [IX_FK_WallPosts_PosterId] ON [WallPosts] ([PosterId]);

GO

CREATE INDEX [IX_FK_ReplyPosts_WallPostId] ON [ReplyPosts] ([WallPostId]);
CREATE INDEX [IX_FK_ReplyPosts_PosterId] ON [ReplyPosts] ([PosterId]);

GO

CREATE INDEX [IX_FK_Topics_ForumId] ON [Topics] ([ForumId]);
CREATE INDEX [IX_FK_Topics_PosterId] ON [Topics] ([PosterId]);

GO 

CREATE INDEX [IX_FK_Replies_TopicId] ON [Replies] ([TopicId]);
CREATE INDEX [IX_FK_Replies_PosterId] ON [Replies] ([PosterId]);

GO

CREATE INDEX [IX_FK_Friends_PersonOneId] ON [Friends] ([PersonOneId]);
CREATE INDEX [IX_FK_Friends_PersonTwoId] ON [Friends] ([PersonTwoId]);

GO

CREATE INDEX [IX_FK_Groups_GroupCreatorId] ON [Groups] ([GroupCreatorId]);

GO

CREATE INDEX [IX_FK_Blogs_PosterId] ON [Blogs] ([PosterId]);

GO

CREATE INDEX [IX_FK_BlogComments_PosterId] ON [BlogComments] ([PosterId]);

GO

CREATE INDEX [IX_FK_Stores_StoreOwnerId] ON [Stores] ([StoreOwnerId]);

GO

CREATE INDEX [IX_FK_StoreItems_StoreId] ON [StoreItems] ([StoreId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190726011148_InitialSchema', N'2.2.1-servicing-10028');

GO

