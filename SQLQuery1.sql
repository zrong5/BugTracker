select * from dbo.AspNetRoleClaims;
select * from dbo.AspNetRoles;
select * from dbo.AspNetUserLogins;
select * from dbo.AspNetUserRoles;
select * from dbo.AspNetUsers;
select * from dbo.AspNetUserTokens;
select * from dbo.Bug;
select * from dbo.ProcessLog;
select * from dbo.Project;
select * from dbo.[Status];
select * from dbo.Team;
select * from dbo.Urgency;
select * from dbo.UserProject;

insert into 
Team (Id, Name) 
values
(NEWID(), 'SysEng'),
(NEWID(), 'NetEng'),
(NEWID(), 'DataScience');

