use TrackerDev;
delete from Project;
delete from Status;
delete from Team;
delete from Bug;
delete from Urgency;
delete from ProcessLog;

select Detail from ProcessLog where Id = 1;
