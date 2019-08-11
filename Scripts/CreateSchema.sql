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
    CONSTRAINT [PK_Persons] PRIMARY KEY ([PersonId]),
);

GO

CREATE TABLE [TimelinePosts] (
    [TimelinePostId] int NOT NULL IDENTITY,
	[PosterName] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
	[DatePosted] datetime2 NOT NULL,
	[PersonId] int NOT NULL,
    CONSTRAINT [PK_TimelinePosts] PRIMARY KEY ([TimelinePostId]),
    CONSTRAINT [FK_TimelinePosts_Persons_PersonId] FOREIGN KEY ([PersonId]) REFERENCES [Persons] ([PersonId]) ON DELETE NO ACTION
);

GO

CREATE TABLE [ReplyPosts] (
    [ReplyPostId] int NOT NULL IDENTITY,
	[PosterName] nvarchar(max) NULL,
    [Description] nvarchar(max) NULL,
	[DatePosted] datetime2 NOT NULL,
	[TimelinePostId] int NOT NULL,
	[PersonId] int NOT NULL,
    CONSTRAINT [PK_ReplyPosts] PRIMARY KEY ([ReplyPostId]),
    CONSTRAINT [FK_ReplyPosts_TimelinePosts_TimelinePostId] FOREIGN KEY ([TimelinePostId]) REFERENCES [TimelinePosts] ([TimelinePostId]) ON DELETE NO ACTION
);

GO

INSERT INTO [__EFMigrationsHistory] ([MigrationId], [ProductVersion])
VALUES (N'20190726011148_InitialSchema', N'2.2.1-servicing-10028');

GO

