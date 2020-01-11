select * from dbo.AspNetRoleClaims;
select * from dbo.AspNetRoles;
select * from dbo.AspNetUserLogins;
select * from dbo.AspNetUserRoles;
select * from dbo.AspNetUsers;
select * from dbo.Team;
select * from dbo.AspNetUserTokens;
select * from dbo.Bug;
select * from dbo.ProcessLog;
select * from dbo.Project;
select * from dbo.[Status];

select * from dbo.Urgency;
select * from dbo.UserProject;

insert into 
Team (Id, Name) 
values
(NEWID(), 'SysEng'),
(NEWID(), 'NetEng'),
(NEWID(), 'DataScience');

update dbo.AspNetUsers set FirstName = 'Howard', LastName = 'Tee' where Id = 'a8c935ef-ae89-4ca4-89ec-08d792265289';
update dbo.AspNetUsers set FirstName = 'Fredrick', LastName = 'Williams' where Id = '7ce7fb34-be71-48e5-e225-08d792e778ed';
update dbo.AspNetUsers set FirstName = 'John', LastName = 'Taylor' where Id = 'fb0d0a96-1c24-4141-d8f7-08d792fb15fa';
update dbo.AspNetUsers set FirstName = 'Eileen', LastName = 'Lee' where Id = 'c59f753e-2341-4324-d8f8-08d792fb15fa';
update dbo.AspNetUsers set FirstName = 'Terri', LastName = 'Pica' where Id = 'ad08d5e0-69f1-4658-0a2d-08d792fb7461';
update dbo.AspNetUsers set FirstName = 'Thomas', LastName = 'Stewart' where Id = 'a8cadbcf-2c86-4c1d-d8f9-08d792fb15fa';
