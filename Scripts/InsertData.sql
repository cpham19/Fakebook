
SET IDENTITY_INSERT Persons ON 
INSERT INTO Persons (PersonId, Name, Username, Password, IsAdmin, ProfileDescription) VALUES (1, N'John', N'John', N'abcd', 1, N'My name is John');
INSERT INTO Persons (PersonId, Name, Username, Password, ProfileDescription) VALUES (2, N'Calvin', N'Calvin', N'abcd', N'My name is Calvin');
INSERT INTO Persons (PersonId, Name, Username, Password, ProfileDescription) VALUES (3, N'Jane', N'Jane', N'abcd', N'My name is Jane');
INSERT INTO Persons (PersonId, Name, Username, Password, ProfileDescription) VALUES (4, N'Bill', N'Bill', N'abcd', N'My name is Bill');
INSERT INTO Persons (PersonId, Name, Username, Password, ProfileDescription) VALUES (5, N'Louis', N'Louis', N'abcd', N'My name is Louis');
SET IDENTITY_INSERT Persons OFF

GO

SET IDENTITY_INSERT Forums ON 
INSERT INTO Forums (ForumId,ForumName, PosterId) VALUES (1, N'News', 2);
INSERT INTO Forums (ForumId,ForumName, PosterId) VALUES (2, N'Marketplace', 2);
SET IDENTITY_INSERT Forums OFF
GO

SET IDENTITY_INSERT Topics ON 
INSERT INTO Topics (TopicId, TopicName, TopicContent, TopicDate, ForumId, PosterId) VALUES (1, N'Updates involving home pages', N'You can add posts now...', '2019-07-03', 1, 2);
INSERT INTO Topics (TopicId, TopicName, TopicContent, TopicDate, ForumId, PosterId) VALUES (2, N'Updates involving forum', N'You can look at forums now...', '2019-07-03', 1, 2);
INSERT INTO Topics (TopicId, TopicName, TopicContent, TopicDate, ForumId, PosterId) VALUES (3, N'Selling dirt', N'Fresh and good quality dirt', '2019-07-01', 2, 2);
INSERT INTO Topics (TopicId, TopicName, TopicContent, TopicDate, ForumId, PosterId) VALUES (4, N'Selling a car', N'Model Honda, Year 2005, Condition Used', '2019-07-03', 2, 2);
SET IDENTITY_INSERT Topics OFF 

GO

SET IDENTITY_INSERT Replies ON
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId) VALUES (1, N'Nice updates...', '2019-07-03', 1, 1);
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId) VALUES (2, N'Yes... Very nice updates...', '2019-07-03', 1, 3);
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId) VALUES (3, N'Keep up on the good updates...', '2019-07-03', 2, 4);
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId) VALUES (4, N'Great work....', '2019-07-03', 2, 5);
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId) VALUES (5, N'How much for the dirt?', '2019-07-03', 3, 2);
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId) VALUES (6, N'$20', '2019-07-03', 3, 1);
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId) VALUES (7, N'Deal....', '2019-07-03', 3, 2);
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId) VALUES (8, N'How much for the car??', '2019-07-03', 4, 4);
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId) VALUES (9, N'For you, $10.', '2019-07-03', 4, 1);
INSERT INTO Replies (ReplyId, ReplyContent, ReplyDate, TopicId, PosterId) VALUES (10, N'Nah, too expensive.', '2019-07-03', 4, 4);
SET IDENTITY_INSERT Replies OFF

GO

INSERT INTO Friends (PersonOneId, PersonTwoId, StatusCode) VALUES (1, 2, 1);
INSERT INTO Friends (PersonOneId, PersonTwoId, StatusCode) VALUES (2, 3, 1);

GO

SET IDENTITY_INSERT Groups ON
INSERT INTO Groups (GroupId, GroupName, Description, DateCreated, GroupPictureUrl, GroupCreatorId) VALUES (1, N'El Monte Boys', N'This is a group for people from El Monte', '2019-07-03', N'https://cdn4.iconfinder.com/data/icons/blast/127/batman-512.png' , 2);
INSERT INTO Groups (GroupId, GroupName, Description, DateCreated, GroupPictureUrl, GroupCreatorId) VALUES (2, N'Los Angeles Boys', N'This is a group for people from LA', '2019-07-03', N'https://www.freepngimg.com/thumb/city/36187-4-city-transparent-picture-thumb.png' , 2);
SET IDENTITY_INSERT Groups OFF

GO

SET IDENTITY_INSERT GroupMembers ON
INSERT INTO GroupMembers (Id, GroupId, GroupMemberId) VALUES (1, 1, 1);
INSERT INTO GroupMembers (Id, GroupId, GroupMemberId) VALUES (2, 1, 2);
SET IDENTITY_INSERT GroupMembers OFF

GO

SET IDENTITY_INSERT WallPosts ON 
INSERT INTO WallPosts (WallPostId, Description, DatePosted, PosterId, UserIdOfProfile) VALUES (1, N'I am bored.', '2019-06-19', 2, 2);
INSERT INTO WallPosts (WallPostId, Description, DatePosted, PosterId, UserIdOfProfile) VALUES (2, N'I am still bored', '2019-06-19', 2, 2);
INSERT INTO WallPosts (WallPostId, Description, DatePosted, PosterId, UserIdOfProfile) VALUES (3, N'Nice morning.', '2019-06-19', 1, 1);
INSERT INTO WallPosts (WallPostId, Description, DatePosted, PosterId, UserIdOfProfile) VALUES (4, N'I ate steak this morning!', '2019-06-20', 1, 1);
INSERT INTO WallPosts (WallPostId, Description, DatePosted, PosterId, UserIdOfProfile) VALUES (5, N'Nice night.', '2019-06-19', 3, 3);
INSERT INTO WallPosts (WallPostId, Description, DatePosted, PosterId, UserIdOfProfile) VALUES (6, N'Sleeping... Good night!', '2019-06-19', 3, 3);
INSERT INTO WallPosts (WallPostId, GroupId, Description, DatePosted, PosterId, UserIdOfProfile) VALUES (7, 1, N'Thanks inviting me to this group!', '2019-06-19', 2, 0);
SET IDENTITY_INSERT WallPosts OFF

GO

SET IDENTITY_INSERT ReplyPosts ON 
INSERT INTO ReplyPosts (ReplyPostId, Description, DatePosted, WallPostId, PosterId) VALUES (1, N'Are you?.', '2019-06-19', 1, 2);
INSERT INTO ReplyPosts (ReplyPostId, Description, DatePosted, WallPostId, PosterId) VALUES (2, N'Yes I am', '2019-06-19', 1, 2);
INSERT INTO ReplyPosts (ReplyPostId, Description, DatePosted, WallPostId, PosterId) VALUES (3, N'Good morning to you, John!.', '2019-06-19', 3, 2);
INSERT INTO ReplyPosts (ReplyPostId, Description, DatePosted, WallPostId, PosterId) VALUES (4, N'Yes, Good morning to you John!', '2019-06-19', 3, 3);
INSERT INTO ReplyPosts (ReplyPostId, Description, DatePosted, WallPostId, PosterId) VALUES (5, N'Does this work?', '2019-06-19', 7, 2);
SET IDENTITY_INSERT ReplyPosts OFF

GO

SET IDENTITY_INSERT Blogs ON
INSERT INTO Blogs (BlogId, Headline, PictureUrl, Title, Description, DatePosted, PosterId) VALUES (1, N'Special Headline #1', N'http://www.sclance.com/pngs/blog-png/blog_png_147442.png', N'Random Blog #1', N'The triumph persists in the jest. Its exchanged jungle buys the temper past the unauthorized mailbox. The override moans under any maker! The ridiculous client displays an infamous identifier. The bomb lies on top of the stimulated cheat.', '2019-06-19', 2);
INSERT INTO Blogs (BlogId, Headline, PictureUrl, Title, Description, DatePosted, PosterId) VALUES (2, N'Special Headline #2', N'http://www.sclance.com/pngs/blog-png/blog_png_147442.png', N'Random Blog #2', N'Does an awaited antique redirect a fooling overflow? The ideological bubble works throughout an instrumental tennis. The guide alarms his unsatisfactory norm under a plotter. A day gasps into the bare mug.', '2019-06-19', 2);
INSERT INTO Blogs (BlogId, Headline, PictureUrl, Title, Description, DatePosted, PosterId) VALUES (3, N'Special Headline #3', N'http://www.sclance.com/pngs/blog-png/blog_png_147442.png', N'Random Blog #3', N'The famine decays? The afternoon listens after the pan horse. A spiritual award hopes the invited backlog. A bargain rebuilds the science. With the screw consents the skull.', '2019-06-19', 2);
SET IDENTITY_INSERT Blogs OFF

GO

SET IDENTITY_INSERT BlogComments ON
INSERT INTO BlogComments (BlogCommentId,  BlogId, Description, DatePosted, PosterId) VALUES (1, 1, N'Nice blog post..', '2019-06-19', 1);
INSERT INTO BlogComments (BlogCommentId, BlogId, Description, DatePosted, PosterId) VALUES (2, 2, N'Nice blog post..', '2019-06-19', 2);
INSERT INTO BlogComments (BlogCommentId, BlogId, Description, DatePosted, PosterId) VALUES (3, 3, N'Nice blog post..', '2019-06-19', 3);
SET IDENTITY_INSERT BlogComments OFF

GO

SET IDENTITY_INSERT Stores ON
INSERT INTO Stores (StoreId, StoreImageUrl , StoreName, StoreDescription, DateCreated, StoreOwnerId) VALUES (1, N'http://pixsector.com/cache/a35c7d7b/avd437689ef3a02914ac1.png', N'Calvin Shop', N'This is my shop!', '2019-06-19', 2);
INSERT INTO Stores (StoreId, StoreImageUrl , StoreName, StoreDescription, DateCreated, StoreOwnerId) VALUES (2, N'https://www.pinclipart.com/picdir/middle/11-110975_available-app-store-vector-online-store-icon-png.png', N'John Shop', N'This is my shop!', '2019-06-19', 1);
INSERT INTO Stores (StoreId, StoreImageUrl , StoreName, StoreDescription, DateCreated, StoreOwnerId) VALUES (3, N'https://image.flaticon.com/icons/png/512/123/123403.png', N'Jane Shop', N'This is my shop!', '2019-06-19', 3);
SET IDENTITY_INSERT Stores OFF

GO

SET IDENTITY_INSERT StoreItems ON
INSERT INTO StoreItems (StoreItemId, StoreId, ItemImageUrl , ItemName, ItemCondition, ItemDescription, DateCreated, Quantity, Price) VALUES (1, 1, N'https://banner2.kisspng.com/20180125/eve/kisspng-stock-photography-banana-fruit-stock-footage-berry-golden-banana-5a6a7c02aed7e5.3327397415169280027162.jpg', N'Bananas', N'Fresh', N'A fresh pair of bananas for sale!', '2019-06-19', 20, 51.24);
INSERT INTO StoreItems (StoreItemId, StoreId, ItemImageUrl , ItemName, ItemCondition, ItemDescription, DateCreated, Quantity, Price) VALUES (2, 2, N'https://www.pngfind.com/pngs/m/176-1763979_strawberry-png-image-fruits-transparent-png.png', N'Strawberries', N'Fresh', N'A fresh pair of strawberries for sale!', '2019-06-20', 5, 24.25);
INSERT INTO StoreItems (StoreItemId, StoreId, ItemImageUrl , ItemName, ItemCondition, ItemDescription, DateCreated, Quantity, Price) VALUES (3, 1, N'https://png.pngtree.com/png-clipart/20190117/ourmid/pngtree-summer-delicious-watermelon-summer-fruit-png-image_422603.jpg', N'Watermelon', N'Fresh', N'One fresh watermelon for sale!', '2019-06-19', 1, 2455.24);
SET IDENTITY_INSERT Stores OFF

GO