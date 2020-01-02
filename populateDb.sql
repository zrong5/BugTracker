use TrackerDev;
insert into 
Team (Name) 
values
('Front-End'),
('Back-End'),
('DevOps'),
('DBA');

insert into
ProcessLog(Detail)
values
(CONCAT(SYSDATETIME(), '<br />Lorem ipsum dolor sit amet, consectetur adipiscing elit, sed do eiusmod tempor incididunt ut labore et dolore magna aliqua.<br />')),
(CONCAT(SYSDATETIME(), '<br />Ut enim ad minim veniam, quis nostrud exercitation ullamco laboris nisi ut aliquip ex ea commodo consequat.<br />')),
(CONCAT(SYSDATETIME(), '<br />Duis aute irure dolor in reprehenderit in voluptate velit esse cillum dolore eu fugiat nulla pariatur.<br />')),
(CONCAT(SYSDATETIME(), '<br />Excepteur sint occaecat cupidatat non proident, sunt in culpa qui officia deserunt mollit anim id est laborum.<br />'));
insert into
Urgency(Level, Description)
values
('Low','Under low priority, does not need immediate attention'),
('Medium','Should fix before next update'),
('High','Fix immediately and push update'),
('Critical','Fix immediately or you are in trouble'),
('Catastrophic','Fix or no sleep for you');

insert into
Status(Name, Description)
values
('Open','The bug is open for the grabs'),
('Assigned','The bug is recently assigned to someone'),
('In-progress','The bug is assigned and is being worked on'),
('Closed','The bug is resolved or no longer being worked on'),
('Reopened','The bug is reopened to be worked on');

insert into
Project(Name, Description, OwnerId)
values
('Log-In Update', 'Modernize the entire log-in page with material design elements and bootstrap', 1),
('Star-link', 'Design and implement a modern backend for log-in page in accordance to microservices architecture',2),
('Checkout-Api','Design and implement a light weight API for checkout and transaction process',3),
('Star-link Update','Design and implement a database system using MongoDB',4);


--insert into
--Bug(Title, Description, UrgencyId, CreatedOn, 
--ProjectAffectedId, StatusId, ClosedOn, LogDetailId, OwnerId)
--values
--('Stuff', 'Front-end fucked. Please fix some stuff.', 1, SYSDATETIME(),1,1,null,1,1),
--('Bug', 'Backend fucked. Quick fix needed.', 2, SYSDATETIME(), 2,2,null,2,2),
--('Shitstorm', 'Some stuff fucked. Please fix some stuff.', 3, SYSDATETIME(), 3, 3,null,3,3),
--('HolyFuck', 'Db fucked. Please fix that shitty stuff.', 4, SYSDATETIME(), 4, 4,null,4,4);

insert into
Bug(Title, Description, UrgencyId, CreatedOn, 
ProjectAffectedId, StatusId, ClosedOn, LogDetailId, OwnerId)
values
('Stuff', 'Front-end fucked. Please fix some stuff.', 1, Getdate(),1,1,null,1,1),
('Bug', 'Backend fucked. Quick fix needed.', 2, Getdate(), 2,2,null,2,2),
('Shitstorm', 'Some stuff fucked. Please fix some stuff.', 3, Getdate(), 3, 3,null,3,3),
('HolyFuck', 'Db fucked. Please fix that shitty stuff.', 4, Getdate(), 4, 4,null,4,4);


update Bug set LogDetailId = null;
delete from ProcessLog;

select * from Project;
select * from ProcessLog;
select * from Status;
select * from Urgency;
select * from Team;
select * from Bug;