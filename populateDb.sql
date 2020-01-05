use TrackerDev;
insert into 
Team (Id, Name) 
values
(NEWID(), 'Front-End'),
(NEWID(), 'Back-End'),
(NEWID(), 'DevOps'),
(NEWID(), 'DBA');

insert into
ProcessLog(Detail)
values
(CONCAT(SYSDATETIME(), '<br />Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.<br />')),
(CONCAT(SYSDATETIME(), '<br />Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.<br />')),
(CONCAT(SYSDATETIME(), '<br />Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.<br />')),
(CONCAT(SYSDATETIME(), '<br />Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.<br />'));
insert into
Urgency(Id, Level, Description)
values
(NEWID(),'Low','Under low priority, does not need immediate attention'),
(NEWID(),'Medium','Should fix before next update'),
(NEWID(),'High','Fix immediately and push update'),
(NEWID(),'Critical','Fix immediately or you are in trouble'),
(NEWID(),'Catastrophic','Fix or no sleep for you');

insert into
Status(Id, Name, Description)
values
(NEWID(),'Open','The bug is open for the grabs'),
(NEWID(),'Assigned','The bug is recently assigned to someone'),
(NEWID(),'In-progress','The bug is assigned and is being worked on'),
(NEWID(),'Closed','The bug is resolved or no longer being worked on'),
(NEWID(),'Reopened','The bug is reopened to be worked on');


insert into
Project(Id, Name, Description, OwnerId)
values
(NEWID(),'Log-In Update', 'Modernize the entire log-in page with material design elements and bootstrap', '8941617f-d056-438e-841f-384cc89df2a8'),
(NEWID(),'Star-link', 'Design and implement a modern backend for log-in page in accordance to microservices architecture','391d0752-355d-4a0c-b224-48f22efe58e2'),
(NEWID(),'Checkout-Api','Design and implement a light weight API for checkout and transaction process','3494bb7b-051b-43e6-9d30-953d9395bb46'),
(NEWID(),'Star-link Update','Design and implement a database system using MongoDB','f775aa1f-79b9-40b6-98dd-a5df5ce60f93');


--insert into
--Bug(Title, Description, UrgencyId, CreatedOn, 
--ProjectAffectedId, StatusId, ClosedOn, LogDetailId, OwnerId)
--values
--('Stuff', 'Front-end fucked. Please fix some stuff.', 1, SYSDATETIME(),1,1,null,1,1),
--('Bug', 'Backend fucked. Quick fix needed.', 2, SYSDATETIME(), 2,2,null,2,2),
--('Shitstorm', 'Some stuff fucked. Please fix some stuff.', 3, SYSDATETIME(), 3, 3,null,3,3),
--('HolyFuck', 'Db fucked. Please fix that shitty stuff.', 4, SYSDATETIME(), 4, 4,null,4,4);

insert into
Bug(Title, Description, UrgencyId, CreatedOn, CreatedById,
ProjectAffectedId, StatusId, ClosedOn, LogDetailId, OwnerId)
values
('Stuff', 'Front-end fucked. Please fix some stuff.', 1, Getdate(), '28a41dd3-872a-4b51-ad40-f304e7a0db40', 1,1,null,1,1),
('Bug', 'Backend fucked. Quick fix needed.', 2, Getdate(),'3c0105ba-abb7-4461-9ac9-b548136b6032', 2,2,null,2,2),
('Shitstorm', 'Some stuff fucked. Please fix some stuff.', 3, Getdate(),'3c0105ba-abb7-4461-9ac9-b548136b6032', 3, 3,null,3,3),
('HolyFuck', 'Db fucked. Please fix that shitty stuff.', 4, Getdate(), '3c0105ba-abb7-4461-9ac9-b548136b6032',4, 4,null,4,4);


select * from dbo.AspNetUsers

update Bug set LogDetailId = null;
delete from ProcessLog;

select * from Project;
select * from ProcessLog;
select * from Status;
select * from Urgency;
select * from Team;
select * from Bug;

delete from ProcessLog;
delete from Project;
DELETE from [Status];
delete from Urgency;
delete from Team;
delete from Bug;