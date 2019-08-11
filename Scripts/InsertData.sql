
SET IDENTITY_INSERT Persons ON 
INSERT INTO Persons (PersonId, Name, Username, Password, IsAdmin) VALUES (1, N'John', N'John', N'abcd', 1);
INSERT INTO Persons (PersonId, Name, Username, Password) VALUES (2, N'Calvin', N'Calvin', N'abcd');
INSERT INTO Persons (PersonId, Name, Username, Password) VALUES (3, N'Jane', N'Jane', N'abcd');
INSERT INTO Persons (PersonId, Name, Username, Password) VALUES (4, N'Bill', N'Bill', N'abcd');
INSERT INTO Persons (PersonId, Name, Username, Password) VALUES (5, N'Louis', N'Louis', N'abcd');
SET IDENTITY_INSERT Persons OFF

GO

SET IDENTITY_INSERT TimelinePosts ON 
INSERT INTO TimelinePosts (TimelinePostId, PosterName, Description, DatePosted, PersonId) VALUES (1, N'Calvin', N'I am bored.', '2019-06-19', 2);
INSERT INTO TimelinePosts (TimelinePostId, PosterName, Description, DatePosted, PersonId) VALUES (2, N'Calvin', N'I am still bored', '2019-06-19', 2);
SET IDENTITY_INSERT TimelinePosts OFF

GO

SET IDENTITY_INSERT ReplyPosts ON 
INSERT INTO ReplyPosts (ReplyPostId, PosterName, Description, DatePosted, TimelinePostId, PersonId) VALUES (1, N'Calvin', N'Are you?.', '2019-06-19', 1, 2);
INSERT INTO ReplyPosts (ReplyPostId, PosterName, Description, DatePosted, TimelinePostId, PersonId) VALUES (2, N'Calvin', N'Yes I am', '2019-06-19', 1, 2);
SET IDENTITY_INSERT ReplyPosts OFF

GO