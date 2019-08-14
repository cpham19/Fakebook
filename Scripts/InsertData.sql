
SET IDENTITY_INSERT Persons ON 
INSERT INTO Persons (PersonId, Name, Username, Password, IsAdmin, ProfileDescription) VALUES (1, N'John', N'John', N'abcd', 1, N'My name is John');
INSERT INTO Persons (PersonId, Name, Username, Password, ProfileDescription) VALUES (2, N'Calvin', N'Calvin', N'abcd', N'My name is Calvin');
INSERT INTO Persons (PersonId, Name, Username, Password, ProfileDescription) VALUES (3, N'Jane', N'Jane', N'abcd', N'My name is Jane');
INSERT INTO Persons (PersonId, Name, Username, Password, ProfileDescription) VALUES (4, N'Bill', N'Bill', N'abcd', N'My name is Bill');
INSERT INTO Persons (PersonId, Name, Username, Password, ProfileDescription) VALUES (5, N'Louis', N'Louis', N'abcd', N'My name is Louis');
SET IDENTITY_INSERT Persons OFF

GO

SET IDENTITY_INSERT TimelinePosts ON 
INSERT INTO TimelinePosts (TimelinePostId, PosterName, Description, DatePosted, PosterId) VALUES (1, N'Calvin', N'I am bored.', '2019-06-19', 2);
INSERT INTO TimelinePosts (TimelinePostId, PosterName, Description, DatePosted, PosterId) VALUES (2, N'Calvin', N'I am still bored', '2019-06-19', 2);
INSERT INTO TimelinePosts (TimelinePostId, PosterName, Description, DatePosted, PosterId) VALUES (3, N'John', N'Nice morning.', '2019-06-19', 1);
INSERT INTO TimelinePosts (TimelinePostId, PosterName, Description, DatePosted, PosterId) VALUES (4, N'John', N'I ate steak this morning!', '2019-06-20', 1);
INSERT INTO TimelinePosts (TimelinePostId, PosterName, Description, DatePosted, PosterId) VALUES (5, N'Jane', N'Nice night.', '2019-06-19', 3);
INSERT INTO TimelinePosts (TimelinePostId, PosterName, Description, DatePosted, PosterId) VALUES (6, N'Jane', N'Sleeping... Good night!', '2019-06-19', 3);
SET IDENTITY_INSERT TimelinePosts OFF

GO

SET IDENTITY_INSERT ReplyPosts ON 
INSERT INTO ReplyPosts (ReplyPostId, PosterName, Description, DatePosted, TimelinePostId, PosterId) VALUES (1, N'Calvin', N'Are you?.', '2019-06-19', 1, 2);
INSERT INTO ReplyPosts (ReplyPostId, PosterName, Description, DatePosted, TimelinePostId, PosterId) VALUES (2, N'Calvin', N'Yes I am', '2019-06-19', 1, 2);
INSERT INTO ReplyPosts (ReplyPostId, PosterName, Description, DatePosted, TimelinePostId, PosterId) VALUES (3, N'Calvin', N'Good morning to you, John!.', '2019-06-19', 3, 1);
INSERT INTO ReplyPosts (ReplyPostId, PosterName, Description, DatePosted, TimelinePostId, PosterId) VALUES (4, N'Jane', N'Yes, Good morning to you John!', '2019-06-19', 3, 1);
SET IDENTITY_INSERT ReplyPosts OFF

GO

SET IDENTITY_INSERT Forums ON 
INSERT INTO Forums (ForumId,ForumName, PosterId) VALUES (1, N'CS 4220 Database Systems', 2);
INSERT INTO Forums (ForumId,ForumName, PosterId) VALUES (2, N'CS 4661 Data Science', 2);
SET IDENTITY_INSERT Forums OFF
GO

SET IDENTITY_INSERT Topics ON 
INSERT INTO Topics (TopicId, TopicName, TopicContent, TopicDate, ForumId, PosterId) VALUES (1, N'Homework 1', N'Create a database using MYSQL.', '2019-07-03', 1, 2);
INSERT INTO Topics (TopicId, TopicName, TopicContent, TopicDate, ForumId, PosterId) VALUES (2, N'Lab 1', N'Create a relational schema.', '2019-07-03', 1, 2);
INSERT INTO Topics (TopicId, TopicName, TopicContent, TopicDate, ForumId, PosterId) VALUES (3, N'Homework 1', N'Implement Decision Tree algorithm.', '2019-07-01', 2, 2);
INSERT INTO Topics (TopicId, TopicName, TopicContent, TopicDate, ForumId, PosterId) VALUES (4, N'Lab 1', N'Implement Random Forest algorithm.', '2019-07-03', 2, 2);
SET IDENTITY_INSERT Topics OFF 

GO

SET IDENTITY_INSERT Replies ON
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId) VALUES (1, N'How do I do this homework?', '2019-07-03', 1, 2);
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId) VALUES (2, N'Look at your book and implement the database.', '2019-07-03', 1, 2);
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId) VALUES (3, N'How do I do this lab?', '2019-07-03', 2, 2);
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId) VALUES (4, N'By looking at the book for examples.', '2019-07-03', 2, 2);
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId) VALUES (5, N'How do I do this homework?', '2019-07-03', 3, 2);
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId) VALUES (6, N'Follow the example that Dr.Mo sent us.', '2019-07-03', 3, 2);
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId) VALUES (7, N'How do I do this lab?', '2019-07-03', 4, 2);
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId) VALUES (8, N'The lab is really close the the example in the powerpoint slides.', '2019-07-03', 4, 2);
SET IDENTITY_INSERT Replies OFF

GO