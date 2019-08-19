
SET IDENTITY_INSERT Persons ON 
INSERT INTO Persons (PersonId, Name, Username, Password, IsAdmin, ProfileDescription) VALUES (1, N'John', N'John', N'abcd', 1, N'My name is John');
INSERT INTO Persons (PersonId, Name, Username, Password, ProfileDescription) VALUES (2, N'Calvin', N'Calvin', N'abcd', N'My name is Calvin');
INSERT INTO Persons (PersonId, Name, Username, Password, ProfileDescription) VALUES (3, N'Jane', N'Jane', N'abcd', N'My name is Jane');
INSERT INTO Persons (PersonId, Name, Username, Password, ProfileDescription) VALUES (4, N'Bill', N'Bill', N'abcd', N'My name is Bill');
INSERT INTO Persons (PersonId, Name, Username, Password, ProfileDescription) VALUES (5, N'Louis', N'Louis', N'abcd', N'My name is Louis');
SET IDENTITY_INSERT Persons OFF

GO

SET IDENTITY_INSERT TimelinePosts ON 
INSERT INTO TimelinePosts (TimelinePostId, PosterName, Description, DatePosted, PosterId, UserIdOfProfile) VALUES (1, N'Calvin', N'I am bored.', '2019-06-19', 2, 2);
INSERT INTO TimelinePosts (TimelinePostId, PosterName, Description, DatePosted, PosterId, UserIdOfProfile) VALUES (2, N'Calvin', N'I am still bored', '2019-06-19', 2, 2);
INSERT INTO TimelinePosts (TimelinePostId, PosterName, Description, DatePosted, PosterId, UserIdOfProfile) VALUES (3, N'John', N'Nice morning.', '2019-06-19', 1, 1);
INSERT INTO TimelinePosts (TimelinePostId, PosterName, Description, DatePosted, PosterId, UserIdOfProfile) VALUES (4, N'John', N'I ate steak this morning!', '2019-06-20', 1, 1);
INSERT INTO TimelinePosts (TimelinePostId, PosterName, Description, DatePosted, PosterId, UserIdOfProfile) VALUES (5, N'Jane', N'Nice night.', '2019-06-19', 3, 3);
INSERT INTO TimelinePosts (TimelinePostId, PosterName, Description, DatePosted, PosterId, UserIdOfProfile) VALUES (6, N'Jane', N'Sleeping... Good night!', '2019-06-19', 3, 3);
SET IDENTITY_INSERT TimelinePosts OFF

GO

SET IDENTITY_INSERT ReplyPosts ON 
INSERT INTO ReplyPosts (ReplyPostId, PosterName, Description, DatePosted, TimelinePostId, PosterId) VALUES (1, N'Calvin', N'Are you?.', '2019-06-19', 1, 2);
INSERT INTO ReplyPosts (ReplyPostId, PosterName, Description, DatePosted, TimelinePostId, PosterId) VALUES (2, N'Calvin', N'Yes I am', '2019-06-19', 1, 2);
INSERT INTO ReplyPosts (ReplyPostId, PosterName, Description, DatePosted, TimelinePostId, PosterId) VALUES (3, N'Calvin', N'Good morning to you, John!.', '2019-06-19', 3, 2);
INSERT INTO ReplyPosts (ReplyPostId, PosterName, Description, DatePosted, TimelinePostId, PosterId) VALUES (4, N'Jane', N'Yes, Good morning to you John!', '2019-06-19', 3, 3);
SET IDENTITY_INSERT ReplyPosts OFF

GO

SET IDENTITY_INSERT Forums ON 
INSERT INTO Forums (ForumId,ForumName, PosterId) VALUES (1, N'News', 2);
INSERT INTO Forums (ForumId,ForumName, PosterId) VALUES (2, N'Marketplace', 2);
SET IDENTITY_INSERT Forums OFF
GO

SET IDENTITY_INSERT Topics ON 
INSERT INTO Topics (TopicId, TopicName, TopicContent, TopicDate, ForumId, PosterId, PosterName) VALUES (1, N'Updates involving home pages', N'You can add posts now...', '2019-07-03', 1, 2, N'Calvin');
INSERT INTO Topics (TopicId, TopicName, TopicContent, TopicDate, ForumId, PosterId, PosterName) VALUES (2, N'Updates involving forum', N'You can look at forums now...', '2019-07-03', 1, 2, N'Calvin');
INSERT INTO Topics (TopicId, TopicName, TopicContent, TopicDate, ForumId, PosterId, PosterName) VALUES (3, N'Selling dirt', N'Fresh and good quality dirt', '2019-07-01', 2, 2, N'John');
INSERT INTO Topics (TopicId, TopicName, TopicContent, TopicDate, ForumId, PosterId, PosterName) VALUES (4, N'Selling a car', N'Model Honda, Year 2005, Condition Used', '2019-07-03', 2, 2, N'John');
SET IDENTITY_INSERT Topics OFF 

GO

SET IDENTITY_INSERT Replies ON
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId, PosterName) VALUES (1, N'Nice updates...', '2019-07-03', 1, 1, N'John');
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId, PosterName) VALUES (2, N'Yes... Very nice updates...', '2019-07-03', 1, 3, N'Jane');
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId, PosterName) VALUES (3, N'Keep up on the good updates...', '2019-07-03', 2, 4, N'Bill');
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId, PosterName) VALUES (4, N'Great work....', '2019-07-03', 2, 5, N'Louis');
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId, PosterName) VALUES (5, N'How much for the dirt?', '2019-07-03', 3, 2, N'Calvin');
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId, PosterName) VALUES (6, N'$20', '2019-07-03', 3, 1, N'John');
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId, PosterName) VALUES (7, N'Deal....', '2019-07-03', 3, 2, N'Calvin');
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId, PosterName) VALUES (8, N'How much for the car??', '2019-07-03', 4, 4, N'Bill');
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId, PosterName) VALUES (9, N'For you, $10.', '2019-07-03', 4, 1, N'John');
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId, PosterName) VALUES (10, N'Nah, too expensive.', '2019-07-03', 4, 4, N'Bill');
SET IDENTITY_INSERT Replies OFF

GO