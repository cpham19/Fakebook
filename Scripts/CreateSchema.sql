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

CREATE TABLE [TimelinePosts] (
    [TimelinePostId] int NOT NULL IDENTITY,
	[PosterName] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
	[DatePosted] datetime2 NOT NULL,
	[PosterId] int NOT NULL,
    CONSTRAINT [PK_TimelinePosts] PRIMARY KEY ([TimelinePostId]),
    CONSTRAINT [FK_TimelinePosts_Persons_PosterId] FOREIGN KEY ([PosterId]) REFERENCES [Persons] ([PersonId]) ON DELETE CASCADE
);

GO

CREATE TABLE [ReplyPosts] (
    [ReplyPostId] int NOT NULL IDENTITY,
	[PosterName] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
	[DatePosted] datetime2 NOT NULL,
	[TimelinePostId] int NOT NULL,
	[PosterId] int NOT NULL,
    CONSTRAINT [PK_ReplyPosts] PRIMARY KEY ([ReplyPostId]),
    CONSTRAINT [FK_ReplyPosts_TimelinePosts_TimelinePostId] FOREIGN KEY ([TimelinePostId]) REFERENCES [TimelinePosts] ([TimelinePostId]) ON DELETE CASCADE
);

GO

CREATE TABLE [Forums] (
    [ForumId] int NOT NULL IDENTITY,
    [ForumName] nvarchar(max) NULL,
	[PosterId] int NOT NULL,
    CONSTRAINT [PK_Forums] PRIMARY KEY ([ForumId]),
);

GO

CREATE TABLE [Topics] (
    [TopicId] int NOT NULL IDENTITY,
    [TopicName] nvarchar(max) NULL,
    [TopicContent] nvarchar(max) NULL,
    [TopicDate] datetime2 NOT NULL,
    [ForumId] int NOT NULL,
	[PosterID] int NOT NULL,
    CONSTRAINT [PK_Topics] PRIMARY KEY ([TopicId]),
	CONSTRAINT [FK_Topics_Forums_ForumId] FOREIGN KEY ([ForumId]) REFERENCES [Forums] ([ForumId]) ON DELETE CASCADE
);

GO

CREATE TABLE [Replies] (
    [ReplyId] int NOT NULL IDENTITY,
    [ReplyContent] nvarchar(max) NULL,
	[ReplyDate] datetime2 NOT NULL,
    [TopicId] int NOT NULL,
	[PosterId] int NOT NULL,
    CONSTRAINT [PK_Replies] PRIMARY KEY ([ReplyId]),
    CONSTRAINT [FK_Replies_Topics_TopicId] FOREIGN KEY ([TopicId]) REFERENCES [Topics] ([TopicId]) ON DELETE CASCADE
);

GO

CREATE INDEX [IX_FK_TimelinePosts_PosterId] ON [TimelinePosts] ([PosterId]);

GO

CREATE INDEX [IX_FK_ReplyPosts_TimelinePostId] ON [ReplyPosts] ([TimelinePostId]);

GO

CREATE INDEX [IX_FK_Topics_ForumId] ON [Topics] ([ForumId]);

GO 

CREATE INDEX [IX_FK_Replies_TopicId] ON [Replies] ([TopicId]);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190726011148_InitialSchema', N'2.2.1-servicing-10028');

GO

